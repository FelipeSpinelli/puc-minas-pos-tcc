namespace ArquiveSe.UI.Clients.ArquiveSeApi.Models.Requests
{
    public record CreateAccountRequest
    {
        public string UserId { get; init; } = null!;
    }
}
