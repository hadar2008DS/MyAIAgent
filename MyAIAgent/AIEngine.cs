using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyAIAgent
{
    public class AIEngine
    {
        private readonly Kernel kernel;
        private readonly IChatCompletionService chatService;
        private readonly ChatHistory history;

        //Setup
        public AIEngine(String apiKey, string modelId = "gpt-4o-mini")
        {
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(modelId, apiKey);

            //Connecting plagIns
            var dbPlugin = new DatabasePlugin(connectionString);
            builder.Plugins.AddFromObject(dbPlugin, "JamLinkDatabase");

            kernel = builder.Build();
            chatService = kernel.GetRequiredService<IChatCompletionService>();

            //memory and promt
            history.AddSystemMessage("You are JamLink AI, a professional music collaboration assistant. " +
                                     "You have access to the musician and producer database. " +
                                     "Use the 'JamLinkDatabase' tools to answer questions about users, instruments, and music segments.");
        }

        //Main function
        public async Task<string> SendMessageAsync(string userPrompt)
        {
            history.AddUserMessage(userPrompt);

            // הגדרות המאפשרות לאג'נט להפעיל את הפלאגין אוטומטית לפי הצורך
            var settings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            // קבלת התשובה מהמודל
            var response = await chatService.GetChatMessageContentAsync(
                history,
                executionSettings: settings,
                kernel: kernel);

            // Update history in the response
            history.AddAssistantMessage(response.Content);

            return response.Content;
        }


    }
}
