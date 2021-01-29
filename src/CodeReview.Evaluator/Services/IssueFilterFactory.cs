using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ReviewItEasy.Evaluator.Models;

namespace ReviewItEasy.Evaluator.Services
{
    public class IssueFilterFactory : IIssueFilterFactory
    {
        public IIssueFilter Create(ScopeManifest manifest)
        {
            if (manifest == null) 
                throw new ArgumentNullException(nameof(manifest));


            return new CompositeIssueFilter(
                false,
                CreateFilterFromManifest(true, manifest.Include),
                new NotFilter(CreateFilterFromManifest(false, manifest.Exclude)));
        }

        private static IIssueFilter CreateFilterFromManifest(bool resultOnEmptyFilterList, FilterManifest manifest)
        {
            var filter = new List<IIssueFilter>();

            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.RuleId, x => x.RuleId));
            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.Level, x => x.Level));
            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.Title, x => x.Title));
            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.Message, x => x.Message));
            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.Description, x => x.Description));
            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.DetailsUrl, x => x.DetailsUrl));
            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.Category, x => x.Category));
            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.Tag, x => x.Tags));
            filter.AddRange(CreateFiltersFromFilterDefinitions(manifest.FilePath, x => x.Locations));

            if (!filter.Any())
                return new ConstantFilter(resultOnEmptyFilterList);
            
            return new CompositeIssueFilter(true, filter.ToArray());
        }

        private static IEnumerable<IIssueFilter> CreateFiltersFromFilterDefinitions(IEnumerable<FilterDefinition> definitions, Func<Issue, IssueLocation[]> valueSelector)
        {
            foreach (var definition in definitions)
            {
                Func<IssueLocation[], bool> predicate = x => x.Any(p => (p.FilePath ?? string.Empty).Equals(definition.Pattern, StringComparison.OrdinalIgnoreCase));

                if (definition.IsRegex)
                {
                    var regex = new Regex(definition.Pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    predicate = x => x.Any(p => regex.IsMatch(p.FilePath ?? string.Empty));
                }

                yield return new GenericIssueFilter<IssueLocation[]>(valueSelector, predicate);
            }
        }

        private static IEnumerable<IIssueFilter> CreateFiltersFromFilterDefinitions(IEnumerable<FilterDefinition> definitions, Func<Issue, string[]> valueSelector)
        {
            foreach (var definition in definitions)
            {
                Func<string[], bool> predicate = x => x.Any(p => (p ?? string.Empty).Equals(definition.Pattern, StringComparison.OrdinalIgnoreCase));

                if (definition.IsRegex)
                {
                    var regex = new Regex(definition.Pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    predicate = x => x.Any(p => regex.IsMatch(p ?? string.Empty));
                }

                yield return new GenericIssueFilter<string[]>(valueSelector, predicate);
            }
        }

        private static IEnumerable<IIssueFilter> CreateFiltersFromFilterDefinitions(IEnumerable<FilterDefinition> definitions, Func<Issue, string> valueSelector)
        {
            foreach (var definition in definitions)
            {
                Func<string, bool> predicate = x => (x ?? string.Empty).Equals(definition.Pattern, StringComparison.OrdinalIgnoreCase);
                
                if (definition.IsRegex)
                {
                    var regex = new Regex(definition.Pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    predicate = x => regex.IsMatch(x ?? string.Empty);
                }

                yield return new GenericIssueFilter<string>(valueSelector, predicate);
            }
        }
    }
}
