using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);

        this.Title = Resources.placeberry.Home_Title;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(this.txtSearch.Text.Trim()))
            return;

        Response.Redirect(String.Format("~/{0}?q={1}", Resources.placeberry.General_VacationUrl, HttpUtility.UrlEncode(this.txtSearch.Text.Trim())));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (UltimateDC.UltimateDataContext dc = new UltimateDC.UltimateDataContext())
            {
                repPopularQueries.DataSource = (from p in dc.PopularQueries
                                                where p.Active == true && p.LanguageId == Common.GetLanguageId()
                                                orderby p.Priority descending
                                                select new
                                                {
                                                    Title = p.Query,
                                                    Link = String.Format("/{0}?q={1}", Resources.placeberry.General_VacationUrl, HttpUtility.UrlEncode(p.Query))
                                                }).Take(12);
                repPopularQueries.DataBind();
            }
        }
    }
}