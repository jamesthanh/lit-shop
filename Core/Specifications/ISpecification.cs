using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        // Object is the most generic things we can use
        List<Expression<Func<T, object>>> Includes { get; }
    }
}