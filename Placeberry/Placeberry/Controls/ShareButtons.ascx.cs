using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_ShareButtons : System.Web.UI.UserControl
{
    public bool Custom = false;

    public string Url = string.Empty;
    public string Title = string.Empty;
    public string Description = string.Empty;
    public string Image = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Custom)
        {
            if (!string.IsNullOrEmpty(Url))
                addthisdiv.Attributes.Add("addthis:url", Url);

            if (!string.IsNullOrEmpty(Title))
                addthisdiv.Attributes.Add("addthis:title", Title);

            if (!string.IsNullOrEmpty(Description))
                addthisdiv.Attributes.Add("addthis:description", Description);

            if (!string.IsNullOrEmpty(Image))
                addthisdiv.Attributes.Add("addthis:image", Image);

            
        }
    }
}