using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Globalization;
using UltimateDC;


public partial class Vacation : System.Web.UI.Page
{
    public string query = "";

    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);

        this.Title = Resources.placeberry.Vacation_Title;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            query = Request["q"];

            if (String.IsNullOrEmpty(query))
                Response.Redirect("/", true);

            int languageId = Common.GetLanguageId();

            HtmlMeta metaKeywords = new HtmlMeta() { Name = "keywords", Content = string.Empty };
            HtmlMeta metaDescription = new HtmlMeta() { Name = "description", Content = string.Empty };

            metaKeywords.Content = query.Replace(" ", ", ");


            this.lblQuery.Text = this.txtSearch.Text = HttpUtility.UrlDecode(query);
            this.Title = query + " - " + this.Title;

            var sortOrder = 0; // by relevance po defaultu
            int.TryParse(Request["o"], out sortOrder);

            using (UltimateDataContext dc = new UltimateDataContext())
            {

                string queryMessage = string.Empty;

                odsResults.SelectParameters.Add(new Parameter("query", TypeCode.String, query));
                odsResults.SelectParameters.Add(new Parameter("languageId", TypeCode.Int32, languageId.ToString()));
                odsResults.SelectParameters.Add(new Parameter("top", TypeCode.Int32, null));
                odsResults.SelectParameters.Add(new Parameter("orderBy", TypeCode.Int32, sortOrder.ToString()));
                var paramqueryMessage = new Parameter("queryMessage", TypeCode.String);
                paramqueryMessage.Direction = ParameterDirection.Output;
                odsResults.SelectParameters.Add(paramqueryMessage);

                // Poznati bug s DataPagerom!! PageSize u markupu nije bitan, ovdje se mora postavljati! Inače zna stvarat probleme s prikazom krive stranice ...
                dpResults.PageSize = 10;
                int startRow = 0;
                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString[dpResults.QueryStringField]))
                        startRow = dpResults.PageSize * (Convert.ToInt32(Request.QueryString[dpResults.QueryStringField]) - 1);
                }
                catch
                {
                    startRow = 0;
                }
                dpResults.SetPageProperties(startRow, dpResults.PageSize, true);
                // *******


                dlResults.DataBind();

                this.lblResultsCount.Text = dpResults.TotalRowCount.ToString();



                var ult = (dc.GetParsedQuery(query, languageId)).ToList();


                var seodirectory = (from p in ult
                                    select p).Take(1).SingleOrDefault();

                if (seodirectory != null)
                {
                    SeoDirectory1.ParentTerm = seodirectory.Title;
                }



                string accommOrActivity = string.Empty;
                string place = string.Empty;
                string parentPlace = string.Empty;

                accommOrActivity = (from p in ult
                                    where p.ObjectTypeCode == "ACCOMM" || p.ObjectTypeCode == "ACTIVITY"
                                    orderby p.Priority descending
                                    select p.Title).Take(1).SingleOrDefault() ?? Resources.placeberry.General_Accommodation;

                var term = (from p in ult
                            where p.ObjectTypeGroupCode == "GEO"
                            orderby p.Priority descending
                            select p).Take(1).SingleOrDefault();

                if (term != null)
                {
                    place = term.Title ?? string.Empty;

                    parentPlace = (from p in dc.UltimateTableRelations
                                   join q in dc.UltimateTableTranslations on p.Parent equals q.UltimateTable into trans
                                   from r in trans.Where(i => i.Language.Id == languageId && i.Active == true).DefaultIfEmpty()
                                   where p.ChildId == term.Id
                                   select r != null ? r.Title : p.Parent.Title).SingleOrDefault() ?? string.Empty;
                }


                metaDescription.Content = String.Format(Resources.placeberry.Vacation_MetaDescription, accommOrActivity, place, parentPlace);
                Header.Controls.Add(metaDescription);
            }

        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(this.txtSearch.Text.Trim()))
            return;

        Response.Redirect(String.Format("~/{0}?q={1}", Resources.placeberry.General_VacationUrl, this.txtSearch.Text.Trim()));
    }
    protected void dlResults_DataBound(object sender, EventArgs e)
    {
        var currentPage = (dpResults.StartRowIndex / dpResults.PageSize) + 1;

        if (dpResults.TotalRowCount <= dpResults.PageSize)
        {
            dpResults.Visible = false;
        }
        else if (currentPage == 1)
        {

        }
    }
    protected void odsResults_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ltlQueryMessage.Text = e.OutputParameters["queryMessage"] as string;
    }


}
