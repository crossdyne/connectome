using Connectome.SocialGraph.Domain.Exceptions;
using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;

namespace Connectome.SocialGraph.Domain.ValueObjects.Person
{
    public readonly record struct PersonId
    {
        public Guid Value { get; }

        private PersonId(Guid value)
        {
            Value = value;
        }

        public static PersonId New() => new(Guid.CreateVersion7());

        public static PersonId From(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, 
            () => new EmptyValueException(new Error(ErrorCode.EmptyValue, $"Произошла критическая ошибка создание ValueObject - {nameof(PersonId)}. Было передано пустое входное значение.")));
            
            return new PersonId(value);
        }
    }
}