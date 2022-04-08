using System;
using System.Collections.Generic;

using Modbus.IO.Interfaces;
using Modbus.Validation;
using Modbus.Extensions;

namespace Modbus.IO {
    public class WordReadResponse : ReadResponse, IWordResponse {
        #region Fields

        private MBAPHeader _mbapCopy = null;
        private ushort[] _values;

        #endregion

        #region Constructor(s)

        public WordReadResponse(IReadRequest request)
            : base(request) {
            _values = new ushort[request.Quantity];

            this.FunctionCode = request.FunctionCode;

            this.ByteCount = (byte)(_values.Length * sizeof(ushort));

            _mbapCopy = request.MBAP;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the values of the registers requested from the read request.
        /// </summary>
        public ushort[] Values {
            get { return _values; }
        }

        #endregion

        #region Transportation

        /// <summary>
        /// Converts the response into bytes.
        /// This is the response package sent to the client.
        /// </summary>
        /// <returns></returns>
        public override byte[] GetPackage() {
            List<byte> bytes = new List<byte>();

            bytes.Add(FunctionCode);
            bytes.Add(ByteCount);
            foreach (ushort val in this.Values) {
                bytes.AddRange(BitConverter.GetBytes(val).ChangeEndianness());
            }

            _mbapCopy.Length = (short)(bytes.Count + Validator.SizeOf(_mbapCopy.UnitId));

            bytes.InsertRange(0, _mbapCopy.ToBytes());

            return bytes.ToArray();
        }

        #endregion
    }
}
