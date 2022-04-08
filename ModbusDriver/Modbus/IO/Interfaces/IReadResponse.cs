namespace Modbus.IO.Interfaces {
    public interface IReadResponse : IResponse {
        /// <summary>
        /// Defines the function code specified from the read request.
        /// </summary>
        byte FunctionCode { get; set; }

        /// <summary>
        /// Gets or sets the amount of bytes following the MBAP header.
        /// </summary>
        byte ByteCount { get; set; }

        /// <summary>
        /// Determines if the byte count fits within the specified width based
        /// on the quantity of registers requested in the request.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="quantity"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        bool IsValidCount(int count, short quantity, ushort width);
    }
}
