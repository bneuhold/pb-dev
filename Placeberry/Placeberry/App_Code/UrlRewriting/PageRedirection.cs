using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

public class UrlRedirection
{
	XmlDocument oDoc = new XmlDocument();

	private string strFile = System.Web.HttpContext.Current.Server.MapPath("/RewriteRules.config");

	private System.Collections.Generic.List<RedirectRule> colRules;

	public System.Collections.Generic.List<RedirectRule> Rules
	{
		get
		{
			if (colRules == null)
				colRules = GetRules();
			
			return colRules;
		}
	}

	System.Collections.Generic.List<RedirectRule> GetRules()
	{
		System.Collections.Generic.List<RedirectRule> col = new List<RedirectRule>();
		System.Xml.XmlNode _oRules = oDoc.SelectSingleNode("//urlrewrites");

		foreach (System.Xml.XmlNode oNode in _oRules.SelectNodes("rule"))
		{
			//url/text()
			if ((oNode.SelectSingleNode("url/text()") != null) && (oNode.SelectSingleNode("rewrite/text()") != null))
			{
				RedirectRule oR = new RedirectRule();
				oR.Name = oNode.Attributes["name"].Value;
				oR.URL = oNode.SelectSingleNode("url/text()").Value;
				oR.Rewrite = oNode.SelectSingleNode("rewrite/text()").Value;
				col.Add(oR);
			}
		}
		return col;
	}

	public string GetMatchingRewrite(string URL)
	{
		string strRtrn = "";
		System.Text.RegularExpressions.Regex oReg;

		foreach (RedirectRule oRule in Rules)
		{
			oReg = new Regex(oRule.URL, RegexOptions.IgnoreCase);
			Match oMatch = oReg.Match(URL);

            if (oMatch.Success)
            {
                strRtrn = oReg.Replace(URL, oRule.Rewrite);
            }
		}

		return strRtrn;
    }

	public UrlRedirection()
	{
        oDoc.Load(strFile);
	}
}

public class RedirectRule
{
	public RedirectRule()
	{

	}

	string strUrl = "";

	string strRewrite = "";

	string strName = "";

	public string Name
	{
		get { return strName; }
		set { strName = value; }
	}

	public string URL
	{
		get { return strUrl; }
		set { strUrl = value; }
	}

	public string Rewrite
	{
		get { return strRewrite; }
		set { strRewrite = value; }
	}

}
