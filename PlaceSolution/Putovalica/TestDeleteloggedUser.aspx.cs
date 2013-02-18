using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class TestDeleteloggedUser : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.btnDeleteUser.Click += new EventHandler(btnDeleteUser_Click);
        this.btnChangePass.Click += new EventHandler(btnChangePass_Click);
    }

    void btnChangePass_Click(object sender, EventArgs e)
    {
        string username = tbUserName.Text;//"domagoj.peceli@yahoo.com"
        string password = "pass@word";

        MembershipUser user = Membership.GetUser(username);
        if (user != null)
        {
            this.lblNewPassword.Text = user.ResetPassword();
        }
        else
        {
            this.lblNewPassword.Text = "krivi user name";
        }

        //mu.ChangePassword(mu.ResetPassword(), password);
    }

    void btnDeleteUser_Click(object sender, EventArgs e)
    {
        MembershipUser memUser = Membership.GetUser();
        Collective.User collUser = Collective.User.GetUser(memUser);
        Membership.DeleteUser(memUser.Email);
        if(collUser != null)
            collUser.Delete();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}