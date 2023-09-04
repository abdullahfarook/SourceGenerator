using System.Collections.Immutable;
using HotChocolate.Types.Analyzers.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HotChocolate.Types.Analyzers.Inspectors;

public sealed class Method
{
    public ImmutableArray<IParameterSymbol> Services { get; set; }
    public IParameterSymbol Request { get; set; }
    public ITypeSymbol? Response { get; set; }
    public bool IsTask { get; set; } = false;

    public Method(DataLoaderInfo dataLoader)
    {
        // map properties above 
        var parameters = dataLoader.MethodSymbol.Parameters;
        Request = parameters.First();
        Services = parameters.Skip(1).SkipLast(1).ToImmutableArray();
        var args = ((INamedTypeSymbol)dataLoader.MethodSymbol.ReturnType)?.TypeArguments;
        if (args is {Length:>0})
        {
            Response = args.Value[0];
        }
    }
}
public sealed class ServiceMethod : ISyntaxInfo, IEquatable<ServiceMethod>
{
    public ServiceMethod(
        AttributeSyntax attributeSyntax,
        IMethodSymbol attributeSymbol,
        IMethodSymbol methodSymbol,
        MethodDeclarationSyntax methodSyntax)
    {
        AttributeSyntax = attributeSyntax;
        AttributeSymbol = attributeSymbol;
        MethodSymbol = methodSymbol;
        MethodSyntax = methodSyntax;

        var attribute = methodSymbol.GetDataLoaderAttribute();

        Name = methodSymbol.Name;
        Namespace = methodSymbol.ContainingNamespace.ToDisplayString();
        FullName = $"{Namespace}.{Name}";
        IsPublic = attribute.IsPublic();
        var type = methodSymbol.ContainingType;
        ContainingType = type.ToDisplayString();
    }


    public string Name { get; }

    public string FullName { get; }

    public string Namespace { get; }

    public string ContainingType { get; }

    public bool? IsPublic { get; }

    public AttributeSyntax AttributeSyntax { get; }

    public IMethodSymbol AttributeSymbol { get; }

    public IMethodSymbol MethodSymbol { get; }

    public MethodDeclarationSyntax MethodSyntax { get; }

    public bool Equals(ServiceMethod? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return AttributeSyntax.Equals(other.AttributeSyntax) &&
            MethodSyntax.Equals(other.MethodSyntax);
    }
    
    public bool Equals(ISyntaxInfo other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is ServiceMethod info && Equals(info);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj)
            || obj is ServiceMethod other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = AttributeSyntax.GetHashCode();
            hashCode = (hashCode * 397) ^ MethodSyntax.GetHashCode();
            return hashCode;
        }
    }

    
}