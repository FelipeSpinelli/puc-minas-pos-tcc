namespace ArquiveSe.App.Services.ArquiveSeApi
{
    public class ApiSettings
    {
        public const string SECTION = nameof(ApiSettings);

        public Uri BaseUrl { get; set; } = new Uri("http://0.0.0.0");
    }
}
