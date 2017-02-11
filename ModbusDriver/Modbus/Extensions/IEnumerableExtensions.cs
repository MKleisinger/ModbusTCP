using System;
using System.Collections.Generic;
using System.Linq;

namespace Modbus.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Retrieves a specified range of elements from an array.
        /// Throws ArgumentOutOfRangeException when start index or length
        /// extend beyond the bounds of the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetRange<T>(this T[] array, int startIndex, int length)
        {
            if (array.Length < (startIndex + length))
            {
                throw new ArgumentOutOfRangeException(string.Format("Length: {0} is out of bounds of the collection", length));
            }

            for (int i = 0; i < length; ++i)
            {
                yield return array[startIndex + i];
            }
        }

        /// <summary>
        /// Retrieves a range of elements from an enumerable object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetRange<T>(this IEnumerable<T> enumerable, int startIndex, int length)
        {
            if (enumerable.Count() < (startIndex + length))
            {
                throw new ArgumentOutOfRangeException(string.Format("Length: {0} is out of bounds of the collection", length));
            }

            for (int i = 0; i < length; ++i)
            {
                yield return enumerable.ToArray()[startIndex + i];
            }
        }

        /// <summary>
        /// Reverses the byte order of the array if dealing with a Little-Endian processor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] ChangeEndianness<T>(this T[] array)
        {
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }

            return array;
        }

        /// <summary>
        /// Takes all contiguous elements from the specified starting index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeAllStartingAt<T>(this T[] array, int startIndex)
        {
            for(int i = startIndex; i < array.Length; ++i)
            {
                yield return array[i];
            }
        }

        /// <summary>
        /// Converts an array of bytes to an array of 16-bit
        /// unsigned values containing the same data sequence.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static IEnumerable<ushort> ToUnsignedShorts(this IEnumerable<byte> bytes)
        {
            for (int i = 0; (i + 1) < bytes.Count(); i += sizeof(ushort))
            {
                yield return BitConverter.ToUInt16(bytes.GetRange(i, sizeof(ushort)).ToArray().ChangeEndianness(), 0);                                    
            }
        }
    }
}
