namespace Modbus.IO.Interfaces
{
    public interface IWriteRequest : IRequest
    {
        /// <summary>
        /// Defines the starting address of the request.
        /// </summary>
        ushort StartingAddress { get; set; }

        /// <summary>
        /// Defines the quantity of coils/registers to read.
        /// </summary>
        ushort Quantity { get; set; }

        /// <summary>
        /// Gets or sets the amount of bytes following the MBAP header.
        /// </summary>
        byte ByteCount { get; set; }
    }
}
