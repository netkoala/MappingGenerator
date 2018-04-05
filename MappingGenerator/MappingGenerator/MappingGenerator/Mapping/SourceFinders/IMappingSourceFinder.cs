using Microsoft.CodeAnalysis;

namespace MappingGenerator
{
    public interface IMappingSourceFinder
    {
        MappingElement FindMappingSource(string targetName, ITypeSymbol targetType);
    }
}