using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using UltimateDC;
using System.Data;

public partial class Customer : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool admin = User.IsInRole("Administrators");

        plhAdminOptions.Visible = admin;

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            var agencies = from p in dc.Agencies
                           where admin || p.ManagerId == (Guid)Membership.GetUser().ProviderUserKey
                           select p;

            if (agencies.Any())
            {
                //Ako je samo jedna onda neka odmah ide na stranicu s agencijom
                if (agencies.Count() == 1 && !admin)
                    Response.Redirect(String.Format("/manage/Agency.aspx?agencyId={0}", agencies.Single().Id));

                repAgencies.DataSource = agencies;
                repAgencies.DataBind();

            }
            else if(!admin)
            {
                //Ako nema ni jedne Agencije neka korisnik stvori novu agenciju
                Response.Redirect(String.Format("/manage/Agency.aspx?action=newagency"));

            }
        }


    }


}

