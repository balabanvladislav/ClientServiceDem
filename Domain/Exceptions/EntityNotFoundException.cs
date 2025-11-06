using System;

namespace Domain.Exceptions;

public interface IEntityNotFoundException
{
    Type EntityType { get; }
    object Identifier { get; }
}
public class EntityNotFoundException<T> : Exception, IEntityNotFoundException
{
    public EntityNotFoundException(object identifier)
        : base($"{typeof(T).Name} with identifier '{identifier}' was not found.")
    {
        Identifier = identifier;
    }

    public EntityNotFoundException(string identifierName, object identifier)
        : base($"{typeof(T).Name} with {identifierName} '{identifier}' was not found.")
    {
        IdentifierName = identifierName;
        Identifier = identifier;
    }

    public Type EntityType => typeof(T);
    public string IdentifierName { get; } = "identifier";
    public object Identifier { get; }
}