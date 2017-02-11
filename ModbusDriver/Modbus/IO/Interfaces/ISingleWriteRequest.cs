namespace Modbus.IO.Interfaces
{
    interface ISingleWriteRequest : IRequest
    {
        /// <summary>
        /// Specifies the address of the coil/register to write to.
        /// </summary>
        ushort Address { get; set; }

        /// <summary>
        /// Specifies the value of the coil/register that will be written.
        /// </summary>
        ushort Value { get; set; }
    }
}
