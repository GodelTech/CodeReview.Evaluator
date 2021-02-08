using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Services.LocDetailsFilters;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class LocDetailsFilterFactory : ILocDetailsFilterFactory
    {
        public ILocDetailsFilter Create(ScopeManifest manifest)
        {
            if (manifest == null)
                throw new ArgumentNullException(nameof(manifest));

            return new CompositeFilter(
                false,
                CreateFilterFromManifest(true, manifest.Include),
                new NotFilter(CreateFilterFromManifest(false, manifest.Exclude)));
        }

        private static ILocDetailsFilter CreateFilterFromManifest(bool resultOnEmptyFilterList, FilterManifest manifest)
        {
            var filter = new List<ILocDetailsFilter>();

            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.FilePath, x => x.FilePath));

            if (!filter.Any())
                return new ConstantFilter(resultOnEmptyFilterList);

            return new CompositeFilter(true, filter.ToArray());
        }

        private static IEnumerable<ILocDetailsFilter> CreateFiltersFromFilterDefinitions(IEnumerable<FilterDefinition> definitions, Func<FileLocDetails, string> valueSelector)
        {
            foreach (var definition in definitions)
            {
                Func<string, bool> predicate = x => (x ?? string.Empty).Equals(definition.Pattern, StringComparison.OrdinalIgnoreCase);

                if (definition.IsRegex)
                {
                    var regex = new Regex(definition.Pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    predicate = x => regex.IsMatch(x ?? string.Empty);
                }

                yield return new GenericFilter<string>(valueSelector, predicate);
            }
        }
    }
}