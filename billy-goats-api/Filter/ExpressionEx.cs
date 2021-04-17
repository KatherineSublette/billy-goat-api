using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BillyGoats.Api.Utils.Extensions;
using Newtonsoft.Json;

namespace BillyGoats.Api.Filter
{

    public static class ExpressionEx
    {
        /// <summary>
        /// Convert the FilterOperations To ExpressionType
        /// </summary>
        /// <returns>The expression.</returns>
        /// <param name="fo">Fo.</param>
        public static ExpressionType ToExpression(this FilterOperations fo)
        {
            if (fo == FilterOperations.Eq)
                return ExpressionType.Equal;
            if (fo == FilterOperations.Gt)
                return ExpressionType.GreaterThan;
            if (fo == FilterOperations.Gte)
                return ExpressionType.GreaterThanOrEqual;
            if (fo == FilterOperations.Lt)
                return ExpressionType.LessThan;
            if (fo == FilterOperations.Lte)
                return ExpressionType.LessThanOrEqual;
            if (fo == FilterOperations.Neq)
                return ExpressionType.NotEqual;

            return ExpressionType.New;
        }

        /// <summary>
        /// Value to Expression based on propType
        /// </summary>
        /// <returns>The to expression.</returns>
        /// <param name="itemValue">Item value.</param>
        /// <param name="propType">Property type.</param>
        public static Expression ValueToExpression(object itemValue, Type propType)
        {
            if (itemValue == null || itemValue.ToString() == "null")
                return Expression.Constant(null);
            Type pType = propType;
            if (propType.IsArray)
            {
                pType = propType.GetElementType();
            }
            TypeConverter conv = TypeDescriptor.GetConverter(pType);
            Expression valExpr = Expression.Constant(conv.ConvertFrom(itemValue));
            return Expression.Convert(valExpr, pType);
        }

        /// <summary>
        /// Array values to expression based on propType
        /// </summary>
        /// <returns>The value to expression.</returns>
        /// <param name="arrayVal">Item value.</param>
        /// <param name="propType">Property type.</param>
        public static Expression ArrayValueToExpression(object[] arrayVal, Type propType)
        {
            var pType = propType;
            if (pType.IsArray)
                pType = propType.GetElementType();

            TypeConverter conv = TypeDescriptor.GetConverter(pType);
            Expression valueExpr = Expression.Constant(arrayVal);

            if (pType == typeof(Guid))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (Guid)conv.ConvertFrom(p)));
            }

            if (pType == typeof(Guid?))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (Guid?)conv.ConvertFrom(p)));
            }

            if (pType == typeof(bool))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (bool)conv.ConvertFrom(p)));
            }

            if (pType == typeof(bool?))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (bool?)conv.ConvertFrom(p)));
            }

            if (pType == typeof(long))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (long)conv.ConvertFrom(p)));
            }

            if (pType == typeof(long?))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (long?)conv.ConvertFrom(p)));
            }

            if (pType == typeof(double))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (double)conv.ConvertFrom(p)));
            }

            if (pType == typeof(double?))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (double?)conv.ConvertFrom(p)));
            }

            if (pType == typeof(int))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (int)conv.ConvertFrom(p)));
            }

            if (pType == typeof(int?))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (int?)conv.ConvertFrom(p)));
            }

            if (pType == typeof(DateTimeOffset))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (DateTimeOffset)conv.ConvertFrom(p)));
            }

            if (pType == typeof(DateTimeOffset?))
            {
                valueExpr = Expression.Constant(arrayVal.Select(p => (DateTimeOffset?)conv.ConvertFrom(p)));
            }

            return valueExpr;
        }

        /// <summary>
        /// Get chained predicate: left AND right
        /// </summary>
        /// <typeparam name="T">The type of the Target object the filer acts on</typeparam>
        /// <param name="left">The first predicate</param>
        /// <param name="right">The second predicate</param>
        /// <returns>Chained predicate: left AND right</returns>
        //public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        //{
        //    var param = left.Parameters[0];
        //    if (ReferenceEquals(param, right.Parameters[0]))
        //    {
        //        // simple version
        //        return Expression.Lambda<Func<T, bool>>(
        //            Expression.AndAlso(left.Body, right.Body), param);
        //    }

        //    // otherwise, keep expr1 "as is" and invoke expr2
        //    return Expression.Lambda<Func<T, bool>>(
        //        Expression.AndAlso(
        //            left.Body,
        //            Expression.Invoke(right, param)), param);
        //}

        /// <summary>
        /// Get chained predicate: left OR right
        /// </summary>
        /// <typeparam name="T">The type of the Target object the filer acts on</typeparam>
        /// <param name="left">The first predicate</param>
        /// <param name="right">The second predicate</param>
        /// <returns>Chained predicate: left OR right</returns>
        //public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        //{
        //    var param = left.Parameters[0];
        //    if (ReferenceEquals(param, right.Parameters[0]))
        //    {
        //        // simple version
        //        return Expression.Lambda<Func<T, bool>>(
        //            Expression.OrElse(left.Body, right.Body), param);
        //    }

        //    // otherwise, keep expr1 "as is" and invoke expr2
        //    return Expression.Lambda<Func<T, bool>>(
        //        Expression.OrElse(
        //            left.Body,
        //            Expression.Invoke(right, param)), param);
        //}

        /// <summary>
        /// GetPropertyExpression
        /// </summary>
        /// <typeparam name="T">Type of the Target Object</typeparam>
        /// <param name="propertyName"></param>
        /// <returns>Lambda Expression: o = > o.PropertyName </returns>
        public static Expression<Func<T, object>> GetPropertyExpression<T>(string propertyName)
        {
            // Manually build the expression tree for
            // the lambda expression o => o.PropertyName.
            // (T o) =>
            try
            {
                var source = Expression.Parameter(typeof(T), "p");
                Expression propProperty = source;
                var props = propertyName.Split('.');
                foreach (var member in props)
                {
                    propProperty = Expression.PropertyOrField(propProperty, member);
                    var t = propProperty.Type;
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    {
                        var itemName = props.Last();
                        Type propType = typeof(string);
                        var s = itemName.Split("|");
                        var key = s[0];
                        if (s.Length > 1)
                        {
                            if (s[1] == "date")
                                propType = typeof(DateTime?);
                            if (s[1] == "numeric")
                                propType = typeof(double?);
                        }
                        var keyExpr = Expression.Constant(key);
                        propProperty = Expression.Property(propProperty, "item", keyExpr);
                        propProperty = Expression.TypeAs(propProperty, propType);
                        break;
                    }
                }

                propProperty = Expression.Convert(propProperty, typeof(object));
                return Expression.Lambda<Func<T, object>>(propProperty, source);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public static Expression FilterByProperty(
            ParameterExpression source,
            string propName,
            FilterOperations filterOperator,
            object itemValue)
        {
            if (propName.StartsWith("IdList", StringComparison.OrdinalIgnoreCase))
            {
                // idList='filterIdentifier'
                // the filter 'filterIdentifier' && createdBy currentUser
                // will be handled in the service
                return null;
            }

            if (propName == "Q" || propName.StartsWith("Q(", StringComparison.OrdinalIgnoreCase))
            {
                // q is special filter item, it is for search all string field contains value
                // Q(fieldName1, fieldName2, ...) is also valid in order to search specified fields only
                string[] fields = { };

                if (propName.StartsWith("Q(", StringComparison.OrdinalIgnoreCase))
                {
                    // strip off Q(
                    string fieldstring = propName.Substring(2);

                    // strip off )
                    fieldstring = fieldstring.Substring(0, fieldstring.Length - 1);
                    fields = fieldstring.Split(',');

                    // run though field list, trim and proper case
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (!fields[i].Contains("."))
                            fields[i] = fields[i].Trim().ToTitleCase();
                    }
                }
                return StringContainsValue(source, itemValue, fields);
            }

            Expression propExpr = source;
            Expression expr = null;
            Expression propNotNull = null;
            var members = propName.Split('.');
            string itemName = members.Last();
          
            foreach (var member in members)
            {
                // if the property cannot be found, return null and give the service a chance to
                // provide a custom expression
                try
                {
                    propExpr = Expression.PropertyOrField(propExpr, member);
                }
                catch (ArgumentException)
                {
                    return null;
                }
                catch (Exception e)
                {
                    throw e;
                }                

                var t = propExpr.Type;
                if (filterOperator == FilterOperations.Ct && t == typeof(string) && itemValue.GetType() != typeof(string[]))
                {
                    return StringContainsValue(source, itemValue, new string[] { propName });
                }

                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>))
                {
                    if (itemValue is FilterItem)
                    {
                        var itemType = propExpr.Type.GetInterfaces()
                        .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .GetGenericArguments()[0];

                        var s = Expression.Parameter(itemType, "e");
                        var exp = (itemValue as FilterItem).ToExpression(s);
                        var predicate = Expression.Lambda(exp, s);
                        expr = ChildrenHaveAny(propExpr, predicate);
                    }
                    else
                        expr = ChildrenHaveAny(propExpr, itemName, filterOperator, itemValue);
                    break;
                }
                if (members.Length > 1)
                {
                    // navigation property
                    var pNotNull = Expression.NotEqual(propExpr, Expression.Constant(null));
                    if (propNotNull == null)
                    {
                        propNotNull = pNotNull;
                    }
                    else
                        propNotNull = Expression.AndAlso(propNotNull, pNotNull);
                }
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    expr = DictionaryContainsItem(propExpr, itemName, filterOperator, itemValue);
                    expr = Expression.AndAlso(propNotNull, expr);
                    break;
                }

                if (propExpr.Type.IsArray)
                {
                    expr = ArrayContainsValue(propExpr, itemValue);
                    break;
                }
            }
            if(expr == null)
                expr = MakePropertyValueComparison(propExpr, filterOperator, itemValue);

            return expr;
        }

        public static Expression<Func<T, bool>> FilterByProperty<T>(
            string propName,
            FilterOperations filterOperator,
            object itemValue)
        {
            var source = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, bool>>(FilterByProperty(source, propName, filterOperator, itemValue), source);
        }

        /// <summary>
        /// MakePropertyValueComparison
        /// </summary>
        /// <returns>The property value comparison.</returns>
        /// <param name="propExpr">Property expr.</param>
        /// <param name="filterOperator">Filter operator.</param>
        /// <param name="propValue">Property value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Expression MakePropertyValueComparison(Expression propExpr, FilterOperations filterOperator, object propValue)
        {
            Type propType = propExpr.Type;

            Expression valueExpr = Expression.Constant(propValue);
            Expression expr;

            if (filterOperator != FilterOperations.Ct && filterOperator != FilterOperations.Nct)
            {
                valueExpr = ValueToExpression(propValue, propType);
            }
            else if (propValue as object[] != null)
            {
                valueExpr = ArrayValueToExpression(propValue as object[], propType);
                expr = ExpressionEx.ArrayContains(propExpr, valueExpr);
                return expr;
            }
            switch (filterOperator)
            {
                case FilterOperations.Eq:
                    expr = Expression.Equal(propExpr, valueExpr);
                    break;

                case FilterOperations.Neq:
                    expr = Expression.NotEqual(propExpr, valueExpr);
                    break;

                case FilterOperations.Ct:
                    expr = ExpressionEx.Contains(propExpr, valueExpr);
                    break;

                case FilterOperations.Nct:
                    expr = Expression.Not(ExpressionEx.Contains(propExpr, valueExpr));
                    break;

                case FilterOperations.Sw:
                    expr = ExpressionEx.StartsWith(propExpr, valueExpr);
                    break;

                case FilterOperations.Ew:
                    expr = ExpressionEx.EndsWith(propExpr, valueExpr);
                    break;

                case FilterOperations.Gt:
                    expr = Expression.GreaterThan(propExpr, valueExpr);
                    break;

                case FilterOperations.Gte:
                    expr = Expression.GreaterThanOrEqual(propExpr, valueExpr);
                    break;

                case FilterOperations.Lt:
                    expr = Expression.LessThan(propExpr, valueExpr);
                    break;

                case FilterOperations.Lte:
                    expr = Expression.LessThanOrEqual(propExpr, valueExpr);
                    break;

                default:
                    throw new NotSupportedException(
                        string.Format("Filter operator '{0}' is not supported yet.",
                            filterOperator.ToString()));
            }
            return expr;
        }

        /// <summary>
        /// MakeDictionayValueComparison
        /// </summary>
        /// <returns>The dictionay value comparison.</returns>
        /// <param name="propExpr">Property expr.</param>
        /// <param name="filterOperator">Filter operator.</param>
        /// <param name="propValue">Property value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static Expression MakeDictionayValueComparison(Expression propExpr, FilterOperations filterOperator, object propValue)
        {
            Expression valueExpr = Expression.Constant(propValue);
            Expression expr;
            if (propValue == null)
            {
                return filterOperator == FilterOperations.Eq ? Expression.Equal(propExpr, valueExpr) : Expression.NotEqual(propExpr, valueExpr);
            }

            string val = propValue.ToString();
            Type propType = propExpr.Type;
            if (propValue is Array)
            {
                val = (propValue as Array).GetValue(0).ToString();
                if (val == string.Empty && ((Array)propValue).Length > 1)
                    val = (propValue as Array).GetValue(1).ToString();
            }

            if (propType == typeof(string) && filterOperator == FilterOperations.Eq)
            {
                filterOperator = FilterOperations.Ct;
            }

            if (filterOperator != FilterOperations.Ct && filterOperator != FilterOperations.Nct)
            {
                // Creates (x.Value as propType) 
                valueExpr = ValueToExpression(propValue, propType);
            }
            else
            {
                if (propValue as object[] != null)
                {
                    valueExpr = ArrayValueToExpression(propValue as object[], propType);
                    return ExpressionEx.ArrayContains(propExpr, valueExpr);
                }
            }

            switch (filterOperator)
            {
                case FilterOperations.Eq:
                    expr = Expression.Equal(propExpr, valueExpr);
                    break;

                case FilterOperations.Neq:
                    expr = Expression.NotEqual(propExpr, valueExpr);
                    break;

                case FilterOperations.Ct:
                    expr = ExpressionEx.Contains(propExpr, valueExpr);
                    break;

                case FilterOperations.Nct:
                    expr = Expression.Not(ExpressionEx.Contains(propExpr, valueExpr));
                    break;

                case FilterOperations.Sw:
                    expr = ExpressionEx.StartsWith(propExpr, valueExpr);
                    break;

                case FilterOperations.Ew:
                    expr = ExpressionEx.EndsWith(propExpr, valueExpr);
                    break;

                case FilterOperations.Gt:
                    expr = Expression.GreaterThan(propExpr, valueExpr);
                    break;

                case FilterOperations.Gte:
                    expr = Expression.GreaterThanOrEqual(propExpr, valueExpr);
                    break;

                case FilterOperations.Lt:
                    expr = Expression.LessThan(propExpr, valueExpr);
                    break;

                case FilterOperations.Lte:
                    expr = Expression.LessThanOrEqual(propExpr, valueExpr);
                    break;

                default:
                    throw new NotSupportedException(
                        string.Format("Filter operator '{0}' is not supported yet.",
                            filterOperator.ToString()));
            }

            return expr;
        }

        /// <summary>
        /// FilterByDictionayProperty
        /// Original idea: https://stackoverflow.com/questions/32901989/dynamic-linq-function-using-dictionary-as-a-parameter-to-filter-method
        /// </summary>
        /// <returns>The by dictionay property.</returns>
        /// <param name="propName">Property name.</param>
        /// <param name="itemName">Item name.</param>
        /// <param name="filterOperation">Filter operation.</param>
        /// <param name="itemValue">Item value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Expression FilterByDictionayProperty(
            ParameterExpression source,
            string propName,
            string itemName,
            FilterOperations filterOperation,
            object itemValue, bool stringSearch = false)
        {
            Expression propProperty = source;
            Expression notNUllExp = null;
            foreach (var member in propName.Split('.'))
            {
                if (member == itemName)
                    break;

                propProperty = Expression.PropertyOrField(propProperty, member);

                // p != null && p.Data != null
                if (notNUllExp == null)
                {
                    notNUllExp = Expression.NotEqual(propProperty, Expression.Constant(null));
                }
                else
                    notNUllExp = Expression.AndAlso(notNUllExp, Expression.NotEqual(propProperty, Expression.Constant(null)));
            }

            var comparison = DictionaryContainsItem(propProperty, itemName, filterOperation, itemValue);

            var itemExpr = Expression.AndAlso(notNUllExp, comparison);

            return itemExpr;
        }

        private static Expression DictionaryContainsItem(Expression propExpr,
            string itemName,
            FilterOperations filterOperation,
            object itemValue)
        {
            // Get the one defined as Any<TSource>(IEnumerable<TSource>, Func<TSource, bool>)
            var anyMethodInfo = typeof(Enumerable).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Where(m => m.Name == "Any" && m.IsGenericMethod) // Search for Any methods...
                .Select(m => new
                {
                    Method = m,
                    Params = m.GetParameters(),
                    Args = m.GetGenericArguments()
                })
                .Where(x => x.Args.Length == 1
                    && x.Params.Length == 2
                    && x.Params[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(x.Args)
                    && x.Params[1].ParameterType == typeof(Func<,>).MakeGenericType(new Type[] { x.Args.First(), typeof(bool) }))
                .Select(x => x.Method)
                .First();

            Type propType = typeof(string);
            var s = itemName.Split("|");
            var key = s[0];
            if(s.Length > 1)
            {
                if (s[1] == "date" && itemValue.CanConvertTo(typeof(DateTime?)))
                {
                    propType = typeof(DateTime?);
                    if (filterOperation == FilterOperations.Ct)
                        filterOperation = FilterOperations.Eq;
                }
                if (s[1] == "numeric" && itemValue.CanConvertTo(typeof(double?)))
                {
                    propType = typeof(double?);
                    if (filterOperation == FilterOperations.Ct)
                        filterOperation = FilterOperations.Eq;
                }
             }

            // Creates x Parameter
            var keyValuePairP = Expression.Parameter(typeof(KeyValuePair<string, object>), "x");

            // Creates x.Key
            var KeyProp = Expression.Property(keyValuePairP, "Key");

            // Creates the value that will be compared to x.Key
            var keyComparisonValue = Expression.Constant(key);

            // Creates the comparison (x.Key == key)
            var keyComparison = Expression.Equal(KeyProp, keyComparisonValue);

            // Creates x.Value
            Expression valueProp = Expression.Property(keyValuePairP, "Value");
            valueProp = Expression.TypeAs(valueProp, propType);

            Expression valueComparison = null;
            if (filterOperation != FilterOperations.HasKey && filterOperation != FilterOperations.NoKey)
            {
                // Creates the comparison (x.Value == "XXX")  
                valueComparison = MakeDictionayValueComparison(valueProp, filterOperation, itemValue);
            }

            // Creates x => x.Key == itemName
            var anyPredicate = Expression.Lambda(keyComparison, new ParameterExpression[] { keyValuePairP });

            // Creates x => x.Key == itemName && (x.Value as ItemType) == "XXX"
            if (valueComparison != null)
                anyPredicate = Expression.Lambda(Expression.AndAlso(keyComparison, valueComparison), new ParameterExpression[] { keyValuePairP });

            Expression anyExpr = Expression.Call(
                        anyMethodInfo.MakeGenericMethod(new Type[] { typeof(KeyValuePair<string, object>) }),
                                                        new Expression[] { propExpr, anyPredicate }
                        );

            if (filterOperation == FilterOperations.NoKey)
            {
                anyExpr = Expression.Not(anyExpr);
            }

            // return expression
            // p.Data.Any(x => x.Key == itemName && (x.Value as itemType) == >= <= ... itemValue))
            return anyExpr;
        }

        private static Expression ArrayContainsValue(
           Expression propExpr,
           object propValue)
        {
            var propType = propExpr.Type;
            Expression expr = null;
            if (propValue as object[] == null)
            {
                var valueExpr = ValueToExpression(propValue, propType);
                expr = ExpressionEx.ArrayContains(propExpr, valueExpr);
            }
            else
            {
                foreach (var val in propValue as object[])
                {
                    var valueExpr = ValueToExpression(val, propType);
                    var itemExpr = ExpressionEx.ArrayContains(propExpr, valueExpr);

                    if (expr == null)
                        expr = itemExpr;
                    else
                        expr = Expression.Or(expr, itemExpr);
                }
            }
            return expr;
        }

        //public static Expression FilterByNavigationProperty(
        //    ParameterExpression source,
        //    string propName,
        //    FilterOperations filterOperation,
        //    object itemValue)
        //{
        //    var s = propName.Split(".");

        //    Expression propProperty = source;
        //    foreach (var member in propName.Split('.'))
        //    {
        //        propProperty = Expression.PropertyOrField(propProperty, member);

        //        var t = propProperty.Type;
        //        if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>))
        //        {
        //            return FilterByDictionayProperty(source, propName, propName.Split('.').Last(), filterOperation, itemValue);
        //        }
        //    }

        //    if (propProperty.Type.IsArray)
        //    {
        //        return ArrayContainsValue(propProperty, itemValue);
        //    }

        //    return MakePropertyValueComparison(propProperty, filterOperation, itemValue);

        //}

        /// <summary>
        ///  ChildrenCountPredicate
        /// </summary>
        /// <typeparam name="T">T is the type of the Target object the filer acts on</typeparam>
        /// <param name="collection">Collection Property Expression.</param>
        /// <param name="itemName">Property Name on the Children Object</param>
        /// <param name="itemComparison">Comparison Operator: ExpressionType.Equal</param>
        /// <param name="itemValue">Comparison Value</param>
        /// <param name="countComparison">Count Comparison: ExpressionType.GreaterThan, ExpressionType.LessThan etc.</param>
        /// <param name="countValue">Count Comparison Value</param>
        /// <returns>Expression</returns>
        /// <source>https://www.codesd.com/item/expression-trees-filtered-account-on-navigation-property.html</source>
        /// <usage>
        ///  Usage: db.Event.Where(s => s.Attendees.Count(a => a.CancelledOn == null) > 10);
        ///  var predicate = MakeCountPredicate<Event>("Attendees","CancelledOn", ExpressionType.Equal, "null", ExpressionType.GreaterThan, 10);
        ///  </usage>
        public static Expression<Func<T, bool>> ChildrenCountPredicate<T>(
            string propName,
            string itemName,
            FilterOperations itemComparison,
            object itemValue,
            FilterOperations countComparison,
            int countValue)
        {
            var source = Expression.Parameter(typeof(T), "p");

            Expression propExpr = source;
            foreach (var member in propName.Split('.'))
            {
                propExpr = Expression.PropertyOrField(propExpr, member);

                var t = propExpr.Type;
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>))
                {
                    var expr = ChildrenAggregationFunc(Aggregations.Count, propExpr, itemName, itemComparison, itemValue, countComparison, countValue);
                    return Expression.Lambda<Func<T, bool>>(expr, source);
                }
            }
            return null;
        }

        /// <summary>
        /// Childrens the aggregation func pridicate.
        /// </summary>
        /// <returns>The aggregation func pridicate.</returns>
        /// <param name="agg">Agg.</param>
        /// <param name="collection">Collection Property Expression.</param>
        /// <param name="itemName">Item name.</param>
        /// <param name="itemComparison">Item comparison.</param>
        /// <param name="itemValue">Item value.</param>
        /// <param name="countComparison">Count comparison.</param>
        /// <param name="countValue">Count value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Expression ChildrenAggregationFunc(
            Aggregations agg,
            Expression collection,
            string itemName,
            FilterOperations itemComparison,
            object itemValue,
            FilterOperations countComparison,
            object countValue)
        {
        
            var itemType = collection.Type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()[0];

            var itemSource = Expression.Parameter(itemType, "e");
            var itemProperty = Expression.Property(itemSource, itemName);

            Expression itemExpr = MakePropertyValueComparison(itemProperty, itemComparison, itemValue);

            var itemPredicate = Expression.Lambda(itemExpr, itemSource);
              
            return ChildrenAggregationFunc(agg, collection, itemPredicate, countComparison.ToExpression(), countValue);   
        }

        public static Expression ChildrenAggregationFunc(
            Aggregations agg,
            Expression collection,
            LambdaExpression itemPredicate,
            ExpressionType countComparison,
            object countValue)
        {
            var itemType = collection.Type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()[0];

            var itemCount = Expression.Call(
                typeof(Enumerable), agg.ToString(), new[] { itemType },
                collection, itemPredicate);

            return Expression.MakeBinary(countComparison, itemCount, Expression.Constant(countValue));
        }

        /// <summary>
        ///  ChildrenAny
        /// </summary>
        /// <typeparam name="T">T is the type of the Target object the filer acts on</typeparam>
        /// <param name="propExpr">CollectionNavigation Property Expression</param>
        /// <param name="itemName">Property Name on the Children Object</param>
        /// <param name="itemComparison">Comparison Operator: ExpressionType.Equal</param>
        /// <param name="itemValue">Comparison Value</param>
        /// <returns>Expression</returns>
        /// <source>https://www.codesd.com/item/expression-trees-filtered-account-on-navigation-property.html</source>
        /// <usage>
        ///  Usage: db.Event.Where(s => s.Attendees.Any(a => a.CancelledOn == null));
        ///  var predicate = MakeAnyPredicate<Event>("Attendees","CancelledOn", ExpressionType.Equal, "null");
        ///  </usage>
        public static Expression ChildrenHaveAny(
            Expression propExpr,
            string itemName,
            FilterOperations itemComparison,
            object itemValue)
        {
            return ChildrenAggregationFunc(Aggregations.Any, propExpr, itemName, itemComparison, itemValue, FilterOperations.Eq, true);
        }

        public static Expression ChildrenHaveAny(
            Expression propExpr,
            LambdaExpression itemPredicate)
        {
            return ChildrenAggregationFunc(Aggregations.Any, propExpr, itemPredicate, ExpressionType.Equal, true);
        }

        /// <summary>
        /// Find records with Date Fields (if not specified, will filter all Date Fields on the object) has value before today.
        /// </summary>
        /// <returns>The records with data field value before today.</returns>
        /// <param name="fields">Fields name array to specify which field to filter</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static Expression DateBefore(ParameterExpression source, DateTime date, string[] fields = null)
        {
            return DateComparison(source, FilterOperations.Lte, date, fields);
        }

        public static Expression<Func<T, bool>> DateBefore<T>(DateTime date, string[] fields = null)
        {
            var source = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, bool>>(DateBefore(source, date, fields), source);
        }
        /// <summary>
        /// Find records with Date Fields (if not specified, will filter all Date Fields on the object) has value before today.
        /// </summary>
        /// <returns>The records with data field value before today.</returns>
        /// <param name="fields">Fields name array to specify which field to filter</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static Expression DateAfter(ParameterExpression source, DateTime date, string[] fields = null)
        {
            return DateComparison(source, FilterOperations.Gte, date, fields);
        }

        public static Expression<Func<T, bool>> DateAfter<T>(DateTime date, string[] fields = null)
        {
            var source = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, bool>>(DateAfter(source, date, fields), source);
        }

        /// <summary>
        /// Find records with Date Fields (if not specified, will filter all Date Fields on the object) has value before today.
        /// </summary>
        /// <returns>The records with data field value before today.</returns>
        /// <param name="fields">Fields name array to specify which field to filter</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static Expression DateOnToday<T>(ParameterExpression source, string[] fields = null)
        {
            return DateBetween(source, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1), fields);
        }

        /// <summary>
        /// Find records with Date Fields (if not specified, will filter all Date Fields on the object) has value before today.
        /// </summary>
        /// <returns>The records with data field value before today.</returns>
        /// <param name="fields">Fields name array to specify which field to filter</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static Expression DateBetween(ParameterExpression source, DateTime Date1, DateTime Date2, string[] fields = null)
        {
            return Expression.AndAlso(DateAfter(source, Date1, fields), DateBefore(source, Date2, fields));
        }

        /// <summary>
        /// Find records with Date Fields (if not specified, will filter all Date Fields on the object) has value before today.
        /// </summary>
        /// <returns>The records with data field value after today.</returns>
        /// <param name="fields">Fields name array to specify which field to filter</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static Expression DateComparison(ParameterExpression source, FilterOperations op, DateTime value, string[] fields = null)
        {
            var type = source.Type;
           
            var props = type.GetProperties()
                .Where(p => p.PropertyType == typeof(DateTimeOffset) ||
                       p.PropertyType == typeof(DateTimeOffset?) ||
                       p.PropertyType == typeof(DateTime) ||
                       p.PropertyType == typeof(DateTime?));

            if (fields != null)
            {
                props = props.Where(p => fields.Contains(p.Name));
            }

            Expression valueExpr = Expression.Constant(value);
            Expression selectLeft = null;
            Expression selectRight = null;
            Expression filterExpression = null;
            foreach (var prop in props)
            {
                valueExpr = Expression.Convert(valueExpr, prop.PropertyType);
                Expression left = Expression.Property(source, prop.Name);
                Expression comparison = Expression.Equal(left, valueExpr);

                if (op == FilterOperations.Lt)
                {
                    comparison = Expression.LessThan(left, valueExpr);
                }
                if (op == FilterOperations.Lte)
                {
                    comparison = Expression.LessThanOrEqual(left, valueExpr);
                }
                if (op == FilterOperations.Gt)
                {
                    comparison = Expression.GreaterThan(left, valueExpr);
                }
                if (op == FilterOperations.Gte)
                {
                    comparison = Expression.GreaterThanOrEqual(left, valueExpr);
                }

                if (selectLeft == null)
                {
                    selectLeft = comparison;
                    filterExpression = selectLeft;
                    continue;
                }
                if (selectRight == null)
                {
                    selectRight = comparison;
                    filterExpression =
                        Expression.AndAlso(selectLeft, selectRight);
                    continue;
                }
                filterExpression =
                    Expression.AndAlso(filterExpression, comparison);
            }
            return filterExpression;
        }

        private static Expression StartsWith(Expression propExpr, Expression valueExpr, bool caseSensitive = false)
        {
            Expression caseSensExpr = Expression.Constant(StringComparison.OrdinalIgnoreCase);
            if (caseSensitive)
            {
                caseSensExpr = Expression.Constant(StringComparison.Ordinal);
            }
            return Expression.Call(
                           propExpr,
                           typeof(string).GetMethod("StartsWith", new Type[] { typeof(string), typeof(StringComparison) }),
                           valueExpr,
                           caseSensExpr);

        }

        private static Expression EndsWith(Expression propExpr, Expression valueExpr, bool caseSensitive = false)
        {
            Expression caseSensExpr = Expression.Constant(StringComparison.OrdinalIgnoreCase);
            if (caseSensitive)
            {
                caseSensExpr = Expression.Constant(StringComparison.Ordinal);
            }
            return Expression.Call(
                           propExpr,
                           typeof(string).GetMethod("EndsWith", new Type[] { typeof(string), typeof(StringComparison) }),
                           valueExpr,
                           caseSensExpr);
        }

        private static Expression Contains(Expression propExpr, Expression valueExpr, bool caseSensitive = false)
        {
            Expression caseSensExpr = Expression.Constant(StringComparison.OrdinalIgnoreCase);
            if (caseSensitive)
            {
                caseSensExpr = Expression.Constant(StringComparison.Ordinal);
            }

            Expression exp = Expression.Call(
                           propExpr,
                           typeof(string).GetMethod("Contains", new Type[] { typeof(string), typeof(StringComparison) }),
                           valueExpr,
                           caseSensExpr);

            exp = Expression.Condition(Expression.NotEqual(propExpr, Expression.Constant(null, typeof(string))), exp, Expression.Constant(false));
            return exp;
        }

        private static Expression ArrayContains(Expression propExpr, Expression valueExpr)
        {
            var propType = propExpr.Type;

            if (propType.IsArray)
                propType = propType.GetElementType();

            var containsMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Single(x => x.Name == "Contains" && x.GetParameters().Length == 2)
                    .MakeGenericMethod(propType);

            if (propExpr.Type.IsArray)
            {
                return Expression.Call(containsMethod, propExpr, valueExpr);
            }
            else
            {
                return Expression.Call(containsMethod, valueExpr, propExpr);
            }
        }

        /// <summary>
        /// Find records with string Fields (if not specified, will filter all string Fields on the object) has contains the value.
        /// </summary>
        /// <returns>The records with data field value after today.</returns>
        /// <param name="fields">Fields name array to specify which field to filter</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static Expression StringContainsValue(ParameterExpression source, object value, string[] fields = null, bool caseSensitive = false)
        {
            var type = source.Type;
           
            if (fields == null || fields.Length == 0)
            {
                fields = type.GetProperties()
                         .Where(p => p.PropertyType == typeof(string))
                         .Select(p => p.Name).ToArray();
            }

            var valueExpr = Expression.Constant(value);
            Expression selectLeft = null;
            Expression selectRight = null;
            Expression filterExpression = null;

            foreach (var field in fields)
            {
                Expression propExpr = source;
                var members = field.Split('.');
                Expression comparison = null;

                Expression propNotNull = null;
                foreach (var member in members)
                {
                    //navigation property
                    propExpr = Expression.PropertyOrField(propExpr, member);

                    var t = propExpr.Type;
                    if (members.Length > 1)
                    {
                        var pNotNull = Expression.NotEqual(propExpr, Expression.Constant(null));
                        if (propNotNull == null)
                        {
                            propNotNull = pNotNull;
                        }
                        else
                            propNotNull = Expression.AndAlso(propNotNull, pNotNull);
                    }
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    {
                        // remove prop type info, treat all item as string item
                        comparison = DictionaryContainsItem(propExpr, members.Last(), FilterOperations.Ct, value);
                        comparison = Expression.AndAlso(propNotNull, comparison);
                        break;
                    }
                }

                if (propExpr.Type == typeof(string))
                    comparison = ExpressionEx.Contains(propExpr, valueExpr, caseSensitive);

                if (comparison == null)
                    continue;

                if (selectLeft == null)
                {
                    selectLeft = comparison;
                    filterExpression = selectLeft;
                    continue;
                }
                if (selectRight == null)
                {
                    selectRight = comparison;
                    filterExpression =
                        Expression.Or(selectLeft, selectRight);
                    continue;
                }
                filterExpression =
                    Expression.Or(filterExpression, comparison);
            }
            return filterExpression;
               
        }

        public static Expression<Func<T, bool>> StringContainsValue<T>(object value, string[] fields = null, bool caseSensitive = false)
        {
            var source = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, bool>>(StringContainsValue(source, value, fields, caseSensitive), source);
        }
    }
}
