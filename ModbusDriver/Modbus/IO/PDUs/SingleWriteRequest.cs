using System;
using System.Collections.Generic;
using System.Linq;

using Modbus.Extensions;
using Modbus.IO.Interfaces;
using Modbus.Validation;
using Modbus.Enums;

namespace Modbus.IO
{
    public class SingleWriteRequest : ISingleWriteRequest, IResponse
    {
        #region Fields

        private byte _functionCode = 0x05;
        private ushort _address = 0x0000;
        private ushort _value = 0;

        #endregion

        #region Initialization

        public SingleWriteRequest(MBAPHeader mbap, byte[] data)
        {
            this.MBAP = mbap;

            InitFromBytes(data);
        }

        private void InitFromBytes(byte[] bytes)
        {
            this.FunctionCode = bytes.First();

            this.Address = BitConverter.ToUInt16(bytes.GetRange(1, sizeof(ushort))
                .ToArray()
                .ChangeEndianness(), 0); // To Little-Endian format

            this.Value = BitConverter.ToUInt16(bytes.GetRange(3, sizeof(ushort))
                .ToArray()
                .ChangeEndianness(), 0); // To Little-Endian format
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the MBAP header attached to the single write modbus request.
        /// </summary>
        public MBAPHeader MBAP { get; set; }

        /// <summary>
        /// Gets or sets the function code specified in the modbus request.
        /// </summary>
        public byte FunctionCode
        {
            get { return _functionCode; }
            set
            {
                if (Validator.IsDefined(Activator.CreateInstance<FunctionCode>(), value))
                {
                    _functionCode = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the address of the coil/register that the modbus request has 
        /// specified for its target operation.
        /// </summary>
        public ushort Address
        {
            get { return _address; }
            set
            {
                if(Validator.IsValidAddress(value))
                {
                    _address = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value to write to the modbus coil/register.
        /// </summary>
        public ushort Value
        {
            get { return _value; }
            set
            {                
                _value = value;
            }
        }

        #endregion

        #region Transportation

        /// <summary>
        /// Converts the request back into bytes to be echoed back to the client as the response.
        /// The single write response is an echo of the request.
        /// </summary>
        /// <returns></returns>
        public byte[] GetPackage()
        {
            List<byte> bytes = new List<byte>();

            bytes.Add(FunctionCode);
            bytes.AddRange(BitConverter.GetBytes(_address).ChangeEndianness());
            bytes.AddRange(BitConverter.GetBytes(_value).ChangeEndianness());

            this.MBAP.Length = (short)(bytes.Count + Validator.SizeOf(this.MBAP.UnitId));

            bytes.InsertRange(0, this.MBAP.ToBytes());

            return bytes.ToArray();
        }

        #endregion  
    }
}
