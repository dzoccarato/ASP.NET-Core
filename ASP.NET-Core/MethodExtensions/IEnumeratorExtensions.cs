using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore.MethodExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IEnumeratorExtensions
    {
        /// <summary>
        /// ToIEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> self)
        {
            while (self.MoveNext())
            {
                yield return self.Current;
            }
        }
    }
}
