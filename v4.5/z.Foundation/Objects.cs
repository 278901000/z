﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Data;


namespace z.Foundation
{
    /// <summary>
    /// 对象操作扩展方法
    /// </summary>
    public static class Objects
    {
        /// <summary>
        /// 对象序列化为Base64字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Serialize<T>(this T source)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, source);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] bt = new byte[ms.Length];
            ms.Read(bt, 0, bt.Length);
            ms.Close();
            return Convert.ToBase64String(bt);
        }

        /// <summary>
        /// 对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(this T source, Encoding encoding)
        {
            string xmlString = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, source);
                if (encoding == null)
                {
                    xmlString = Encoding.UTF8.GetString(ms.ToArray());
                }
                else
                {
                    xmlString = encoding.GetString(ms.ToArray());
                }
            }
            return xmlString;
        }

        /// <summary>
        /// 对象序列化为json字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string JsonSerialize(this object source)
        {
            return JsonConvert.SerializeObject(source);
        }

        /// <summary>
        /// 浅拷贝两对象相同名称的属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity">目标对象</param>
        public static void CopyTo(this object entity, object targetEntity)
        {
            CopyTo(entity, targetEntity, false);
        }

        /// <summary>
        /// 浅拷贝两对象相同名称的属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity">目标对象</param>
        /// <param name="isNeedNullValue">连同NULL属性值也拷贝时，设置为true</param>
        public static void CopyTo(this object entity, object targetEntity, bool isNeedNullValue)
        {
            PropertyInfo[] entityProperties = entity.GetType().GetProperties();
            PropertyInfo[] targetEntityProperties = targetEntity.GetType().GetProperties();

            IDictionary<string, PropertyInfo> entityPropertiesNamePair = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo pInfo in entityProperties)
            {
                if (!pInfo.CanRead) continue;
                entityPropertiesNamePair[pInfo.Name] = pInfo;
            }

            var targetPInfos = from targetPropertyInfo in targetEntityProperties
                               where entityPropertiesNamePair.Keys.Contains(targetPropertyInfo.Name)
                               select targetPropertyInfo;
            foreach (PropertyInfo targetPropertyInfo in targetPInfos)
            {
                if (!targetPropertyInfo.CanWrite) continue;
                object entityPInfoValue = entityPropertiesNamePair[targetPropertyInfo.Name].GetValue(entity, null);
                if ((isNeedNullValue) || (entityPInfoValue != null))
                {
                    targetPropertyInfo.SetValue(targetEntity, entityPInfoValue, null);
                }
            }
        }

        /// <summary>
        /// DataTable转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> DataTableToModel<T>(this DataTable dataTable) where T : class,new()
        {
            List<T> list = new List<T>();
            Type type = typeof(T);


            foreach (DataRow dataRow in dataTable.Rows)
            {
                T model = new T();

                PropertyInfo[] propertyInfos = type.GetProperties();

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (!dataTable.Columns.Contains(propertyInfo.Name))
                    {
                        continue;
                    }

                    if (!propertyInfo.CanWrite)
                    {
                        continue;
                    }

                    if (dataRow[propertyInfo.Name] == DBNull.Value)
                    {
                        continue;
                    }

                    propertyInfo.SetValue(model, dataRow[propertyInfo.Name], null);
                }
                list.Add(model);
            }
            return list;
        }
    }
}