using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using UltimateDC;

public partial class Login : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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

        string activationkey = Request.QueryString["activationkey"];
        if (!string.IsNullOrEmpty(activationkey))
        {
            Guid key = new Guid(activationkey);
            using (UltimateDataContext dc = new UltimateDataContext())
            {
                var user = (from p in dc.PlaceberryUsers
                            where p.ActivationKey.HasValue && p.ActivationKey.Value == key
                            select p).SingleOrDefault();

                if (user != null)
                {
                    user.aspnet_User.aspnet_Membership.IsApproved = true;
                    user.ActivationKey = null;

                    dc.SubmitChanges();

                    FormsAuthentication.SetAuthCookie(user.aspnet_User.UserName, true);

                    ltlSucess.Visible = true;

                    Response.AddHeader("REFRESH", "3;URL=/manage");
                }
                else
                {
                    ltlError.Visible = true;
                }
            }
        }
        else
        {
            ltlError.Visible = true;
        }

    }
    private void DoResetPassword()
    {
        mvwLogin.ActiveViewIndex = 2;
    }





    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Validate("login");
        if (IsValid)
        {
            string username = tbxUserName.Text.Trim();
            string password = tbxPassword.Text.Trim();

            if (Membership.ValidateUser(username,password))
            {
                if (Membership.GetUser(username).IsApproved)
                {
                    //FormsAuthentication.SetAuthCookie(username, chbxRememberMe.Checked);
                    FormsAuthentication.RedirectFromLoginPage(username, chbxRememberMe.Checked);
                }
                else
                {
                    ltlLoginMessage.Text = "Vaš račun nije aktiviran";
                }
            }
            else
            {
                ltlLoginMessage.Text = "Krivo korisničko ime ili šifra";
                ltlLoginMessage.Visible = true;
            }


        }
    }
}