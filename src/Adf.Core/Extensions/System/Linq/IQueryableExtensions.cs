﻿using Adf.Core.Data;
using System;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Some useful extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static IQueryable<T> PageBy<T>( this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            //Check.NotNull(query, nameof(query));

            return query.Skip(skipCount).Take(maxResultCount);
        }

        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static TQueryable PageBy<T, TQueryable>( this TQueryable query, int skipCount, int maxResultCount)
            where TQueryable : IQueryable<T>
        {
            //Check.NotNull(query, nameof(query));

            return (TQueryable)query.Skip(skipCount).Take(maxResultCount);
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>( this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            //Check.NotNull(query, nameof(query));

            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static TQueryable WhereIf<T, TQueryable>( this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            //Check.NotNull(query, nameof(query));

            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>( this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            //Check.NotNull(query, nameof(query));

            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static TQueryable WhereIf<T, TQueryable>( this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            //Check.NotNull(query, nameof(query));

            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }


        //public static IQueryable<TResult> SelectCustom<TResult>(this IQueryable source, string[] columns)
        //{
        //    var sourceType = source.ElementType;
        //    var resultType = typeof(TResult);
        //    var parameter = Expression.Parameter(sourceType, "e");
        //    var bindings = columns.Select(column => Expression.Bind(
        //        resultType.GetProperty(column), Expression.PropertyOrField(parameter, column)));
        //    var body = Expression.MemberInit(Expression.New(resultType), bindings);
        //    var selector = Expression.Lambda(body, parameter);
        //    return source.Provider.CreateQuery<TResult>(
        //        Expression.Call(typeof(Queryable), "Select", new Type[] { sourceType, resultType },
        //            source.Expression, Expression.Quote(selector)));
        //}

        private static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Filters<T> filters)
        {
            return !filters.IsValid() ? query : filters.Get().Aggregate(query, (current, filter) => current.Where(filter.Expression));
        }

    }

}
