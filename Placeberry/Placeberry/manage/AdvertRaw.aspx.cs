using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Placeberry.DAL;

public partial class AdvertRaw : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
            Response.Redirect("/Default.aspx");

        if (!IsPostBack)
        {
            repAdverts.DataSource = GetAdvertRaw.Execute(null, (Guid) Membership.GetUser().ProviderUserKey);
            repAdverts.DataBind();
        }
    }
}