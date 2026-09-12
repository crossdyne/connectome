namespace Connectome.SocialGraph.Infrastructure.Helpers
{
    internal static class CypherLoader
    {
        public static string Load<TRepo>(string fileName) where TRepo : class
        {
            var assembly = typeof(TRepo).Assembly;
            var resourceName = $"Cyphers.{fileName}";

            using var stream = assembly.GetManifestResourceStream(typeof(TRepo), resourceName) 
                ?? throw new InvalidOperationException($"Ресурс '{resourceName}' не найден для типа {typeof(TRepo).Name}");

            using var reader = new StreamReader(stream);
            
            return reader.ReadToEnd();
        }
    }
}