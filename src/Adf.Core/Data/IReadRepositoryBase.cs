using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Adf.Core.Data
{

    /// <summary>
    /// <para>
    /// A <see cref="IRepositoryBase{T}" /> can be used to query and save instances of <typeparamref name="T" />.
    /// An <see cref="ISpecification{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of entity being operated on by this repository.</typeparam>
    public interface IReadWriteRepositoryBase<T> : IReadRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Adds an entity in the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="T" />.
        /// </returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates an entity in the database
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes an entity in the database
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes the given entities in the database
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        /// <summary>
        /// Persists changes to the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public interface IReadRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Finds an entity with the given primary key value.
        /// </summary>
        /// <typeparam name="TId">The type of primary key.</typeparam>
        /// <param name="id">The value of the primary key for the entity to be found.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
        /// </returns>
        Task<T> GetByIdAsync<TId>(TId id, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default) where TId : notnull;


        /// <summary>
        /// Finds an entity that matches the encapsulated query logic of the <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="TResult" />.
        /// </returns>
        Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all entities of <typeparamref name="T" /> from the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<T>> ListAsync([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<T>> ListAsync(ISpecification<T> specification, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// <para>
        /// Projects each entity into a new form, being <typeparamref name="TResult" />.
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a number that represents how many entities satisfy the encapsulated query logic
        /// of the <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the
        /// number of elements in the input sequence.
        /// </returns>
        Task<int> CountAsync(ISpecification<T> specification, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0,  CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the total number of records.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the
        /// number of elements in the input sequence.
        /// </returns>
        Task<int> CountAsync([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a boolean that represents whether any entity satisfy the encapsulated query logic
        /// of the <paramref name="specification"/> or not.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains true if the 
        /// source sequence contains any elements; otherwise, false.
        /// </returns>
        Task<bool> AnyAsync(ISpecification<T> specification, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a boolean whether any entity exists or not.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains true if the 
        /// source sequence contains any elements; otherwise, false.
        /// </returns>
        Task<bool> AnyAsync([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);
    }
}
