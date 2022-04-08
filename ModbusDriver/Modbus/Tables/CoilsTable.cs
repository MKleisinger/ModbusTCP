using System.Collections;

using Modbus.IO;
using Modbus.IO.Interfaces;
using Modbus.Enums;
using Modbus.Extensions;
using Modbus.Factories;
using Modbus.Validation;

namespace Modbus.Tables {
    public sealed class CoilsTable : BooleanTable {
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
                case FunctionCode.WriteSingleCoil:
                    this.Write(((ISingleWriteRequest)request).Address, ((ISingleWriteRequest)request).Value);

                    // Echo the request as the response
                    return request as IResponse;
                case FunctionCode.WriteMultipleCoils:
                    BitArray bits = new BitArray(((MultiBitWriteRequest)request).Outputs);
                    for (int i = 0; i < ((IWriteRequest)request).Quantity; ++i) {
                        BoolState state = Validator.GetInstance<BoolState>().ConvertToState(bits[i]);
                        this.Write((ushort)(((MultiBitWriteRequest)request).StartingAddress + i), (ushort)state);
                    }

                    return ResponseFactory.GetInstance(request);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Identifies the abstract instance as the HoldingRegisters table
        /// </summary>
        /// <returns></returns>
        public override ModbusTable GetTable() {
            return ModbusTable.Coils;
        }
    }
}
