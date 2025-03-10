using System.Text;
using System.Text.RegularExpressions;
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
            "Common.g.cs", 
            """

            using System;
            using System.CodeDom.Compiler;
            using System.Runtime.CompilerServices;
            using System.Text.RegularExpressions;

            namespace Klinkby.VCard;

            /// <inheritdoc />
            [GeneratedCode("Klinkby.VCard.Generators", "1.0")]
            [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
            internal sealed class VCardToStringAttribute() : Attribute;

            /// <inheritdoc />
            [GeneratedCode("Klinkby.VCard.Generators", "1.0")]
            [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
            internal sealed class VCardWritableAttribute() : Attribute;

            /// <summary>Type can serialize to vCard format</summary>
            [GeneratedCode("Klinkby.VCard.Generators", "1.0")]
            public interface IVCardWriter
            {
                /// <summary>Serialize to a TextWriter</summary>
                void WriteVCard(TextWriter writer);
            }
            
            /// <summary>Provide escaping for text <seealso href="https://datatracker.ietf.org/doc/html/rfc5545#section-3.3.11"/></summary>
            [GeneratedCode("Klinkby.VCard.Generators", "1.0")]
            internal static class VCardText
            {
                private readonly static Regex _escaper = new (@"[\n\;\,\:]", 
                    RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture); 
                
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static string Escape(string value) => _escaper.Replace(value, @"\$0");    
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
              using Klinkby.VCard;

              namespace {{namespaceName}};

              partial record {{classSymbol.Name}}
              {
                  /// <inheritdoc />
                  [GeneratedCode("Klinkby.VCard.Generators", "1.0")]
                  public void WriteVCard(TextWriter writer)
                  {
                      writer.Write("BEGIN:{{classSymbol.Name.ToUpperInvariant()}}\n");

              """);

        // create properties for each field 
        foreach (var propertySymbol in properties) ProcessProperty(source, propertySymbol);

        source.AppendLine(
            $$"""
                      writer.Write("END:{{classSymbol.Name.ToUpperInvariant()}}\n");
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
                         writer.Write({propertyName}.ToString({dtFormat}CultureInfo.InvariantCulture));
                         writer.Write("\n");
                 """);
        }
        else if (fieldType.SpecialType == SpecialType.System_String)
        {
            source.AppendLine(
                string.Equals("Organizer", propertyName, StringComparison.OrdinalIgnoreCase)
                    ? $$"""
                                if (!string.IsNullOrEmpty({{propertyName}}))
                                {
                                    writer.Write("{{propertyName.ToUpperInvariant()}}:CN=\"");
                                    writer.Write(VCardText.Escape({{propertyName}}));
                                    writer.Write("\"\n");
                                }
                        """
                    : $$"""
                                if (!string.IsNullOrEmpty({{propertyName}}))
                                {
                                    writer.Write("{{propertyName.ToUpperInvariant()}}:");
                                    writer.Write(VCardText.Escape({{propertyName}}));
                                    writer.Write("\n");
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