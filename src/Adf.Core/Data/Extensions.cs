﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Adf.Core.Data
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;



    #endregion

    /// <summary>
    ///     The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     The member access.
        /// </summary>
        public const BindingFlags MemberAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static
                                                 | BindingFlags.Instance | BindingFlags.IgnoreCase;

        /// <summary>
        ///     The member public instance access.
        /// </summary>
        public const BindingFlags MemberPublicInstanceAccess =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

        /// <summary>
        /// Copies the content of a data row to another. Runs through the target's fields
        ///     and looks for fields of the same name in the source row. Structure must mathc
        ///     or fields are skipped.
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CopyDataRow(this DataRow source, DataRow target)
        {
            var columns = target.Table.Columns;

            for (var x = 0; x < columns.Count; x++)
            {
                var fieldname = columns[x].ColumnName;

                try
                {
                    target[x] = source[fieldname];
                }
                catch
                {
                    // ignored
                }

                // skip any errors
            }

            return true;
        }

        /// <summary>
        /// The copy object from data row.
        /// </summary>
        /// <param name="row">
        /// The row.
        /// </param>
        /// <param name="targetObject">
        /// The target object.
        /// </param>
        public static void CopyObjectFromDataRow(DataRow row, object targetObject)
        {
            var miT = targetObject.GetType().FindMembers(
                MemberTypes.Field | MemberTypes.Property,
                MemberAccess,
                null,
                null);
            foreach (var field in miT)
            {
                var name = field.Name;
                if (!row.Table.Columns.Contains(name))
                {
                    continue;
                }

                if (field.MemberType == MemberTypes.Field)
                {
                    ((FieldInfo)field).SetValue(targetObject, row[name]);
                }
                else if (field.MemberType == MemberTypes.Property)
                {
                    ((PropertyInfo)field).SetValue(targetObject, row[name], null);
                }
            }
        }

        // public static async Task<PagedList<T>> GetPagedAsync<T>(this IQueryable<T> query, int pageNum, int pageSize)
        // {

        // //IQueryable<T> collection  = query.Skip((pageNum - 1) * pageSize).Take(pageSize);
        // return new PagedList<T>(query, pageNum, pageSize);
        // }

        /// <summary>
        /// The get paged.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <typeparam name="TOrderBy">
        /// The type of the order by.
        /// </typeparam>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <param name="isDescending">
        /// The is descending.
        /// </param>
        /// <param name="pageNum">
        /// The page num.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <returns>
        /// The <see cref="PagedList"/>.
        /// </returns>
        /// <exception cref="System.Exception">
        /// To do Paging you MUST provide valid OrderBy value
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        public static PagedList<T> GetPaged<T, TOrderBy>(
            this IQueryable<T> query,
            Expression<Func<T, TOrderBy>> orderBy,
            bool isDescending,
            int pageNum,
            int pageSize)
        {
            if (orderBy != null)
            {
                query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            else
            {
                throw new Exception("To do Paging you MUST provide valid OrderBy value");
            }

            return new PagedList<T>(query, pageNum, pageSize);
        }

        /// <summary>
        /// The get paged.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <param name="isDescending">
        /// The is descending.
        /// </param>
        /// <param name="pageNum">
        /// The page num.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <typeparam name="TOrderBy">
        /// </typeparam>
        /// <returns>
        /// The <see cref="PagedList"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static PagedList<T> GetPaged<T, TOrderBy>(
            this List<T> query,
            Func<T, TOrderBy> orderBy,
            bool isDescending,
            int pageNum,
            int pageSize)
        {
            if (orderBy != null)
            {
                query = isDescending ? query.OrderByDescending(orderBy).ToList() : query.OrderBy(orderBy).ToList();
            }
            else
            {
                throw new Exception("To do Paging you MUST provide valid OrderBy value");
            }

            return new PagedList<T>(query, pageNum, pageSize);
        }

        ///// <summary>
        ///// The get paged async.
        ///// </summary>
        ///// <param name="query">
        ///// The query.
        ///// </param>
        ///// <param name="orderBy">
        ///// The order by.
        ///// </param>
        ///// <param name="isDescending">
        ///// The is descending.
        ///// </param>
        ///// <param name="pageNum">
        ///// The page num.
        ///// </param>
        ///// <param name="pageSize">
        ///// The page size.
        ///// </param>
        ///// <typeparam name="T">
        ///// </typeparam>
        ///// <typeparam name="TOrderBy">
        ///// </typeparam>
        ///// <returns>
        ///// The <see cref="Task"/>.
        ///// </returns>
        ///// <exception cref="Exception">
        ///// </exception>
        //public static async Task<PagedList<T>> GetPagedAsync<T, TOrderBy>(
        //    this IQueryable<T> query,
        //    Expression<Func<T, TOrderBy>> orderBy,
        //    bool isDescending,
        //    int pageNum,
        //    int pageSize)
        //{
        //    if (orderBy != null)
        //    {
        //        query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        //    }
        //    else
        //    {
        //        throw new Exception("To do Paging you MUST provide valid OrderBy value");
        //    }

        //    query = query.Skip((pageNum - 1) * pageSize).Take(pageSize);

        //    var result = await query.ToListAsync();
        //    return new PagedList<T>(result, pageNum, pageSize);
        //}


    }
}
