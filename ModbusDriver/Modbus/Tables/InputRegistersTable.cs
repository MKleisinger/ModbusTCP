using Modbus.Enums;

namespace Modbus.Tables {
    public sealed class InputRegistersTable : WordTable {
        public override ModbusTable GetTable() {
            return ModbusTable.InputRegisters;
        }
    }
}
