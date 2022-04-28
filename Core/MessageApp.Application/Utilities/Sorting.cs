﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MessageApp.Application.Utilities.SortingUtility;

namespace MessageApp.Application.Utilities
{
    public class Sorting<T>
    {
        public static IEnumerable<T> GroupingData(IEnumerable<T> data, IEnumerable<string> groupingColumns)
        {
            IOrderedEnumerable<T> groupedData = null;

            foreach (string grpCol in groupingColumns.Where(x => !String.IsNullOrEmpty(x)))
            {
                var col = typeof(T).GetProperty(grpCol, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (col != null)
                {
                    groupedData = groupedData == null ? data.OrderBy(x => col.GetValue(x, null))
                                                    : groupedData.ThenBy(x => col.GetValue(x, null));
                }
            }

            return groupedData ?? data;
        }
        public static IEnumerable<T> SortData(IEnumerable<T> data, IEnumerable<SortingParams> sortingParams)
        {
            IOrderedEnumerable<T> sortedData = null;
            foreach (var sortingParam in sortingParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)))
            {
                var col = typeof(T).GetProperty(sortingParam.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (col != null)
                {
                    sortedData = sortedData == null ? sortingParam.SortOrder == SortOrders.Asc ? data.OrderBy(x => col.GetValue(x, null))
                                                                                               : data.OrderByDescending(x => col.GetValue(x, null))
                                                    : sortingParam.SortOrder == SortOrders.Asc ? sortedData.ThenBy(x => col.GetValue(x, null))
                                                                                        : sortedData.ThenByDescending(x => col.GetValue(x, null));
                }
            }
            return sortedData ?? data;
        }

    }
}
