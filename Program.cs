using System.Diagnostics;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/generate", async (HttpRequest request) =>
{
    // Parse request body
    using var reader = new StreamReader(request.Body);
    var bodyString = await reader.ReadToEndAsync();

    if (string.IsNullOrWhiteSpace(bodyString))
    {
        return Results.BadRequest(new { error = "Request body is required" });
    }

    var body = JsonSerializer.Deserialize<Dictionary<string, string>>(bodyString);
    if (body == null || !body.TryGetValue("prompt", out var prompt) || string.IsNullOrWhiteSpace(prompt))
    {
        return Results.BadRequest(new { error = "Prompt is required" });
    }

    // Run the Docker model command
    var processStartInfo = new ProcessStartInfo
    {
        FileName = "docker",
        Arguments = $"model run ai/gemma3 \"{prompt}\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    string output;
    using (var process = new Process { StartInfo = processStartInfo })
    {
        process.Start();
        output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
    }

    return Results.Ok(new
    {
        prompt,
        response = output.Trim()
    });
});

app.Run();
