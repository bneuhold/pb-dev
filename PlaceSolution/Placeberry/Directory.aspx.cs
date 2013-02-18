using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using UltimateDC;
using System.Collections;
using System.Web.UI.HtmlControls;


public partial class Directory : System.Web.UI.Page
{

    public class UltimateTerm
    {
        private int id = 0;
        private string title = string.Empty;
        private string urlTag = string.Empty;
        private string urlLink = string.Empty;

        public int Id { get { return this.id; } set { this.id = value; } }
        public string Title { get { return this.title; } set { this.title = value; } }
        public string UrlTag { get { return this.urlTag; } set { this.urlTag = value; } }
        public string UrlLink { get { return this.urlLink; } set { this.urlLink = value; } }
    }

    private List<UltimateTerm> breadcrumbs = new List<UltimateTerm>();
    private List<UltimateTerm> childList = new List<UltimateTerm>();

    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);

        this.Title = Resources.placeberry.Directory_TitleBasic;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int languageId = Common.GetLanguageId();
        string termTag = Request.QueryString["tag"] ?? string.Empty;
        termTag = termTag.Split('/').LastOrDefault();

        Repeater topAdverts = TopAdverts.Adverts;

        if (!string.IsNullOrEmpty(termTag))
        {
            using (UltimateDataContext dc = new UltimateDataContext())
            {

                var query = (from p in dc.UltimateTableUrlTags
                             where p.UrlTag == termTag && p.LanguageId == languageId
                             select new
                             {
                                 Id = p.UltimateTable.Id,
                                 Title = p.UltimateTable.Title,
                                 UrlTag = p.UrlTag
                             }).Distinct();

                if (query.Any())
                {
                    var term = query.First();

                    string termTitle = (from p in dc.UltimateTableTranslations
                                        where p.UltimateTableId == term.Id && p.Active == true && p.LanguageId == languageId
                                        select p.Title).SingleOrDefault() ?? term.Title;

                    ltlCurrentTerm.Text = termTitle;

                    this.Title = String.Format(Resources.placeberry.Directory_TitleFormat, termTitle);


                    string termDesc = (from p in dc.UltimateTableInfos
                                       where p.UltimateTableId == term.Id && p.LanguageId == languageId
                                       select p.WikiDescription).SingleOrDefault() ?? string.Empty;

                    if (!string.IsNullOrEmpty(termDesc))
                    {
                        termDesc = termDesc.Replace("\n", "<br />");
                        ltlDescription.Text = termDesc;
                        pnlDescription.Visible = true;
                    }
                    else
                    {
                        pnlDescription.Visible = false;
                    }


                    //BREADCRUMBS
                    //dodajemo root
                    breadcrumbs.Add(new UltimateTerm() { Id = 0, Title = "dir", UrlLink = Resources.placeberry.URL_Directory, UrlTag = Resources.placeberry.URL_Directory });

                    //dodajemo ostale elemente
                    breadcrumbs.AddRange(from p in dc.GetParents(null, term.Id, languageId)
                                         where p.ObjectTypeGroupCode != null && p.ObjectTypeGroupCode == "GEO"
                                         select new UltimateTerm()
                                         {
                                             Id = p.ParentId,
                                             Title = p.ParentTitle,
                                             UrlTag = p.UrlTag,
                                             UrlLink = string.Empty
                                         });

                    //gradimo linkove
                    for (int i = 1; i < breadcrumbs.Count; i++)
                    {
                        breadcrumbs[i].UrlLink = breadcrumbs[i - 1].UrlLink + "/" + breadcrumbs[i].UrlTag;
                    }

                    //dodajemo trenutni pojam zato što GetParents vrati samo roditelje, a ne i trenutni
                    var currTerm = new UltimateTerm() { Id = term.Id, Title = termTitle, UrlTag = term.UrlTag };
                    var previousTerm = breadcrumbs.Last();
                    breadcrumbs.Add(currTerm);
                    currTerm.UrlLink = previousTerm.UrlLink + "/" + currTerm.UrlTag;

                    repBreadcrumbs.DataSource = breadcrumbs;
                    repBreadcrumbs.DataBind();


                    //CHILDREN
                    var children = from p in dc.UltimateTableRelations
                                   where p.ParentId == term.Id && p.Child.UltimateTableObjectType.GroupCode == "GEO"
                                   select p.Child;

                    var childrenTranslated = from p in children
                                             join q in dc.UltimateTableTranslations on p equals q.UltimateTable into trans
                                             join t in dc.UltimateTableUrlTags on p equals t.UltimateTable into tags
                                             from r in trans.Where(i => i.LanguageId == languageId && i.Active == true).DefaultIfEmpty()
                                             from s in tags.Where(i => i.LanguageId == languageId).DefaultIfEmpty()
                                             select new
                                             {
                                                 Id = p.Id,
                                                 Title = r != null ? r.Title : p.Title,
                                                 UrlLink = String.Format("{0}/{1}", currTerm.UrlLink, s.UrlTag)
                                             };

                    if (childrenTranslated.Any())
                    {
                        repChildren.DataSource = childrenTranslated;
                        repChildren.DataBind();
                    }
                    else
                    {
                        repChildren.Visible = false;
                    }


                    string queryMessage = string.Empty;
                    topAdverts.DataSource = Placeberry.DAL.GetResults.Execute(termTitle, languageId, 5, 0, 5, 0, out queryMessage);
                    topAdverts.DataBind();

                    aCurrentTerm.HRef = String.Format("/{0}?q={1}", Resources.placeberry.General_VacationUrl, termTitle);
                    aCurrentTerm.InnerText = String.Format(Resources.placeberry.Directory_SearchCurrentTerm, termTitle);
                    aCurrentTerm.Visible = true;


                    string keywords = string.Join(", ", breadcrumbs.Skip(1).Select(i => i.Title).ToArray());

                    HtmlMeta metaKeywords = new HtmlMeta() { Name = "keywords", Content = string.Empty };
                    HtmlMeta metaDescription = new HtmlMeta() { Name = "description", Content = string.Empty };

                    metaDescription.Content = String.Format(Resources.placeberry.Directory_MetaDescription, termTitle);
                    metaKeywords.Content = String.Format(Resources.placeberry.Directory_MetaKeywords, keywords);

                    Header.Controls.Add(metaDescription);
                    Header.Controls.Add(metaKeywords);
                }
                else
                {
                    //ERROR ne postoji taj pojam u direktoriju
                }

            }
        }
        else
        {
            using (UltimateDataContext dc = new UltimateDataContext())
            {
                var countries = from p in dc.UltimateTables
                                where p.Active == true && p.UltimateTableObjectType.Code == "COUNTRY"
                                select p;

                var countriesTranslated = from p in countries
                                          join q in dc.UltimateTableTranslations on p equals q.UltimateTable into trans
                                          join t in dc.UltimateTableUrlTags on p equals t.UltimateTable into tags
                                          from r in trans.Where(i => i.LanguageId == languageId && i.Active == true).DefaultIfEmpty()
                                          from s in tags.Where(i => i.LanguageId == languageId).DefaultIfEmpty()
                                          select new
                                          {
                                              Id = p.Id,
                                              Title = r != null ? r.Title : p.Title,
                                              UrlLink = String.Format("{0}/{1}", Resources.placeberry.URL_Directory, s.UrlTag)
                                          };

                if (countriesTranslated.Any())
                {

                    repChildren.DataSource = countriesTranslated;
                    repChildren.DataBind();
                }

                ltlTopAdsText.Visible = false;
                topAdverts.Visible = false;

                HtmlMeta metaKeywords = new HtmlMeta() { Name = "keywords", Content = string.Empty };
                HtmlMeta metaDescription = new HtmlMeta() { Name = "description", Content = string.Empty };

                metaDescription.Content = String.Format(Resources.placeberry.Directory_MetaDescription, "smještaj");
                metaKeywords.Content = String.Format(Resources.placeberry.Directory_MetaKeywords, "smještaj");

                Header.Controls.Add(metaDescription);
                Header.Controls.Add(metaKeywords);

            }
        }



    }
}