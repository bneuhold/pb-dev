using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class WSpay_Success : UserPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();

        foreach (string key in Request.QueryString.AllKeys)
        {
            sb.Append(key + "=" + Request.QueryString[key]);
            sb.Append("<br />");
        }

        this.ltQS.Text = sb.ToString();

        if (HttpContext.Current.Session["BoughtOfferId"] == null || HttpContext.Current.Session["BoughtCouponCount"] == null || HttpContext.Current.Session["BoughtShopingCartID"] == null)
        {
            this.ltCouponSaveMsg.Text = "Nedostaju vrijednosti u sessionu.";
            return; 
        }

        int offerId = Convert.ToInt32(HttpContext.Current.Session["BoughtOfferId"]);
        int couponCount = Convert.ToInt32(HttpContext.Current.Session["BoughtCouponCount"]);
        string shopingCartID = HttpContext.Current.Session["BoughtShopingCartID"].ToString();

        HttpContext.Current.Session["BoughtOfferId"] = null;
        HttpContext.Current.Session["BoughtCouponCount"] = null;
        HttpContext.Current.Session["BoughtShopingCartID"] = null;

        if (!shopingCartID.Equals(Request.QueryString["ShoppingCartID"]))
        {
            this.ltCouponSaveMsg.Text = "ShoppingCartID ne odgovara!";
            return;
        }

        string ipgShopID = (string)(System.Configuration.ConfigurationManager.AppSettings.GetValues("ipgShopID")[0]);
        string ipgShopKey = (string)(System.Configuration.ConfigurationManager.AppSettings.GetValues("ipgShopKey")[0]);

        string signature = PutovalicaUtil.MD5HashString(ipgShopID +
            ipgShopKey +
            Request.QueryString["ShoppingCartID"] +
            ipgShopKey +
            Request.QueryString["Success"] +
            ipgShopKey +
            Request.QueryString["ApprovalCode"] +
            ipgShopKey);

        if (!signature.Equals(Request.QueryString["Signature"]))
        {
            this.ltCouponSaveMsg.Text = "Signature ne odgovara!";
            return;
        }

        UserPageBase pageBase = (UserPageBase)Page;

        Collective.User user = pageBase.GetLoggedCollectiveUser();
        Collective.Offer offer = Collective.Offer.GetOffer(offerId, PutovalicaUtil.GetLanguageId());

        if (user == null || offer == null)
        {
            this.ltCouponSaveMsg.Text = "Greška pri dohvatu korisnika ili ponude iz baze.";
            return;
        }

        // svaka uspjesna transakcija imati ce jedinstveni ShoppingCartID, al kuponi nastali pod tom transakcijom imati ce jednaki
        if (Collective.Coupon.CheckShopingCartID(shopingCartID))
        {
            this.ltCouponSaveMsg.Text = "ShoppingCartID vec postoji u bazi.";
            return;
        }

        this.ltCouponSaveMsg.Text = string.Empty;

        for (int i = 0; i < couponCount; ++i)
        {
            Collective.Coupon.CreateResultType result = Collective.Coupon.CreateCoupon(user.Id, offer.OfferId, shopingCartID);
            this.ltCouponSaveMsg.Text += result.ToString() + "<br />";
        }
    }
}