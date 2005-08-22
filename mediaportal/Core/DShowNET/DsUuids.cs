/* 
 *	Copyright (C) 2005 Media Portal
 *	http://mediaportal.sourceforge.net
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

using System;
using System.Runtime.InteropServices;

namespace DShowNET
{

	public class AmInterlace
	{
		static public readonly uint IsInterlaced          =  0x00000001;  // if 0, other interlace bits are irrelevent
		static public readonly uint OneFieldPerSample       =  0x00000002;  // else 2 fields per media sample
		static public readonly uint Field1First           =  0x00000004;  // else Field 2 is first;  top field in PAL is field 1, top field in NTSC is field 2?
		static public readonly uint UNUSED                =  0x00000008;  //
		static public readonly uint FieldPatternMask      =  0x00000030;  // use this mask with AMINTERLACE_FieldPat*
		static public readonly uint FieldPatField1Only    =  0x00000000;  // stream never contains a Field2
		static public readonly uint FieldPatField2Only    =  0x00000010;  // stream never contains a Field1
		static public readonly uint FieldPatBothRegular   =  0x00000020;  // There will be a Field2 for every Field1 (required for Weave?)
		static public readonly uint FieldPatBothIrregular =  0x00000030;  // Random pattern of Field1s and Field2s
		static public readonly uint DisplayModeMask       =  0x000000c0;
		static public readonly uint DisplayModeBobOnly    =  0x00000000;
		static public readonly uint DisplayModeWeaveOnly  =  0x00000040;
		static public readonly uint DisplayModeBobOrWeave =  0x00000080;
	}

  public class HResult
  {
    static public readonly int  S_OK                  =unchecked(0);
    static public readonly int  E_HANDLE              =unchecked((int)0x80070006);
    static public readonly int  E_NOTIMPL             =unchecked((int)0x80004001);
    static public readonly int  E_PROP_ID_UNSUPPORTED =unchecked((int)0x80070490);
    static public readonly int  E_PROP_SET_UNSUPPORTED=unchecked((int)0x80070492);
    static public readonly int  E_INSUFFICIENT_BUFFER=unchecked((int)0x8007007A);
  }
	public class ClassId
	{
		/// <summary>Prevent instantiation.</summary>
		private ClassId(){}

		/// <summary>The File Writer filter can be used to write files to disc regardless of format. </summary>
		public static readonly Guid FileWriter = new Guid("8596E5F0-0DA5-11D0-BD21-00A0C911CE86");

		/// <summary>The Filter Graph Manager builds and controls filter graphs.</summary>
		public static readonly Guid FilterGraph = new Guid("E436EBB3-524F-11CE-9F53-0020AF0BA770");

		/// <summary>The WM ASF Writer filter accepts a variable number of input streams and creates an ASF file.</summary>
		public static readonly Guid WMAsfWriter = new Guid("7C23220E-55BB-11D3-8B16-00C04FB6BD3D");

		/// <summary>The RecComp object creates new content recordings by concatenating existing recordings.</summary>
		public static readonly Guid RecComp = new Guid("D682C4BA-A90A-42FE-B9E1-03109849C423");

		/// <summary>The Recording object creates permanent recordings from streams that the Stream Buffer Sink filter captures.</summary>
		public static readonly Guid RecordingAttributes = new Guid("CCAA63AC-1057-4778-AE92-1206AB9ACEE6");

		/// <summary>The WavDes filter writes an audio stream to a WAV file.</summary>
		public static readonly Guid WavDest = new Guid("3C78B8E2-6C4D-11d1-ADE2-0000F8754B99");

		/// <summary>The Decrypter/Detagger filter conditionally decrypts samples that are encrypted by the Encrypter/Tagger filter.</summary>
		public static readonly Guid DecryptTag = new Guid("C4C4C4F2-0049-4E2B-98FB-9537F6CE516D");

		/// <summary>Creates an instance of a COM object by class ID.</summary>
		/// <param name="id">The class ID of the component to instantiate.</param>
		/// <returns>A new instance of the class.</returns>
		public static object CoCreateInstance(Guid id)
		{
			return Activator.CreateInstance(Type.GetTypeFromCLSID(id));
		}
	}
	[ComVisible(false)]
public class FilterCategory		// uuids.h  :  CLSID_*
{
		/// <summary> CLSID_AudioRendererCategory, audio render category </summary>
		public static readonly Guid AudioRendererDevice= new Guid( 0xe0f158e1, 0xcb04, 0x11d0, 0xbd, 0x4e, 0x0, 0xa0, 0xc9, 0x11, 0xce, 0x86);

		/// <summary> CLSID_AudioInputDeviceCategory, audio capture category </summary>
	public static readonly Guid AudioInputDevice	= new Guid( 0x33d9a762,0x90c8,0x11d0,0xbd,0x43,0x00,0xa0,0xc9,0x11,0xce,0x86 );

		/// <summary> CLSID_VideoInputDeviceCategory, video capture category </summary>
	public static readonly Guid VideoInputDevice	= new Guid( 0x860BB310,0x5D01,0x11d0,0xBD,0x3B,0x00,0xA0,0xC9,0x11,0xCE,0x86 );

		/// <summary> CLSID_VideoCompressorCategory, Video compressor category </summary>
		public static readonly Guid VideoCompressorCategory	= new Guid( 0x33d9a760, 0x90c8, 0x11d0, 0xbd, 0x43, 0x0, 0xa0, 0xc9, 0x11, 0xce, 0x86 );
	public static readonly Guid AM_KSTvTuner  = new Guid(0xA799A800, 0xA46D, 0x11D0, 0xA1,0x8C,0x00,0xA0,0x24,0x01,0xDC,0xD4 );
	public static readonly Guid AM_KS_BDA_RECEIVER_COMPONENT		= new Guid(0xFD0A5AF4, 0xB41D, 0x11d2, 0x9c, 0x95, 0x00, 0xc0, 0x4f, 0x79, 0x71, 0xe0);
	  public static readonly Guid AM_KSCrossBar      = new Guid(0xA799A801 , 0xA46D, 0x11D0, 0xA1 ,0x8C ,0x00, 0xA0, 0x24, 0x01, 0xDC, 0xD4 );
    public static readonly Guid AM_KSEncoder       = new Guid(0x19689bf6, 0xc384, 0x48fd, 0xad, 0x51, 0x90, 0xe5, 0x8c, 0x79, 0xf7, 0xb);
		/// <summary> CLSID_AudioCompressorCategory, audio compressor category </summary>
		public static readonly Guid AudioCompressorCategory	= new Guid( 0x33d9a761, 0x90c8, 0x11d0, 0xbd, 0x43, 0x0, 0xa0, 0xc9, 0x11, 0xce, 0x86 );

		/// <summary> CLSID_LegacyAmFilterCategory, legacy filters </summary>
		public static readonly Guid LegacyAmFilterCategory	= new Guid( 0x083863F1,0x70DE,0x11d0,0xBD,0x40,0x00,0xA0,0xC9,0x11,0xCE,0x86 );

		/// <summary>
		/// #MW# CLSID_ActiveMovieCategory, a superset of all the available filters
		/// </summary>
		public static readonly Guid ActiveMovieCategory	= new Guid(0xda4e3da0, 0xd07d, 0x11d0, 0xbd, 0x50, 0x0, 0xa0, 0xc9, 0x11, 0xce, 0x86);
	
    public static readonly Guid IID_IKsPropertySet = new Guid(0x31efac30, 0x515c, 0x11d0, 0xa9, 0xaa, 0x00, 0xaa, 0x00, 0x61, 0xbe, 0x93);

  }

  [ComVisible(false)]
  public class Deinterlace
  {
    public static readonly Guid DXVA_DeinterlaceBobDevice = new Guid(0x335aa36e,0x7884,0x43a4,0x9c,0x91,0x7f,0x87,0xfa,0xf3,0xe3,0x7e);
  }

	[ComVisible(false)]
	public class Clsid		// uuids.h  :  CLSID_*
	{
		public static readonly Guid Clsid_FilterMapper2= new Guid(0xCDA42200,0xBD88,0x11D0,0xBD,0x4E,0x00,0xA0,0xC9,0x11,0xCE,0x86);

		/// <summary> CLSID_SystemDeviceEnum for ICreateDevEnum </summary>
	public static readonly Guid SystemDeviceEnum			= new Guid( 0x62BE5D10,0x60EB,0x11d0,0xBD,0x3B,0x00,0xA0,0xC9,0x11,0xCE,0x86 );

		/// <summary> CLSID_FilterGraph, filter Graph </summary>
	public static readonly Guid FilterGraph		= new Guid( 0xe436ebb3, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

		/// <summary> CLSID_CaptureGraphBuilder2, new Capture graph building </summary>
	public static readonly Guid CaptureGraphBuilder2	= new Guid( 0xBF87B6E1, 0x8C27, 0x11d0, 0xB3, 0xF0, 0x0, 0xAA, 0x00, 0x37, 0x61, 0xC5 );

	public static readonly Guid StreamBufferSource	 = new    Guid( 0xC9F5FE02, 0xF851, 0x4eb5, 0x99, 0xEE, 0xAD, 0x60, 0x2A, 0xF1, 0xE6, 0x19);
		/// <summary> CLSID_SampleGrabber, Sample Grabber filter </summary>
	public static readonly Guid SampleGrabber	= new Guid( 0xC1F400A0, 0x3F08, 0x11D3, 0x9F, 0x0B, 0x00, 0x60, 0x08, 0x03, 0x9E, 0x37 );

		/// <summary> CLSID_DvdGraphBuilder,  DVD graph builder </summary>
	public static readonly Guid DvdGraphBuilder	= new Guid( 0xFCC152B7, 0xF372, 0x11d0, 0x8E, 0x00, 0x00, 0xC0, 0x4F, 0xD7, 0xC0, 0x8B );
	// VMR9
		/// <summary> CLSID_DvdGraphBuilder,  DVD graph builder </summary>
		public static readonly Guid VideoMixingRenderer9 = new Guid( 0x51b4abf3, 0x748f, 0x4e3b, 0xa2, 0x76, 0xc8, 0x28, 0x33, 0x0e, 0x92, 0x6a );
		public static readonly Guid VideoMixingRenderer = new Guid( 0xB87BEB7B, 0x8D29, 0x423f, 0xAE, 0x4D, 0x65, 0x82, 0xC1, 0x01, 0x75, 0xAC);
		/// <summary> CLSID_DvdGraphBuilder,  DVD graph builder </summary>
		public static readonly Guid AllocPresenter9  	= new Guid( 0x2d2e24cb, 0x0cd5, 0x458f, 0x86, 0xea, 0x3e, 0x6f, 0xa2, 0x2c, 0x83, 0x64 );
		/// <summary> CLSID_DvdGraphBuilder,  DVD graph builder </summary>
		public static readonly Guid ImageSynchronization9	= new Guid( 0xE4979309, 0x7A32, 0x495E, 0x8A, 0x92, 0x7b, 0x01, 0x4A, 0xAD, 0x49, 0x61 );

	/// <summary> CLSID_AviSplitter, split an AVI stream into separate Video and audio streams </summary>
	public static readonly Guid AviSplitter = new Guid( 0x1b544c20, 0xfd0b, 0x11ce, 0x8c, 0x63, 0x0, 0xaa, 0x00, 0x44, 0xb5, 0x1e );

	/// <summary> CLSID_SmartTee, create a preview stream when device only provides a capture stream. </summary>
	public static readonly Guid SmartTee = new Guid( 0xcc58e280, 0x8aa1, 0x11d1, 0xb3, 0xf1, 0x0, 0xaa, 0x0, 0x37, 0x61, 0xc5 );
	public static readonly Guid InfTee = new Guid( 0xf8388a40, 0xd5bb, 0x11d0, 0xbe, 0x5a, 0x0, 0x80, 0xc7, 0x6, 0x56, 0x8e);

  public static readonly Guid Mpeg2Demultiplexer = new Guid(0xafb6c280, 0x2c41, 0x11d3, 0x8a, 0x60, 0x00, 0x00, 0xf8, 0x1e, 0x0e, 0x4a);
	public static readonly Guid MPTSWriter = new Guid("8943BEB7-E0BC-453b-9EA5-EB93899FA51C");
	public static readonly Guid MPStreamAnalyzer = new Guid("BAAC8911-1BA2-4ec2-96BA-6FFE42B62F72");

	public static readonly Guid PinCategoryVBI = new Guid(0xfb6c4284, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

}


  [ComVisible(false)]
  public class TimeFormat
{
  public static readonly Guid None     =new Guid(0x0       , 0x0   , 0x0   , 0x0 , 0x0 , 0x00, 0x00, 0x0 , 0x0 , 0x0 , 0x0);
  public static readonly Guid Frame    =new Guid(0x7b785570, 0x8c82, 0x11cf, 0xbc, 0xc , 0x0 , 0xaa, 0x0 , 0xac, 0x74, 0xf6);
  public static readonly Guid Byte     =new Guid(0x7b785571, 0x8c82, 0x11cf, 0xbc, 0xc , 0x0 , 0xaa, 0x0 , 0xac, 0x74, 0xf6);
  public static readonly Guid Sample   =new Guid(0x7b785572, 0x8c82, 0x11cf, 0xbc, 0xc , 0x0 , 0xaa, 0x0 , 0xac, 0x74, 0xf6);
  public static readonly Guid Field    =new Guid(0x7b785573, 0x8c82, 0x11cf, 0xbc, 0xc , 0x0 , 0xaa, 0x0 , 0xac, 0x74, 0xf6);
  public static readonly Guid MediaTime=new Guid(0x7b785574, 0x8c82, 0x11cf, 0xbc, 0xc , 0x0 , 0xaa, 0x0 , 0xac, 0x74, 0xf6);
}

	[ComVisible(false)]
public class MediaType		// MEDIATYPE_*
{
		/// <summary> MEDIATYPE_Video 'vids' </summary>
	public static readonly Guid Video		= new Guid( 0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71 );

		/// <summary> MEDIATYPE_Interleaved 'iavs' </summary>
	public static readonly Guid Interleaved	= new Guid( 0x73766169, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71 );

		/// <summary> MEDIATYPE_Audio 'auds' </summary>
	public static readonly Guid Audio		= new Guid( 0x73647561, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71 );

		/// <summary> MEDIATYPE_Text 'txts' </summary>
	public static readonly Guid Text		= new Guid( 0x73747874, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71 );

	public static readonly Guid Midi		= new Guid( 0x7364696D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIATYPE_Stream </summary>
	public static readonly Guid Stream		= new Guid( 0xe436eb83, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

  public static readonly Guid MPEG1SystemStream		= new Guid( 0xe436eb82, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
  public static readonly Guid File		= new Guid( 0x656c6966, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

	public static readonly Guid ScriptCommand		= new Guid( 	0x73636d64, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
	public static readonly Guid AUXLine21Data = new Guid(	0x670aea80, 0x3a82, 0x11d0, 0xb7, 0x9b, 0x0, 0xaa, 0x0, 0x37, 0x67, 0xa7);
	public static readonly Guid VBI = new Guid(	0xf72a76e1, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);
	public static readonly Guid TimeCode = new Guid(	0x482dee3, 0x7817, 0x11cf, 0x8a, 0x3, 0x0, 0xaa, 0x0, 0x6e, 0xcb, 0x65);
	public static readonly Guid LMRT = new Guid(0x74726c6d, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
  public static readonly Guid UrlStream = new Guid(0x736c7275, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
	public static readonly Guid AnalogVideo = new Guid(0x482dde1, 0x7817, 0x11cf, 0x8a, 0x3, 0x0, 0xaa, 0x0, 0x6e, 0xcb, 0x65);
	public static readonly Guid AnalogAudio   = new Guid(0x482dee1 , 0x7817, 0x11cf, 0x8a, 0x3, 0x0 , 0xaa, 0x0 , 0x6e, 0xcb, 0x65);
	public static readonly Guid Mpeg2Sections = new Guid(0x455F176C, 0x4B06, 0x47CE, 0x9A, 0xEF,0x8C, 0xAE, 0xF7, 0x3D, 0xF7, 0xB5);
	}

	[ComVisible(false)]
public class MediaSubType		// MEDIASUBTYPE_*
{
		public static readonly Guid DolbyAC3= new Guid(0xe06d802c, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
		public static readonly Guid DolbyAC3SPDIF = new Guid(0x00000092, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

    //083863F1-70DE-11D0-BD40-00A0C911CE86}\AA7BB8A0-5659-11D4-98B2-00A0C9EE6FD9
    //083863F1-70DE-11D0-BD40-00A0C911CE86}\7C069D00-777A-11D3-AEE3-00600857EED8}
  //public static readonly Guid WinTVMPEG2Writer	= new Guid( 0x083863F1,0x70DE,0x11D0,0xBD,0x40,0x00,0xA0,0xC9,0x11,0xCE,0x86);
  public static readonly Guid WinTVMPEG2Writer	= new Guid( 0x7C069D00,0x777A,0x11D3,0xAE,0xE3,0x00,0x60,0x08,0x57,0xEE,0xD8);
  //public static readonly Guid WinTVMPEG2Writer	= new Guid( 0xAA7BB8A0,0x5659,0x11D4,0x98,0xB2,0x00,0xA0,0xC9,0xEE,0x6F,0xD9);
  public static readonly Guid FileWriter	= new Guid( 0x8596e5f0, 0xda5, 0x11d0, 0xbd, 0x21, 0x0, 0xa0, 0xc9, 0x11, 0xce, 0x86);

    /// <summary> MEDIASUBTYPE_YUYV 'YUYV' </summary>
	public static readonly Guid YUYV	= new  Guid( 0x56595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71 );

		/// <summary> MEDIASUBTYPE_IYUV 'IYUV' </summary>
	public static readonly Guid IYUV	= new Guid( 0x56555949, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71 );

		/// <summary> MEDIASUBTYPE_DVSD 'DVSD' </summary>
	public static readonly Guid DVSD	= new Guid( 0x44535644, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71 );

		/// <summary> MEDIASUBTYPE_RGB1 'RGB1' </summary>
	public static readonly Guid RGB1	= new Guid( 0xe436eb78, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

		/// <summary> MEDIASUBTYPE_RGB4 'RGB4' </summary>
	public static readonly Guid RGB4	= new Guid( 0xe436eb79, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

		/// <summary> MEDIASUBTYPE_RGB8 'RGB8' </summary>
	public static readonly Guid RGB8	= new Guid( 0xe436eb7a, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

		/// <summary> MEDIASUBTYPE_RGB565 'RGB565' </summary>
	public static readonly Guid RGB565	= new Guid( 0xe436eb7b, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

		/// <summary> MEDIASUBTYPE_RGB555 'RGB555' </summary>
	public static readonly Guid RGB555	= new Guid( 0xe436eb7c, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

		/// <summary> MEDIASUBTYPE_RGB24 'RGB24' </summary>
	public static readonly Guid RGB24	= new Guid( 0xe436eb7d, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

		/// <summary> MEDIASUBTYPE_RGB32 'RGB32' </summary>
	public static readonly Guid RGB32	= new Guid( 0xe436eb7e, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );


		/// <summary> MEDIASUBTYPE_Avi </summary>
	public static readonly Guid Avi	= new Guid( 0xe436eb88, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70 );

		/// <summary> MEDIASUBTYPE_Asf </summary>
	public static readonly Guid Asf	=         new Guid( 0x3db80f90, 0x9412, 0x11d1, 0xad, 0xed, 0x0 , 0x0 , 0xf8, 0x75, 0x4b, 0x99 );
	public static readonly Guid MPEG2 = new Guid(0xe06d8026, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

 public static readonly Guid MPEG1_Audio	= new Guid( 0xe436eb87, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
 public static readonly Guid MPEG1_Video	= new Guid( 0xe436eb86, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
 public static readonly Guid MPEG1_System	= new Guid( 0xe436eb84, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
 public static readonly Guid MPEG1_VideoCD	= new Guid( 0xe436eb85, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
	public static readonly Guid MPEG1AudioPayload = new Guid(0x00000050, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);

 public static readonly Guid MPEG2_Data	= new Guid( 0xc892e55b, 0x252d, 0x42b5, 0xa3, 0x16, 0xd9, 0x97, 0xe7, 0xa5, 0xd9, 0x95);
 public static readonly Guid MPEG2_Video	= new Guid( 0xe06d8026, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);                   
 public static readonly Guid MPEG2Program	= new Guid( 0xe06d8022, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
 public static readonly Guid MPEG2Transport	= new Guid( 0xe06d8023, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
 public static readonly Guid MPEG2Transport_Stride	= new Guid( 0x138aa9a4, 0x1ee2, 0x4c5b, 0x98, 0x8e, 0x19, 0xab, 0xfd, 0xbc, 0x8a, 0x11);
 public static readonly Guid MPEG2Transport_Audio	= new Guid( 0xe06d802b, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
 public static readonly Guid MPEG2_Audio	= new Guid( 0xe06d802b, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
 public static readonly Guid DVB_SI	= new Guid( 0xe9dd31a3, 0x221d, 0x4adb, 0x85, 0x32, 0x9a, 0xf3, 0x9, 0xc1, 0xa4, 0x8);   
 public static readonly Guid ARGB32 = new Guid( 0x773c9ac0, 0x3274, 0x11d0, 0xb7, 0x24, 0x0, 0xaa, 0x0, 0x6c, 0x1a, 0x1 );   
 public static readonly Guid YUY2 = new Guid( 0x32595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
 public static readonly Guid YVYU = new Guid( 0x55595659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
 public static readonly Guid UYVY = new Guid( 0x59565955, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

public static readonly Guid None = new Guid(0xe436eb8e, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

}   


	[ComVisible(false)]
public class FormatType		// FORMAT_*
{
	
	public static readonly Guid DolbyAC3SPDIF = new Guid(0xe06d80e4, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);

		/// <summary> FORMAT_None </summary>
	public static readonly Guid None		= new Guid( 0x0F6417D6, 0xc318, 0x11d0, 0xa4, 0x3f, 0x00, 0xa0, 0xc9, 0x22, 0x31, 0x96 );

		/// <summary> FORMAT_VideoInfo </summary>
	public static readonly Guid VideoInfo	= new Guid( 0x05589f80, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a );

		/// <summary> FORMAT_VideoInfo2 </summary>
	public static readonly Guid VideoInfo2	= new Guid( 0xf72a76A0, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba );

		/// <summary> FORMAT_WaveFormatEx </summary>
	public static readonly Guid WaveEx		= new Guid( 0x05589f81, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a );

		/// <summary> FORMAT_MPEGVideo </summary>
	public static readonly Guid MpegVideo	= new Guid( 0x05589f82, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a );

	public static readonly Guid Mpeg2Video  = new Guid(0xE06D80E3, 0xDB46, 0x11CF, 0xB4, 0xD1, 0x00, 0x80, 0x005F, 0x6C, 0xBB, 0xEA);
		/// <summary> FORMAT_MPEGStreams </summary>
	public static readonly Guid MpegStreams	= new Guid( 0x05589f83, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a );

		/// <summary> FORMAT_DvInfo </summary>
	public static readonly Guid DvInfo		= new Guid( 0x05589f84, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a );
}




	[ComVisible(false)]
public class PinCategory		// PIN_CATEGORY_*
{
		/// <summary> PIN_CATEGORY_CAPTURE </summary>
	public static readonly Guid Capture		= new Guid( 0xfb6c4281, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba );

		/// <summary> PIN_CATEGORY_PREVIEW </summary>
	public static readonly Guid Preview		= new Guid( 0xfb6c4282, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba );
    
  public static readonly Guid VideoPort		= new Guid( 0xfb6c4285, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);
}

	[ComVisible(false)]
	public class FindDirection
	{
		/// <summary> LOOK_UPSTREAM_ONLY </summary>
		public static readonly Guid UpstreamOnly	= new Guid( 0xac798be0, 0x98e3, 0x11d1, 0xb3, 0xf1, 0x0, 0xaa, 0x0, 0x37, 0x61, 0xc5 );

		/// <summary> LOOK_DOWNSTREAM_ONLY </summary>
		public static readonly Guid DownstreamOnly	= new Guid( 0xac798be1, 0x98e3, 0x11d1, 0xb3, 0xf1, 0x0, 0xaa, 0x0, 0x37, 0x61, 0xc5 );
	}
	public class D3DGuids 
	{
		public static readonly Guid Texture = new Guid (0x85c31227, 0x3de5, 0x4f00, 0x9b, 0x3a, 0xf1, 0x1a, 0xc3, 0x8c, 0x18, 0xb5);
    public static readonly Guid Surface = new Guid (0xcfbaf3a , 0x9ff6, 0x429a, 0x99, 0xb3, 0xa2, 0x79, 0x6a, 0xf8, 0xb8, 0x9b);
	}


} // namespace DShowNET
