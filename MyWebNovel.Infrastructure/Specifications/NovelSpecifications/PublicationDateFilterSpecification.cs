using MyWebNovel.Domain.Entities.Novels;
using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.NovelSpecifications
{
    public class PublicationDateFilterSpecification(DateTimeOffset startDate, DateTimeOffset endDate) : BaseSpecification<Novel>
    {
        public override Expression<Func<Novel, bool>> ToExpression()
        {
            return n => n.PublicationDate >= startDate && n.PublicationDate <= endDate;
        }
    }

}
