using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Configuration;
using System.IO;

public partial class Controls_FormRegistrationCtrl : System.Web.UI.UserControl
{
    private bool isBuyPage;

    protected void Page_Load(object sender, EventArgs e)
    {
        isBuyPage = Request.Url.AbsoluteUri.ToLower().StartsWith("http://" + Request.Url.Authority + "/buyoffer.aspx");

        UserPageBase pageBase = (UserPageBase)Page;

        if (isBuyPage)
        {
            if (pageBase.GetLoggedCollectiveUser() != null)
            {
                this.Visible = false;
                return;
            }
        }

        this.phCaptcha.Visible = !isBuyPage;

        this.lbSubmit.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(this.lbSubmit, "").ToString());        
    }

    protected void lbSubmit_Click(object sender, EventArgs e)
    {
        Page.Validate("register");
        if (Page.IsValid)
        {
            if (!isBuyPage)
            {
                ccJoin.ValidateCaptcha(tbCaptcha.Text);
                if (!ccJoin.UserValidated)
                {
                    ltlCaptchaError.Visible = true;
                    return;
                }
                else
                {
                    ltlCaptchaError.Visible = false;
                }
            }

            string email = tbEmail.Text.Trim().ToLower();
            string password = tbPassword.Text.Trim();

            //Ako postoji NEAKTIVIRANI korisnik s istim korisničkim imenom/emailom pobriši ga, da bezveze ne baca duplicate email grešku
            Collective.User inactiveUser = Collective.User.GetInactiveUserByEmail(email);

            //if (inactiveUser != null)
            //{
            //    Membership.DeleteUser(inactiveUser.Email);
            //    inactiveUser.Delete();  // foreign key na place usera sa asp-ovog je disablean radi bookinga
            //}


            MembershipCreateStatus status;
            MembershipUser newuser = Membership.CreateUser(email, password, email, null, null, false, Guid.NewGuid(), out status);

            switch (status)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    //ltlStatusErrorMessage.Text = status.ToString();
                    ltlStatusErrorMessage.Text = inactiveUser != null ? "Korisnik sa ovom email adresom vec postoji u bazi, ali aktivacija emailom nije izvršena." : "Korisnik sa ovom email adresom vec postoji u bazi.";
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    //ltlStatusErrorMessage.Text = status.ToString();
                    ltlStatusErrorMessage.Text = inactiveUser != null ? "Korisnik sa ovom email adresom vec postoji u bazi, ali aktivacija emailom nije izvršena." : "Korisnik sa ovom email adresom vec postoji u bazi.";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.ProviderError:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.UserRejected:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.Success:

                    CreateCollectiveUser(newuser);

                    Roles.AddUserToRole(newuser.Email, "Users");

                    if (isBuyPage)
                    {
                        // ovak ce ic za registraciju prije kupnje
                        FormsAuthentication.SetAuthCookie(newuser.Email, true);    // login ce mu biti email                        
                        Response.Redirect(Request.Url.AbsoluteUri); // mora redirect da bi login upalio
                    }

                    this.phRegisterForm.Visible = !(this.phSucessMsg.Visible = true);

                    break;
                default:
                    break;
            }
        }
    }

    private void CreateCollectiveUser(MembershipUser memUser)
    {
        Guid activationKey = Guid.NewGuid();

        Collective.User newCollUser = Collective.User.CreateCollectiveUser(memUser, activationKey.ToString(), tbFirstName.Text, tbLastName.Text, tbPhone.Text, tbCountry.Text, tbCity.Text, tbZipCode.Text, tbStreet.Text);

        SendActivationEmailToUser(memUser, activationKey.ToString());
    }

    private void SendActivationEmailToUser(MembershipUser memUser, string activationKey)
    {
        string emailpath = Server.MapPath(WebConfigurationManager.AppSettings["UserActivationEmail"]);

        if (File.Exists(emailpath))
        {
            string body = File.ReadAllText(emailpath);
            body = body.Replace("{activation_link}", String.Format("http://" + Request.Url.Authority + "/login.aspx?action=activate&activationkey={0}", activationKey));

            Emailing.SendEmail(string.Empty, memUser.Email, "Aktivacija accounta", body);
        }
    }
}