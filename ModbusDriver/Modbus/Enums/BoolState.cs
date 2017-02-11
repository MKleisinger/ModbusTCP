namespace Modbus.Enums
{
    /// <summary>
    /// The On/Off state of a Modbus coil. 
    /// The Modbus specification defines ON state as equaling 0xFF00
    /// and OFF state as 0x0000
    /// </summary>
    public enum BoolState
    {
        On = 0xFF00,
        Off = 0x0000
    }
}
