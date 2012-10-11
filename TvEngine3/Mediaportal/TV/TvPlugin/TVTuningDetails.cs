#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
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
using System.Collections.Generic;
using System.Globalization;
using MediaPortal.GUI.Library;
using Mediaportal.TV.Server.TVDatabase.Entities;
using Mediaportal.TV.Server.TVLibrary.Interfaces;
using Mediaportal.TV.Server.TVControl;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Interfaces;
using Mediaportal.TV.Server.TVService.Interfaces.Services;
using Mediaportal.TV.Server.TVService.ServiceAgents;

namespace Mediaportal.TV.TvPlugin
{
  public class TVTuningDetails : GUIInternalWindow
  {
    public TVTuningDetails()
    {
      GetID = (int)Window.WINDOW_TV_TUNING_DETAILS;
    }

    #region Overrides

    public override bool Init()
    {
      bool bResult = Load(GUIGraphicsContext.Skin + @"\mytvtuningdetails.xml");
      return bResult;
    }

    protected override void OnPageLoad()
    {
      base.OnPageLoad();
      GUIPropertyManager.SetProperty("#TV.TuningDetails.ChannelName", TVHome.Card.ChannelName);
      GUIPropertyManager.SetProperty("#TV.TuningDetails.RTSPURL", TVHome.Card.RTSPUrl);
      Channel chan = ServiceAgents.Instance.ChannelServiceAgent.GetChannel(TVHome.Navigator.Channel.Entity.IdChannel);
      if (chan != null)
      {
        try
        {
          GUIPropertyManager.SetProperty("#TV.TuningDetails.HasCiMenuSupport", TVHome.Card.CiMenuSupported().ToString(CultureInfo.InvariantCulture));
        }
        catch (System.Exception ex)
        {
          Log.Error("Error loading TuningDetails /  HasCiMenuSupport:" + ex.StackTrace);
        }

        IList<TuningDetail> details = chan.TuningDetails;
        if (details.Count > 0)
        {
          TuningDetail detail = null;
          switch (TVHome.Card.Type)
          {
            case CardType.Analog:
              foreach (TuningDetail t in details)
              {
                if (t.ChannelType == 0)
                  detail = t;
              }
              break;
            case CardType.Atsc:
              foreach (TuningDetail t in details)
              {
                if (t.ChannelType == 1)
                  detail = t;
              }
              break;
            case CardType.DvbC:
              foreach (TuningDetail t in details)
              {
                if (t.ChannelType == 2)
                  detail = t;
              }
              break;
            case CardType.DvbS:
              foreach (TuningDetail t in details)
              {
                if (t.ChannelType == 3)
                  detail = t;
              }
              break;
            case CardType.DvbT:
              foreach (TuningDetail t in details)
              {
                if (t.ChannelType == 4)
                  detail = t;
              }
              break;
            default:
              detail = details[0];
              break;
          }

          switch (detail.ChannelType)
          {
            case 0:
              GUIPropertyManager.SetProperty("#TV.TuningDetails.channelType", "Analog");

              CountryCollection cc = new CountryCollection();
              Country c = cc.GetTunerCountryFromID(detail.CountryId);
              GUIPropertyManager.SetProperty("#TV.TuningDetails.Country", c.Name);
              GUIPropertyManager.SetProperty("#TV.TuningDetails.VideoSource", ((AnalogChannel.VideoInputType)detail.VideoSource).ToString());
              GUIPropertyManager.SetProperty("#TV.TuningDetails.AudioSource", ((AnalogChannel.AudioInputType)detail.AudioSource).ToString());
              break;
            case 1:
              GUIPropertyManager.SetProperty("#TV.TuningDetails.channelType", "Atsc");
              break;
            case 2:
              GUIPropertyManager.SetProperty("#TV.TuningDetails.channelType", "DVB-C");
              break;
            case 3:
              GUIPropertyManager.SetProperty("#TV.TuningDetails.channelType", "DVB-S");
              GUIPropertyManager.SetProperty("#TV.TuningDetails.Polarisation", ((Polarisation)detail.Polarisation).ToString());
              GUIPropertyManager.SetProperty("#TV.TuningDetails.FecRate", ((BinaryConvolutionCodeRate)detail.InnerFecRate).ToString());
              GUIPropertyManager.SetProperty("#TV.TuningDetails.Pilot", ((Pilot)detail.Pilot).ToString());
              GUIPropertyManager.SetProperty("#TV.TuningDetails.RollOff", ((RollOff)detail.RollOff).ToString());
              break;
            case 4:
              GUIPropertyManager.SetProperty("#TV.TuningDetails.channelType", "DVB-T");
              GUIPropertyManager.SetProperty("#TV.TuningDetails.Bandwidth", detail.Bandwidth.ToString());
              break;
          }

          IUser user = TVHome.Card.User;
          IVideoStream videoStream = TVHome.Card.GetCurrentVideoStream((User)user);
          IEnumerable<IAudioStream> audioStreams = TVHome.Card.AvailableAudioStreams;

          String audioPids = String.Empty;
          String videoPid = String.Empty;

          if (audioStreams != null)
          {
            foreach (IAudioStream stream in audioStreams)
            {
              audioPids += stream.Pid + " (" + stream.StreamType + ") ";
            } 
          }          

          GUIPropertyManager.SetProperty("#TV.TuningDetails.Provider", detail.Provider);
          GUIPropertyManager.SetProperty("#TV.TuningDetails.FreeToAir", detail.FreeToAir.ToString());

          GUIPropertyManager.SetProperty("#TV.TuningDetails.ChannelNumber", detail.ChannelNumber.ToString());
          GUIPropertyManager.SetProperty("#TV.TuningDetails.Frequency", detail.Frequency.ToString());
          GUIPropertyManager.SetProperty("#TV.TuningDetails.SymbolRate", detail.Symbolrate.ToString());
          GUIPropertyManager.SetProperty("#TV.TuningDetails.Modulation", ((ModulationType)detail.Modulation).ToString());

          GUIPropertyManager.SetProperty("#TV.TuningDetails.ServiceId", detail.ServiceId.ToString());
          GUIPropertyManager.SetProperty("#TV.TuningDetails.NetworkId", detail.NetworkId.ToString());
          GUIPropertyManager.SetProperty("#TV.TuningDetails.TransportId", detail.TransportId.ToString());
          GUIPropertyManager.SetProperty("#TV.TuningDetails.PmtPid", detail.PmtPid.ToString());
          GUIPropertyManager.SetProperty("#TV.TuningDetails.VideoStream", videoStreams);
          GUIPropertyManager.SetProperty("#TV.TuningDetails.AudioStreams", audioStreams);
        }
      }
    }

    private DateTime _updateTimer = DateTime.Now;
    public override void Process()
    {

      TimeSpan ts = DateTime.Now - _updateTimer;
      if (ts.TotalMilliseconds < 500)
      {
        return;
      }

      GUIPropertyManager.SetProperty("#TV.TuningDetails.SignalLevel", TVHome.Card.SignalLevel.ToString());
      GUIPropertyManager.SetProperty("#TV.TuningDetails.SignalQuality", TVHome.Card.SignalQuality.ToString());

      int totalTSpackets = 0;
      int discontinuityCounter = 0;
      TVHome.Card.GetStreamQualityCounters(out totalTSpackets, out discontinuityCounter);

      GUIPropertyManager.SetProperty("#TV.TuningDetails.TSPacketsTransferred", Convert.ToString(totalTSpackets));
      GUIPropertyManager.SetProperty("#TV.TuningDetails.Discontinuities", Convert.ToString(discontinuityCounter));

      _updateTimer = DateTime.Now;
    }

    #endregion
  }
}