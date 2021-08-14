using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeatureFlag.Management.Firebase
{
    [Generator]
    public class RemoteConfigDefaultValueGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // retrieve the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;

            receiver.SetSemanticModelGetter(a => context.Compilation.GetSemanticModel(a));

            string classSource = ProcessClass(receiver.Properties, context);
            context.AddSource("RemoteConfigDefaultValues.cs", SourceText.From(classSource, Encoding.UTF8));
        }

        private string ProcessClass(List<IPropertySymbol> properties, GeneratorExecutionContext context)
        {
            var defaultValueAttributeSymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.DefaultValueAttribute");
            var nameAttributeSymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.DisplayNameAttribute");

            StringBuilder source = new StringBuilder($@"
namespace FeaturueFlag.Management.Firebase
{{
    public static class RemoteConfig
    {{

        public static Dictionary<string, object> DefaultValues = new Dictionary<string, object>()
        {{");

            var isFirst = true;
            foreach (var property in properties)
            {
                source.Append($@"        {(isFirst ? string.Empty: ",")}{{ {GetPropertyName(property, nameAttributeSymbol)}, {GetDefaultValue(property, defaultValueAttributeSymbol)} }}");
                isFirst = false;
            }
                
            source.Append(@"        }}
    };

    }}
}}
");

            return source.ToString();
        }

        private string GetPropertyName(IPropertySymbol property, ISymbol attributeSymbol)
        {
            var attributeData = property.GetAttributes().FirstOrDefault(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
            var overridenValueOpt = attributeData?.NamedArguments.FirstOrDefault(kvp => kvp.Key == "DisplayName").Value;

            if (overridenValueOpt.HasValue && !overridenValueOpt.Value.IsNull)
            {
                overridenValueOpt.Value.Value.ToString();
            }

            return property.Name;
        }

        private string GetDefaultValue(IPropertySymbol property, ISymbol attributeSymbol)
        {
            var attributeData = property.GetAttributes().FirstOrDefault(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
            var overridenValueOpt = attributeData?.NamedArguments.FirstOrDefault(kvp => kvp.Key == "Value").Value;

            if (overridenValueOpt.HasValue && !overridenValueOpt.Value.IsNull)
            {
                overridenValueOpt.Value.Value.ToString();
            }

            return "default";
        }

        /// <summary>
        /// Created on demand before each generation pass
        /// </summary>
        class SyntaxReceiver : ISyntaxReceiver
        {
            public List<IPropertySymbol> Properties { get; } = new List<IPropertySymbol>();

            private Func<SyntaxTree, SemanticModel> _semanticModelGetter;

            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(SyntaxNode context)
            {

                if (!(context is ClassDeclarationSyntax classDeclarationSyntax))
                    return;

                var semanticModel = _semanticModelGetter?.Invoke(classDeclarationSyntax.SyntaxTree);
                if (semanticModel is null)
                    return;

                if (!(semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is INamedTypeSymbol namedSymbol))
                    return;

                if (!InheritsFrom(namedSymbol, "FeatureFlag.Management.IFeature"))
                    return;

                foreach (var member in classDeclarationSyntax.Members)
                {
                    if (!(member is PropertyDeclarationSyntax p))
                        continue;

                    if (semanticModel.GetDeclaredSymbol(p) is IPropertySymbol propertySymbol)
                        Properties.Add(propertySymbol);
                }
            }

            public void SetSemanticModelGetter(Func<SyntaxTree, SemanticModel> p)
            {
                _semanticModelGetter = p;
            }

            private bool InheritsFrom(INamedTypeSymbol symbol, string fullnameType)
            {
                while (true)
                {
                    if (symbol.ToString() == fullnameType)
                        return true;

                    if (symbol.BaseType != null)
                    {
                        symbol = symbol.BaseType;
                        continue;
                    }

                    break;
                }

                return false;
            }
        }
    }
}
