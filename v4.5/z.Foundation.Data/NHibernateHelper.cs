using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.Foundation.Data
{
    /// <summary>
    /// 基于NHibernate数据仓储类辅助类
    /// </summary>
    public class NHibernateHelper<T>
    {
        static readonly object lockObj = new object();
        static Dictionary<string, string> dict = new Dictionary<string, string>();

        static ISessionFactory _SessionFactory;

        /// <summary>
        /// 获取SessionFactory
        /// </summary>
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_SessionFactory == null)
                {
                    _SessionFactory = (new Configuration()).Configure(string.Format("{0}/{1}.cfg.xml", Utility.ApplicationPath(), ConfigFileName)).BuildSessionFactory();
                }
                return _SessionFactory;
            }
        }

        /// <summary>
        /// 获取Session对象
        /// </summary>
        /// <returns></returns>
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        /// <summary>
        /// 获取数据库连接配置文件名称
        /// </summary>
        private static string ConfigFileName
        {
            get
            {
                var classFullName = typeof(T).FullName;
                if (!dict.ContainsKey(classFullName))
                {
                    lock (lockObj)
                    {
                        if (!dict.ContainsKey(classFullName))
                        {
                            dict[classFullName] = "";

                            var customAttributes = typeof(T).GetCustomAttributes(false);
                            foreach (var item in customAttributes)
                            {
                                if (item is CustomDataAttribute)
                                {
                                    CustomDataAttribute customDataAttribute = item as CustomDataAttribute;
                                    dict[classFullName] = customDataAttribute.ConnectionName;

                                    break;
                                }
                            }
                        }
                    }
                }

                return dict[classFullName];
            }
        }
    }
}
