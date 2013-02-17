using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Globalization;
using System.Web.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

public partial class Controls_FormLoginCtrl : System.Web.UI.UserControl
{
    private bool isBuyPage;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.lbRetPass.Click += new EventHandler(lbRetPass_Click);
        this.lbNewActCode.Click += new EventHandler(lbNewActCode_Click);
    }

    void lbRetPass_Click(object sender, EventArgs e)
    {
        MembershipUser user = Membership.GetUser(tbRetPassEmail.Text);
        if (user != null)
        {
            if (user.IsApproved)
            {
                Emailing.SendEmail(string.Empty, tbRetPassEmail.Text, "Promjena lozinke", "Vaša nova lozinka: " + user.ResetPassword());
                this.lblRetPassMsg.Text = "Na email vam je poslana nova lozinka.";
                this.lblRetPassMsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                this.lblRetPassMsg.Text = "Korisnik sa ovim emailom nije aktiviran.";
                this.lblRetPassMsg.ForeColor = System.Drawing.Color.Red;
            }
        }
        else
        {
            this.lblRetPassMsg.Text = "Nepostojeća email adresa.";
            this.lblRetPassMsg.ForeColor = System.Drawing.Color.Red;
        }
    }

    void lbNewActCode_Click(object sender, EventArgs e)
    {
        MembershipUser user = Membership.GetUser(tbNewActCodeEmail.Text);
        if (user != null)
        {
            if (!user.IsApproved)
            {
                string newActCode = Collective.User.GetNewActivationKey((Guid)user.ProviderUserKey);

                string emailpath = Server.MapPath(WebConfigurationManager.AppSettings["UserActivationEmail"]);

                if (File.Exists(emailpath))
                {
                    string body = File.ReadAllText(emailpath);
                    body = body.Replace("{activation_link}", String.Format("http://" + Request.Url.Authority + "/login.aspx?action=activate&activationkey={0}", newActCode));

                    Emailing.SendEmail(string.Empty, tbNewActCodeEmail.Text, "Novi aktivacijski kod", body);
                }
                
                this.lblNewActCodeMsg.Text = "Na email vam je poslan novi aktivacijski kod.";
                this.lblNewActCodeMsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                this.lblNewActCodeMsg.Text = "Korisnik sa ovom email adresom već je aktivan.";
                this.lblNewActCodeMsg.ForeColor = System.Drawing.Color.Red;
            }
        }
        else
        {
            this.lblNewActCodeMsg.Text = "Nepostojeća email adresa.";
            this.lblNewActCodeMsg.ForeColor = System.Drawing.Color.Red;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        isBuyPage = Request.Url.AbsoluteUri.ToLower().StartsWith("http://" + Request.Url.Authority + "/buyoffer.aspx");

        if (isBuyPage && ((UserPageBase)Page).GetLoggedCollectiveUser() != null)
        {
            this.Visible = false;
            return;
        }

            // OV JE NAKON LOGIN-a
        if (HttpContext.Current.User.IsInRole("Administrators"))
        {
            Response.Redirect(Page.ResolveUrl("~/admin/Default.aspx"));
        }
        else if (HttpContext.Current.User.IsInRole("Users"))
        {
            Response.Redirect(Page.ResolveUrl("~/Default.aspx"));
        }

        string action = Request.QueryString["action"] ?? string.Empty;

        switch (action)
        {
            case "login":
                DoLogin();
                break;
            case "activate":
                DoActivate();
                break;
            case "resetpassword":
                DoResetPassword();
                break;
            default:
                DoLogin();
                break;
        }
    }

    private void DoLogin()
    {
        mvwLogin.ActiveViewIndex = 0;
    }

    private void DoActivate()
    {
        mvwLogin.ActiveViewIndex = 1;

        ltlError.Visible = true;

        string activationkey = Request.QueryString["activationkey"];
        if (!string.IsNullOrEmpty(activationkey))
        {
            Guid key = new Guid(activationkey);

            Collective.User user = Collective.User.ActivateUser(key);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Email, true);    // login ce mu biti email
                if (!Roles.IsUserInRole(user.Email, "Users"))
                {
                    Roles.AddUserToRole(user.Email, "Users");
                }
                ltlSucess.Visible = true;
                ltlError.Visible = false;
                Response.AddHeader("REFRESH", "3;URL=/Default.aspx");
            }
        }
    }

    private void DoResetPassword()
    {
        mvwLogin.ActiveViewIndex = 2;
    }

    protected void lbLogin_Click(object sender, EventArgs e)
    {
        Page.Validate("login");
        if (Page.IsValid)
        {
            string username = tbxUserName.Text.Trim();
            string password = tbxPassword.Text.Trim();

            MembershipUser user = Membership.GetUser(username);

            if (user == null)
            {
                lblLoginMessage.Visible = true;
                lblLoginMessage.Text = "Krivo korisničko ime.";
                return;
            }
            else if (!Membership.ValidateUser(username, password))
            {
                lblLoginMessage.Visible = true;

                if (!user.IsApproved)
                {
                    lblLoginMessage.Text = "Korisnik sa ovom email adresom vec postoji u bazi, ali aktivacija emailom nije izvršena.";
                    return;
                }

                lblLoginMessage.Text = "Krivo unesena šifra";
                return;
            }

            if (isBuyPage)
            {
                FormsAuthentication.SetAuthCookie(username, chbxRememberMe.Checked);
                Response.Redirect(Request.Url.AbsoluteUri); // mora se redirectati da bi login upalio
            }
            else
            {
                FormsAuthentication.RedirectFromLoginPage(username, chbxRememberMe.Checked);
            }
        }
    }
}