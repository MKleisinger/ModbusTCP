using Modbus.Enums;

namespace Modbus.Tables {
    public sealed class DiscreteInputsTable : BooleanTable {
        public override ModbusTable GetTable() {
            return ModbusTable.DiscreteInputs;
        }
    }
}
