using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OfferBuyEbankForm : UserPageBase
{
    protected Collective.Offer _currentOffer;

    protected void Page_Load(object sender, EventArgs e)
    {
        _currentOffer = ((UserPageBase)Page).GetCurrentOffer();
    }
}