

using System.Linq.Expressions;

namespace Core.Application.Specifications;

public abstract class Specification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria) => Criteria = criteria;
    protected void AddInclude(Expression<Func<T, object>> include) => Includes.Add(include);
    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;
    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc) => OrderByDescending = orderByDesc;
}