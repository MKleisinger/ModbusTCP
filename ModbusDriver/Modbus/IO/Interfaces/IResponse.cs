namespace Modbus.IO.Interfaces
{
    public interface IResponse
    {
        /// <summary>
        /// Converts the Modbus response to a byte array that can be
        /// sent to the client.  Data fields must be encoded in 
        /// Big-Endian format.
        /// </summary>
        /// <returns></returns>
        byte[] GetPackage();
    }
}
