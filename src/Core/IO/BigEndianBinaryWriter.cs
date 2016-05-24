using System;
using System.IO;

namespace Hasseware.IO
{
	public class BigEndianBinaryWriter : BinaryWriter
	{
		public BigEndianBinaryWriter(Stream stream) : base(stream) { }

		public BigEndianBinaryWriter(Stream stream, System.Text.Encoding encoding) : base(stream, encoding) { }

		public override void Write(short value)
		{
			ReverseWrite(BitConverter.GetBytes(value));
		}

		public override void Write(ushort value)
		{
			ReverseWrite(BitConverter.GetBytes(value));
		}

		public override void Write(int value)
		{
			ReverseWrite(BitConverter.GetBytes(value));
		}

		public override void Write(uint value)
		{
			ReverseWrite(BitConverter.GetBytes(value));
		}

		public override void Write(long value)
		{
			ReverseWrite(BitConverter.GetBytes(value));
		}

		public override void Write(ulong value)
		{
			ReverseWrite(BitConverter.GetBytes(value));
		}

		public void Write(Guid value)
		{
			ReverseWrite(value.ToByteArray());
		}

		private void ReverseWrite(byte[] bytes)
		{
			for (int i = bytes.Length - 1; i >= 0; i--)
				Write(bytes[i]);
		}
	}
}