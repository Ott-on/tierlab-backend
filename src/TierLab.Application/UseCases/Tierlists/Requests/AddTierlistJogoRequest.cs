namespace TierLab.Application.UseCases.Tierlists.Requests;

public sealed class AddTierlistJogoRequest
{
    public long JogoId { get; set; }
    public string? Tier { get; set; }
    public int? Posicao { get; set; }
}
