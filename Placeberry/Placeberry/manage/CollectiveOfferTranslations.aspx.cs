using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class manage_CollectiveOfferTranslations : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.grdOfferTrans.RowDataBound += new GridViewRowEventHandler(grdOfferTrans_RowDataBound);
        this.grdOfferTrans.RowDeleting += new GridViewDeleteEventHandler(grdOfferTrans_RowDeleting);
    }

    void grdOfferTrans_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        List<Collective.Language> allLangs = GetLanguages();

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Collective.OfferTranslation offtrans = e.Row.DataItem as Collective.OfferTranslation;
            if (offtrans.OfferId == 0)  // ako nema prijevoda pa je samo onaj dummy
                return;

            Collective.Language currLang = (from l in allLangs
                                            where l.Id == offtrans.LanguageId
                                            select l).FirstOrDefault();

            Label lblLang = e.Row.FindControl("lblLang") as Label;
            if (lblLang != null)
            {
                lblLang.Text = currLang.Title;
            }

            HyperLink hlEdit = e.Row.FindControl("hlEdit") as HyperLink;
            hlEdit.NavigateUrl = Page.ResolveUrl("~/manage/CollectiveOfferTranslationCreateEdit.aspx?offerid=" + offtrans.OfferId.ToString() + "&langid=" + offtrans.LanguageId.ToString());

            LinkButton lbDelete = e.Row.FindControl("lbDelete") as LinkButton;
            if (lbDelete != null && currLang != null)
            {
                lbDelete.Attributes.Add("onclick", "javascript:return confirm('Dali ste sigurni da želite obrisati " + currLang.Title + " prijevod?');");
            }
        }
    }


    void grdOfferTrans_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey dKey = grdOfferTrans.DataKeys[e.RowIndex];
        int offerId = Convert.ToInt32(dKey.Values["OfferId"]);
        int langId = Convert.ToInt32(dKey.Values["LanguageId"]);

        Collective.OfferTranslation.DeleteOfferTranslaton(offerId, langId);

        FillGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if(GetOffer() == null)
            {
                this.lblErrMsg.Text = "Pogrešni parametri.";
                return;
            }

            this.lblOfferName.Text = GetOffer().OfferName;

            FillGrid();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    private void FillGrid()
    {
        int offerId = Int32.Parse(Request.QueryString["offerid"]);

        List<Collective.OfferTranslation> offTrans = Collective.OfferTranslation.ListOfferTranslations(offerId);

        if (offTrans.Count > 0)
        {
            grdOfferTrans.DataSource = offTrans;
            grdOfferTrans.DataBind();
        }
        else
        {
            List<Collective.OfferTranslation> lstDummy = new List<Collective.OfferTranslation>();
            lstDummy.Add(new Collective.OfferTranslation());    // add dummy item

            grdOfferTrans.DataSource = lstDummy;
            grdOfferTrans.DataBind();

            int totalColumns = grdOfferTrans.Rows[0].Cells.Count;
            grdOfferTrans.Rows[0].Cells.Clear();
            grdOfferTrans.Rows[0].Cells.Add(new TableCell());
            grdOfferTrans.Rows[0].Cells[0].ColumnSpan = totalColumns;
            grdOfferTrans.Rows[0].Cells[0].Text = "Nema dodjeljenih prijevoda.";
            grdOfferTrans.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            grdOfferTrans.Rows[0].Cells[0].CssClass = "grid_item";
            grdOfferTrans.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Gray;
        }
    }

    private Collective.Offer _offer;

    protected Collective.Offer GetOffer()
    {
        if (_offer == null)
        {
            int offerId;
            if (Int32.TryParse(Request.QueryString["offerid"], out offerId))
            {
                _offer = Collective.Offer.GetOffer(offerId, null);
            }
        }

        return _offer;
    }

    private List<Collective.Language> _lstLang;

    private List<Collective.Language> GetLanguages()
    {
        if (_lstLang == null)
        {
            _lstLang = Collective.Language.ListLanguages(true);
        }

        return _lstLang;
    }
}