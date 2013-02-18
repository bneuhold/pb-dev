using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using AdvertUrlChecking;
using System.Threading;
using System.Xml.Linq;

namespace UrlCheckerService
{
    public partial class Service1 : ServiceBase
    {
        public const string LOG_SOURCE = "PlaceberryUrlCheckerSource";
        public const string LOG_NAME = "PlaceberryUrlCheckerLog";

        private ConfigManager _configManager;

        public Service1()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists(LOG_SOURCE))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    LOG_SOURCE, LOG_NAME);
            }
            _eventLog.Source = LOG_SOURCE;
            _eventLog.Log = LOG_NAME;
            _eventLog.MaximumKilobytes = 51200;
            _eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
        }

        protected override void OnStart(string[] args)
        {
            _timer.Enabled = true;
            _eventLog.WriteEntry("Service start.");

            try
            {
                _configManager = new ConfigManager();
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry(ex.ToString());
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
            _eventLog.WriteEntry("Service stop.");
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Enabled = false;

            _eventLog.WriteEntry("AdvertUrl checking start.");

            int startRow = 1;
            List<Thread> lstThreads = new List<Thread>();
            List<StringBuilder> lstSbLogs = new List<StringBuilder>();
            List<AdvertsUrlChecker> lstAdUrlChecker = new List<AdvertsUrlChecker>();

            try
            {
                while (lstThreads.Count < _configManager.NumOfThreads)
                {
                    // recieve "pages" of adverts
                    List<AdvertUrl> lstAdUrl = AdvertsUrlChecker.GetAdvertsForUrlChecking(_configManager.ConnString, startRow, _configManager.NumOfAdvertsPerThread);
                    startRow += _configManager.NumOfAdvertsPerThread;

                    // create StringBuilders for logs
                    StringBuilder sbLog = new StringBuilder();
                    lstSbLogs.Add(sbLog);

                    // create AdvertsUrlCheckers with AdvertLists to be checked in threads
                    AdvertsUrlChecker adUrlChecker = new AdvertsUrlChecker(lstAdUrl, _configManager.WebReqTimeout, sbLog);
                    lstAdUrlChecker.Add(adUrlChecker);

                    // create threads for checking urls
                    Thread th = new Thread(new ThreadStart(adUrlChecker.CheckUrls));
                    lstThreads.Add(th);

                    _eventLog.WriteEntry(lstAdUrl.Count + " adverts recieved from database for threadId: " + th.ManagedThreadId.ToString());
                }

                foreach (Thread th in lstThreads)
                {
                    th.Start();
                }
                foreach (Thread th in lstThreads)
                {
                    th.Join();
                }

                foreach (StringBuilder sb in lstSbLogs)
                {
                    // paziti da sb.ToString() ne bude prevelik za zapis u log
                    try
                    {
                        _eventLog.WriteEntry(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        _eventLog.WriteEntry(ex.ToString());
                    }
                }

                foreach (AdvertsUrlChecker adUrlCh in lstAdUrlChecker)
                {
                    // poslati liste provjerenih urlova u bazu
                    XDocument xdoc = new XDocument(new XElement("root"));

                    foreach (AdvertUrl adUrl in adUrlCh.LstAdvertUrl)
                    {
                        xdoc.Root.Add(new XElement("advert",
                            new XAttribute("id", adUrl.AdvertId.ToString()),
                            new XAttribute("isUrlValid", adUrl.IsValid ? "1" : "0")));
                    }

                    _eventLog.WriteEntry("Sending xml to database:" + Environment.NewLine + xdoc.ToString());

                    AdvertsUrlChecker.UpdateAdvertsUrlChecked(_configManager.ConnString, xdoc);
                }
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry(ex.ToString());
            }

            _timer.Interval = _configManager.ServiceTimeStep;
            _timer.Enabled = true;
        }
    }
}
