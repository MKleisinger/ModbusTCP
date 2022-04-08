using System;
using System.Runtime.InteropServices;

namespace Modbus.Validation {
    public static class Validator {
        /// <summary>
        /// Determines if a value is defined in an enumeration.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefined(Enum e, int value) {
            return Enum.IsDefined(e.GetType(), value);
        }

        /// <summary>
        /// Returns an instance of an IConvertible data structure.
        /// 
        /// Example: IConvertible instance = Validator.GetInstance<MyCustomEnumeration>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>()
            where T : struct, IConvertible {
            return (T)Activator.CreateInstance(typeof(T));
        }

        /// <summary>
        /// Returns the size of a type in bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static int SizeOf<T>(T arg) {
            return Marshal.SizeOf(typeof(T));
        }

        /// <summary>
        /// Determines if the starting address is within a
        /// valid range.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsValidAddress(ushort address) {
            return (address >= ushort.MinValue) && (address <= ushort.MaxValue);
        }
    }
}
