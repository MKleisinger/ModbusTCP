namespace Modbus.IO.Interfaces {
    public interface IReadRequest : IRequest {
        /// <summary>
        /// Defines the starting address of the request.
        /// </summary>
        ushort StartingAddress { get; set; }

        /// <summary>
        /// Defines the quantity of coils/registers to read.
        /// </summary>
        short Quantity { get; set; }

        /// <summary>
        /// Determines that the specified quantity is in a
        /// valid range.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        bool IsValidQuantity(short quantity);
    }
}
