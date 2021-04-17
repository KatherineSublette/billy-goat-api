using System;
using System.Collections.Generic;
using System.Linq;
using BillyGoats.Api.Utils.Extensions;

namespace BillyGoats.Api.Filter
{
    public class FilterRequest
    {
        public IEnumerable<string> SelectedColumns { get; set; } = null;

        public IDictionary<string, string> FilterCriteria { get; set; } = new Dictionary<string, string>();

        public void Reset()
        {
            _filter = null;
            _sorts.Clear();
        }
        
        private FilterItem _filter;
        public FilterItem Filter
        {
            get
            {
                var filterEntries = FilterCriteria.Where(kv => kv.Key != "sort");
                if (_filter == null && filterEntries.Any())
                {
                    _filter = new FilterItem();
                    var currentItem = _filter;
                    foreach (var criteria in filterEntries)
                    {
                        var newItem = new FilterItem();
                        newItem.Field = criteria.Key.CamelToPascalCase();
                        var opval = criteria.Value.Split(":");
                        if (opval.Length > 1)
                        {
                            var op = opval[0];
                            if (op == "ct")
                            {
                                newItem.Operator = FilterOperations.Ct;
                                newItem.Value = opval[1];
                                if (opval[1].Contains("||"))
                                {
                                    newItem.Value = opval[1].Split("||");
                                }

                            }
                            else if (op == "bt")
                            {
                                var val = opval[1].Split("||");
                                if (val.Length < 2)
                                {
                                    throw new Exception($"Invalid input data for {criteria.Key}");
                                }
                                if (val[0] == string.Empty && val[1] == string.Empty)
                                {
                                    continue;
                                }
                                if (val[0] != string.Empty)
                                {
                                    newItem.Operator = FilterOperations.Gte;
                                    newItem.Value = val[0];
                                }
                                else if (val[1] != string.Empty)
                                {
                                    newItem.Operator = FilterOperations.Lt;
                                    newItem.Value = val[1];
                                }

                                if (val[0] != string.Empty && val[1] != string.Empty)
                                {
                                    if (currentItem.Field == null)
                                    {
                                        currentItem.Field = newItem.Field;
                                        currentItem.Value = newItem.Value;
                                        currentItem.Operator = newItem.Operator;
                                    }
                                    else
                                    {
                                        currentItem.Right = newItem;
                                        currentItem = currentItem.Right;
                                    }

                                    newItem = new FilterItem();
                                    newItem.Field = criteria.Key.CamelToPascalCase();
                                    newItem.Operator = FilterOperations.Lt;
                                    newItem.Value = val[1];
                                }
                            }
                            else
                            {
                                newItem.Operator = op.ParseEnum<FilterOperations>();
                                newItem.Value = opval[1];
                                if (opval[1].Contains("||"))
                                {
                                    newItem.Operator = FilterOperations.Ct;
                                    newItem.Value = opval[1].Split("||");
                                }
                            }
                        }
                        else
                        {
                            newItem.Value = opval[0];
                            if (opval[0].Contains("||"))
                            {
                                newItem.Operator = FilterOperations.Ct;
                                newItem.Value = opval[0].Split("||");
                            }
                        }

                        if (currentItem.Field == null)
                        {
                            currentItem.Field = newItem.Field;
                            currentItem.Value = newItem.Value;
                            currentItem.Operator = newItem.Operator;
                        }
                        else
                        {
                            currentItem.Right = newItem;
                            currentItem = currentItem.Right;
                        }
                    }
                }
                return _filter;
            }
        }

        private IList<Sort> _sorts = new List<Sort>();

        public IList<Sort> Sorts
        {
            get
            {
                if (!_sorts.Any() && FilterCriteria.Keys.Contains("sort"))
                {
                    var sorts = FilterCriteria.FirstOrDefault(kv => kv.Key == "sort").Value.Split(",");

                    foreach (var sort in sorts)
                    {
                        string[] s = sort.Split(" ");
                        _sorts.Add(new Sort
                        {
                            Field = s[0].CamelToPascalCase(),
                            Dir = s.Length > 1 ? s[1].ParseEnum<SortDirection>() : SortDirection.Asc
                        });
                    }
                }
                return _sorts;
            }
        }

        public void RemoveSort(string field = "")
        {
            // remove sort criteria as they are not needed for datatile
            foreach (var kv in FilterCriteria
               .Where(kv => kv.Key == "sort")
               .ToArray())
            {
                if (field == "")
                    FilterCriteria.Remove(kv.Key);
                else
                {
                    if(kv.Value.Contains(field))
                        FilterCriteria.Remove(kv.Key);
                }
            }
            Reset();
        }

        public IQueryable<T> ApplyFilter<T>(IQueryable<T> query) where T : class
        {
            IQueryable<T> filteredQuery = this.Filter == null ? query : this.Filter.FilterIQueryable<T>(query);

            bool hasSort = false;
            var filteredQuery0 = filteredQuery;
            foreach (var sort in this.Sorts)
            {
                var sortExpression = ExpressionEx.GetPropertyExpression<T>(sort.Field);
                if (sortExpression != null)
                {
                    hasSort = true;

                    if (sort.Dir == SortDirection.Asc)
                    {
                        filteredQuery = filteredQuery.OrderBy(sortExpression);
                    }
                    else
                    {
                        filteredQuery = filteredQuery.OrderByDescending(sortExpression);
                    }

                }
            }

            return filteredQuery;
        }
    }
}
