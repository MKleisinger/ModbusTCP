using System.Collections;

namespace Modbus.Extensions
{
    public static class BitArrayExtensions
    {
        /// <summary>
        /// Sets a bit in the bit array to a new value.
        /// 
        /// Returns true if the bit has changed; false if the
        /// change was not necessary and the bit was not set.
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="offset"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetBit(this BitArray bits, int offset, bool newValue)
        {
            bool hasChanged = false;

            if ((bits[(bits.Length - 1) - offset]) != newValue)
            {
                bits[(bits.Length - 1) - offset] = newValue;
                hasChanged = true;
            }

            return hasChanged;
        }

        /// <summary>
        /// Gets the integer value of the bits.        
        /// 
        /// For example 00000111 would yield the value of 7
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static int GetValue(this BitArray bits)
        {
            int[] arr = new int[1];

            bits.CopyTo(arr, 0);

            return arr[0];
        }
    }
}
