using System.Runtime.InteropServices;
using Connectome.SocialGraph.Domain.Exceptions;
using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;

namespace Connectome.SocialGraph.Domain.ValueObjects.Common
{
    public readonly record struct UserId
    {
        public Guid Value { get; }

        private UserId(Guid value)
        {
            Value = value;
        }

        public static UserId Create(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, 
            () => new EmptyValueException(new Error(ErrorCode.EmptyValue, $"Произошла критическая ошибка создание ValueObject - {nameof(UserId)}. Было передано пустое входное значение.")));
            
            return new UserId(value);
        }
    }
}