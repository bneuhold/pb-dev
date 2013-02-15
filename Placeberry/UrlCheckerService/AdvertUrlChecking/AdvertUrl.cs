using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace AdvertUrlChecking
{
    public class AdvertUrl
    {
        public int AdvertId { get; private set; }
        public string Url { get; private set; }
        public bool IsValid { get; set; }

        public AdvertUrl(int advertId, string url)
        {
            this.AdvertId = advertId;
            this.Url = url;
            this.IsValid = true;
        }
    }
}
