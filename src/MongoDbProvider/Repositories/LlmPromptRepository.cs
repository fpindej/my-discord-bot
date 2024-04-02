using MongoDbProvider.DataAccess;
using MongoDbProvider.Models;

namespace MongoDbProvider.Repositories;

public class LlmPromptRepository
{
    private readonly LlmPromptContext _context;

    public LlmPromptRepository(LlmPromptContext context)
    {
        _context = context;
    }
    
    public Task CreateAsync(LlmPrompt prompt)
    {
        return _context.LlmPromptsContext.InsertOneAsync(prompt);
    }
}