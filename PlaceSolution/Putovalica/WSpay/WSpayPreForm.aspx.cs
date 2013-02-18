using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class WSpay_WSpayPreForm : UserPageBase
{
    protected string ipgShopID = "";
    protected string ipgShopKey = "";
    protected string ipgTotalAmount = "";
    protected string shopingCartID = "";
    protected string signature = "";
    protected string offer = "";

    protected string firstName = "";
    protected string lastName = "";
    protected string address = "";
    protected string city = "";
    protected string country = "";
    protected string zip = "";
    protected string phone = "";
    protected string email = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        UserPageBase pageBase = (UserPageBase)Page;

        Collective.Offer offer = pageBase.GetCurrentOffer();
        Collective.User user = pageBase.GetLoggedCollectiveUser();
        int count;

        if (offer == null || user == null || !Int32.TryParse(Request.QueryString["count"], out count))
        {
            Response.Redirect(pageBase.ResolveUrl("~/default.aspx"));
        }
        else if ((offer.GetUserBoughtCount(user.Id) + count) > (offer.NumberOfCouponsPerUser.HasValue ? offer.NumberOfCouponsPerUser.Value : 1))
        {
            Response.Redirect(pageBase.ResolveUrl("~/default.aspx"));
        }
        else
        {
            ipgShopID = (string)(System.Configuration.ConfigurationManager.AppSettings.GetValues("ipgShopID")[0]);
            ipgShopKey = (string)(System.Configuration.ConfigurationManager.AppSettings.GetValues("ipgShopKey")[0]);

            // GENERIRANJE ShoppingCardID-a

            // samo datum
            //shopingCartID = String.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);

            // datum sa random brojem da taman stane u bigint
            //Random r = new Random();
            //shopingCartID = String.Format("{0:yyMMddHHmmss}", DateTime.Now) + r.Next(100000, 999999).ToString();

            // guid
            shopingCartID = Guid.NewGuid().ToString();

            ipgTotalAmount = String.Format(CultureInfo.CreateSpecificCulture("hr-HR"), "{0:N2}", (offer.Price * count)).Replace(".", string.Empty);   // mora imati ,00 da bi radilo
            signature = PutovalicaUtil.MD5HashString(ipgShopID + ipgShopKey + shopingCartID + ipgShopKey + ipgTotalAmount.Replace(",", "") + ipgShopKey);

            // sa ovima sljaka
            //shopingCartID = "20121119184013030";
            //ipgTotalAmount = "123,00";
            //signature = "3a96d10f11e55c9508620826519938d0";

            firstName = user.FirstName;
            lastName = user.LastName;
            address = user.Street;
            city = user.City;
            zip = user.ZipCode;
            country = user.Country;
            email = user.Email;
            phone = user.Phone;

            // pospremiti potrebne podatke u session
            HttpContext.Current.Session["BoughtOfferId"] = offer.OfferId;
            HttpContext.Current.Session["BoughtCouponCount"] = count;
            HttpContext.Current.Session["BoughtShopingCartID"] = shopingCartID;
        }
    }
}