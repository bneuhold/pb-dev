using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;
using System.Globalization;
using System.Web.Security;

public partial class test : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);


    }


    protected void Page_Load(object sender, EventArgs e)
    {
  
        string lala = string.Empty;
        
        var reze = Placeberry.DAL.GetResults.Execute("dubrovnik", 1, null, 0, 10, null, out lala);

        Response.Write(lala);

    }




    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }
}