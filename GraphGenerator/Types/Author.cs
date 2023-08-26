namespace GraphGenerator.Types;

public record Author(string Name);
[QueryType]
public class HelloWorld3
{
    public string World { get; set; } = "Hello World";
    // data loader Author
    [DataLoader] 
    public static async Task<IReadOnlyDictionary<string,Author>> GetAuthorsByNameAsync(
        IReadOnlyList<string> names,
        CancellationToken cancellationToken)
    {
        // dummy authors
        var authors= new List<Author>();
        await Task.Delay(1);
        return authors.ToDictionary(t => t.Name);
    }
    
}
[QueryType]
public class HelloWorld5
{
    public string World { get; set; } = "Hello World";
    // data loader Author
    [DataLoader] 
    public static async Task<IReadOnlyDictionary<string,Author>> GetAuthorsByIdAsync(
        IReadOnlyList<string> names,
        CancellationToken cancellationToken)
    {
        // dummy authors
        var authors= new List<Author>();
        await Task.Delay(1);
        return authors.ToDictionary(t => t.Name);
    }
    
}