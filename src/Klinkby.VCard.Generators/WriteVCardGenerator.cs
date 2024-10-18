using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Klinkby.VCard.Generators;

[Generator]
public sealed class WriteVCardGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Register the attribute source
        context.RegisterForPostInitialization(i => i.AddSource(
            "VCardToStringAttribute.g.cs", 
            """

            using System;
            using System.CodeDom.Compiler;

            namespace Klinkby.VCard;

            /// <inheritdoc />
            [GeneratedCode("WriteVCardGenerator", "1.0")]
            [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
            internal sealed class VCardToStringAttribute() : Attribute;

            /// <inheritdoc />
            [GeneratedCode("WriteVCardGenerator", "1.0")]
            [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
            internal sealed class VCardWritableAttribute() : Attribute;

            /// <summary>Type can serialize to vCard format</summary>
            [GeneratedCode("WriteVCardGenerator", "1.0")]
            public interface IVCardWriter
            {
                /// <summary>Serialize to a TextWriter</summary>
                void WriteVCard(TextWriter writer);
            }

            """
            ));

        // Register a syntax receiver that will be created for each generation pass
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
            return;

        var attributeSymbol = context.Compilation.GetTypeByMetadataName("Klinkby.VCard.VCardToStringAttribute");
        if (attributeSymbol is null) return;

        // group the fields by class, and generate the source
        var classGroups =
            receiver.Properties.GroupBy<IPropertySymbol, INamedTypeSymbol>(
                f => f.ContainingType, SymbolEqualityComparer.Default);
        foreach (var group in classGroups)
        {
            var classSource = ProcessClass(group.Key, group.ToList());
            context.AddSource($"{group.Key.Name}.g.cs", SourceText.From(classSource, Encoding.UTF8));
        }
    }

    private static string ProcessClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> properties)
    {
        if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            throw new InvalidOperationException($"{classSymbol.Name} must be top level");

        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

        // begin building the generated source
        var source = new StringBuilder(
            $$"""
              using System;
              using System.CodeDom.Compiler;
              using System.Globalization;

              namespace {{namespaceName}};

              partial record {{classSymbol.Name}}
              {
                  /// <inheritdoc />
                  [GeneratedCode("WriteVCardGenerator", "1.0")]
                  public void WriteVCard(TextWriter writer)
                  {
                      writer.WriteLine("BEGIN:{{classSymbol.Name.ToUpperInvariant()}}");

              """);

        // create properties for each field 
        foreach (var propertySymbol in properties) ProcessProperty(source, propertySymbol);

        source.AppendLine(
            $$"""
                      writer.WriteLine("END:{{classSymbol.Name.ToUpperInvariant()}}");
                  }
              }

              """);
        return source.ToString();
    }

    private static void ProcessProperty(StringBuilder source, IPropertySymbol propertySymbol)
    {
        // get the name and type of the field
        var propertyName = propertySymbol.Name;
        var fieldType = propertySymbol.Type;

        var isWritable = fieldType.Interfaces.Any(x => x.Name == "IVCardWriter");
        if (isWritable)
        {
            source.AppendLine($"        {propertyName}?.WriteVCard(writer);");
        }
        else if (propertySymbol.GetAttributes().Any(x => x.AttributeClass?.Name == "VCardWritableAttribute"))
        {
            source.AppendLine($"        foreach (var item in {propertyName}) item.WriteVCard(writer);");
        }
        else if (fieldType.IsValueType)
        {
            var dtFormat = fieldType.SpecialType == SpecialType.System_DateTime
                ? "\"yyyyMMddTHHmmssZ\", "
                : "";
            source.AppendLine(
                $"""
                         writer.Write("{propertyName.ToUpperInvariant()}:");
                         writer.WriteLine({propertyName}.ToString({dtFormat}CultureInfo.InvariantCulture));
                 """);
        }
        else if (fieldType.SpecialType == SpecialType.System_String)
        {
            source.AppendLine(
                $$"""
                          if (!string.IsNullOrEmpty({{propertyName}}))
                          {
                              writer.Write("{{propertyName.ToUpperInvariant()}}:");
                              writer.WriteLine({{propertyName}});
                          }
                  """);
        }
    }

    /// <summary>
    ///     Created on demand before each generation pass
    /// </summary>
    private sealed class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<IPropertySymbol> Properties { get; } = [];

        /// <summary>
        ///     Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for
        ///     generation
        /// </summary>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            // any field with at least one attribute is a candidate for property generation
            if (context.Node is not PropertyDeclarationSyntax propertyDeclarationSyntax) return;
            // Get the symbol being declared by the field, and keep it if its annotated
            var propertySymbol = context.SemanticModel.GetDeclaredSymbol(propertyDeclarationSyntax);
            if (propertySymbol is null) return;
            Properties.Add(propertySymbol);
        }
    }
}