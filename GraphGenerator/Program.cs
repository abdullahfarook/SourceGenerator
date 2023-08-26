using GraphGenerator.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddTypes();

var app = builder.Build();

app.MapGraphQL();

app.RunWithGraphQLCommands(args);

[QueryType]
public class Hello
{
    public string World { get; set; } = "Hello World";
}
[QueryType]
public class HelloWorld
{
    public string World { get; set; } = "Hello World";
}
// [QueryType]
public class HelloWorld2
{
    public HelloWorld2(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public static HelloWorld2 Hello = new HelloWorld2("hello");
    public static HelloWorld2 World = new HelloWorld2("world");
}

 
public sealed class HelloWorld2Extension 
    : EnumType<HelloWorld2>
{
    protected override void Configure(IEnumTypeDescriptor<HelloWorld2> descriptor)
    {
        descriptor.Value(HelloWorld2.Hello).Name(HelloWorld2.Hello.Name.ToUpperInvariant());
        descriptor.Value(HelloWorld2.World).Name(HelloWorld2.World.Name.ToUpperInvariant());
    }
}

