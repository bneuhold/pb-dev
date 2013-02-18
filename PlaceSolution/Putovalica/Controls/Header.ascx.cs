using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Controls_Header : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.lbLogout.Click += new EventHandler(lbLogout_Click);
        this.lbLogin.Click += new EventHandler(lbLogin_Click);
    }

    void lbLogout_Click(object sender, EventArgs e)
    {
        UserPageBase pageBase = (UserPageBase)Page;
        pageBase.LogoutCollectiveUser();
        FormsAuthentication.SignOut();
        Roles.DeleteCookie();
        Session.Clear();

        Response.Redirect(Request.Url.AbsoluteUri);
        //Response.Redirect(FormsAuthentication.DefaultUrl);
        //FormsAuthentication.RedirectToLoginPage();
    }

    void lbLogin_Click(object sender, EventArgs e)
    {
        FormsAuthentication.RedirectToLoginPage();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (((UserPageBase)Page).GetLoggedCollectiveUser() == null)
        {
            // FACEBOOK LOGIN
            string fbtoken = Request.QueryString["fbtoken"];
            if (!String.IsNullOrEmpty(fbtoken))
            {
                FacebookResponse fbResp = PutovalicaUtil.GetFacebookResponse(fbtoken);
                if (fbResp != null && fbResp.Verified)
                {
                    MembershipUser user = Membership.GetUser(fbResp.Email);

                    if (user == null)
                    {
                        MembershipCreateStatus status;
                        user = Membership.CreateUser(fbResp.Email, Membership.GeneratePassword(8, 3), fbResp.Email, null, null, true, Guid.NewGuid(), out status);
                        if (status == MembershipCreateStatus.Success)
                        {
                            Collective.User newCollUser = Collective.User.CreateCollectiveUser(user, null, fbResp.FirstName, fbResp.LastName, null, null, null, null, null);
                            Roles.AddUserToRole(user.Email, "Users");
                        }
                    }

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.Email, false);   // sta ako ce ovdje trebat postaviti zapamti me?

                        if (!user.IsApproved)
                        {
                            user.IsApproved = true;
                            Membership.UpdateUser(user);
                        }
                    }

                    string url = PutovalicaUtil.QSRemoveArg(Request.Url.AbsoluteUri, "fbtoken");
                    Response.Redirect(url); // mora se redirectati da bi login upalio
                }
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (((UserPageBase)Page).GetLoggedCollectiveUser() == null)
        {
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