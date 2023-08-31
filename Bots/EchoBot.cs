// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.17.1

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;

namespace EchoBot.Bots;

public class EchoBot : ActivityHandler
{
    private readonly AiBotConfiguration Configuration;
    private OpenAIClient OpenAiClient;

    public EchoBot(IOptions<AiBotConfiguration> configuration)
    {
        Configuration = configuration.Value;
        OpenAiClient = new(new Uri(Configuration.AzureOpenAiEndoint), new AzureKeyCredential(Configuration.AzureOpenAiApiKey));
    }

    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            Messages =
            {
                new ChatMessage(ChatRole.System, "あなたはプログラマーを助ける賢いアシスタントです。"),
                new ChatMessage(ChatRole.System, "プログラマーの質問に優しく答えてください。"),
                new ChatMessage(ChatRole.System, "説明は簡潔に記載してください。"),
                new ChatMessage(ChatRole.User, turnContext.Activity.Text),
            },
            MaxTokens = Configuration.AzureOpenAiToken
        };

        Response<ChatCompletions> response = OpenAiClient.GetChatCompletions(
            deploymentOrModelName: Configuration.AzureOpenAiModelName,
            chatCompletionsOptions);

        var responseMessage = response.Value.Choices[0].Message.Content;

        await turnContext.SendActivityAsync(MessageFactory.Text(responseMessage, responseMessage), cancellationToken);
    }

    protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        var welcomeText = "Hello and welcome!";
        foreach (var member in membersAdded)
        {
            if (member.Id != turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
            }
        }
    }
}
