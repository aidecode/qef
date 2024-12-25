using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace QEF.Context.Extensions
{
    /// <summary>
    /// Entity Framework LINQ related extension methods.
    /// </summary>
    public static class EntityFrameworkQueryableExtensions
    {
        private const int DefaultPortionSize = 500;

        /// <summary>
        /// Creates <see cref="List{TSource}"/> filled by portions.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <typeparam name="TKey">Type of values.</typeparam>
        /// <param name="source">Data source.</param>
        /// <param name="valuesSet">Values.</param>
        /// <param name="keySelector">Key field selector.</param>
        /// <param name="portionSize">Portion size.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>Filtered list.</returns>
        public static Task<List<TSource>> ToListByPortionsAsync<TSource, TKey>(
            this IQueryable<TSource> source,
            HashSet<TKey> valuesSet,
            Expression<Func<TSource, TKey>> keySelector,
            int portionSize = DefaultPortionSize,
            CancellationToken cancellationToken = default)
        {
            if (valuesSet == null)
            {
                throw new ArgumentNullException(nameof(valuesSet));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (portionSize <= 0)
            {
                portionSize = int.MaxValue;
            }

            if (valuesSet.Count <= portionSize)
            {
                return source.In(valuesSet, keySelector).ToListAsync(cancellationToken);
            }

            return GetListByPortionsAsync(source, valuesSet, keySelector, portionSize, cancellationToken);
        }

        /// <summary>
        /// Gets filled list by portions.
        /// </summary>
        private static async Task<List<TSource>> GetListByPortionsAsync<TSource, TKey>(
            IQueryable<TSource> source,
            HashSet<TKey> valuesSet,
            Expression<Func<TSource, TKey>> keySelector,
            int portionSize,
            CancellationToken cancellationToken)
        {
            var list = new List<TSource>();
            var skip = 0;

            do
            {
                var portionValues = valuesSet.Skip(skip).Take(portionSize).ToHashSet();
                var batchList = await source.
                    In(portionValues, keySelector).
                    ToListAsync(cancellationToken);

                list.AddRange(batchList);
                skip += portionSize;
            }
            while (skip < valuesSet.Count);

            return list;
        }
    }
}
