using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
        bool IsSatisfiedBy(T entity) => ToExpression().Compile()(entity);
    }

}
