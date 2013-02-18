using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using UltimateDC;
using System.IO;
using System.Web.Configuration;
using System.Text.RegularExpressions;


public partial class Registration : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Validate();
        if (IsValid)
        {
            ccJoin.ValidateCaptcha(tbxCaptcha.Text);
            if (!ccJoin.UserValidated)
            {
                ltlCaptchaError.Visible = true;
                return;
            }

            string username = tbxUserName.Text.Trim();
            string password = tbxPassword.Text.Trim();
            string email = tbxEmail.Text.Trim().ToLower();

            //Ako postoji NEAKTIVIRANI korisnik s istim korisničkim imenom/emailom pobriši ga, da bezveze ne baca duplicate email grešku
            using (UltimateDataContext dc = new UltimateDataContext())
            {
                var user = (from p in dc.PlaceberryUsers
                            where   p.aspnet_User.aspnet_Membership.LoweredEmail == email && 
                                    p.aspnet_User.aspnet_Membership.IsApproved == false && 
                                    p.ActivationKey != null
                            select p).SingleOrDefault();
                if (user != null)
                {
                    Membership.DeleteUser(user.aspnet_User.UserName);
                }
            }


            MembershipCreateStatus status;
            MembershipUser newuser = Membership.CreateUser(username, password, email, null, null, false, Guid.NewGuid(), out status);

            switch (status)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    ltlStatusErrorMessage.Text = status.ToString();
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    ltlStatusErrorMessage.Text = status.ToString();
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
                    CreatePlaceberryUser(newuser);
                    ltlStatusErrorMessage.Text = "Uspješno ste se registirali, na email vam trebaju stići podaci za registraciju";
                    break;
                default:
                    break;
            }
        }
    }

    private void CreatePlaceberryUser(MembershipUser user)
    {
        using (UltimateDataContext dc = new UltimateDataContext())
        {
            PlaceberryUser newuser = new PlaceberryUser();
            newuser.UserId = (Guid)user.ProviderUserKey;
            newuser.ActivationKey = Guid.NewGuid();

            dc.PlaceberryUsers.InsertOnSubmit(newuser);
            dc.SubmitChanges();

            SendActivationEmailToUser(user, newuser.ActivationKey.Value.ToString());
        }
    }

    private void SendActivationEmailToUser(MembershipUser user, string activationKey)
    {
        string emailpath = Server.MapPath(WebConfigurationManager.AppSettings["UserActivationEmail"]);

        if (File.Exists(emailpath))
        {
            string body = File.ReadAllText(emailpath);
            body = body.Replace("{activation_link}", String.Format("http://www.placeberry.com/login.aspx?action=activate&activationkey={0}", activationKey));

            Emailing.SendEmail(string.Empty, user.Email, "Aktivacija accounta", body);
        }
    }
}