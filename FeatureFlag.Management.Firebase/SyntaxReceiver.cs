using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureFlag.Management.Firebase
{
    internal sealed class SyntaxReceiver : ISyntaxReceiver
    {
        private List<ClassDeclarationSyntax> _classes = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode context)
        {
            if (!(context is ClassDeclarationSyntax classDeclarationSyntax))
                return;

            _classes.Add(classDeclarationSyntax);
        }

        public List<IPropertySymbol> GetProperties(Func<SyntaxTree, SemanticModel> semanticModelGetter)
        {
            var properties = new List<IPropertySymbol>();

            foreach (var classDeclarationSyntax in _classes)
            {
                var semanticModel = semanticModelGetter?.Invoke(classDeclarationSyntax.SyntaxTree);
                if (semanticModel is null)
                    continue;

                if (!(semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is INamedTypeSymbol namedSymbol))
                    continue;

                if (!InheritsFrom(namedSymbol, "FeatureFlag.Management.IFeature"))
                    continue;

                foreach (var member in classDeclarationSyntax.Members)
                {
                    if (!(member is PropertyDeclarationSyntax p))
                        continue;

                    if (semanticModel.GetDeclaredSymbol(p) is IPropertySymbol propertySymbol)
                        properties.Add(propertySymbol);
                }
            }

            return properties;
        }

        private bool InheritsFrom(INamedTypeSymbol symbol, string fullnameType)
        {
            while (true)
            {
                if (symbol.AllInterfaces.Any(i => i.ToString() == fullnameType))
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
