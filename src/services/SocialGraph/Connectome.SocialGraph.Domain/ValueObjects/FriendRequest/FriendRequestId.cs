using Connectome.SocialGraph.Domain.Exceptions;
using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;

namespace Connectome.SocialGraph.Domain.ValueObjects.FriendRequest
{
    public readonly record struct FriendRequestId
    {
        public Guid Value { get; }

        private FriendRequestId(Guid value)
        {
            Value = value;
        }

        public static FriendRequestId New() => new(Guid.CreateVersion7());

        public static FriendRequestId From(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, 
                () => new EmptyValueException(new Error(ErrorCode.EmptyValue, $"Произошла критическая ошибка создание ValueObject - {nameof(FriendRequestId)}. Было передано пустое входное значение.")));
            
            return new FriendRequestId(value);
        }
        
    }
}