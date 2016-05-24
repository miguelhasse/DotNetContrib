using System;
using System.IO;

namespace Hasseware.IO
{
	public class BigEndianBinaryReader : BinaryReader
	{
		public BigEndianBinaryReader(Stream input) : base(input) { }

		public BigEndianBinaryReader(Stream input, System.Text.Encoding encoding) : base(input, encoding) { }

		public override short ReadInt16()
		{
			return BitConverter.ToInt16(ReverseRead(2), 0);
		}

		public override ushort ReadUInt16()
		{
			return BitConverter.ToUInt16(ReverseRead(2), 0);
		}

		public override int ReadInt32()
		{
			return BitConverter.ToInt32(ReverseRead(4), 0);
		}

		public override uint ReadUInt32()
		{
			return BitConverter.ToUInt32(ReverseRead(4), 0);
		}

		public override long ReadInt64()
		{
			return BitConverter.ToInt64(ReverseRead(8), 0);
		}

		public override ulong ReadUInt64()
		{
			return BitConverter.ToUInt64(ReverseRead(8), 0);
		}

		public Guid ReadGuid()
		{
			return new Guid(ReverseRead(16));
		}

		private byte[] ReverseRead(int length)
		{
			byte[] bytes = new byte[length];
			for (int i = bytes.Length - 1; i >= 0; i--)
				bytes[i] = ReadByte();
			return bytes;
		}
	}
}
