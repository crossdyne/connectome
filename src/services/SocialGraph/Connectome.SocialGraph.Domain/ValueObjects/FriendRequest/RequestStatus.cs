namespace Connectome.SocialGraph.Domain.ValueObjects.FriendRequest
{
    public readonly record struct RequestStatus
    {
        public int Value { get; }
        public string Name { get; }

        public static readonly RequestStatus Request = new(1, nameof(Request));
        public static readonly RequestStatus Accepted = new(2, nameof(Accepted));
        public static readonly RequestStatus Rejected = new(3, nameof(Rejected));

        public static IReadOnlyList<RequestStatus> All => [Request, Accepted, Rejected];

        private RequestStatus(int value, string name)
        {
            if (value <= 0) 
                throw new ArgumentException("Значение RequestStatus должно быть положительным.", nameof(value));
            
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Название RequestStatus не может быть пустым или отсутствовать.", nameof(name));

            Value = value;
            Name = name;
        }

        public static RequestStatus FromName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не должно быть пустым или отсутствовать.", nameof(name));

            var isType = All.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (!isType)
                throw new ArgumentException($"Неизвестное название RequestStatus: '{name}'", nameof(name));

            return All.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static RequestStatus FromValue(int value)
        {
            var isType = All.Any(t => t.Value == value);

            if (!isType)
                throw new ArgumentException($"Неизвестное значение RequestStatus: {value}", nameof(value));

            return All.FirstOrDefault(t => t.Value == value);
        }

        public override string ToString() => Name;
        
    }
}