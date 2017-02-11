namespace Modbus.IO.Interfaces
{
    public interface IBitResponse : IReadResponse
    {
        /// <summary>
        /// Stores the values that indicate On/Off for
        /// the specified coils/inputs requested in the query.
        /// 
        /// *****************************************************
        /// * The LSB of the first data byte contains the input *
        /// * addressed in the query.  The other inputs follow  *
        /// * toward the high order end of this byte, and from  *
        /// * low order to high order in subsequent bytes.      *
        /// *****************************************************
        /// </summary>
        byte[] Status { get; }
    }
}
