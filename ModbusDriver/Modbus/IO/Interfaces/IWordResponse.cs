namespace Modbus.IO.Interfaces
{
    public interface IWordResponse : IReadResponse
    {
        /// <summary>
        /// Gets the values of the registers requested from the read request.
        /// </summary>
        ushort[] Values { get; }
    }
}
