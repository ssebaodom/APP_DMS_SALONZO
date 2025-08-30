using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SSE.Core.Services.Helpers
{
    public class ConvertHelper
    {
        /// <summary>
        /// Convert a List of object into a DataTable
        /// </summary>
        /// <typeparam name="T">Object destination</typeparam>
        /// <param name="ls">List object need to be convert</param>
        /// <param name="table_name">Table Name to identity</param>
        /// <returns>DataTable</returns>
        public static DataTable ListToDataTable<T>(List<T> ls, string table_name = "Table")
        {
            DataTable dt = new DataTable();
            try
            {
                dt.TableName = table_name;

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    dt.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                }

                foreach (var vehicle in ls)
                {
                    DataRow newRow = dt.NewRow();
                    foreach (PropertyInfo property in vehicle.GetType().GetProperties())
                    {
                        newRow[property.Name] = vehicle.GetType().GetProperty(property.Name).GetValue(vehicle, null);
                    }
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Convert a DataTable into a List of object
        /// </summary>
        /// <typeparam name="T">Object source</typeparam>
        /// <param name="dt">DataTable need to be convert</param>
        /// <returns>List of T object</returns>
        public static List<T> DataTableToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }

        /// <summary>
        /// Convert a DataTable into a List of object
        /// using convert method: DataRow to object
        /// </summary>
        /// <typeparam name="T">Object destination</typeparam>
        /// <param name="dt">DataTable need to be convert</param>
        /// <returns>List of T object</returns>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetObjectFromDataRow<T>(row);
                data.Add(item);
            }
            return data;
        }

        /// <summary>
        /// Get T object from DataRow
        /// </summary>
        /// <typeparam name="T">Object destination</typeparam>
        /// <param name="dr">DataRow</param>
        /// <returns>T object</returns>
        public static T GetObjectFromDataRow<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public static T DataTableConvert<T>(DataTable data)
        {
            string data_str = JsonConvert.SerializeObject(data);
            T DataRes = JsonConvert.DeserializeObject<List<T>>(data_str)[0];
            return DataRes;
        }

        public static List<T> DataTableConvertList<T>(DataTable data)
        {
            string data_str = JsonConvert.SerializeObject(data);
            List<T> DataRes = JsonConvert.DeserializeObject<List<T>>(data_str);
            return DataRes;
        }
    }
}