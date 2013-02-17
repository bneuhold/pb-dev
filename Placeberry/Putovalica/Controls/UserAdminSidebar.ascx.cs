using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_UserAdminSidebar : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Url.LocalPath.ToLower().StartsWith("/useradmin/coupons.aspx"))
            {
                this.liCoupons.Attributes.Add("class", "active");
            }
            else
            {
                this.liProfile.Attributes.Add("class", "active");
            }

        }
    }
}