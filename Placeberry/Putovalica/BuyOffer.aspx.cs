using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class BuyOffer : UserPageBase
{
    protected Collective.Offer _currOffer;
    protected int _userBoughtCount;

    protected UserPageBase _pageBase;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        //this.lbCardBuy.Click += new EventHandler(lbCardBuy_Click);
    }

    void lbCardBuy_Click(object sender, EventArgs e)
    {
        Collective.User user = _pageBase.GetLoggedCollectiveUser();
        if (user == null || _currOffer == null)
        {
            return;
        }

        int couponsToBuyCnt;
        Int32.TryParse(this.ddlSelCount.SelectedValue, out couponsToBuyCnt);

        for (int i = 0; i < couponsToBuyCnt; ++i)
        {
            Collective.Coupon.CreateCoupon(user.Id, _currOffer.OfferId, "000000");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        _pageBase = (UserPageBase)Page;

        _currOffer = _pageBase.GetCurrentOffer();

        if (_currOffer == null)
            Response.Redirect(Page.ResolveUrl("~/Default.aspx"));

        if (!IsPostBack)
        {
            this.ddlSelCount.Attributes["onChange"] = "changeCount(this)";
            this.ddlSelCount.Attributes["basicPrice"] = _currOffer.Price.ToString().Replace(',', '.');

            int couponsPerUser = _currOffer.NumberOfCouponsPerUser.HasValue ? _currOffer.NumberOfCouponsPerUser.Value : 1;
            for (int i = 0; i < couponsPerUser; ++i)
            {
                this.ddlSelCount.Items.Add(new ListItem((i + 1).ToString(), (i + 1).ToString()));
            }

            if (_pageBase.GetLoggedCollectiveUser() != null)
            {
                _userBoughtCount = _currOffer.GetUserBoughtCount(_pageBase.GetLoggedCollectiveUser().Id);

                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("\n");
                sbScript.Append("var _userBoughtCount=" + _userBoughtCount.ToString() + ";\n");
                sbScript.Append("var _maxBoughtCount=" + (_currOffer.NumberOfCouponsPerUser.HasValue ? _currOffer.NumberOfCouponsPerUser.Value : 1).ToString() + ";\n");
                sbScript.Append("\n");

                Page.ClientScript.RegisterStartupScript(this.GetType(), "startupScript", sbScript.ToString(), true);

                phLoginRegister.Visible = false;
                phBuyForm.Visible = true;
            }
            else
            {
                phLoginRegister.Visible = true;
                phBuyForm.Visible = false;
            }
        }
    }
}