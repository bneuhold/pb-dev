using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Slider : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            List<int> lst = new List<int>();
            for (int i = 0; i < 4; ++i) // tu ih mora bit 4 jer onih tockica je 4
                lst.Add(i);

            this.rptSidebarOffers.DataSource = lst;
            this.rptSidebarOffers.DataBind();
        }
    }
}