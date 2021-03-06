﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MappingGenerator.MethodHelpers
{
    public static class MethodHelper
    {
        public static IEnumerable<ImmutableArray<IParameterSymbol>> GetOverloadParameterSets(IMethodSymbol methodSymbol, SemanticModel semanticModel)
        {
            var overloadParameterSets = methodSymbol.DeclaringSyntaxReferences.Select(ds =>
            {
                var overloadDeclaration = (MethodDeclarationSyntax) ds.GetSyntax();
                var overloadMethod = semanticModel.GetDeclaredSymbol(overloadDeclaration);
                return overloadMethod.Parameters;
            });
            return overloadParameterSets;
        }
       

        public static MatchedParameterList FindBestParametersMatch(IMappingSourceFinder mappingSourceFinder, IEnumerable<ImmutableArray<IParameterSymbol>> overloadParameterSets)
        {
            return overloadParameterSets.Select(x=> FindArgumentsMatch(x, mappingSourceFinder))
                .Where(x=>x.HasAnyMatch())
                .OrderByDescending(x=> x.IsCompletlyMatched())
                .ThenByDescending(x => x.MatchedCount)
                .FirstOrDefault();
        }

        private static MatchedParameterList FindArgumentsMatch(ImmutableArray<IParameterSymbol> parameters, IMappingSourceFinder mappingSourceFinder)
        {
            var matchedArgumentList = new MatchedParameterList();
            foreach (var parameter in parameters)
            {
                var mappingSource = mappingSourceFinder.FindMappingSource(parameter.Name, parameter.Type);
                matchedArgumentList.AddMatch(parameter, mappingSource?.Expression);
            }
            return matchedArgumentList;
        }
    }
}