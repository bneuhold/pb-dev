using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Adverts : System.Web.UI.UserControl
{

    public Repeater Adverts
    {
        get { return repAdverts; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}