using System;
using System.Linq;

using Modbus.Extensions;
using Modbus.IO.Interfaces;

namespace Modbus.IO
{
    public abstract class MultiWriteRequest : IRequest, IWriteRequest
    {
        public MBAPHeader MBAP { get; set; }

        public byte FunctionCode { get; set; }

        public ushort StartingAddress { get; set; }

        public ushort Quantity { get; set; }

        public byte ByteCount { get; set; }

        protected virtual void InitFromBytes(byte[] bytes)
        {
            this.FunctionCode = bytes.First();

            this.StartingAddress = BitConverter.ToUInt16(bytes.GetRange(1, sizeof(ushort))
                .ToArray()
                .ChangeEndianness(), 0); // To Little-Endian format

            this.Quantity = BitConverter.ToUInt16(bytes.GetRange(3, sizeof(ushort))
                .ToArray()
                .ChangeEndianness(), 0); // To Little-Endian format

            this.ByteCount = bytes.ElementAt(5);
        }
    }
}
