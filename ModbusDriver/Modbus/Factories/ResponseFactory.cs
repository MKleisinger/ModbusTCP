using Modbus.Enums;
using Modbus.IO;
using Modbus.IO.Interfaces;

namespace Modbus.Factories {
    public static class ResponseFactory {
        /// <summary>
        /// Generates an instance of a Modbus response based on the function
        /// code defined in the request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IResponse GetInstance(IRequest request) {
            FunctionCode funcCode = (FunctionCode)request.FunctionCode;

            switch (funcCode) {
                case FunctionCode.ReadCoils:
                case FunctionCode.ReadDiscreteInputs:
                    return new BitReadResponse(request as IReadRequest);
                case FunctionCode.ReadHoldingRegisters:
                case FunctionCode.ReadInputRegisters:
                    return new WordReadResponse(request as IReadRequest);
                case FunctionCode.WriteMultipleCoils:
                case FunctionCode.WriteMultipleRegisters:
                    return new MultiWriteResponse(request as MultiWriteRequest);
                default:
                    return null;
            }
        }
    }
}
