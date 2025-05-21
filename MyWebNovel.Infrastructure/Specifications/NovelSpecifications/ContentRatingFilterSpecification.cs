using MyWebNovel.Domain.Entities.Novels;
using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.NovelSpecifications
{
    public class ContentRatingFilterSpecification(List<ContentRating> contentRatings) : BaseSpecification<Novel>
    {
        public override Expression<Func<Novel, bool>> ToExpression()
        {
            return n => contentRatings.Contains(n.ContentRating);
        }
    }

}
