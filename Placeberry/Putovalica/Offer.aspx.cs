using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Offer : UserPageBase
{
    protected Collective.Offer currOffer;
    protected Collective.Agency offerAgency;
    protected bool isOfferSuccess;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserPageBase pageBase = (UserPageBase)Page;
            currOffer = pageBase.GetCurrentOffer();

            if (currOffer == null)
                Response.Redirect(Page.ResolveUrl("~/Default.aspx"));

            this.phGMapsIframe.Visible = currOffer.Longitude.HasValue && currOffer.Latitude.HasValue;

            this.phAgencyInfo.Visible = false;

            if (currOffer.AgencyId.HasValue)
            {
                this.offerAgency = Collective.Agency.GetAgency(currOffer.AgencyId.Value);
                if (this.offerAgency != null)
                {
                    this.phAgencyInfo.Visible = true;
                    string contactLine = !String.IsNullOrEmpty(this.offerAgency.ContactPhone) ? this.offerAgency.ContactPhone : string.Empty;
                    if (!String.IsNullOrEmpty(offerAgency.ContactEmail))
                    {
                        contactLine += !String.IsNullOrEmpty(contactLine) ? ", " : string.Empty;
                        contactLine += offerAgency.ContactEmail;
                    }

                    this.ltContact.Text = contactLine;
                }
            }

            isOfferSuccess = currOffer.BoughtCount >= currOffer.MinBoughtCount;
        }
        else
        {
            this.Visible = false;
            return;
        }
    }
}