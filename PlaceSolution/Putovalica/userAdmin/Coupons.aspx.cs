using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class userAdmin_Coupons : UserPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime dtCurr = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            List<Collective.Coupon> lstCoups = Collective.Coupon.ListCouponsForClient(GetLoggedCollectiveUser().Id, PutovalicaUtil.GetLanguageId());

            List<Collective.Coupon> lstAvail = (from c in lstCoups
                                                where !c.DateUsed.HasValue && c.DateEnd > dtCurr
                                                select c).ToList<Collective.Coupon>();

            if (lstAvail.Count > 0)
            {
                this.rptAvail.DataSource = lstAvail;
                this.rptAvail.DataBind();
            }
            else
            {
                this.rptAvail.Visible = false;
                this.lblAvilEmptyMsg.Visible = true;
            }

            List<Collective.Coupon> lstUsed = (from c in lstCoups
                                               where c.DateUsed.HasValue
                                               select c).ToList<Collective.Coupon>();

            if (lstUsed.Count > 0)
            {
                this.rptUsed.DataSource = lstUsed;
                this.rptUsed.DataBind();
            }
            else
            {
                this.rptUsed.Visible = false;
                this.lblUsedEmptyMsg.Visible = true;
            }

            List<Collective.Coupon> lstTimeout = (from c in lstCoups
                                                  where !c.DateUsed.HasValue && c.DateEnd < dtCurr
                                                   select c).ToList<Collective.Coupon>();

            if (lstTimeout.Count > 0)
            {
                this.rptTimeout.DataSource = lstTimeout;
                this.rptTimeout.DataBind();
            }
            else
            {
                this.rptTimeout.Visible = false;
                this.lblTimeoutEmptyMsg.Visible = true;
            }
        }
    }
}