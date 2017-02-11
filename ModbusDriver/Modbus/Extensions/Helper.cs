using Modbus.Enums;
using System;
using System.Collections.Generic;

namespace Modbus.Extensions
{
    public static class Helper
    {
        /// <summary>
        /// Converts a 16-bit word (ushort) to bytes.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static IEnumerable<byte> WordToBytes(ushort word)
        {
            List<byte> wordBytes = new List<byte>();

            wordBytes.Add((byte)(word & 0xff));
            wordBytes.Add((byte)(word >> 8));

            return wordBytes;
        }

        /// <summary>
        /// Converts a floating point value to unsigned shorts.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<ushort> FloatToUShorts(float f)
        {
            List<ushort> unsignedShorts = new List<ushort>();
            byte[] bytes = BitConverter.GetBytes(f);

            unsignedShorts.Add(BitConverter.ToUInt16(bytes, 2));
            unsignedShorts.Add(BitConverter.ToUInt16(bytes, 0));

            return unsignedShorts;
        }

        /// <summary>
        /// Determines if two values are equal.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        /// <returns></returns>
        public static bool ArgeEqual(object newValue, object oldValue)
        {
            if (newValue == null || oldValue == null)
                return false;

            if (newValue is bool)
            {
                return ((bool)newValue).Equals(Convert.ToBoolean(oldValue));
            }
            else if (newValue is ushort)
            {
                return ((ushort)newValue).Equals(Convert.ToUInt16(oldValue));
            }
            else if (newValue is float)
            {
                return ((float)newValue).Equals(Convert.ToSingle(oldValue));
            }
            else if (newValue is double)
            {
                return ((double)newValue).Equals(Convert.ToDouble(oldValue));
            }
            else if (newValue is string)
            {
                return ((string)newValue).Equals(Convert.ToString(oldValue));
            }
            else
            {
                // default to reference equality
                return newValue.Equals(oldValue);
            }
        }
    }
}
