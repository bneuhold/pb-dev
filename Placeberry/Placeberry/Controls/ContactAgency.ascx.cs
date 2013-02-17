using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.IO;

public partial class Controls_ContactAgency : System.Web.UI.UserControl
{
    private string agencyEmail = string.Empty;
    public string AgencyEmail
    {
        get { return this.agencyEmail; }
        set { this.agencyEmail = value ?? string.Empty; }
    }
    private string agencyName = string.Empty;
    public string AgencyName
    {
        get { return this.agencyName; }
        set { this.agencyName = value ?? string.Empty; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(agencyEmail))
        {
            this.Visible = false;
        }
    }
    protected void btnContactSubmit_Click(object sender, EventArgs e)
    {
        Page.Validate("contact");
        if (Page.IsValid)
        {
            string emailpath = Server.MapPath(WebConfigurationManager.AppSettings["AgencyContactEmail"]);

            if (File.Exists(emailpath))
            {
                string body = File.ReadAllText(emailpath);
                body = body.Replace("{name}", tbxName.Text.Trim());
                body = body.Replace("{email}", tbxEmail.Text.Trim().ToLower());
                body = body.Replace("{phone}", tbxTelephone.Text.Trim());
                body = body.Replace("{capacity}", tbxCapacity.Text.Trim());
                body = body.Replace("{date_start}", tbxDateStart.Text.Trim());
                body = body.Replace("{date_end}", tbxDateEnd.Text.Trim());
                body = body.Replace("{message}", tbxMessage.Text.Trim().Replace("\n\r", "<br />").Replace("\n","<br />"));
                
                string heading = String.Format("{0} contact email", agencyName);

                bool sucess = Emailing.SendEmail(string.Empty, "mariosloba@gmail.com", heading, body);

                if (sucess)
                {
                    mvwSendState.ActiveViewIndex = 1;
                }
                else
                {
                    mvwSendState.ActiveViewIndex = 2;
                }
            }

        }


    }
}