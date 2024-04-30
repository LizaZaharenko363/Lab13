using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenTelemetryTracing(builder =>
{
    builder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"))
        .AddAspNetCoreInstrumentation(options =>
        {
            options
                .AddW3CTraceContextPropagator() 
                .Enrich = (activity, eventName, _, attributes) =>
                {
                    attributes.Add("user_id", "123");
                    attributes.Add("request_path", "/api/users");
                };
        })
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://your-otel-collector:4317");
        })
        .SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(0.1))) 
        .AddProcessorPipeline(config => config.AddProcessor(_ => new MyProcessor())); 
});


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
