using System.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace Domain.SeedWork.Transactions;

/// <summary>
/// Indicates that the command handler should be executed within a database transaction.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class TransactionalAttribute : Attribute
{
    /// <summary>
    /// Specifies the isolation level for the transaction.
    /// </summary>
    public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;

    /// <summary>
    /// Determines whether to throw an exception if the transaction fails.
    /// </summary>
    public bool ThrowOnFailure { get; set; } = true;
}