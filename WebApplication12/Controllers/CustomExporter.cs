using OpenTelemetry;


public class CustomExporter : SpanExporter
{
    public override Task<ExportResult> ExportAsync(IEnumerable<SpanData> batch, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var spanData in batch)
            {
                Console.WriteLine($"Exporting span: {spanData.Name}, TraceId: {spanData.Context.TraceId}");
            }

            return Task.FromResult(ExportResult.Success);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export failed: {ex.Message}");
            return Task.FromResult(ExportResult.Failure);
        }
    }

    public override Task ShutdownAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Exporter shutdown.");
        return Task.CompletedTask;
    }
}
