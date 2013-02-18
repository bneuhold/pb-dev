using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Controls_OfferList : System.Web.UI.UserControl
{
    private int firstOfferId = 0;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.rptOffers.ItemDataBound += new RepeaterItemEventHandler(rptOffers_ItemDataBound);
    }

    void rptOffers_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Collective.Offer offer = e.Item.DataItem as Collective.Offer;

            if (offer.OfferId == firstOfferId)
            {
                e.Item.Visible = false;
                return;
            }

            Image imgOffer = e.Item.FindControl("imgOffer") as Image;
            imgOffer.ImageUrl = !String.IsNullOrEmpty(offer.FirstImgSrc) ? offer.FirstImgSrc : "/uploads/offerimages/default.jpg";
            imgOffer.AlternateText = offer.Translation.Title;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserPageBase pageBase = (UserPageBase)Page;

            List<Collective.Offer> lstOffers = pageBase.ListOffers();
            if (lstOffers.Count > 0)
            {
                firstOfferId = lstOffers.FirstOrDefault().OfferId;
            }

            this.rptOffers.DataSource = lstOffers;
            this.rptOffers.DataBind();

        }
    }
}