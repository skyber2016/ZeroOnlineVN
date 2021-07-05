using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

namespace NEWS_MVC
{
    public static class IEnumerableExtensions
    {
        public static DataSet ToDataSet<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return null;
            var name = typeof(T).Name;
            var converted = new DataSet(name);
            converted.Tables.Add(NewTable(name, source));
            return converted;
        }

        private static DataTable NewTable<T>(string name, IEnumerable<T> list)
        {
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            DataTable table = Table(name, propInfo);
            IEnumerator<T> enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
                table.Rows.Add(CreateRow<T>(table.NewRow(), enumerator.Current, propInfo));
            return table;
        }

        private static DataRow CreateRow<T>(DataRow row, T listItem, PropertyInfo[] pi)
        {
            foreach (PropertyInfo p in pi)
            {
                if (p.GetMethod.IsVirtual)
                {
                    continue;
                }
                var column = p.GetCustomAttribute<ColumnAttribute>(true);
                row[column?.Name ?? p.Name.ToString()] = p.GetValue(listItem, null);
            }
            return row;
        }

        private static DataTable Table(string name, PropertyInfo[] pi)
        {
            DataTable table = new DataTable(name);
            foreach (PropertyInfo p in pi)
                table.Columns.Add(p.Name, p.PropertyType);
            return table;
        }

        private static DataTable Table(PropertyInfo[] pi)
        {
            DataTable table = new DataTable();
            foreach (PropertyInfo p in pi)
            {
                Type columnType =  p.PropertyType;
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = p.PropertyType.GetGenericArguments()[0];
                }
                if (p.GetMethod.IsVirtual)
                {
                    continue;
                }
                var column = p.GetCustomAttribute<ColumnAttribute>(true);
                table.Columns.Add(new DataColumn(column?.Name ?? p.Name, columnType));
            }
            return table;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> ie)
        {
            var properties = typeof(T).GetProperties();
            var table = Table(properties);
            var enumerator = ie.GetEnumerator();
            while (enumerator.MoveNext())
                table.Rows.Add(CreateRow<T>(table.NewRow(), enumerator.Current, properties));
            return table;
        }
    }
}
