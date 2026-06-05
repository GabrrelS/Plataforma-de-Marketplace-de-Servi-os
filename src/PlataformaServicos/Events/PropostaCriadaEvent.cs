namespace PlataformaServicos.Events;

public record PropostaCriadaEvent
{
    public int PropostaId   { get; init; }
    public int ClienteId    { get; init; }
    public int PrestadorId  { get; init; }
    public string Titulo    { get; init; } = string.Empty;
    public decimal Valor    { get; init; }
    public DateTime CriadaEm { get; init; } = DateTime.UtcNow;
}