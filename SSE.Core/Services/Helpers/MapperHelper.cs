using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SSE.Core.Services.Helpers
{
    public class MapperHelper
    {
        private static string CamelCase(string value)
        {
            return Char.ToLower(value[0]) + value.Substring(1);
        }

        /// <summary>
        /// Tạo 1 đối tượng mới map giá trị của source
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">Dữ liệu nguồn</param>
        /// <param name="isSourceCamelCase">Thuộc tính có dạnh camelcase hay là mặc định</param>
        /// <returns></returns>
        public static TResult Map<TResult>(object source, bool isSourceCamelCase = false) where TResult : class
        {
            var TypeResult = typeof(TResult);
            var TypeSource = source.GetType();
            var PropertiesResult = TypeResult.GetProperties();
            var PropertiesSource = TypeSource.GetProperties().ToDictionary(item => item.Name, item => item);
            var constructorFirst = TypeResult.GetConstructors()[0];
            var ParamsInjectConstructorLength = constructorFirst.GetParameters().Length;
            var objectResult = constructorFirst.Invoke(new object[ParamsInjectConstructorLength]);
            foreach (var pro in PropertiesResult)
            {
                string name = isSourceCamelCase ? CamelCase(pro.Name) : pro.Name;
                if (PropertiesSource.ContainsKey(name) && PropertiesSource[name].PropertyType == pro.PropertyType)
                {
                    var valueExits = PropertiesSource[name].GetValue(source, null);
                    if (valueExits != null)
                    {
                        pro.SetValue(objectResult, valueExits);
                    }
                }
            }
            return (TResult)objectResult;
        }

        /// <summary>
        /// Copy giá trị thuộc tính của source vào des
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="des"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TResult Map<TResult>(object des, object source, bool isSourceCamelCase = false) where TResult : class
        {
            if (des == null) des = (object)Activator.CreateInstance(typeof(TResult));

            var TypeDes = des.GetType();
            var TypeSource = source.GetType();
            var PropertiesDes = TypeDes.GetProperties();
            var PropertiesSource = TypeSource.GetProperties().ToDictionary(item => item.Name, item => item);
            foreach (var pro in PropertiesDes)
            {
                string name = isSourceCamelCase ? CamelCase(pro.Name) : pro.Name;
                if (PropertiesSource.ContainsKey(name) && PropertiesSource[name].PropertyType == pro.PropertyType)
                {
                    var valueExits = PropertiesSource[name].GetValue(source, null);
                    if (valueExits != null)
                    {
                        pro.SetValue(des, valueExits);
                    }
                }
            }
            return (TResult)des;
        }

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

        public static DataTable ConvertObjectToDataTable<T>(T obj)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor p in props)
                dataTable.Columns.Add(p.Name);

            object[] values = new object[props.Count];
            for (int i = 0; i < props.Count; i++)
            {
                values[i] = props[i].GetValue(obj);
            }
            dataTable.Rows.Add(values);
            return dataTable;
        }
    }
}