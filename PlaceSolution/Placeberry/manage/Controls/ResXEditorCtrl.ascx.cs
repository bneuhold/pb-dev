﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using ResxWebEditor;
using System.Drawing;
using System.Data;
using System.IO;
using System.Configuration;


public partial class manage_Controls_ResXEditorCtrl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            GetResX();
            //FillLanguages();
        }
    }

    /// <summary>
    /// Fill the language drop down.  Either with a list of predefined cultures or from a list of default
    /// cultures from .NET.
    /// </summary>
    private void FillLanguages()
    {
        SortedList<string, string> langs;
        if (Unified != null)
            langs = Unified.GetLanguages();
        else
            langs = new SortedList<string, string>();

        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        SortedList<string, CultureInfo> cults = new SortedList<string, CultureInfo>();
        foreach (CultureInfo culture in cultures)
            if (!cults.ContainsKey(culture.DisplayName))
                cults.Add(culture.DisplayName, culture);

        ddLanguage.Items.Clear();

        foreach (CultureInfo culture in cults.Values)
        {
            if (!langs.ContainsKey(culture.Name))
            {
                if (IncludeLanguages == "*" && ExcludeLanguages == "")
                    AddToLanguages(culture);
                else
                {
                    List<string> excludes = StringToList(ExcludeLanguages);
                    if (excludes.Contains(culture.Name.ToLowerInvariant()))
                        break;

                    if (IncludeLanguages == "*")
                        AddToLanguages(culture);
                    else
                    {
                        List<string> includes = StringToList(IncludeLanguages);
                        if (includes.Contains(culture.Name.ToLowerInvariant()))
                            AddToLanguages(culture);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Add a culture info to the drop down of languages
    /// </summary>
    /// <param name="culture"></param>
    protected virtual void AddToLanguages(CultureInfo culture)
    {
        if (culture.DisplayName.Length > 30)
            ddLanguage.Items.Add(new ListItem(string.Format("{0} ({1})", culture.DisplayName.Substring(0, 30), culture.Name), culture.Name));
        else
            ddLanguage.Items.Add(new ListItem(string.Format("{0} ({1})", culture.DisplayName, culture.Name), culture.Name));
    }

    /// <summary>
    /// Get all the Resx in the web site and list them in a list
    /// </summary>
    protected void GetResX()
    {
        SortedList<string, string> list = ResXUnified.GetResXInDirectory(GetTempPath(),
            new GenericPredicate<string, string>(delegate(string[] path)
            {
                return path[0].Replace(path[1], "").Replace("App_LocalResources", "").Replace("App_GlobalResources", "");
            }));

        if (list.Count > 0)
        {
            phDisplay.Visible = true;
            phEmptyFolderMsg.Visible = false;
            CurrentSelection = list.First().Value;
            FillGridView(CurrentSelection, true);
            FillLanguages();
        }
        else
        {
            phDisplay.Visible = false;
            phEmptyFolderMsg.Visible = true;
            ltEmptyFolderMsg.Text = "Directory: " + GetTempPath() + " is empty!";
        }
    }

    private void FillGridView()
    {
        FillGridView(CurrentSelection, false);
    }

    private void FillGridView(string p, bool reloadFile)
    {
        if (Unified == null || reloadFile)
            Unified = new ResXUnified(p);

        gridView.Columns.Clear();

        SortedList<string, string> langs = Unified.GetLanguages();

        Unit columnSize = new Unit((gridView.Width.Value - 30) / (langs.Values.Count), UnitType.Pixel);

        /*
        ImageField keyColumn = new ImageField();
        keyColumn.HeaderText = "Key";
        keyColumn.DataAlternateTextField = "Key";
        keyColumn.DataImageUrlField = "Key";
        keyColumn.DataImageUrlFormatString = "~/images/information.png"; // ignore key
        //keyColumn.DataField = "Key";
        //keyColumn.ItemStyle.BackColor = Color.Gray;
        //keyColumn.ItemStyle.ForeColor = Color.White;
        keyColumn.ReadOnly = true;
        keyColumn.ItemStyle.Width = new Unit(30);
        keyColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        keyColumn.ControlStyle.Width = new Unit(16);
        gridView.Columns.Add(keyColumn);
         * */
        BoundField keyColumn = new BoundField();
        keyColumn.HeaderText = "Key";
        keyColumn.DataField = "Key";
        keyColumn.ItemStyle.BackColor = Color.Gray;
        keyColumn.ItemStyle.ForeColor = Color.White;
        keyColumn.ReadOnly = true;
        keyColumn.ItemStyle.Width = new Unit(30);
        keyColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        keyColumn.ControlStyle.Width = new Unit(16);
        gridView.Columns.Add(keyColumn);

        foreach (string lang in langs.Values)
        {
            BoundField field = new BoundField();
            CultureInfo culture = null;
            try { culture = new CultureInfo(lang); }
            catch { }
            if (culture != null)
                field.HeaderText = culture.DisplayName;
            else
                field.HeaderText = "Default";

            field.DataField = lang;
            field.ItemStyle.Width = columnSize;
            field.ControlStyle.Width = columnSize;

            gridView.Columns.Add(field);
        }

        gridView.DataSource = Unified.ToDataTable(!chShowEmpty.Checked);
        gridView.DataBind();
    }

    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRow row = ((DataRowView)e.Row.DataItem).Row;
            string key = (string)row["Key"];
            bool flagEmpty = false;

            for (int i = 1; i < gridView.Columns.Count; i++)
            {
                string lang = ((BoundField)gridView.Columns[i]).DataField;
                if (!string.IsNullOrEmpty(Unified[lang][key]))
                {
                    flagEmpty = true;
                    break;
                }
            }

            for (int i = 1; i < gridView.Columns.Count; i++)
            {
                string lang = ((BoundField)gridView.Columns[i]).DataField;

                if (string.IsNullOrEmpty(Unified[lang][key]) && flagEmpty)
                {
                    ((TextBox)e.Row.Cells[i].Controls[0]).BackColor = Color.LightBlue;
                    ((TextBox)e.Row.Cells[i].Controls[0]).Attributes.Add("onblur", "if (this.value.length==0) {this.style.backgroundColor='lightblue'; } else { this.style.backgroundColor='white'; } ");
                }
            }
        }
    }

    protected void gridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable table = Unified.ToDataTable(!chShowEmpty.Checked);

        string key = (string)table.Rows[e.RowIndex]["Key"];

        for (int i = 1; i < gridView.Columns.Count; i++)
        {
            BoundField field = gridView.Columns[i] as BoundField;
            TextBox txt = gridView.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox;
            Unified[field.DataField][key] = txt.Text;
        }
    }

    protected void gridView_Saved(object sender, EventArgs e)
    {
        FillGridView();

        try
        {
            Unified.Save();

            MultiViewMsg.ActiveViewIndex = 0;
            lblMsg.Text = string.Format("Saved sucessfully {0}", DateTime.Now.ToShortTimeString());
        }
        catch
        {
            MultiViewMsg.ActiveViewIndex = 1;
            lblMsg.Text = "Error while saving.";
        }

        btSave.Enabled = true;
        pnlMsg.Visible = true;
    }

    protected void btAddLang_Click(object sender, EventArgs e)
    {
        Unified.AddLanguage(ddLanguage.SelectedValue);
        FillLanguages();
        FillGridView();
    }

    protected List<string> StringToList(string str)
    {
        List<string> toReturn = new List<string>();
        string[] splits = str.Split(',', ';');
        foreach (string split in splits)
            toReturn.Add(split.Trim().ToLowerInvariant());

        return toReturn;
    }

    protected void OnShowEmpty(object sender, EventArgs e)
    {
        FillGridView();
    }

    private string GetTempPath()
    {
        //return (string)(ViewState["Path"] ?? Server.MapPath("~//App_GlobalResources"));

        string tempPath = ViewState["Path"] as string;
        if(tempPath == null)
        {
            tempPath = ConfigurationManager.AppSettings["ResXTempFolder"];
            DirectoryInfo di = new DirectoryInfo(tempPath);
            if(!di.Exists)
            {
                di.Create();
            }

            ViewState["Path"] = tempPath;
        }

        return tempPath;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        DirectoryInfo sourcedinfo = new DirectoryInfo(GetTempPath());
        DirectoryInfo destinfo = new DirectoryInfo(Server.MapPath("~//App_GlobalResources"));

        foreach (FileInfo fi in sourcedinfo.GetFiles())
        {
            fi.CopyTo(Path.Combine(destinfo.ToString(), fi.Name), true);
        }
    }
    
    //
    // Properties
    //
    #region Properties

    /// <summary>
    /// The current RESX file selected
    /// </summary>
    protected string CurrentSelection
    {
        get
        {
            return (string)ViewState["FilePath"];
        }
        set
        {
            ViewState["FilePath"] = value;
        }
    }

    protected ResXUnified Unified
    {
        get
        {
            return (ResXUnified)(ViewState["Unified"]);
        }
        set
        {
            ViewState["Unified"] = value;
        }
    }

    public string IncludeLanguages
    {
        get
        {
            return (string)(ViewState["IncludeLanguages"] ?? "*");
        }
        set
        {
            ViewState["IncludeLanguages"] = value;
        }
    }

    public string ExcludeLanguages
    {
        get
        {
            return (string)(ViewState["ExcludeLanguages"] ?? "");
        }
        set
        {
            ViewState["ExcludeLanguages"] = value;
        }
    }

    #endregion
}
