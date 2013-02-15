using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_CollectiveOfferList : System.Web.UI.UserControl
{
    public List<Collective.Offer> OfferList { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.rptOffers.DataSource = OfferList;
            this.rptOffers.DataBind();
        }
    }
}