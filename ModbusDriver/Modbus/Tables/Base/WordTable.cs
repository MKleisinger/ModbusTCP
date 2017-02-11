using System.Linq;

using Modbus.Enums;
using Modbus.Factories;
using Modbus.IO.Interfaces;
using Modbus.IO;

namespace Modbus.Tables
{
    public abstract class WordTable : ModbusTable<ushort>
    {
        #region Initialization

        public WordTable()
        {
            Init();
        }

        private void Init()
        {
            this.Table = TableFactory.Create<ushort>();
        }

        #endregion

        #region Access

        public override ushort Read(ushort register)
        {
            return this[register];
        }

        public virtual void Write(ushort register, ushort value)
        {
            if (this.Table[register] != value)
            {
                this.Table[register] = value;
            }
        }

        public void WriteMultiple(ushort[] values, ushort startAddress)
        {
            for (int i = 0; i < values.Count(); ++i)
            {
                this.Write((ushort)(startAddress + i), values[i]);
            }

            for (int i = 0; i < values.Count(); ++i)
            {
                _notificationStream.OnNext
                (
                    new AddressChangedArgs(this.GetTable(), (ushort)(startAddress + i), this.Read((ushort)(startAddress + i)))
                );
            }
        }

        private ushort this[ushort register]
        {
            get { return this.Table[register]; }
            set { this.Table[register] = value; }
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
                case FunctionCode.ReadHoldingRegisters:
                case FunctionCode.ReadInputRegisters:
                    // Process the read request
                    var response = ResponseFactory.GetInstance(readRequest);

                    if (response is WordReadResponse)
                    {
                        for (int i = 0; i < readRequest.Quantity; ++i)
                        {
                            ((IWordResponse)response).Values[i] = this.Table[(ushort)(readRequest.StartingAddress + i)];
                        }
                    }

                    return response;                    
                default:
                    return null;
            }
        }

        public override DataType GetValueType()
        {
            return DataType.UShort;
        }

        #endregion  
    }
}
