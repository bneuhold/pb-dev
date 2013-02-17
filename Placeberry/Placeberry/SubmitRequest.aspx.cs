using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Placeberry.DAL;

public partial class SubmitRequest : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    protected void btnSendMessage_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            InsertRequest.Execute(txtURL.Text,
                                  txtName.Text,
                                  txtEmail.Text,
                                  txtPhone.Text,
                                  DateTime.Now);

            successForm.Visible = true;
            contactForm.Visible = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}