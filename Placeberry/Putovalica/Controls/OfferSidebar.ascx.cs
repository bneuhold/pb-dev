using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_OfferSidebar : System.Web.UI.UserControl
{
    protected int? _selectedPlaceId = null;
    UserPageBase _pageBase;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.rptOtherOffers.ItemDataBound += new RepeaterItemEventHandler(rptOtherOffers_ItemDataBound);
    }

    void rptOtherOffers_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Collective.Offer offer = e.Item.DataItem as Collective.Offer;
            if (offer.OfferId == _pageBase.GetCurrentOffer().OfferId)
            {
                e.Item.Visible = false;
                return;
            }

            Image imgOffer = e.Item.FindControl("imgOffer") as Image;
            imgOffer.ImageUrl = Page.ResolveUrl(String.IsNullOrEmpty(offer.FirstImgSrc) ? "~/uploads/offerimages/default.jpg" : "~" + offer.FirstImgSrc);
            imgOffer.AlternateText = offer.Translation.Title;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            _pageBase = (UserPageBase)Page;

            if (_pageBase.GetCurrentOffer() == null)
            {
                this.Visible = false;
                return;
            }

            List<Collective.Offer> lstLinkedOffers = Collective.Offer.ListOffersForClient(_pageBase.GetCurrentOffer().CollectiveCategoryId, null, PutovalicaUtil.GetLanguageId(), 3);

            this.rptOtherOffers.DataSource = lstLinkedOffers;
            this.rptOtherOffers.DataBind();
        }
    }
}