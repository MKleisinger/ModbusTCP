using System;

using Modbus.Enums;
using Modbus.IO.Interfaces;
using Modbus.Validation;

namespace Modbus.IO
{
    public abstract class ReadResponse : IReadResponse
    {
        #region Fields

        IReadRequest _request;
        private byte _functionCode = 0x01;
        private byte _byteCount = 0x00;

        #endregion

        #region Constructor(s)

        public ReadResponse(IReadRequest request)
        {
            _request = request;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the function code that was specified in the modbus request.
        /// </summary>
        public byte FunctionCode
        {
            get { return _functionCode; }
            set
            {
                if(Validator.IsDefined(Activator.CreateInstance<FunctionCode>(), value))
                {
                    _functionCode = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of bytes following the MBAP header.
        /// </summary>
        public byte ByteCount
        {
            get { return _byteCount; }
            set { _byteCount = value; }            
        }

        #endregion

        #region Virtual/Abstract Methods

        /// <summary>
        /// Determines if the byte count fits within the specified width based
        /// on the quantity of registers requested in the request.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="quantity"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public virtual bool IsValidCount(int count, short quantity, ushort width)
        {
            return count.Equals(quantity * width);
        }

        /// <summary>
        /// Converts the response to bytes that can be sent to the client.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] GetPackage();

        #endregion
    }
}
