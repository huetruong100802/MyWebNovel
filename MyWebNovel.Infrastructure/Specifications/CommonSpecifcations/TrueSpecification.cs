using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.CommonSpecifcations
{
    public class TrueSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> ToExpression()
        {
            return _ => true;
        }
    }

}
