#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Runtime.InteropServices;
using DirectShowLib;
using TvLibrary.Channels;
using TvLibrary.Implementations.Helper;
using TvLibrary.Interfaces;
using TvLibrary.Interfaces.Analyzer;
using TvLibrary.Interfaces.Device;

namespace TvLibrary.Implementations.DVB
{
  /// <summary>
  /// Implementation of <see cref="T:TvLibrary.Interfaces.ITVCard"/> which handles MediaPortal DVB-IP sources.
  /// </summary>
  public class TvCardDVBIP : TvCardDvbBase, ITVCard
  {
    #region variables

    private IBaseFilter _filterStreamSource = null;
    private int _sequenceNumber = -1;

    /// <summary>
    /// The source filter's default URL.
    /// </summary>
    protected string _defaultUrl;

    /// <summary>
    /// The CLSID/UUID for the source filter.
    /// </summary>
    protected Guid _sourceFilterGuid = Guid.Empty;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TvCardDVBIP"/> class.
    /// </summary>
    /// <param name="epgEvents">The EPG events interface.</param>
    /// <param name="device">The device.</param>
    /// <param name="sequenceNumber">A sequence number or index for this instance.</param>
    public TvCardDVBIP(IEpgEvents epgEvents, DsDevice device, int sequenceNumber)
      : base(epgEvents, device)
    {
      _cardType = CardType.DvbIP;
      _defaultUrl = "udp://@0.0.0.0:1234";
      _sourceFilterGuid = new Guid(0xd3dd4c59, 0xd3a7, 0x4b82, 0x97, 0x27, 0x7b, 0x92, 0x03, 0xeb, 0x67, 0xc0);

      _sequenceNumber = sequenceNumber;
      if (_sequenceNumber > 0)
      {
        _name += "_" + _sequenceNumber;
      }
    }

    #region graphbuilding

    /// <summary>
    /// Build the BDA filter graph.
    /// </summary>
    public override void BuildGraph()
    {
      Log.Log.Debug("TvCardDvbIp: BuildGraph()");
      try
      {
        if (_graphState != GraphState.Idle)
        {
          Log.Log.Error("TvCardDvbIp: the graph is already built");
          throw new TvException("The graph is already built.");
        }

        _graphBuilder = (IFilterGraph2)new FilterGraph();
        _capBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
        _capBuilder.SetFiltergraph(_graphBuilder);
        _rotEntry = new DsROTEntry(_graphBuilder);

        _infTee = (IBaseFilter)new InfTee();
        int hr = _graphBuilder.AddFilter(_infTee, "Inf Tee");
        if (hr != 0)
        {
          Log.Log.Error("TvCardDvbIp: failed to add infinite tee, hr = 0x{0:x} ({1})", hr, HResult.GetDXErrorString(hr));
          throw new TvException("Failed to add infinite tee.");
        }

        AddTsWriterFilterToGraph();
        AddStreamSourceFilter(_defaultUrl);
        IBaseFilter lastFilter = _filterStreamSource;

        // Check for and load plugins, adding any additional device filters to the graph.
        LoadPlugins(_filterStreamSource, _capBuilder, ref lastFilter);

        if (!ConnectTsWriter(lastFilter))
        {
          throw new TvExceptionGraphBuildingFailed("Failed to connect TS writer filter.");
        }

        _graphState = GraphState.Created;

        bool startGraph = false;
        foreach (ICustomDevice deviceInterface in _customDeviceInterfaces)
        {
          bool start;
          deviceInterface.OnGraphBuilt(this, out start);
          if (start)
          {
            startGraph = true;
          }
        }
        if (startGraph)
        {
          RunGraph(-1);
        }
      }
      catch (Exception ex)
      {
        Log.Log.Write(ex);
        Dispose();
        _graphState = GraphState.Idle;
        throw new TvExceptionGraphBuildingFailed("Graph building failed.", ex);
      }
    }

    /// <summary>
    /// Add the DVB-IP stream source filter to the BDA graph.
    /// </summary>
    /// <param name="url">url</param>
    protected virtual void AddStreamSourceFilter(string url)
    {
      Log.Log.WriteFile("TvCardDvbIp: add source filter");
      _filterStreamSource = FilterGraphTools.AddFilterFromClsid(_graphBuilder, _sourceFilterGuid,
                                                                "DVB-IP Source Filter");
      AMMediaType mpeg2ProgramStream = new AMMediaType();
      mpeg2ProgramStream.majorType = MediaType.Stream;
      mpeg2ProgramStream.subType = MediaSubType.Mpeg2Transport;
      mpeg2ProgramStream.unkPtr = IntPtr.Zero;
      mpeg2ProgramStream.sampleSize = 0;
      mpeg2ProgramStream.temporalCompression = false;
      mpeg2ProgramStream.fixedSizeSamples = true;
      mpeg2ProgramStream.formatType = FormatType.None;
      mpeg2ProgramStream.formatSize = 0;
      mpeg2ProgramStream.formatPtr = IntPtr.Zero;
      ((IFileSourceFilter)_filterStreamSource).Load(url, mpeg2ProgramStream);

      // Connect the source filter to the infinite tee.
      Log.Log.WriteFile("TvCardDvbIp: render [source]->[inf tee]");
      int hr = _capBuilder.RenderStream(null, null, _filterStreamSource, null, _infTee);
      if (hr != 0)
      {
        Log.Log.Error("TvCardDvbIp: failed to render stream, hr = 0x{0:x} ({1})", hr, HResult.GetDXErrorString(hr));
        throw new TvException("Failed to render stream.");
      }
    }

    /// <summary>
    /// Remove the DVB-IP stream source filter from the BDA graph.
    /// </summary>
    protected virtual void RemoveStreamSourceFilter()
    {
      Log.Log.WriteFile("TvCardDvbIp: remove source filter");
      if (_filterStreamSource != null)
      {
        if (_graphBuilder != null)
        {
          _graphBuilder.RemoveFilter(_filterStreamSource);
        }
        Release.ComObject("DVB-IP Source Filter", _filterStreamSource);
        _filterStreamSource = null;
      }
    }

    /// <summary>
    /// Start the BDA filter graph (subject to a few conditions).
    /// </summary>
    /// <param name="subChannelId">The subchannel ID for the channel that is being started.</param>
    protected virtual void RunGraph(int subChannelId)
    {
      // DVB-IP "tuning" (if there is such a thing) occurs during this stage of the process. We stop the
      // graph, then replace the stream source filter with a new filter that is configured to stream from
      // the appropriate URL.
      Log.Log.Debug("TvCardDvbIp: RunGraph()");
      int hr;
      bool graphRunning = GraphRunning();

      TvDvbChannel subchannel = null;
      if (_mapSubChannels.ContainsKey(subChannelId))
      {
        subchannel = (TvDvbChannel)_mapSubChannels[subChannelId];
      }

      if (subchannel != null)
      {
        if (graphRunning)
        {
          hr = (_graphBuilder as IMediaControl).StopWhenReady();
          if (hr < 0 || hr > 1)
          {
            Log.Log.WriteFile("TvCardDvbIp: failed to stop graph, hr = 0x{0:x} ({1})", hr, HResult.GetDXErrorString(hr));
            throw new TvException("TvCardDvbIp: failed to stop graph");
          }
          subchannel.OnGraphStopped();
        }

        subchannel.AfterTuneEvent -= new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        subchannel.AfterTuneEvent += new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        subchannel.OnGraphStart();

        DVBIPChannel ch = subchannel.CurrentChannel as DVBIPChannel;
        if (ch != null)
        {
          RemoveStreamSourceFilter();
          AddStreamSourceFilter(ch.Url);
        }
      }

      if (graphRunning)
      {
        Log.Log.Debug("TvCardDvbIp: graph already running");
        return;
      }
      Log.Log.Debug("TvCardDvbIp: start graph");
      hr = (_graphBuilder as IMediaControl).Run();
      if (hr < 0 || hr > 1)
      {
        Log.Log.Debug("TvCardDvbIp: failed to start graph, hr = 0x{0:x} ({1})", hr, HResult.GetDXErrorString(hr));
        throw new TvException("TvCardDvbIp: failed to start graph");
      }

      _epgGrabbing = false;
      if (subchannel != null)
      {
        foreach (ICustomDevice deviceInterface in _customDeviceInterfaces)
        {
          deviceInterface.OnGraphRunning(this, subchannel.CurrentChannel);
          IPowerDevice powerDevice = deviceInterface as IPowerDevice;
          if (powerDevice != null)
          {
            powerDevice.SetPowerState(true);
          }
        }
        subchannel.AfterTuneEvent -= new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        subchannel.AfterTuneEvent += new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        subchannel.OnGraphStarted();
      }
    }

    #endregion

    /// <summary>
    /// Actually tune to a channel.
    /// </summary>
    /// <param name="channel">The channel to tune to.</param>
    protected override void Tune(IChannel channel)
    {
      foreach (ICustomDevice deviceInterface in _customDeviceInterfaces)
      {
        ICustomTuner customTuner = deviceInterface as ICustomTuner;
        if (customTuner != null && customTuner.CanTuneChannel(channel))
        {
          Log.Log.Debug("TvCardDvbIp: using custom tuning");
          if (!customTuner.Tune(channel, _parameters))
          {
            throw new TvException("TvCardDvbIp: failed to tune to channel");
          }
          break;
        }
      }
    }

    #region ITVCard members

    /// <summary>
    /// Check if the tuner can tune to a given channel.
    /// </summary>
    /// <param name="channel">The channel to check.</param>
    /// <returns><c>true</c> if the tuner can tune to the channel, otherwise <c>false</c></returns>
    public override bool CanTune(IChannel channel)
    {
      if ((channel as DVBIPChannel) == null)
      {
        return false;
      }
      return true;
    }

    /// <summary>
    /// ScanningInterface
    /// </summary>
    public override ITVScanning ScanningInterface
    {
      get
      {
        if (!CheckThreadId()) return null;
        return new DVBIPScanning(this);
      }
    }

    #endregion

    /// <summary>
    /// Dispose resources
    /// </summary>
    public override void Dispose()
    {
      base.Dispose();
      RemoveStreamSourceFilter();
    }

    /// <summary>
    /// Gets wether or not card supports pausing the graph.
    /// </summary>
    public override bool SupportsPauseGraph
    {
      get { return false; }
    }

    /// <summary>
    /// Stop the BDA filter graph (subject to a few conditions).
    /// </summary>
    public override void StopGraph()
    {
      base.StopGraph();
      //FIXME: this logic should be checked (removing and adding filters in stopgraph?) (morpheus_xx)
      if (_graphState == GraphState.Created)
      {
        RemoveStreamSourceFilter();
        AddStreamSourceFilter(_defaultUrl);
      }
    }

    /// <summary>
    /// UpdateSignalQuality
    /// </summary>
    protected override void UpdateSignalQuality()
    {
      if (GraphRunning() == false ||
        CurrentChannel == null ||
        _filterStreamSource == null ||
        !CheckThreadId())
      {
        _tunerLocked = false;
        _signalLevel = 0;
        _signalPresent = false;
        _signalQuality = 0;
        return;
      }
      _tunerLocked = true;
      _signalLevel = 100;
      _signalPresent = true;
      _signalQuality = 100;
    }

    /// <summary>
    /// return the DevicePath
    /// </summary>
    public override string DevicePath
    {
      get
      {
        if (_sequenceNumber == 0)
        {
          return base.DevicePath;
        }
        return base.DevicePath + "(" + _sequenceNumber + ")";
      }
    }

    protected override DVBBaseChannel CreateChannel(int networkid, int transportid, int serviceid, string name)
    {
      DVBIPChannel channel = new DVBIPChannel();
      channel.NetworkId = networkid;
      channel.TransportId = transportid;
      channel.ServiceId = serviceid;
      channel.Name = name;
      return channel;
    }
  }
}