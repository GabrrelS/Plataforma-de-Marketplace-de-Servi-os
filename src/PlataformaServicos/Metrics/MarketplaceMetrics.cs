using Prometheus;

namespace PlataformaServicos.Metrics;

public static class MarketplaceMetrics
{
    public static readonly Counter ClientesCriados =
        Prometheus.Metrics.CreateCounter(
            "marketplace_clientes_criados_total",
            "Total de clientes cadastrados"
        );

    public static readonly Counter PrestadoresCriados =
        Prometheus.Metrics.CreateCounter(
            "marketplace_prestadores_criados_total",
            "Total de prestadores cadastrados"
        );

    public static readonly Counter PropostasCriadas =
        Prometheus.Metrics.CreateCounter(
            "marketplace_propostas_criadas_total",
            "Total de propostas criadas"
        );

    public static readonly Histogram TempoRequisicao =
        Prometheus.Metrics.CreateHistogram(
            "marketplace_http_request_duration_seconds",
            "Tempo de resposta das requisições"
        );
}