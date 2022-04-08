using System;
using System.Collections.Generic;
using System.Linq;

using Modbus.Extensions;

namespace Modbus.IO {
    public class MBAPHeader {
        #region Read-only/Constant fields

        /// <summary>
        /// Total bytes of the MBAP Header
        /// </summary>
        public static readonly int TotalBytes = 7;

        #endregion  

        #region Initialization

        public MBAPHeader(byte[] mbap) {
            Init(mbap);
        }

        private void Init(byte[] bytes) {
            this.TransactionId = GetShort(bytes, 0);

            this.ProtocolId = GetShort(bytes, 2);

            this.Length = GetShort(bytes, 4);

            this.UnitId = bytes[bytes.Length - 1];
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the transaction identifier for the modbus packet.
        /// </summary>
        public short TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the protocol identifier.  
        /// Zero equals the Modbus protocol.
        /// </summary>
        public short ProtocolId { get; set; }

        /// <summary>
        /// Gets or sets the length of the following bytes.  This includes
        /// the byte for the UnitId.
        /// </summary>
        public short Length { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.  This essentially carries slave
        /// information defined in the serial version of the protocol.
        /// </summary>
        public byte UnitId { get; set; }

        #endregion

        #region Helpers

        /// <summary>
        /// Converts the MBAP header object to a transmittable array of bytes.
        /// Bytes are already in the correct order and containing fields have been 
        /// converted to be in Big-Endian format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(BitConverter.GetBytes(this.TransactionId).ChangeEndianness());   // To Big-Endian format
            bytes.AddRange(BitConverter.GetBytes(this.ProtocolId).ChangeEndianness());      // To Big-Endian format
            bytes.AddRange(BitConverter.GetBytes(this.Length).ChangeEndianness());          // To Big-Endian format
            bytes.Add(this.UnitId);

            return bytes.ToArray();
        }

        /// <summary>
        /// Retrieves a short from a byte array by its starting index.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private short GetShort(byte[] bytes, int startIndex) {
            return BitConverter.ToInt16(bytes.GetRange(startIndex, sizeof(short))
                .ToArray()
                .ChangeEndianness(), 0); // To Little-Endian format
        }

        #endregion
    }
}
