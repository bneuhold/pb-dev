using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CouponView : UserPageBase
{
    protected Collective.Coupon _coup;
    protected Collective.Offer _offer;
    protected Collective.Agency _agency;

    protected void Page_Load(object sender, EventArgs e)
    {
        UserPageBase pageBase = (UserPageBase)Page;
        Collective.User user = pageBase.GetLoggedCollectiveUser();

        if(user == null)
            Response.Redirect(Page.ResolveUrl("~/default.aspx"));

        int coupId;
        if(Int32.TryParse(Request.QueryString["coupid"], out coupId))
        {
            _coup = Collective.Coupon.GetCouponForClient(coupId, PutovalicaUtil.GetLanguageId());

            if(user.Id != _coup.UserId)
                Response.Redirect(Page.ResolveUrl("~/default.aspx"));

            _offer = Collective.Offer.GetOffer(_coup.CollectiveOfferId, PutovalicaUtil.GetLanguageId());

            if (_offer.AgencyId.HasValue)
            {
                _agency = Collective.Agency.GetAgency(_offer.AgencyId.Value);
            }

            phAgency.Visible = _agency != null;
        }
    }
}