using Microsoft.Extensions.Logging;
using MongoDbProvider.Models;
using MongoDbProvider.Repositories;
using OpenAi.Client.Interfaces;

namespace OpenAi.Client.Services;

public class PromptService : IPromptService
{
    private readonly LlmPromptRepository _llmPromptRepository;
    private readonly ILogger<PromptService> _logger;

    public PromptService(LlmPromptRepository llmPromptRepository, ILogger<PromptService> logger)
    {
        _llmPromptRepository = llmPromptRepository;
        _logger = logger;
    }

    public async Task SavePromptAsync(ulong userId, string username, string prompt, string response)
    {
        var promptEntity = new LlmPrompt
        {
            UserId = userId,
            Username = username,
            Prompt = prompt,
            Response = response
        };
        
        try
        {
            await _llmPromptRepository.CreateAsync(promptEntity);
        }
        catch (Exception e)
        {
            _logger.LogWarning("Failed to save LLM prompt to the database. Reason: {Reason}", e.Message);
        }
    }
}