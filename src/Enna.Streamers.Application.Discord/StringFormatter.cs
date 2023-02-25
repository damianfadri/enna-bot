namespace Enna.Streamers.Application.Discord
{
    public class StringFormatter
    {
        public Dictionary<string, object> Parameters { get; }

        public StringFormatter()
        {
            Parameters = new Dictionary<string, object>();
        }

        public void Add(string key, object? val)
        {
            Parameters[key] = val ?? string.Empty;
        }

        public string Format(string template)
        {
            return Parameters.Aggregate(template, (current, parameter) =>
            {
                return current.Replace(
                    parameter.Key, parameter.Value.ToString());
            });
        }
    }
}
