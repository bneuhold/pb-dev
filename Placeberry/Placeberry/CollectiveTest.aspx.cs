using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class CollectiveTest : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.btnTest.Click += new EventHandler(btnTest_Click);
    }

    void btnTest_Click(object sender, EventArgs e)
    {
        lblResult.Text = Collective.Coupon.CreateCoupon(Membership.GetUser(), 47).ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
}