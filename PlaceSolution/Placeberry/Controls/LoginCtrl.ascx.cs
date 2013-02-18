using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Controls_LoginCtrl : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.lbLogout.Click += new EventHandler(lbLogout_Click);
        this.lbLogin.Click += new EventHandler(lbLogin_Click);
    }

    void lbLogin_Click(object sender, EventArgs e)
    {
        FormsAuthentication.RedirectToLoginPage();
    }

    void lbLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Roles.DeleteCookie();
        Session.Clear();

        Response.Redirect(FormsAuthentication.DefaultUrl);

        //FormsAuthentication.RedirectToLoginPage();    // ovo mi je nekaj zeznulo (zablokiralo site). mozda slucajno
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Membership.GetUser() == null)
        {
            this.lbLogin.Visible = true;
            this.phLogout.Visible = false;
        }
        else
        {
            this.phLogin.Visible = false;
            this.phLogout.Visible = true;
            this.lblUserName.Text = Membership.GetUser().UserName;
        }
    }
}