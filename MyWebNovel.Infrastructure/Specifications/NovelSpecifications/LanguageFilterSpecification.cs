using MyWebNovel.Domain.Entities.Novels;
using MyWebNovel.Domain.Entities.Shared.ValueObjects;
using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.NovelSpecifications
{
    public class LanguageFilterSpecification(List<string> languages) : BaseSpecification<Novel>
    {
        private readonly List<Language> _languages = [.. languages.Select(Language.Create)];

        public override Expression<Func<Novel, bool>> ToExpression()
        {
            return n => _languages.Contains(n.OriginalLanguage);
        }
    }

}
