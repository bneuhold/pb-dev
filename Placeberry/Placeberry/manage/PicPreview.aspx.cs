using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class manage_PicPreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string what = Request.QueryString["what"];
        string content = Request.QueryString["content"];
        content = Server.UrlDecode(content);

        if (!string.IsNullOrEmpty(what))
        {
            object sesobj = Session[what];
            if (sesobj != null)
            {
                byte[] buffer = (byte[])sesobj;
                Response.ContentType = content;
                Response.BinaryWrite(buffer);
            }
        }
    }
}