using Connectome.SocialGraph.Domain.Exceptions;
using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;

namespace Connectome.SocialGraph.Domain.ValueObjects.Friendship
{
    public readonly record struct FriendshipId
    {
        public Guid Value { get; }

        private FriendshipId(Guid value)
        {
            Value = value;
        }

        public static FriendshipId New() => new(Guid.CreateVersion7());

        public static FriendshipId From(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, 
                () => new EmptyValueException(new Error(ErrorCode.EmptyValue, $"Произошла критическая ошибка создание ValueObject - {nameof(FriendshipId)}. Было передано пустое входное значение.")));
            
            return new FriendshipId(value);
        }
    }
}