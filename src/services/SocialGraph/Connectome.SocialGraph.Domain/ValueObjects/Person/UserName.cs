using Connectome.SocialGraph.Domain.Exceptions;
using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;

namespace Connectome.SocialGraph.Domain.ValueObjects.Person
{
    public readonly record struct UserName
    {
        public string Value { get; }

        public UserName(string value) => Value = value;

        public static UserName Create(string userName)
        {
            Guard.Against.That(string.IsNullOrWhiteSpace(userName), 
                () => new EmptyValueException(new Error(ErrorCode.EmptyValue, $"Произошла критическая ошибка создание ValueObject - {nameof(UserName)}. Было передано пустое входное значение.")));

            return new UserName(userName);
        }

        public override string ToString() => Value;

        public static implicit operator string(UserName userName) => userName.Value;
    }
}