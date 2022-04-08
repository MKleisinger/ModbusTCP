using Modbus.Enums;

namespace Modbus.IO {
    public class AddressChangedArgs {
        #region Constructor(s)

        public AddressChangedArgs(ModbusTable table, ushort address, object value) {
            this.ModbusTable = table;
            this.Address = address;
            this.Value = value;
        }

        public AddressChangedArgs(ModbusTable table, ushort address, object value, DataType dataType) {
            this.ModbusTable = table;
            this.Address = address;
            this.Value = value;
            this.DataType = dataType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the modbus table of the coil/register address that has changed.
        /// </summary>
        public ModbusTable ModbusTable { get; set; }

        /// <summary>
        /// Gets or sets the coil/register address.  
        /// Identifies which address value has changed.
        /// </summary>
        public ushort Address { get; set; }

        /// <summary>
        /// Gets or sets the value that was written to the register.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the data type of the data item that corresponds with the changed event
        /// </summary>
        public DataType DataType { get; set; }

        #endregion
    }
}
