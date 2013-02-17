using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class WSpay_Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();

        foreach (string key in Request.QueryString.AllKeys)
        {
            sb.Append(key + "=" + Request.QueryString[key]);
            sb.Append("<br />");
        }

        this.ltErr.Text = sb.ToString();
    }
}