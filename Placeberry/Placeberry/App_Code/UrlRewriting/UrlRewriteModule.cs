using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class UrlRewriteModule : IHttpModule
{
	public void Dispose()
	{
	}

	public void Init(HttpApplication context)
	{
		context.BeginRequest += new System.EventHandler(Rewrite_BeginRequest);
	}

	public void Rewrite_BeginRequest(object sender, System.EventArgs e)
	{
		string strPath = HttpContext.Current.Request.Url.AbsolutePath;

		string strQuery = HttpContext.Current.Request.Url.Query.Replace("?", "");

		/*if (strPath.IndexOf("?") > 0)
			strPath = strPath.Substring(0, strPath.IndexOf("?"));*/

		UrlRedirection oPR = new UrlRedirection();

		string strURL = strPath;

        //Ovdje bilježimo Google friendly URL
        HttpContext.Current.Items.Add("CURRENT_URL", strPath);
        
		string strRewrite = oPR.GetMatchingRewrite(strPath);
 

		if ((!String.IsNullOrEmpty(strRewrite)) /*&& (strPath.IndexOf("/admin") < 0)*/)
		{
			strURL = strRewrite;
		}
		else
		{
            //error page - not found
            strURL = /*"/default.aspx";//*/strPath;
		}

        string strUrlFinally = "~" + strURL +(strURL.IndexOf("?") < 0 ? "?" : "&") + strQuery;
        //throw new Exception(strUrlFinally);
		HttpContext.Current.RewritePath(strUrlFinally);
	}
}