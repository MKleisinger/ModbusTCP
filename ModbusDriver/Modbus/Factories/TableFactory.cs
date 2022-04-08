using System;
using System.Collections.Generic;

namespace Modbus.Factories {
    public static class TableFactory {
        /// <summary>
        /// Creates a Modbus table based on the specified value type.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Dictionary<ushort, TValue> Create<TValue>()
            where TValue : struct, IConvertible {
            Dictionary<ushort, TValue> table = new Dictionary<ushort, TValue>();

            for (ushort i = 0; i < ushort.MaxValue; ++i) {
                table.Add(i, default(TValue));
            }

            return table;
        }
    }
}
