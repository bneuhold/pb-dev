using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using UltimateDC;
using System.Globalization;

public partial class Controls_EnterCity : System.Web.UI.UserControl
{
    const int EMPTY_VALUE = -1;
    const string EMPTY_TEXT = "";
    const CityEntryMode DEF_MODE = CityEntryMode.Automatic;

    int languageId = 1;
    UltimateDataContext dc = null;


    public UltimateDataContext DataContext
    {
        get { return this.dc; }
        set { this.dc = value; }
    }
    public int LanguageId
    {
        get { return this.languageId; }
        set { this.languageId = value; }
    }
    public CityEntryMode EntryMode
    {
        get { return (CityEntryMode)(ViewState["cityentrymode"] ?? DEF_MODE); }
        set { ViewState["cityentrymode"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {


    }



}


