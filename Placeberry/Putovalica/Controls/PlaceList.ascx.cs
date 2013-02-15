using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Controls_PlaceList : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // ova se kontrola prikazuje u headeru samo na deafault.aspx
            if (!Request.Url.LocalPath.ToLower().StartsWith("/default.aspx"))
            {
                this.Visible = false;
                return;
            }

            UserPageBase pageBase = (UserPageBase)Page;

            // dodati categoryid argument ako ga nema da uvijek bude ispred placeid-a
            //string navuri = Request.QueryString["categoryid"] == null ? PutovalicaUtil.QSAddArgWithValue(Request.Url.AbsoluteUri, "categoryid", pageBase.ListCategories().FirstOrDefault().Id.ToString()) : Request.Url.AbsoluteUri;

            // po redosljedu prvo mora ici kategorija pa onda place
            Collective.Category selCat = pageBase.GetSelectedCategory();
            if (selCat == null)
            {
                selCat = pageBase.ListCategories().FirstOrDefault();
            }

            string navuri = "/" + selCat.Translation.UrlTag;
            
            List<Collective.Place> lstPlaces = pageBase.ListPlaces();

            Collective.Place selectedPlace = pageBase.GetSelectedPlace();

            if (selectedPlace != null)
            {
                HtmlGenericControl liSelected = new HtmlGenericControl("li");
                HyperLink hlSelected = new HyperLink()
                {                    
                    //NavigateUrl = Page.ResolveUrl(PutovalicaUtil.QSAddArgWithValue(navuri, "placeid", selectedPlace.Id.ToString())),
                    NavigateUrl = navuri + "/" + selectedPlace.UrlTag,
                    Text = selectedPlace.Title
                };

                liSelected.Controls.Add(hlSelected);
                ulCities.Controls.Add(liSelected);

                HtmlGenericControl liAllPlaces = new HtmlGenericControl("li");
                HyperLink hlAllPlaces = new HyperLink()
                {
                    //NavigateUrl = Page.ResolveUrl(PutovalicaUtil.QSRemoveArg(navuri, "placeid")),
                    NavigateUrl = navuri,
                    Text = "Sva mjesta"
                };

                liAllPlaces.Controls.Add(hlAllPlaces);
                ulCities.Controls.Add(liAllPlaces);

                foreach (Collective.Place pl in lstPlaces)
                {
                    if (pl.Id == selectedPlace.Id)
                        continue;

                    HtmlGenericControl li = new HtmlGenericControl("li");
                    HyperLink link = new HyperLink()
                    {
                        //NavigateUrl = Page.ResolveUrl(PutovalicaUtil.QSAddArgWithValue(navuri, "placeid", pl.Id.ToString())),
                        NavigateUrl = navuri + "/" + pl.UrlTag,
                        Text = pl.Title
                    };

                    li.Controls.Add(link);
                    ulCities.Controls.Add(li);
                }
            }
            else
            {
                HtmlGenericControl liSelected = new HtmlGenericControl("li") { InnerText = string.Empty };
                HyperLink hlSelected = new HyperLink()
                {
                    //NavigateUrl = Page.ResolveUrl(PutovalicaUtil.QSRemoveArg(navuri, "placeid")),
                    NavigateUrl = navuri,
                    Text = "Sva mjesta"
                };

                liSelected.Controls.Add(hlSelected);
                //liSelected.InnerHtml = "<a href='" + navuri + "'>Sva mjesta</a>";
                ulCities.Controls.Add(liSelected);

                foreach (Collective.Place pl in lstPlaces)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    HyperLink link = new HyperLink()
                    {
                        //NavigateUrl = Page.ResolveUrl(PutovalicaUtil.QSAddArgWithValue(navuri, "placeid", pl.Id.ToString())),
                        NavigateUrl = navuri + "/" + pl.UrlTag,
                        Text = pl.Title
                    };

                    li.Controls.Add(link);
                    ulCities.Controls.Add(li);
                }
            }
        }
    }
}