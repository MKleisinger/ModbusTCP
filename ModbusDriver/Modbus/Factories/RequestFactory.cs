using System.Linq;

using Modbus.IO;
using Modbus.IO.Interfaces;
using Modbus.Enums;

namespace Modbus.Factories
{
    public static class RequestFactory
    {
        /// <summary>
        /// Generates an instance of the Modbus request based on the
        /// defined function code in the data bytes.
        /// </summary>
        /// <param name="mbap"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IRequest GetInstance(MBAPHeader mbap, byte[] data)
        {
            FunctionCode funcCode = (FunctionCode)data.First();
            
            switch(funcCode)
            {
                case FunctionCode.ReadCoils:
                case FunctionCode.ReadDiscreteInputs:
                case FunctionCode.ReadHoldingRegisters:
                case FunctionCode.ReadInputRegisters:
                    return new ReadRequest(mbap, data);
                case FunctionCode.WriteSingleCoil:
                case FunctionCode.WriteSingleRegister:
                    return new SingleWriteRequest(mbap, data);
                case FunctionCode.WriteMultipleRegisters:
                    return new MultiWordWriteRequest(mbap, data);
                case FunctionCode.WriteMultipleCoils:
                    return new MultiBitWriteRequest(mbap, data);
                default:
                    return null;
            } 
        }
    }
}
