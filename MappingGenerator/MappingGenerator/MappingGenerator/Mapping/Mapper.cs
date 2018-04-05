using System.Collections.Generic;
using MappingGenerator.Mapping.TargetProviders;
using Microsoft.CodeAnalysis;

namespace MappingGenerator.Mapping
{
    static class Mapper
    {
        public static IEnumerable<MappedUnit> Map(IMappingSourceFinder sourceFinder, IMappingTargetProvider targetProvider)
        {
            foreach (var targetElement in targetProvider.GetTargets())
            {
                yield return new MappedUnit
                {
                    Target = targetElement,
                    Source = sourceFinder.FindMappingSource(targetElement.Name, targetElement.ExpressionType)
                };
            }
        }

    }

    public class MappedUnit
    {
        public MappingElement Source { get; set; }
        public MappingElement Target { get; set; }
    }

    public class MethodImplementationGenerator
    {
        private readonly SyntaxNode globalSourceAccessor;
        private readonly SyntaxNode globbalTargetAccessor;

        public MethodImplementationGenerator(SyntaxNode globalSourceAccessor, SyntaxNode globbalTargetAccessor = null)
        {
            this.globalSourceAccessor = globalSourceAccessor;
            this.globbalTargetAccessor = globbalTargetAccessor;
        }
    }
}
