using System;

namespace EchoBot;

public class AiBotConfiguration
{
    public string AzureOpenAiApiKey { get; set; } = String.Empty;

    public string AzureOpenAiEndoint { get; set; } = String.Empty;

    public string AzureOpenAiModelName { get; set; } = String.Empty;

    public int AzureOpenAiToken { get; set; }
}
