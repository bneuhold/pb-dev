using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AdvertUrlChecking
{
    public class ConfigManager
    {
        private XDocument _xdoc;
        private string[] XML_NODES = { "ConnString", "StartHour", "StartMinutes", "NumOfAdvertsPerThread", "NumOfThreads",
                                         "WebReqTimeout", "ServiceTimeStep" };


        private string _connString;
        private int _startHour;
        private int _startMinutes;
        private int _numOfAdvertsPerThread;
        private int _numOfThreads;
        private int _webReqTimeout;
        private int _serviceTimeStep;

        public string ConnString { get { return _connString; } }
        public int StartHour { get { return _startHour; } }
        public int StartMinutes { get { return _startMinutes; } }
        public int NumOfAdvertsPerThread { get { return _numOfAdvertsPerThread; } }
        public int NumOfThreads { get { return _numOfThreads; } }
        public int WebReqTimeout { get { return _webReqTimeout; } }
        public int ServiceTimeStep { get { return _serviceTimeStep; } }

        public ConfigManager()
        {
            try
            {
                // try to load config
                _xdoc = XDocument.Load("PlaceberryConfig.xml");
            }
            catch
            {
                // if load fails, create one
                _xdoc = new XDocument(
                   new XComment("Placeberry UrlServiceChecker configuration"),
                   new XElement("Configuration"));

                foreach (string nodeName in XML_NODES)
                {
                    _xdoc.Root.Add(new XElement(nodeName, String.Empty));
                }

                _xdoc.Save("PlaceberryConfig.xml");

                // throw exception to fulfill config file
                throw new Exception("Configuration file error!" + Environment.NewLine +
                    "PlaceberryConfig.xml could not be found, or file is invalid." + Environment.NewLine +
                    "File created: " + Environment.CurrentDirectory + "\\PlaceberryConfig.xml" + Environment.NewLine);
            }

            // validate xml schema
            foreach (string nodeName in XML_NODES)
            {
                if (_xdoc.Root.Element(nodeName) == null)
                {
                    throw new Exception("Configuration file error!" + Environment.NewLine + nodeName + " missing." + Environment.NewLine);
                }
            }

            // validate config data and populate properties

            string strValue;

            _connString = _xdoc.Root.Element("ConnString").Value.Trim();
            if (String.IsNullOrEmpty(_connString))
            {
                throw new Exception("Configuration file error!" + Environment.NewLine + "ConnString bad format." + Environment.NewLine);
            }

            strValue = _xdoc.Root.Element("StartHour").Value.Trim();
            if (String.IsNullOrEmpty(strValue) || !Int32.TryParse(strValue, out _startHour))
            {
                throw new Exception("Configuration file error!" + Environment.NewLine + "StartHour bad format." + Environment.NewLine);
            }

            strValue = _xdoc.Root.Element("StartMinutes").Value.Trim();
            if (String.IsNullOrEmpty(strValue) || !Int32.TryParse(strValue, out _startMinutes))
            {
                throw new Exception("Configuration file error!" + Environment.NewLine + "StartMinutes bad format." + Environment.NewLine);
            }

            strValue = _xdoc.Root.Element("NumOfAdvertsPerThread").Value.Trim();
            if (String.IsNullOrEmpty(strValue) || !Int32.TryParse(strValue, out _numOfAdvertsPerThread))
            {
                throw new Exception("Configuration file error!" + Environment.NewLine + "NumOfAdverts bad format." + Environment.NewLine);
            }

            strValue = _xdoc.Root.Element("NumOfThreads").Value.Trim();
            if (String.IsNullOrEmpty(strValue) || !Int32.TryParse(strValue, out _numOfThreads))
            {
                throw new Exception("Configuration file error!" + Environment.NewLine + "NumOfThreads bad format." + Environment.NewLine);
            }

            strValue = _xdoc.Root.Element("WebReqTimeout").Value.Trim();
            if (String.IsNullOrEmpty(strValue) || !Int32.TryParse(strValue, out _webReqTimeout))
            {
                throw new Exception("Configuration file error!" + Environment.NewLine + "WebReqTimeout bad format." + Environment.NewLine);
            }

            strValue = _xdoc.Root.Element("ServiceTimeStep").Value.Trim();
            if (String.IsNullOrEmpty(strValue) || !Int32.TryParse(strValue, out _serviceTimeStep))
            {
                throw new Exception("Configuration file error!" + Environment.NewLine + "ServiceTimeStep bad format." + Environment.NewLine);
            }
        }
    }
}
