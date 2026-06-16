namespace TierLab.Domain.Common;

public interface IAuditable
{
    DateTime CriadoEm { get; set; }
    DateTime? AtualizadoEm { get; set; }
}
