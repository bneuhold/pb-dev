using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI.HtmlControls;


public partial class admin_OfferList : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.grdOffers.RowDataBound += new GridViewRowEventHandler(grdOffers_RowDataBound);
        this.grdOffers.RowDeleting += new GridViewDeleteEventHandler(grdOffers_RowDeleting);
    }

    void grdOffers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Collective.Offer offer = e.Row.DataItem as Collective.Offer;

            Label lblDateStart = e.Row.FindControl("lblDateStart") as Label;
            if (lblDateStart != null)
            {
                lblDateStart.Text = offer.DateStart.ToString("dd.MM.yyyy");
            }

            Label lblDateEnd = e.Row.FindControl("lblDateEnd") as Label;
            if (lblDateEnd != null)
            {
                lblDateEnd.Text = offer.DateEnd.ToString("dd.MM.yyyy");
            }
            Label lblMinMaxBoughtCount = e.Row.FindControl("lblMinMaxBoughtCount") as Label;
            if (lblMinMaxBoughtCount != null)
            {
                lblMinMaxBoughtCount.Text = offer.MinBoughtCount.ToString() + "/" + offer.MaxBoughtCount.ToString();
            }

            HyperLink hlEdit = e.Row.FindControl("hlEdit") as HyperLink;
            if (hlEdit != null)
            {
                hlEdit.NavigateUrl = Page.ResolveUrl("~/admin/OfferCreateEdit.aspx?offerid=" + offer.OfferId.ToString());
            }

            LinkButton lbDelete = e.Row.FindControl("lbDelete") as LinkButton;
            if (lbDelete != null && offer != null)
            {
                lbDelete.Attributes.Add("onclick", "javascript:return confirm('Dali ste sigurni da želite obrisati ponudu: " + offer.OfferName + "?');");
            }

            HyperLink hlEditTrans = e.Row.FindControl("hlEditTrans") as HyperLink;
            if (hlEditTrans != null)
            {
                hlEditTrans.NavigateUrl = Page.ResolveUrl("~/admin/OfferTranslations.aspx?offerid=" + offer.OfferId.ToString());
            }

            HyperLink hlImages = e.Row.FindControl("hlImages") as HyperLink;
            if (hlImages != null)
            {
                hlImages.NavigateUrl = Page.ResolveUrl("~/admin/OfferImages.aspx?offerid=" + offer.OfferId.ToString());
            }

            CheckBox cbActive = e.Row.FindControl("cbActive") as CheckBox;
            if (cbActive != null)
            {
                cbActive.Checked = offer.Active;
                cbActive.Attributes.Add("offerId", offer.OfferId.ToString());
            }
        }
    }

    protected void cbActive_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = sender as CheckBox;
        int offerid = Int32.Parse(cb.Attributes["offerId"]);
        Collective.Offer.ToggleActive(offerid);
    }


    void grdOffers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey dKey = grdOffers.DataKeys[e.RowIndex];
        int id = Convert.ToInt32(dKey.Values["OfferId"]);

        Collective.Offer.DeleteOffer(id);

        FillOffersGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hlBack.NavigateUrl = "http://" + Request.Url.Authority + "/admin/Default.aspx";
            this.hlCreateNewOffer.NavigateUrl = "http://" + Request.Url.Authority + "/admin/OfferCreateEdit.aspx";

            // workaroud za WebMethods zbog UrlRewritera
            // OVO IONAK NE RADI!!!
            string script = "var pageUrl='" + Request.Url.AbsolutePath + "';";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenGroupsAndCbIds", script, true);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillOffersGrid();
        }
    }

    private void FillOffersGrid()
    {
        Collective.Offer.SortType sortby = Collective.Offer.SortType.OfferId;
        bool asc = true;
        int page = 1;
        int rows = 15;
        if (!String.IsNullOrEmpty(Request.QueryString["sortby"]))
        {
            try
            {
                sortby = (Collective.Offer.SortType)Enum.Parse(typeof(Collective.Offer.SortType), Request.QueryString["sortby"], true);
            }
            catch
            {
                sortby = Collective.Offer.SortType.OfferId;
            }
        }
        if (!String.IsNullOrEmpty(Request.QueryString["asc"]))
        {
            Boolean.TryParse(Request.QueryString["asc"], out asc);
        }
        if (!String.IsNullOrEmpty(Request.QueryString["page"]))
        {
            Int32.TryParse(Request.QueryString["page"], out page);
        }
        if (!String.IsNullOrEmpty(Request.QueryString["rows"]))
        {
            Int32.TryParse(Request.QueryString["rows"], out rows);
        }

        int totalPageCount;
        List<Collective.Offer> lst = Collective.Offer.PagOffersForAdmin(sortby, asc, page, rows, null, out totalPageCount, out page);    // postavit ce page na zadnji ako je veci od maksimalnog broja stranica

        grdOffers.DataSource = lst;
        grdOffers.DataBind();

        CreateNavigationLinks(sortby, asc, page, rows, totalPageCount);
    }

    private void CreateNavigationLinks(Collective.Offer.SortType sortBy, bool isAsc, int currPage, int numOfRows, int totalPageCount)
    {
        string basicUrl = Request.Url.ToString();
        basicUrl = basicUrl.Substring(0, basicUrl.IndexOf(".aspx") + ".aspx".Count());

        // sort linkovi
        HyperLink hlOfferNameSort = (HyperLink)this.grdOffers.HeaderRow.FindControl("hlOfferNameSort");
        if (sortBy == Collective.Offer.SortType.OfferName)
        {
            hlOfferNameSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.OfferName.ToString() + "&asc=" + (!isAsc).ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlOfferNameSort.Text = isAsc ? "Offer Name ↑" : "Offer Name ↓";
        }
        else
        {
            hlOfferNameSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.OfferName.ToString() + "&asc=" + true.ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlOfferNameSort.Text = "Offer Name ↑";
        }

        HyperLink hlOfferIdSort = (HyperLink)this.grdOffers.HeaderRow.FindControl("hlOfferIdSort");
        if (sortBy == Collective.Offer.SortType.OfferId)
        {
            hlOfferIdSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.OfferId.ToString() + "&asc=" + (!isAsc).ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlOfferIdSort.Text = isAsc ? "ID ↑" : "ID ↓";
        }
        else
        {
            hlOfferIdSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.OfferId.ToString() + "&asc=" + true.ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlOfferIdSort.Text = "ID ↑";
        }

        HyperLink hlCategoryNameSort = (HyperLink)this.grdOffers.HeaderRow.FindControl("hlCategoryNameSort");
        if (sortBy == Collective.Offer.SortType.CategotyName)
        {
            hlCategoryNameSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.CategotyName.ToString() + "&asc=" + (!isAsc).ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlCategoryNameSort.Text = isAsc ? "Category Name ↑" : "Category Name ↓";
        }
        else
        {
            hlCategoryNameSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.CategotyName.ToString() + "&asc=" + true.ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlCategoryNameSort.Text = "Category Name ↑";
        }

        HyperLink hlBoughtCountSort = (HyperLink)this.grdOffers.HeaderRow.FindControl("hlBoughtCountSort");
        if (sortBy == Collective.Offer.SortType.BoughtCount)
        {
            hlBoughtCountSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.BoughtCount.ToString() + "&asc=" + (!isAsc).ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlBoughtCountSort.Text = isAsc ? "Bought Count ↑" : "Bought Count ↓";
        }
        else
        {
            hlBoughtCountSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.BoughtCount.ToString() + "&asc=" + true.ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlBoughtCountSort.Text = "Bought Count ↑";
        }

        HyperLink hlDateStartSort = (HyperLink)this.grdOffers.HeaderRow.FindControl("hlDateStartSort");
        if (sortBy == Collective.Offer.SortType.DateStart)
        {
            hlDateStartSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.DateStart.ToString() + "&asc=" + (!isAsc).ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlDateStartSort.Text = isAsc ? "Date Start ↑" : "Date Start ↓";
        }
        else
        {
            hlDateStartSort.NavigateUrl = basicUrl + "?sortby=" + Collective.Offer.SortType.DateStart.ToString() + "&asc=" + true.ToString() + "&page=" + currPage.ToString() + "&rows=" + numOfRows.ToString();
            hlDateStartSort.Text = "Date Start ↑";
        }



        // paging linovi
        this.hlFirst.NavigateUrl = basicUrl + "?sortby=" + sortBy.ToString() + "&asc=" + isAsc.ToString() + "&page=" + 1.ToString() + "&rows=" + numOfRows.ToString();
        this.hlLast.NavigateUrl = basicUrl + "?sortby=" + sortBy.ToString() + "&asc=" + isAsc.ToString() + "&page=" + totalPageCount.ToString() + "&rows=" + numOfRows.ToString();
        this.hlPrev.NavigateUrl = basicUrl + "?sortby=" + sortBy.ToString() + "&asc=" + isAsc.ToString() + "&page=" + (currPage - 1).ToString() + "&rows=" + numOfRows.ToString();
        this.hlNext.NavigateUrl = basicUrl + "?sortby=" + sortBy.ToString() + "&asc=" + isAsc.ToString() + "&page=" + (currPage + 1).ToString() + "&rows=" + numOfRows.ToString();
        this.hlPrev.Visible = this.hlFirst.Visible = currPage > 1;
        this.hlNext.Visible = this.hlLast.Visible = currPage < totalPageCount;

        SortedList<int, int> pags = new SortedList<int, int>();
        pags.Add(currPage, currPage);
        int pl = currPage;
        int pr = currPage;

        for (int i = 0; i < 3; ++i)
        {
            if (--pl >= 1)
            {
                pags.Add(pl, pl);
            }
            else if (++pr <= totalPageCount)
            {
                pags.Add(pr, pr);
            }

            if (++pr <= totalPageCount)
            {
                pags.Add(pr, pr);
            }
            else if (--pl >= 1)
            {
                pags.Add(pl, pl);
            }
        }

        if (pags.First().Value > 1)
        {
            phPages.Controls.Add(new Literal() { Text = "... " });
        }
        foreach (KeyValuePair<int, int> kwp in pags)
        {
            if (kwp.Value == currPage)
            {
                phPages.Controls.Add(new Literal() { Text = kwp.Value.ToString() });
            }
            else
            {
                phPages.Controls.Add(new HyperLink()
                {
                    Text = kwp.Value.ToString(),
                    NavigateUrl = basicUrl + "?sortby=" + sortBy.ToString() + "&asc=" + isAsc.ToString() + "&page=" + kwp.Value.ToString() + "&rows=" + numOfRows.ToString()
                });
            }
            phPages.Controls.Add(new Literal() { Text = " " });
        }
        if (pags.Last().Value < totalPageCount)
        {
            phPages.Controls.Add(new Literal() { Text = " ..." });
        }
    }
}