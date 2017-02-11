﻿namespace Modbus.Enums
{
    /// <summary>
    /// Available exception codes as defined in the Modbus specification.
    /// </summary>
    public enum ExceptionCode
    {
        IllegalFunction = 0x01,
        IllegalDataAddress = 0x02,
        IllegalDataValue = 0x03,
        ServerDeviceFailure = 0x04,
        Acknowledge = 0x05,
        ServerDeviceBusy = 0x06,
        MemoryParityError = 0x08,
        GatewayPathUnavailable = 0x0A,
        GatewayFailedToRespond = 0x0B
    }
}
