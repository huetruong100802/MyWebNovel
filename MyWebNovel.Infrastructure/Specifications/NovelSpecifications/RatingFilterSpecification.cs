using MyWebNovel.Domain.Entities.Novels;
using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.NovelSpecifications
{
    public class RatingFilterSpecification(decimal minRating) : BaseSpecification<Novel>
    {
        public override Expression<Func<Novel, bool>> ToExpression()
        {
            return n => n.Rating >= minRating;
        }
    }

}
