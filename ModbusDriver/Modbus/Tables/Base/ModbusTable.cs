using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Modbus.Enums;
using Modbus.Interfaces;
using Modbus.IO;
using Modbus.IO.Interfaces;

namespace Modbus.Tables {
    public abstract class ModbusTable<T> : INotifier<AddressChangedArgs> {
        /// <summary>
        /// The hashtable storing register/value pairs
        /// </summary>
        public Dictionary<ushort, T> Table { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public abstract IResponse ProcessRequest(IRequest request);


        /// <summary>
        /// Informs a subscriber that a particular register value has been changed.
        /// </summary>
        public IObservable<AddressChangedArgs> NotificationStream {
            get { return _notificationStream.AsObservable(); }
        }

        /// <summary>
        /// Identifies instance modbus table.
        /// </summary>
        /// <returns></returns>
        public abstract ModbusTable GetTable();

        /// <summary>
        /// Retrieves the value type of the modbus table.
        /// </summary>
        /// <returns></returns>
        public abstract DataType GetValueType();

        /// <summary>
        /// Stream used to inform subscribers of address changes.
        /// </summary>
        protected Subject<AddressChangedArgs> _notificationStream = new Subject<AddressChangedArgs>();

        /// <summary>
        /// Reads a single value from a register
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public abstract ushort Read(ushort register);
    }
}
