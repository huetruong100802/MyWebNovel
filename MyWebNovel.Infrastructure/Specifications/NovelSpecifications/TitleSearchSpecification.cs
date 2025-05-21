using Microsoft.EntityFrameworkCore;
using MyWebNovel.Domain.Entities.Novels;
using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.NovelSpecifications
{
    public class TitleSearchSpecification(string keyword) : BaseSpecification<Novel>
    {
        public override Expression<Func<Novel, bool>> ToExpression()
        {
            return n => EF.Functions.Like(n.Title, $"%{keyword}%");
        }
    }

}
