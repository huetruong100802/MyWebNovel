using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications.CommonSpecifcations
{
    internal class NotSpecification<T>(ISpecification<T> specification) : ISpecification<T>
    {
        private readonly ISpecification<T> _specification = specification;
        public Expression<Func<T, bool>> ToExpression()
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(_specification.ToExpression().Body),
                _specification.ToExpression().Parameters
            );
        }
    }
}