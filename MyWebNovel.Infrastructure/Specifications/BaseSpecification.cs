using MyWebNovel.Infrastructure.Specifications.CommonSpecifcations;
using System.Linq.Expressions;

namespace MyWebNovel.Infrastructure.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        // Abstract method to be implemented by derived classes
        public abstract Expression<Func<T, bool>> ToExpression();

        // Default implementation for checking if an entity satisfies the specification
        public bool IsSatisfiedBy(T entity)
        {
            // Compile the expression into a delegate and evaluate it
            return ToExpression().Compile()(entity);
        }

        // Helper method to combine two specifications with 'AND'
        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        // Helper method to combine two specifications with 'OR'
        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        // Helper method to invert a specification with 'NOT'
        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

}
