using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class userAdmin_Default : UserPageBase
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.lbChange.Click += new EventHandler(lbChange_Click);
        this.lbCancel.Click += new EventHandler(lbCancel_Click);
        this.lbSave.Click += new EventHandler(lbSave_Click);
        this.lbSaveNewPass.Click += new EventHandler(lbSaveNewPass_Click);
    }

    void lbSaveNewPass_Click(object sender, EventArgs e)
    {
        Page.Validate("changePass");
        if (Page.IsValid)
        {
            MembershipUser memUser = Membership.GetUser();

            if (memUser == null)
            {
                Response.Redirect(Page.ResolveUrl("~/default.aspx"));
            }

            if (tbNewPass.Text.Length < Membership.MinRequiredPasswordLength)
            {
                this.lblMsgChangePass.ForeColor = System.Drawing.Color.Red;
                this.lblMsgChangePass.Text = "Nova lozinka mora sadrzavati 5 do 10 znakova.";
                this.divChangePass.Style.Add("display", "block");
            }
            else if (!memUser.ChangePassword(tbOldPass.Text, tbNewPass.Text))
            {
                this.lblMsgChangePass.ForeColor = System.Drawing.Color.Red;
                this.lblMsgChangePass.Text = "Pogrešno unesena stara lozinka.";
                this.divChangePass.Style.Add("display", "block");
            }
            else
            {
                this.lblMsgChangePass.ForeColor = System.Drawing.Color.Green;
                this.lblMsgChangePass.Text = "Lozinka uspješno izmjenjena.";
                this.divChangePass.Style.Add("display", "none");
            }
        }
        else
        {
            this.lblMsgChangePass.Text = string.Empty;
            this.divChangePass.Style.Add("display", "block");
        }
    }

    void lbSave_Click(object sender, EventArgs e)
    {
        UserPageBase pageBase = (UserPageBase)Page;
        Collective.User loggedUser = pageBase.GetLoggedCollectiveUser();
        loggedUser.UpdateUser(tbFirstName.Text, tbLastName.Text, tbPhone.Text, tbCountry.Text, tbCity.Text, tbZipCode.Text, tbStreet.Text);
        this.hfIsUpdate.Value = false.ToString().ToLower();
    }

    void lbCancel_Click(object sender, EventArgs e)
    {
        this.hfIsUpdate.Value = false.ToString().ToLower();
    }

    void lbChange_Click(object sender, EventArgs e)
    {
        this.hfIsUpdate.Value = true.ToString().ToLower();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        UserPageBase pageBase = (UserPageBase)Page;
        Collective.User loggedUser = pageBase.GetLoggedCollectiveUser();

        bool isUpdate = true;

        Boolean.TryParse(this.hfIsUpdate.Value, out isUpdate);

        lblFirstName.Visible = !(tbFirstName.Visible = isUpdate);
        lblLastName.Visible = !(tbLastName.Visible = isUpdate);
        lblPhone.Visible = !(tbPhone.Visible = isUpdate);
        lblCountry.Visible = !(tbCountry.Visible = isUpdate);
        lblCity.Visible = !(tbCity.Visible = isUpdate);
        lblZipCode.Visible = !(tbZipCode.Visible = isUpdate);
        lblStreet.Visible = !(tbStreet.Visible = isUpdate);

        lbChange.Visible = !(lbCancel.Visible = lbSave.Visible = isUpdate);

        if (isUpdate)
        {
            tbFirstName.Text = loggedUser.FirstName;
            tbLastName.Text = loggedUser.LastName;
            tbPhone.Text = loggedUser.Phone;
            tbCountry.Text = loggedUser.Country;
            tbCity.Text = loggedUser.City;
            tbZipCode.Text = loggedUser.ZipCode;
            tbStreet.Text = loggedUser.Street;
        }
        else
        {
            lblFirstName.Text = loggedUser.FirstName;
            lblLastName.Text = loggedUser.LastName;
            lblPhone.Text = loggedUser.Phone;
            lblCountry.Text = loggedUser.Country;
            lblCity.Text = loggedUser.City;
            lblZipCode.Text = loggedUser.ZipCode;
            lblStreet.Text = loggedUser.Street;
        }
    }
}