using System;
using System.Collections.Generic;

using Modbus.Extensions;
using Modbus.IO.Interfaces;
using Modbus.Validation;

namespace Modbus.IO
{
    public class MultiWriteResponse : IResponse
    {
        #region Fields

        private MBAPHeader _mbapCopy = null;

        #endregion

        #region Public Properties

        public byte FunctionCode { get; set; }
        public ushort StartingAddress { get; set; }
        public ushort Quantity { get; set; }

        #endregion

        #region Constructor(s)

        public MultiWriteResponse(IWriteRequest request)
        {
            _mbapCopy = request.MBAP;

            this.FunctionCode = request.FunctionCode;
            this.StartingAddress = request.StartingAddress;
            this.Quantity = request.Quantity;
        }

        #endregion

        #region Transportation

        public byte[] GetPackage()
        {
            List<byte> bytes = new List<byte>();

            bytes.Add(this.FunctionCode);
            bytes.AddRange(BitConverter.GetBytes(this.StartingAddress).ChangeEndianness()); // To Big-Endian format
            bytes.AddRange(BitConverter.GetBytes(this.Quantity).ChangeEndianness());        // To Big-Endian format

            _mbapCopy.Length = (short)(bytes.Count + Validator.SizeOf(_mbapCopy.UnitId));

            bytes.InsertRange(0, _mbapCopy.ToBytes());

            return bytes.ToArray();
        }

        #endregion
    }
}
