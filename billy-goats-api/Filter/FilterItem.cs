using System;
using System.Linq;
using System.Linq.Expressions;

namespace BillyGoats.Api.Filter
{
    public class FilterItem
    {
        public FilterItem Right { get; set; }

        public string Field { get; set; }

        public FilterOperations Operator { get; set; } = FilterOperations.Eq;

        public object Value { get; set; }

        public LogicalOperations LogicOperator { get; set; } = LogicalOperations.AND;

        /// <summary>
        /// Item to expression. 
        /// </summary>
        /// <returns>The expression of the single item </returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private Expression ItemToExpression(ParameterExpression source)
        {
            if (this.Field == null)
            {
                return null;
            }
            return ExpressionEx.FilterByProperty(source, this.Field, this.Operator, this.Value);
        }

        /// <summary>
        /// ToExpression.
        /// </summary>
        /// <returns>The final expression of this item with the RightItem </returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public Expression ToExpression(ParameterExpression source)
        {
            var left = this.ItemToExpression(source);
           
            if (Right == null)
            {
                return left;
            }
            var lo = this.LogicOperator;
            while (lo == LogicalOperations.AND && this.Right != null)
            {
                var right = this.Right.ItemToExpression(source);
                if (right == null)
                    return left;

                left = left != null ? Expression.AndAlso(left, right) : right;

                lo = this.Right.LogicOperator;
                this.Right = this.Right.Right;
            }
            if (lo == LogicalOperations.OR && this.Right != null)
            {
                var right = this.Right.ToExpression(source);
                if (right == null)
                    return left;

                left = left != null ? Expression.Or(left, right) : right;
            }
            return left;
        }

        public Expression<Func<T, bool>> ToLamda<T>()
        {
            var source = Expression.Parameter(typeof(T), "p");
            var exp = this.ToExpression(source);
            if (exp == null)
                return null;
            return Expression.Lambda<Func<T, bool>>(exp, source);
        }
       
        public IQueryable<T> FilterIQueryable<T>(IQueryable<T> inputQuery) where T : class
        {
            var expression = this.ToLamda<T>();
            if (expression != null)
            {
                return inputQuery.Where(expression);
            }
            else
            {
                return inputQuery;
            }
        }
    }
}
