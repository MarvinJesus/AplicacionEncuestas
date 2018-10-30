using Entities_POJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace WebApi.Helper
{
    public static class IQueraybleExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sort)
        {
            if (source == null) throw new ArgumentNullException("source");

            if (sort == null) return source;

            var lstSort = sort.Split(',');
            string completeSortExpression = "";

            foreach (var sortOption in lstSort)
            {
                if (sortOption.StartsWith("-"))
                {
                    completeSortExpression = completeSortExpression + sortOption.Remove(0, 1) + " descending,";
                }
                else
                {
                    completeSortExpression = completeSortExpression + sortOption + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(completeSortExpression))
            {
                source = source.OrderBy(completeSortExpression.Remove(completeSortExpression.Count() - 1));
            }

            return source;
        }

        public static IQueryable<Topic> ApplyFilter(this IQueryable<Topic> source, string filters)
        {
            if (source == null) throw new ArgumentNullException("source");
            ICollection<Topic> topics = new List<Topic>();

            if (filters == null) return source;

            var filtersList = filters.Split(',');

            foreach (var topic in source)
            {
                var categories = topic.Categories.Select(c => c.Name);

                if (categories.Any(c => filtersList.Contains(c)))
                {
                    topics.Add(topic);
                }
            }

            return topics.AsQueryable<Topic>();
        }
    }
}