﻿using System;
using System.IO;

namespace BizHawk.Common.IOExtensions
{
	public static class IOExtensions
	{
		public static void CopyTo(this Stream src, Stream dest)
		{
			int size = (src.CanSeek) ? Math.Min((int)(src.Length - src.Position), 0x2000) : 0x2000;
			byte[] buffer = new byte[size];
			int n;
			do
			{
				n = src.Read(buffer, 0, buffer.Length);
				dest.Write(buffer, 0, n);
			} while (n != 0);
		}

		public static void CopyTo(this MemoryStream src, Stream dest)
		{
			dest.Write(src.GetBuffer(), (int)src.Position, (int)(src.Length - src.Position));
		}

		public static void CopyTo(this Stream src, MemoryStream dest)
		{
			if (src.CanSeek)
			{
				int pos = (int)dest.Position;
				int length = (int)(src.Length - src.Position) + pos;
				dest.SetLength(length);

				while (pos < length)
				{
					pos += src.Read(dest.GetBuffer(), pos, length - pos);
				}
			}
			else
			{
				src.CopyTo(dest);
			}
		}

		public static void Write(this BinaryWriter bw, int[] buffer)
		{
			foreach (int b in buffer)
			{
				bw.Write(b);
			}
		}

		public static void Write(this BinaryWriter bw, uint[] buffer)
		{
			foreach (uint b in buffer)
			{
				bw.Write(b);
			}
		}

		public static void Write(this BinaryWriter bw, short[] buffer)
		{
			foreach (short b in buffer)
			{
				bw.Write(b);
			}
		}

		public static void Write(this BinaryWriter bw, ushort[] buffer)
		{
			foreach (ushort t in buffer)
			{
				bw.Write(t);
			}
		}

		public static int[] ReadInt32s(this BinaryReader br, int num)
		{
			int[] ret = new int[num];
			for (int i = 0; i < num; i++)
			{
				ret[i] = br.ReadInt32();
			}

			return ret;
		}

		public static short[] ReadInt16s(this BinaryReader br, int num)
		{
			short[] ret = new short[num];
			for (int i = 0; i < num; i++)
			{
				ret[i] = br.ReadInt16();
			}

			return ret;
		}

		public static ushort[] ReadUInt16s(this BinaryReader br, int num)
		{
			ushort[] ret = new ushort[num];
			for (int i = 0; i < num; i++)
			{
				ret[i] = br.ReadUInt16();
			}

			return ret;
		}

		public static void WriteBit(this BinaryWriter bw, Bit bit)
		{
			bw.Write((bool)bit);
		}

		public static Bit ReadBit(this BinaryReader br)
		{
			return br.ReadBoolean();
		}
	}
}
