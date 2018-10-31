using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Helper
{
    public static class ExtensionsMethod
    {
        public static void RemoveRange<T>(this List<T> source, IEnumerable<T> rangeToRemove)
        {
            if (rangeToRemove == null || !rangeToRemove.Any()) return;

            foreach (var item in rangeToRemove)
            {
                source.Remove(item);
            }
        }

        public static bool Contains(this string value, List<string> valuesToCompare)
        {
            var result = false;

            foreach (var item in valuesToCompare)
            {
                if (value.Contains(item))
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool Contains(this List<string> value, List<string> valuesToCompare)
        {
            var result = false;

            foreach (var item in valuesToCompare)
            {
                if (value.Contains(item))
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool StartsWith(this string value, List<string> valuesToCompare)
        {
            bool result = false;

            foreach (var item in valuesToCompare)
            {
                if (item.StartsWith(item))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
