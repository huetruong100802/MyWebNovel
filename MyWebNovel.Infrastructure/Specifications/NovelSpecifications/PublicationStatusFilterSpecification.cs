using MyWebNovel.Domain.Entities.Novels;
using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.NovelSpecifications
{
    public class PublicationStatusFilterSpecification(List<PublicationStatus> publicationStatuses) : BaseSpecification<Novel>
    {
        public override Expression<Func<Novel, bool>> ToExpression()
        {
            return n => publicationStatuses.Contains(n.PublicationStatus);
        }
    }

}
