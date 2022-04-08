using Modbus.Enums;
using Modbus.Factories;
using Modbus.IO;
using Modbus.IO.Interfaces;

namespace Modbus.Tables {
    public sealed class HoldingRegistersTable : WordTable {
        /// <summary>
        /// Identifies the abstract instance as the HoldingRegisters table
        /// </summary>
        /// <returns></returns>
        public override ModbusTable GetTable() {
            return ModbusTable.HoldingRegisters;
        }

        /// <summary>
        /// Processes read/write requests sent from a Modbus master.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override IResponse ProcessRequest(IRequest request) {
            if (!(request is IReadRequest)) {
                return ProcessWriteRequest(request);
            }
            else {
                return base.ProcessRequest(request);
            }
        }

        /// <summary>
        /// Processes a write request sent from a Modbus master.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private IResponse ProcessWriteRequest(IRequest request) {
            switch ((FunctionCode)request.FunctionCode) {
                case FunctionCode.WriteSingleRegister:
                    this.WriteMultiple(new ushort[] { ((ISingleWriteRequest)request).Value }, ((ISingleWriteRequest)request).Address);
                    return request as IResponse;
                case FunctionCode.WriteMultipleRegisters:
                    this.WriteMultiple(((MultiWordWriteRequest)request).Values, ((IWriteRequest)request).StartingAddress);

                    return ResponseFactory.GetInstance(request);

                default:
                    break;
            }

            return null;
        }
    }
}
