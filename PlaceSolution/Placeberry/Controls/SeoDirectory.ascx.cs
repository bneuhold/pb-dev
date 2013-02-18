using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;
using System.Globalization;


public partial class Controls_SeoDirectory : System.Web.UI.UserControl
{
    const string DEF_LANG = "hr";
    const string DEF_QSVAR = "q";
    const string DEF_SEPAR = " &nbsp;|&nbsp;";
    private string parentTerm = null;
    private string language = null;
    private string queryStringVar = null;
    private string queryString = null;
    private string landingPage = null;
    private string separator = null;
    private bool showParent = false;

    public string ParentTerm
    {
        get { return this.parentTerm; }
        set { this.parentTerm = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToLower(); }
    }
    public string Language
    {
        get { return this.language; }
        set { this.language = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToLower(); }
    }
    public string QueryStringVariable
    {
        get { return this.queryStringVar; }
        set { this.queryStringVar = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToLower(); }
    }
    public string LandingPage
    {
        get { return this.landingPage; }
        set { this.landingPage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
    }
    public string Separator
    {
        get { return this.separator; }
        set { this.separator = string.IsNullOrEmpty(value) ? string.Empty : value; }
    }
    public bool ShowParent
    {
        get { return this.showParent; }
        set { this.showParent = value; }
    }

    //Ispišemo djecu od ParentTerma prema hijerarhiji COUNTRY->REGION->SUBREGION->ISLAND->CITY na određenom jeziku
    //Ako ParentTerm nije postavljen ili je prazan onda kontrola ispiše popis država
    //Ako Language nije postavljen onda se ispisuje na defaultnom DEF_LANG jeziku
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(language))
        {
            var culture = CultureInfo.CurrentCulture;
            language = culture.TwoLetterISOLanguageName;
        }

        if (string.IsNullOrEmpty(queryStringVar)) queryStringVar = DEF_QSVAR;
        if (string.IsNullOrEmpty(separator)) separator = DEF_SEPAR;

        NameValueCollection qs = HttpUtility.ParseQueryString(Request.QueryString.ToString());
        qs.Remove(queryStringVar);
        if (qs.HasKeys())
        {
            queryString = qs.ToString();
            queryString += "&";
        }
        else
        {
            queryString = string.Empty;
        }


        if (!string.IsNullOrEmpty(parentTerm))
        {
            pnlParent.Visible = showParent;
            GetChildren();
        }
        else
        {
            pnlParent.Visible = false;
            GetCountrys();
        }
    }


    protected void GetChildren()
    {
        using (UltimateDataContext dc = new UltimateDataContext())
        {
            var children = from p in dc.UltimateTableRelations
                           where p.Active == true && p.Child.Active == true &&
                                p.Parent.Title.ToLower() == parentTerm && p.Child.UltimateTableObjectType.GroupCode == "GEO"
                           select p.Child;

            var parents = from p in dc.UltimateTableRelations
                          where p.Active == true && p.Parent.Active == true &&
                                p.Child.Title.ToLower() == parentTerm
                          select p.Parent;

            var brothers = from p in dc.UltimateTableRelations
                           where p.Active == true && p.Child.Active == true &&
                                parents.Contains(p.Parent) && p.Child.Title.ToLower() != parentTerm && p.Child.UltimateTableObjectType.GroupCode == "GEO"
                           select p.Child;

            //Ako parent nema djece onda ispisuje braću
            var query = children.Any() ? children : brothers;

            //LEFT OUTER JOIN pa ako nema prijevoda onda prikazujemo Title iz UltimateTable
            var translation = from p in query
                              join q in dc.UltimateTableTranslations on p equals q.UltimateTable into trans
                              from r in trans.Where(i => i.Language.Abbrevation == language && i.Active == true).DefaultIfEmpty()
                              select new
                              {
                                  UltimateId = p.Id,
                                  Title = r != null ? r.Title : p.Title
                              };

            if (translation.Any())
            {
                var trans = translation.Take(1).SingleOrDefault();
                var parentnodes = (from p in dc.GetParents(null, trans.UltimateId, Common.GetLanguageId())
                                   select p.ParentTitle).ToArray();
                //string parentlink = parentnodes.Any() ? string.Join(" ", parentnodes) + " " : string.Empty;

                aParent.HRef = string.Format("{0}?{1}{2}={3}", landingPage, queryString, queryStringVar, HttpUtility.UrlEncode(parentnodes.Last()));
                aParent.InnerText = parentnodes.Last();

                var data = from p in translation
                           select new
                           {
                               Href = String.Format("{0}?{1}{2}={3}", landingPage, queryString, queryStringVar, HttpUtility.UrlEncode(p.Title)),
                               Title = p.Title
                           };

                repDirectory.DataSource = data;
                repDirectory.DataBind();
            }


        }
    }


    protected void GetCountrys()
    {
        using (UltimateDataContext dc = new UltimateDataContext())
        {
            var children = from p in dc.UltimateTables
                           where p.Active == true && (ObjectType)p.ObjectTypeId == ObjectType.COUNTRY
                           select p;

            //LEFT OUTER JOIN pa ako nema prijevoda onda prikazujemo Title iz UltimateTable
            var translation = from p in children
                              join q in dc.UltimateTableTranslations on p equals q.UltimateTable into trans
                              from r in trans.Where(i => i.Language.Abbrevation == language).DefaultIfEmpty()
                              select new
                              {
                                  UltimateId = p.Id,
                                  Title = r != null ? r.Title : p.Title
                              };

            if (translation.Any())
            {
                var trans = translation.Take(1).SingleOrDefault();
                var parentnodes = (from p in dc.GetParents(null, trans.UltimateId, Common.GetLanguageId())
                                   select p.ParentTitle).ToArray();
                //string parentlink = parentnodes.Any() ? string.Join(" ", parentnodes) + " " : string.Empty;



                var data = from p in translation
                           select new
                           {
                               Href = String.Format("{0}?{1}{2}={3}", landingPage, queryString, queryStringVar, HttpUtility.UrlEncode(p.Title)),
                               Title = p.Title
                           };

                repDirectory.DataSource = data;
                repDirectory.DataBind();
            }
        }
    }

}
