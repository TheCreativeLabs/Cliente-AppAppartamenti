using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace NotificationService
{
    public partial class Service1 : ServiceBase
    {
        public class notification_content
        {
            public string name { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public System.Collections.Generic.Dictionary<string,string> custom_Data { get; set; }
        }

      
        public class notification_target
        {
            public string type { get; set; }
            public List<string> devices { get; set; }
        }

        public class Content
        {
            public notification_content notification_content { get; set; }
            public notification_target notification_target { get; set; }

        }

        Timer tmrExecutor = new Timer();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            tmrExecutor.Elapsed += new ElapsedEventHandler(tmrExecutor_Elapsed); // adding Event
            tmrExecutor.Interval = Properties.Settings.Default.IntervalTime * 1000; // Set your time here 
            tmrExecutor.Enabled =true ;
            tmrExecutor.Start();
        }

        private void tmrExecutor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Do your work here 
            Send();
        }


        internal void TestStartupAndStop(string[] args)
        {
            Send();
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        private async void Send()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-API-Token", "a4a55859acdb083dede34d3d20785f6fb94c638f");

            try
            {
                var listaToNotify = new CodaNotificheDataSet.NotificheCodaListDataTable();

                using (var ta = new CodaNotificheDataSetTableAdapters.NotificheCodaListTableAdapter())
                {
                    listaToNotify = ta.GetData();
                }

                if (listaToNotify.Any())
                {
                    foreach (var item in listaToNotify)
                    {
                        if (item.OsVersion != null)
                        {
                            var appName = "";
                            if (item.OsVersion.ToUpper() == "ANDROID")
                            {
                                appName = Properties.Settings.Default.AppAndroidName;
                            }
                            else
                            {
                                appName = Properties.Settings.Default.AppIosName;
                            }

                            notification_content notContent = new notification_content()
                            {
                                name = "Promuovocasa.it",
                                title = item.Title,
                                body = item.Message,
                                custom_Data = new Dictionary<string, string>()
                            };

                            List<string> lista = new List<string>();
                            lista.Add(item.InstallationId.ToString());

                            notification_target notification_Target = new notification_target()
                            {
                                type = "devices_target",
                                devices = lista
                            };

                            Content content = new Content()
                            {
                                notification_content = notContent,
                                notification_target = notification_Target
                            };

                            var Url = $"https://appcenter.ms/api/v0.1/apps/{Properties.Settings.Default.AccountName}/{appName}/push/notifications";
                            var content2 = JsonConvert.SerializeObject(content);
                            StringContent sContent = new StringContent(content2, Encoding.UTF8, "application/json");
                            var r = await httpClient.PostAsync(Url, sContent);

                            var ta = new CodaNotificheDataSetTableAdapters.NotificheCodaListTableAdapter();
                            ta.UpdateById(item.Id);
                        }
                    }
                }
            }
            catch(Exception ex) {
                var i = 0;
            }
        }

        protected override void OnStop()
        {
            tmrExecutor.Enabled = false;
        }

    }
}
