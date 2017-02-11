using Modbus.Enums;

namespace Modbus.Extensions
{
    public static class BoolStateExtensions
    {
        /// <summary>
        /// Converts a value to its equivalent BoolState (On/Off).
        /// </summary>
        /// <param name="state"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BoolState ConvertToState(this BoolState state, object value)
        {
            BoolState newState = BoolState.Off;

            if (value is ushort)
            {
                newState = value.Equals(1) || value.Equals(0xff00) ? BoolState.On : BoolState.Off;
            }
            else if (value is bool)
            {
                newState = value.Equals(true) ? BoolState.On : BoolState.Off;
            }

            return newState;
        }
    }

    public static class ModbusTableExtensions
    {
        /// <summary>
        /// Gets the ModbusTable from the enumeration based on function code.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ModbusTable GetTable(this ModbusTable table, FunctionCode code)
        {
            switch (code)
            {
                case FunctionCode.ReadCoils:
                case FunctionCode.WriteSingleCoil:
                case FunctionCode.WriteMultipleCoils:
                    return ModbusTable.Coils;
                case FunctionCode.ReadDiscreteInputs:
                    return ModbusTable.DiscreteInputs;
                case FunctionCode.ReadInputRegisters:
                    return ModbusTable.InputRegisters;
                case FunctionCode.ReadHoldingRegisters:
                case FunctionCode.WriteSingleRegister:
                case FunctionCode.WriteMultipleRegisters:
                case FunctionCode.ReadWriteMultipleRegisters:
                    return ModbusTable.HoldingRegisters;
                default:
                    return (ModbusTable)int.MaxValue;
            }
        }
    }
}
