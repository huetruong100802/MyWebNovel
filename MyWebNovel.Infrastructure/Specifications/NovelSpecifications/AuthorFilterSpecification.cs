using MyWebNovel.Domain.Entities.Novels;
using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.NovelSpecifications
{
    public class AuthorFilterSpecification(Guid authorId) : BaseSpecification<Novel>
    {
        public override Expression<Func<Novel, bool>> ToExpression()
        {
            return n => n.AuthorId == authorId;
        }
    }

}
