using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace z.Foundation.LogicInvoke
{
    /// <summary>
    /// WCF方式逻辑调用
    /// </summary>
    internal class WCFLogicTunnel : ILogicTunnel
    {
        /// <summary>
        /// 缓存配置文件的值
        /// </summary>
        private static Dictionary<string, WCFApp> dict;

        /// <summary>
        /// WCF配置文件
        /// </summary>
        private string ConfigFile
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WCFLogicTunnel()
        {
            ConfigFile = string.Format("{0}\\WCF.config", Utility.ApplicationPath());
        }

        /// <summary>
        /// 调用（非泛型）
        /// </summary>
        /// <param name="request">请求参数对象</param>
        /// <returns></returns>
        public IResponse Call(IRequest request)
        {
            IResponse response = new Response();

            try
            {
                string key = string.Format("{0}:{1}:{2}", request.Target, request.Class, request.Method);
                WCFApp wcfApp = GetWcfApp(key);
                //WCF调用
                Binding binding = null;
                switch (wcfApp.Binding)
                {
                    case "BasicHttpBinding":
                        binding = new BasicHttpBinding()
                        {
                            MaxReceivedMessageSize = 2147483647,
                            OpenTimeout = new TimeSpan(0, 30, 0),
                            CloseTimeout = new TimeSpan(0, 30, 0),
                            ReceiveTimeout = new TimeSpan(0, 30, 0),
                            SendTimeout = new TimeSpan(0, 30, 0),
                            Security = new BasicHttpSecurity()
                            {
                                Mode = BasicHttpSecurityMode.None
                            }
                        };
                        break;
                    case "NetTcpBinding":
                    default:
                        binding = new NetTcpBinding()
                        {
                            MaxReceivedMessageSize = 2147483647,
                            OpenTimeout = new TimeSpan(0, 30, 0),
                            CloseTimeout = new TimeSpan(0, 30, 0),
                            ReceiveTimeout = new TimeSpan(0, 30, 0),
                            SendTimeout = new TimeSpan(0, 30, 0),
                            Security = new NetTcpSecurity()
                            {
                                Mode = SecurityMode.None
                            }
                        };
                        break;
                    case "WebHttpBinding":
                        binding = new WebHttpBinding()
                        {
                            MaxReceivedMessageSize = 2147483647
                        };
                        break;
                    case "WSHttpBinding":
                        binding = new WSHttpBinding()
                        {
                            MaxReceivedMessageSize = 2147483647
                        };
                        break;
                }

                //IWCFService service = new WCFServiceClient(binding, new EndpointAddress(wcfApp.Address));
                //Request model = (Request)request;
                //string base64 = model.Serialize<Request>();
                //response.Result = service.Execute(base64);
                //response.Succeeded = true;

                using (WCFServiceClient service = new WCFServiceClient(binding, new EndpointAddress(wcfApp.Address)))
                {
                    Request model = (Request)request;
                    string base64 = model.Serialize<Request>();
                    response.Result = service.Execute(base64);
                    response.Succeeded = true;
                }
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = (ex.InnerException == null ? ex.Message : ex.InnerException.Message) + string.Format("\r\n调用:Target:{0} | Class:{1} | Method:{2} | Parameter:{3}", request.Target, request.Class, request.Method, request.Parameter == null ? "参数为null" : "参数不为null");
                response.Exception = ex;

                Logger.Error(response.Message, ex);
            }

            return response;
        }

        /// <summary>
        /// 调用（泛型）
        /// </summary>
        /// <typeparam name="T1">参数类型</typeparam>
        /// <typeparam name="T2">结果集类型</typeparam>
        /// <param name="request">请求参数对象</param>
        /// <returns></returns>
        public IResponse<T2> Call<T1, T2>(IRequest<T1> request)
        {
            Request requestModel = new Request();
            requestModel.Target = request.Target;
            requestModel.Class = request.Class;
            requestModel.Method = request.Method;
            requestModel.Parameter = request.Parameter;
            IResponse responseModel = Call(requestModel);

            IResponse<T2> response = new Response<T2>();
            response.Succeeded = responseModel.Succeeded;
            response.Message = responseModel.Message;
            response.DebugMessage = responseModel.DebugMessage;
            response.Exception = responseModel.Exception;
            if (responseModel.Succeeded)
            {
                string result = responseModel.Result.ToString();
                if (!string.IsNullOrEmpty(result))
                {
                    response.Result = result.Deserialize<T2>();
                }
            }

            return response;
        }

        /// <summary>
        /// 获取WCF调用所需的相关参数
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        private WCFApp GetWcfApp(string appName)
        {
            if (dict == null)
            {
                ConfigOperate<WCFConfig> model = new ConfigOperate<WCFConfig>(ConfigFile);
                WCFConfig wcfConfig = model.Load();
                dict = wcfConfig.Apps.ToDictionary(p => p.Key);
            }
            WCFApp wcfApp = new WCFApp();
            if (dict.ContainsKey(appName))
            {
                wcfApp = dict[appName];
            }
            else if (dict.ContainsKey("*"))
            {
                wcfApp = dict["*"];
            }
            else
            {
                throw new Exception("App未找到对应的WCF调用地址");
            }
            return wcfApp;
        }
    }
}
