using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modbus.Enums;
using Modbus.IO.Interfaces;
using Modbus.Validation;
using Modbus.Extensions;

namespace Modbus.IO {
    public class ReadRequest : IReadRequest {

        #region Fields

        private byte _functionCode = 0x03;
        private ushort _startingAddress = 0x0000;
        private short _quantity = 0;

        #endregion

        #region Constructor(s)

        public ReadRequest() { }

        public ReadRequest(MBAPHeader mbap, byte funcCode, ushort startAddr, short quantity) {
            this.FunctionCode = funcCode;
            this.StartingAddress = startAddr;
            this.Quantity = quantity;
            this.MBAP = mbap;
        }

        public ReadRequest(MBAPHeader mbap, byte[] requestBytes) {
            this.MBAP = mbap;
            InitFromBytes(requestBytes);
        }

        #endregion

        #region Initialization

        private void InitFromBytes(byte[] bytes) {
            this.FunctionCode = bytes.First();


            this.StartingAddress = BitConverter.ToUInt16(bytes.GetRange(1, sizeof(ushort))
                .ToArray()
                .ChangeEndianness(), 0); // To Little-Endian format

            this.Quantity = BitConverter.ToInt16(bytes.GetRange(3, sizeof(short))
                .ToArray()
                .ChangeEndianness(), 0); // To Little-Endian format
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the MBAP header of the Modbus request.
        /// </summary>
        public MBAPHeader MBAP { get; set; }

        /// <summary>
        /// Gets or sets the descirption of the requested
        /// operation to be performed by the server.
        /// </summary>
        public byte FunctionCode {
            get { return _functionCode; }
            set {
                if (Validator.IsDefined(Activator.CreateInstance<FunctionCode>(), value)) {
                    _functionCode = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the starting address for the 
        /// operation requested.
        /// 
        /// Valid Range is 0x0000 to 0XFFFF
        /// </summary>
        public ushort StartingAddress {
            get { return _startingAddress; }
            set {
                if (Validator.IsValidAddress(value)) {
                    _startingAddress = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of registers for the 
        /// specified operation to perform.
        /// </summary>
        public short Quantity {
            get { return _quantity; }
            set {
                if (IsValidQuantity(value)) {
                    _quantity = value;
                }
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines if the quantity requested is within the
        /// valid range.  
        /// 
        /// This method is overridable, derived classes may need
        /// to specify a more precise range based on the operation type.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public virtual bool IsValidQuantity(short quantity) {
            return !(quantity < 0 || quantity > short.MaxValue);
        }


        #endregion
    }
}
