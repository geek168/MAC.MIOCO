using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC.MIOCO
{
    public static class ExForCastDataToObject
    {
        public static IEnumerable<T> Cast<T>(this DataTable table)
        {
            Type oType = typeof(T);
            if (oType.IsValueType
                || oType == typeof(string)
                || (oType.IsArray && oType.GetElementType().IsValueType))
            {
                return ValueTypeCast<T>(table, oType);
            }
            else
            {
                return NotValueTypeCast<T>(table);
            }
        }
        private static IEnumerable<T> ValueTypeCast<T>(DataTable table, Type pType)
        {
            if (table == null)
            {
                throw new NullReferenceException("DataTable");
            }
            var objList = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T obj;
                var oDbValue = row[0];
                if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (oDbValue == DBNull.Value)
                        obj = default(T);
                    else
                        obj = (T)Convert.ChangeType(oDbValue, pType.GetGenericArguments()[0]);
                }
                else
                {
                    obj = (T)Convert.ChangeType(oDbValue, pType);
                }
                objList.Add(obj);
            }
            return objList;
        }
        private static IEnumerable<T> NotValueTypeCast<T>(DataTable table)
        {
            if (table == null)
            {
                throw new NullReferenceException("DataTable");
            }
            Type oType = typeof(T);
            var propertiesLength = oType.GetProperties().Length;
            if (propertiesLength == 0)
            {
                throw new NullReferenceException("Properties");
            }
            var objList = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                var obj = (T)Activator.CreateInstance(oType);
                var objProperties = obj.GetType().GetProperties();

                for (var i = 0; i < propertiesLength; i++)
                {
                    var property = objProperties[i];
                    string oColName;
                    if (property.GetCustomAttributes(typeof(ModelIgnoreAttribute), false).Any()
                        || !property.CanWrite)
                    {
                        continue;
                    }
                    if (property.GetCustomAttributes(typeof(ColumnAttribute), false).Any())
                    {
                        oColName = property.GetCustomAttributes(typeof(ColumnAttribute), false).Cast<ColumnAttribute>().Single().Name;
                        if (string.IsNullOrEmpty(oColName))
                        {
                            oColName = property.Name;
                        }
                    }
                    else
                    {
                        oColName = property.Name;
                    }
                    if (table.Columns.Contains(oColName))
                    {
                        var objValue = row[oColName];
                        if (objValue.GetType() == typeof(DBNull))
                        {
                            continue;
                        }
                        if (typeof(Enum).IsAssignableFrom(property.PropertyType))
                        {
                            property.SetValue(obj, Enum.Parse(property.PropertyType, objValue.ToString()));
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            property.SetValue(obj, Convert.ChangeType(objValue, Nullable.GetUnderlyingType(property.PropertyType)));
                        }
                        else
                        {
                            property.SetValue(obj, Convert.ChangeType(objValue, property.PropertyType, CultureInfo.CurrentCulture));
                        }
                    }
                }
                objList.Add(obj);
            }

            return objList;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ModelIgnoreAttribute : Attribute
    {
    }
}
