using System.Collections.Generic;

using Modbus.IO.Interfaces;
using Modbus.Validation;

namespace Modbus.IO
{
    public class BitReadResponse : ReadResponse, IBitResponse
    {
        #region Constants

        private const int BitsInByte = 8;

        #endregion

        #region Fields

        private byte[] _status;

        private MBAPHeader _mbapCopy = null;

        #endregion

        #region Constructor(s)

        public BitReadResponse(IReadRequest request)
            : base(request)
        {
            _status = new byte[GetInitialStatusLength(request.Quantity)];

            this.FunctionCode = request.FunctionCode;
            this.ByteCount = (byte)_status.Length;

            _mbapCopy = request.MBAP;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Stores the values that indicate On/Off for
        /// the specified coils/inputs requested in the query.
        /// 
        /// *****************************************************
        /// * The LSB of the first data byte contains the input *
        /// * addressed in the query.  The other inputs follow  *
        /// * toward the high order end of this byte, and from  *
        /// * low order to high order in subsequent bytes.      *
        /// *****************************************************
        /// </summary>
        public byte[] Status
        {
            get { return _status; }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Determines the length of the Status array when initialized.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        private int GetInitialStatusLength(int quantity)
        {          
            while(quantity % BitsInByte != 0)
            {
                quantity += 1;
            }            

            return quantity / BitsInByte;
        }

        #endregion

        #region Transportation

        /// <summary>
        /// Converts the response as transferable bytes that can be sent to the client.
        /// </summary>
        /// <returns></returns>
        public override byte[] GetPackage()
        {
            List<byte> bytes = new List<byte>();

            bytes.Add(FunctionCode);
            bytes.Add(ByteCount);

            bytes.AddRange(this.Status);

            _mbapCopy.Length = (short)(bytes.Count + Validator.SizeOf(_mbapCopy.UnitId));

            bytes.InsertRange(0, _mbapCopy.ToBytes());

            return bytes.ToArray();
        }

        #endregion
    }
}
