using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Controls_AdminLogout : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.lbLogout.Click += new EventHandler(lbLogout_Click);
    }

    void lbLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Roles.DeleteCookie();
        Session.Clear();

        Response.Redirect(Page.ResolveUrl("~/Login.aspx"));

        //FormsAuthentication.RedirectToLoginPage();    // ovo mi je nekaj zeznulo (zablokiralo site). mozda slucajno

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Membership.GetUser() != null)
            {
                this.lblUserName.Text = Membership.GetUser().UserName;
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}