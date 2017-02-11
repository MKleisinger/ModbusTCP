namespace Modbus.Enums
{
    /// <summary>
    /// Refers to a specific Modbus table.
    /// Tables are arranged in order of their prefix.
    /// Prefices: Coils: 0, DiscreteInputs: 1, InputRegisters: 3, HoldingRegisters: 4
    /// </summary>
    public enum ModbusTable
    {
        Coils = 0,
        DiscreteInputs,
        InputRegisters,
        HoldingRegisters
    }
}
