using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using z.Foundation;
using z.AdminCenter.Logic;
using z.Foundation.LogicInvoke;
using System.Reflection;
using System.Diagnostics;
using System.Web.Hosting;
using AdminCenter.WebForm.Driver;

namespace com.admincenter.www
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Logger.Info("Application_Start:" + DateTime.Now.ToString());

            Thread t = new Thread(new ThreadStart(UpdateAllUserSessionData));
            t.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Logger.Info("Application_Error:" + DateTime.Now.ToString());
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Logger.Info("Application_End:" + DateTime.Now.ToString());
            RecordEndReason();
        }

        void UpdateAllUserSessionData()
        {
            IResponse response = new WebFormBase().CallLogic("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminAccountManage", "UpdateAllUserSession", null);
            Logger.Info(string.Format("UpdateAllUserSession_{0}_{1}", response.Succeeded, DateTime.Now.ToString()));

            Thread.Sleep(1000 * 60);
            UpdateAllUserSessionData();
        }

        void RecordEndReason()
        {
            var shutdownReason = HostingEnvironment.ShutdownReason;

            HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField,
                null,
                null,
                null);

            if (runtime == null)
                return;

            string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                null,
                runtime,
                null);

            string shutDownStack = (string)runtime.GetType().InvokeMember(
                "_shutDownStack",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                null,
                runtime,
                null);

            Logger.Info(String.Format("\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}\r\n\r\n_shutdownReason={2}", shutDownMessage, shutDownStack, shutdownReason));
        }
    }
}