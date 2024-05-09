using System.ComponentModel.DataAnnotations;

namespace MongoDbProvider;

public class MongoDbConfiguration
{
    public const string SectionName = "MongoDbConfiguration";

    [Required]
    public string ConnectionString { get; init; } = null!;

    [Required]
    public string DatabaseName { get; init; } = null!;
}