using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb; // קריטי עבור פנייה ל-Access
using System.Threading.Tasks;
using MyAIAgent.DTOs; // גישה ל-DTOs שהגדרת


namespace MyAIAgent
{
    public class AIEngine
    {
        private readonly Kernel kernel;
        private readonly IChatCompletionService chatService;
        private readonly ChatHistory history;

        //Setup
        public AIEngine(string apiKey, string connectionString)
        {
            var builder = Kernel.CreateBuilder();

            // החלפת המודל למודל של Groq והפניית ה-Endpoint לשרתים שלהם
            builder.AddOpenAIChatCompletion(
                modelId: "llama3-8b-8192", // מודל סופר מהיר וחינמי של Groq
                apiKey: apiKey,
                endpoint: new Uri("https://api.groq.com/openai/v1") // הכתובת שמנתבת את זה ל-Groq!
            );

            var dbPlugin = new DatabasePlugin(connectionString);
            builder.Plugins.AddFromObject(dbPlugin, "JamLinkDatabase");

            kernel = builder.Build();
            chatService = kernel.GetRequiredService<IChatCompletionService>();

            history = new ChatHistory();
            history.AddSystemMessage("You are JamLink AI, a professional music collaboration assistant. " +
                                     "You have access to the musician and producer database. " +
                                     "Use the 'JamLinkDatabase' tools to answer questions about users, instruments, and music segments.");
        }

        //Main function
        public async Task<string> SendMessageAsync(string userPrompt)
        {
            history.AddUserMessage(userPrompt);

            var settings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var response = await chatService.GetChatMessageContentAsync(
                history,
                executionSettings: settings,
                kernel: kernel);

            history.AddAssistantMessage(response.Content);

            return response.Content;
        }
    }


}

