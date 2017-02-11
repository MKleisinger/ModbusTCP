using System;
using System.Collections;

using Modbus.Enums;
using Modbus.Factories;
using Modbus.IO;
using Modbus.IO.Interfaces;

namespace Modbus.Tables
{
    public abstract class BooleanTable : ModbusTable<bool>
    {
        #region Initialization

        public BooleanTable()
        {
            Init();
        }

        private void Init()
        {
            this.Table = TableFactory.Create<bool>();
        }

        #endregion

        #region Access

        public override ushort Read(ushort register)
        {
            return (ushort)(this[register] ? 1 : 0);
        }

        public virtual void Write(ushort register, ushort value)
        {
            if (IsValidValue((BoolState)value))
            {
                bool proposedValue = Convert.ToBoolean(value);

                if (this.Table[register] != proposedValue)
                {
                    this.Table[register] = Convert.ToBoolean(value);

                    _notificationStream.OnNext(new AddressChangedArgs(this.GetTable(), register, value));
                }
            }            
        }

        private bool this[ushort key]
        {          
            get { return this.Table[key]; }
            set { this.Table[key] = value; }
        }

        #endregion  

        #region Processing        

        public override IResponse ProcessRequest(IRequest request)
        {
            if (request is IReadRequest)
            {
                return ProcessReadRequest(request as IReadRequest);
            }

            return null;
        }

        private IResponse ProcessReadRequest(IReadRequest readRequest)
        {
            // Check for read request/Process it
            switch ((FunctionCode)readRequest.FunctionCode)
            {
                case FunctionCode.ReadCoils:
                case FunctionCode.ReadDiscreteInputs:
                    // Process the read request
                    var response = ResponseFactory.GetInstance(readRequest);

                    if(response is BitReadResponse)
                    {
                        BitArray bits = new BitArray(((IBitResponse)response).Status);
                        for(int i = 0; i < readRequest.Quantity; ++i)
                        {
                            bits.Set(i, this.Table[(ushort)(readRequest.StartingAddress + i)]);
                        }

                        // copy bits back to the byte array
                        bits.CopyTo(((IBitResponse)response).Status, 0); 
                    }

                    return response;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Determines if the arguement is a valid value for the table.
        /// 
        /// There are only two valid states On and Off.
        /// Modbus defines the values of the On/Off state as 0xFF00 equals ON
        /// state and 0x00000 equals OFF state.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsValidValue(BoolState value)
        {
            return (value.Equals(BoolState.On) || value.Equals(BoolState.Off));            
        }

        public override DataType GetValueType()
        {
            return DataType.Bool;
        }

        #endregion

    }
}
