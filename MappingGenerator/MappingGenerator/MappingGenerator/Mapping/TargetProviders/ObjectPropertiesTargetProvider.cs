using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MappingGenerator.Mapping.TargetProviders
{
    public class ObjectPropertiesTargetProvider:IMappingTargetProvider
    {
        private readonly ITypeSymbol typeSymbol;
        private readonly bool canAccessPrivate;
        private readonly bool canAccessReadoly;

        public ObjectPropertiesTargetProvider(ITypeSymbol typeSymbol, bool canAccessPrivate=false, bool canAccessReadoly=false)
        {
            this.typeSymbol = typeSymbol;
            this.canAccessPrivate = canAccessPrivate;
            this.canAccessReadoly = canAccessReadoly;
        }

        public IReadOnlyList<TargetMappingElement> GetTargets()
        {
            return ObjectHelper.GetPublicPropertySymbols(typeSymbol)
                .Where(MeetSpecification)
                .Select(p => new TargetMappingElement
                {
                    Name = p.Name,
                    Expression = SyntaxFactory.IdentifierName(p.Name),
                    ExpressionType = p.Type
                }).ToList().AsReadOnly();
        }

        private bool MeetSpecification(IPropertySymbol arg)
        {
            if (arg.SetMethod == null)
            {
                return arg.IsReadonlyProperty() && canAccessReadoly;
            }

            if (arg.SetMethod.DeclaredAccessibility != Accessibility.Public)
            {
                return canAccessPrivate;
            }

            return true;
        }
    }

    public interface IMappingTargetProvider
    {
        IReadOnlyList<TargetMappingElement> GetTargets();
    }

    public class TargetMappingElement:MappingElement
    {
        public string Name { get; set; }
    }
}
