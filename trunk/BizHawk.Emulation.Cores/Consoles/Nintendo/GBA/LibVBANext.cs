﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Nintendo.GBA
{
	public class LibVBANext
	{
		const string dllname = "libvbanext.dll";
		const CallingConvention cc = CallingConvention.Cdecl;

		[Flags]
		public enum Buttons : int
		{
			A = 1,
			B = 2,
			Select = 4,
			Start = 8,
			Right = 16,
			Left = 32,
			Up = 64,
			Down = 128,
			R = 256,
			L = 512
		}

		[StructLayout(LayoutKind.Sequential)]
		public class FrontEndSettings
		{
			public enum SaveType : int
			{
				auto = 0,
				eeprom = 1,
				sram = 2,
				flash = 3,
				eeprom_sensor = 4,
				none = 5
			}
			public enum FlashSize : int
			{
				small = 0x10000,
				big = 0x20000
			}
			public SaveType saveType;
			public FlashSize flashSize = FlashSize.big;
			public bool enableRtc;
			public bool mirroringEnable;
			public bool skipBios;

			public bool RTCUseRealTime = true;
			public int RTCyear; // 00..99
			public int RTCmonth; // 00..11
			public int RTCmday; // 01..31
			public int RTCwday; // 00..06
			public int RTChour; // 00..23
			public int RTCmin; // 00..59
			public int RTCsec; // 00..59
		}

		/// <summary>
		/// create a new context
		/// </summary>
		/// <returns></returns>
		[DllImport(dllname, CallingConvention = cc)]
		public static extern IntPtr Create();

		/// <summary>
		/// destroy a context
		/// </summary>
		/// <param name="g"></param>
		[DllImport(dllname, CallingConvention = cc)]
		public static extern void Destroy(IntPtr g);

		/// <summary>
		/// load a rom
		/// </summary>
		/// <param name="g"></param>
		/// <param name="romfile"></param>
		/// <param name="romfilelen"></param>
		/// <param name="biosfile"></param>
		/// <param name="biosfilelen"></param>
		/// <returns>success</returns>
		[DllImport(dllname, CallingConvention = cc)]
		public static extern bool LoadRom(IntPtr g, byte[] romfile, uint romfilelen, byte[] biosfile, uint biosfilelen, [In]FrontEndSettings settings);

		/// <summary>
		/// hard reset
		/// </summary>
		/// <param name="g"></param>
		[DllImport(dllname, CallingConvention = cc)]
		public static extern void Reset(IntPtr g);
		
		/// <summary>
		/// frame advance
		/// </summary>
		/// <param name="g"></param>
		/// <param name="input"></param>
		/// <param name="videobuffer">240x160 packed argb32</param>
		/// <param name="audiobuffer">buffer to recieve stereo audio</param>
		/// <param name="numsamp">number of samples created</param>
		/// <returns>true if lagged</returns>
		[DllImport(dllname, CallingConvention = cc)]
		public static extern bool FrameAdvance(IntPtr g, Buttons input, int[] videobuffer, short[] audiobuffer, out int numsamp);

		[DllImport(dllname, CallingConvention = cc)]
		public static extern int BinStateSize(IntPtr g);
		[DllImport(dllname, CallingConvention = cc)]
		public static extern bool BinStateSave(IntPtr g, byte[] data, int length);
		[DllImport(dllname, CallingConvention = cc)]
		public static extern bool BinStateLoad(IntPtr g, byte[] data, int length);
		[DllImport(dllname, CallingConvention = cc)]
		public static extern void TxtStateSave(IntPtr g, [In]ref TextStateFPtrs ff);
		[DllImport(dllname, CallingConvention = cc)]
		public static extern void TxtStateLoad(IntPtr g, [In]ref TextStateFPtrs ff);

		[DllImport(dllname, CallingConvention = cc)]
		public static extern int SaveRamSize(IntPtr g);
		[DllImport(dllname, CallingConvention = cc)]
		public static extern bool SaveRamSave(IntPtr g, byte[] data, int length);
		[DllImport(dllname, CallingConvention = cc)]
		public static extern bool SaveRamLoad(IntPtr g, byte[] data, int length);
		[DllImport(dllname, CallingConvention = cc)]
		public static extern void GetMemoryAreas(IntPtr g, [Out]MemoryAreas mem);

		[DllImport(dllname, CallingConvention = cc)]
		public static extern void SystemBusWrite(IntPtr g, int addr, byte val);
		[DllImport(dllname, CallingConvention = cc)]
		public static extern byte SystemBusRead(IntPtr g, int addr);

		[UnmanagedFunctionPointer(cc)]
		public delegate void StandardCallback();

		[DllImport(dllname, CallingConvention = cc)]
		public static extern void SetScanlineCallback(IntPtr g, StandardCallback cb, int scanline);


		[StructLayout(LayoutKind.Sequential)]
		public class MemoryAreas
		{
			public IntPtr bios;
			public IntPtr iwram;
			public IntPtr ewram;
			public IntPtr palram;
			public IntPtr vram;
			public IntPtr oam;
			public IntPtr rom;
			public IntPtr mmio;
		}
	}
}
