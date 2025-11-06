using System;

namespace Domain.Exceptions;

public interface IDuplicateEntityException
{
    Type EntityType { get; }
    object Identifier { get; }
    string IdentifierName { get; }
}

public class DuplicateEntityException<T> : Exception, IDuplicateEntityException
{
    public DuplicateEntityException(object identifier)
        : base($"{typeof(T).Name} with identifier '{identifier}' already exists.")
    {
        Identifier = identifier;
    }

    public DuplicateEntityException(string identifierName, object identifier)
        : base($"{typeof(T).Name} with {identifierName} '{identifier}' already exists.")
    {
        IdentifierName = identifierName;
        Identifier = identifier;
    }

    public Type EntityType => typeof(T);
    public string IdentifierName { get; } = "identifier";
    public object Identifier { get; }
}