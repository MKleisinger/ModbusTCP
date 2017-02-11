using System.Collections.Generic;
using System.Linq;

namespace Modbus.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns a list of groupings where the number of items in each 
        /// group matches the specified itemsPerGroup argument.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="itemsPerGroup"></param>
        /// <returns></returns>
        public static List<IGrouping<int, KeyValuePair<TKey, TValue>>> 
            GroupBySpecifiedCount<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, int itemsPerGroup)
        {
            return dictionary.Zip(Enumerable.Range(0, dictionary.Count),
                (selector, result) => new
                {
                    Group = result / itemsPerGroup,
                    Item = selector
                })
            .GroupBy(i => i.Group, g => g.Item)
            .ToList();
        }
    }
}
