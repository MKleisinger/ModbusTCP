namespace Modbus.IO.Interfaces {
    public interface IRequest {
        /// <summary>
        /// Gets or sets the MBAP header of the Modbus request.
        /// </summary>
        MBAPHeader MBAP { get; set; }

        /// <summary>
        /// Gets or sets the descirption of the requested
        /// operation to be performed by the server.
        /// </summary>
        byte FunctionCode { get; set; }
    }
}
