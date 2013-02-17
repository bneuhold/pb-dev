using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class admin_PlacesList : System.Web.UI.Page
{
    private const string ERR_MSG_MISSING_TITLE = "Obavezan unos naslova.";
    private const string ERR_MSG_MISSING_URL_TAG = "Obavezan unos url-taga.";
    private const string ERR_MSG_URL_TAG_CONTAINS_INVALID_CHARACTERS = "URL TAG sadrži nedopuštene znakove.";
    private const string ERR_MSG_ERROR_URL_TAG_EXISTS = "URL TAG već postoji u bazi za drugo mjesto.";

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.grdPlaces.RowDataBound += new GridViewRowEventHandler(grdPlaces_RowDataBound);
        this.grdPlaces.RowEditing += new GridViewEditEventHandler(grdPlaces_RowEditing);
        this.grdPlaces.RowCancelingEdit += new GridViewCancelEditEventHandler(grdPlaces_RowCancelingEdit);
        this.grdPlaces.RowUpdating += new GridViewUpdateEventHandler(grdPlaces_RowUpdating);
        this.grdPlaces.RowCommand += new GridViewCommandEventHandler(grdPlaces_RowCommand);
        this.grdPlaces.RowDeleting += new GridViewDeleteEventHandler(grdPlaces_RowDeleting);
    }

    void grdPlaces_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Collective.Place place = e.Row.DataItem as Collective.Place;

            LinkButton lbDelete = e.Row.FindControl("lbDelete") as LinkButton;
            if (lbDelete != null && place != null)
            {
                lbDelete.Attributes.Add("onclick", "javascript:return confirm('Dali ste sigurni da želite obrisati mjesto: " + place.Title + "?');");
            }
        }
    }

    void grdPlaces_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        grdPlaces.EditIndex = e.NewEditIndex;
        CacheNewInputs();
        FillPlaceGrid();
    }

    void grdPlaces_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        grdPlaces.EditIndex = -1;
        CacheNewInputs();
        FillPlaceGrid();
    }

    void grdPlaces_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string title = ((TextBox)grdPlaces.Rows[e.RowIndex].FindControl("tbTitle")).Text.Trim();
        string desc = ((TextBox)grdPlaces.Rows[e.RowIndex].FindControl("tbDescription")).Text.Trim();
        bool active = ((CheckBox)grdPlaces.Rows[e.RowIndex].FindControl("cbActive")).Checked;
        string metaDesc = ((TextBox)grdPlaces.Rows[e.RowIndex].FindControl("tbMetaDesc")).Text.Trim();
        string metaKeywords = ((TextBox)grdPlaces.Rows[e.RowIndex].FindControl("tbMetaKeywords")).Text.Trim();
        string urlTag = ((TextBox)grdPlaces.Rows[e.RowIndex].FindControl("tbUrlTag")).Text.Trim();

        if (String.IsNullOrEmpty(title))
        {
            lblErrMsg.Text = ERR_MSG_MISSING_TITLE;
            return;
        }

        if (String.IsNullOrEmpty(urlTag))
        {
            lblErrMsg.Text = ERR_MSG_MISSING_URL_TAG;
            return;
        }

        if (!Regex.Match(urlTag, @"^[a-zA-Z\s\-]+$", RegexOptions.IgnoreCase).Success)
        {
            lblErrMsg.Text = ERR_MSG_URL_TAG_CONTAINS_INVALID_CHARACTERS;
            return;
        }

        urlTag = (urlTag.Trim()).Replace(' ', '-');

        DataKey dKey = grdPlaces.DataKeys[e.RowIndex];
        int id = Convert.ToInt32(dKey.Values["Id"]);

        Collective.Place.CreateUpdateResult result = Collective.Place.UpdatePlace(id, title, desc, active, metaDesc, metaKeywords, urlTag);
        if (result == Collective.Place.CreateUpdateResult.TagExistsForOtherPlace)
        {
            lblErrMsg.Text = ERR_MSG_ERROR_URL_TAG_EXISTS;
            return;
        }

        lblErrMsg.Text = string.Empty;
        grdPlaces.EditIndex = -1;
        CacheNewInputs();
        FillPlaceGrid();
    }

    void grdPlaces_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("InsertNew"))
        {
            string title = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewTitle")).Text.Trim();
            string desc = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewDescription")).Text.Trim();
            bool active = ((CheckBox)grdPlaces.FooterRow.FindControl("cbNewActive")).Checked;
            string metaDesc = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewMetaDesc")).Text.Trim();
            string metaKeywords = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewMetaKeywords")).Text.Trim();
            string urlTag = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewUrlTag")).Text.Trim();

            hfSavedNewTitle.Value = title;
            hfSavedNewDesc.Value = desc;
            hfSavedNewActive.Value = active.ToString();
            hfSavedNewMetaDesc.Value = metaDesc;
            hfSavedNewMetaKeywords.Value = metaKeywords;
            hfSavedNewUrlTag.Value = urlTag;

            if (String.IsNullOrEmpty(title))
            {
                lblErrMsg.Text = ERR_MSG_MISSING_TITLE;
                return;
            }

            if (String.IsNullOrEmpty(urlTag))
            {
                lblErrMsg.Text = ERR_MSG_MISSING_URL_TAG;
                return;
            }

            if (!Regex.Match(urlTag, @"^[a-zA-Z\s\-]+$", RegexOptions.IgnoreCase).Success)
            {
                lblErrMsg.Text = ERR_MSG_URL_TAG_CONTAINS_INVALID_CHARACTERS;
                return;
            }

            urlTag = (urlTag.Trim()).Replace(' ', '-');

            Collective.Place.CreateUpdateResult result = Collective.Place.CreatePlace(title, desc, active, metaDesc, metaKeywords, urlTag);
            if (result == Collective.Place.CreateUpdateResult.TagExistsForOtherPlace)
            {
                lblErrMsg.Text = ERR_MSG_ERROR_URL_TAG_EXISTS;
                return;
            }

            hfSavedNewTitle.Value = string.Empty;
            hfSavedNewDesc.Value = string.Empty;
            hfSavedNewActive.Value = string.Empty;
            hfSavedNewMetaDesc.Value = string.Empty;
            hfSavedNewMetaKeywords.Value = string.Empty;
            hfSavedNewUrlTag.Value = string.Empty;

            lblErrMsg.Text = String.Empty;

            FillPlaceGrid();
        }
    }

    void grdPlaces_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey dKey = grdPlaces.DataKeys[e.RowIndex];
        int id = Convert.ToInt32(dKey.Values["Id"]);

        Collective.Place.DeletePlace(id);

        CacheNewInputs();
        FillPlaceGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hlBack.NavigateUrl = "http://" + Request.Url.Authority + "/admin/Default.aspx";
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillPlaceGrid();
        }
    }

    private void FillPlaceGrid()
    {
        List<Collective.Place> lst = Collective.Place.ListPlaces(null);

        if (lst.Count > 0)
        {
            grdPlaces.DataSource = lst;
            grdPlaces.DataBind();
        }
        else
        {
            List<Collective.Place> lstDummy = new List<Collective.Place>();
            lstDummy.Add(new Collective.Place());    // add dummy item

            grdPlaces.DataSource = lstDummy;
            grdPlaces.DataBind();

            int totalColumns = grdPlaces.Rows[0].Cells.Count;
            grdPlaces.Rows[0].Cells.Clear();
            grdPlaces.Rows[0].Cells.Add(new TableCell());
            grdPlaces.Rows[0].Cells[0].ColumnSpan = totalColumns;
            grdPlaces.Rows[0].Cells[0].Text = "Nema pronađenih mjesta.";
            grdPlaces.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            grdPlaces.Rows[0].Cells[0].CssClass = "grid_item";
            grdPlaces.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Gray;
        }

        SetCachedNewInputs();
    }

    private void SetCachedNewInputs()
    {
        if (!String.IsNullOrEmpty(hfSavedNewTitle.Value))
        {
            ((TextBox)grdPlaces.FooterRow.FindControl("tbNewTitle")).Text = hfSavedNewTitle.Value;
            hfSavedNewTitle.Value = String.Empty;
        }
        if (!String.IsNullOrEmpty(hfSavedNewDesc.Value))
        {
            ((TextBox)grdPlaces.FooterRow.FindControl("tbNewDescription")).Text = hfSavedNewDesc.Value;
            hfSavedNewDesc.Value = String.Empty;
        }
        if (!String.IsNullOrEmpty(hfSavedNewActive.Value))
        {
            ((CheckBox)grdPlaces.FooterRow.FindControl("cbNewActive")).Checked = Boolean.Parse(hfSavedNewActive.Value);
            hfSavedNewActive.Value = String.Empty;
        }
        if (!String.IsNullOrEmpty(hfSavedNewMetaDesc.Value))
        {
            ((TextBox)grdPlaces.FooterRow.FindControl("tbNewMetaDesc")).Text = hfSavedNewMetaDesc.Value;
            hfSavedNewMetaDesc.Value = String.Empty;
        }
        if (!String.IsNullOrEmpty(hfSavedNewMetaKeywords.Value))
        {
            ((TextBox)grdPlaces.FooterRow.FindControl("tbNewMetaKeywords")).Text = hfSavedNewMetaKeywords.Value;
            hfSavedNewMetaKeywords.Value = String.Empty;
        }
        if (!String.IsNullOrEmpty(hfSavedNewUrlTag.Value))
        {
            ((TextBox)grdPlaces.FooterRow.FindControl("tbNewUrlTag")).Text = hfSavedNewUrlTag.Value;
            hfSavedNewUrlTag.Value = String.Empty;
        }
    }

    private void CacheNewInputs()
    {
        hfSavedNewTitle.Value = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewTitle")).Text;
        hfSavedNewDesc.Value = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewDescription")).Text;
        hfSavedNewActive.Value = ((CheckBox)grdPlaces.FooterRow.FindControl("cbNewActive")).Checked.ToString();
        hfSavedNewMetaDesc.Value = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewMetaDesc")).Text;
        hfSavedNewMetaKeywords.Value = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewMetaKeywords")).Text;
        hfSavedNewUrlTag.Value = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewUrlTag")).Text;
    }
}