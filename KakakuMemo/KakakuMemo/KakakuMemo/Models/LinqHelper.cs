using System;
using System.Collections.Generic;
using System.Text;

namespace KakakuMemo.Models
{
    public static class LinqHelper
    {
        // 参考ページ：LINQ で最小値・最大値を柔軟に求める
        // https://qiita.com/Zuishin/items/95e171eccc128bdd6429

        /// <summary>
        /// リスト内の特定キーが最小である場合を取得
        /// </summary>
        public static IEnumerable<T> MinBy<T, U>(this IEnumerable<T> source, Func<T, U> selector)
        {
            return SelectBy(source, selector, (a, b) => Comparer<U>.Default.Compare(a, b) < 0);
        }

        /// <summary>
        /// リスト内の特定キーが最大である場合を取得
        /// </summary>
        public static IEnumerable<T> MaxBy<T, U>(this IEnumerable<T> source, Func<T, U> selector)
        {
            return SelectBy(source, selector, (a, b) => Comparer<U>.Default.Compare(a, b) > 0);
        }

        /// <summary>
        /// 比較関数を使用して該当するものを取得
        /// </summary>
        private static IEnumerable<T> SelectBy<T, U>(IEnumerable<T> source, Func<T, U> selector, Func<U, U, bool> comparer)
        {
            var list = new LinkedList<T>();
            U prevKey = default(U);
            foreach (var item in source)
            {
                var key = selector(item);
                if (list.Count == 0 || comparer(key, prevKey))
                {
                    list.Clear();
                    list.AddLast(item);
                    prevKey = key;
                }
                else if (Comparer<U>.Default.Compare(key, prevKey) == 0)
                {
                    list.AddLast(item);
                }
            }
            return list;
        }
    }
}
