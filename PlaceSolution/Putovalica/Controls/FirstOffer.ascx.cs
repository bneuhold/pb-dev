using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_FirstOffer : System.Web.UI.UserControl
{
    protected Collective.Offer _firstOffer = null;
    protected bool isOfferSuccess;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserPageBase pageBase = (UserPageBase)Page;
            List<Collective.Offer> lstOffers = pageBase.ListOffers();

            if (lstOffers.Count == 0)
            {
                this.Visible = false;
                return;
            }

            _firstOffer = lstOffers.FirstOrDefault();

            this.imgOffer.ImageUrl = !String.IsNullOrEmpty(_firstOffer.FirstImgSrc) ? _firstOffer.FirstImgSrc : "/uploads/offerimages/default.jpg";
            imgOffer.AlternateText = _firstOffer.Translation.Title;

            isOfferSuccess = _firstOffer.BoughtCount >= _firstOffer.MinBoughtCount;
        }
        else
        {
            this.Visible = false;
            return;
        }
    }
}