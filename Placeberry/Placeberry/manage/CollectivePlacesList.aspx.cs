using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class manage_CollectivePlacesList : System.Web.UI.Page
{
    private const string ERR_MSG_MISSING_TITLE = "Obavezan unos naslova.";

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
    }

    void grdPlaces_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        grdPlaces.EditIndex = -1;
        CacheNewInputs();
    }

    void grdPlaces_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string title = ((TextBox)grdPlaces.Rows[e.RowIndex].FindControl("tbTitle")).Text.Trim();
        if (String.IsNullOrEmpty(title))
        {
            lblErrMsg.Text = ERR_MSG_MISSING_TITLE;
            return;
        }

        string desc = ((TextBox)grdPlaces.Rows[e.RowIndex].FindControl("tbDescription")).Text.Trim();
        bool active = ((CheckBox)grdPlaces.Rows[e.RowIndex].FindControl("cbActive")).Checked;

        DataKey dKey = grdPlaces.DataKeys[e.RowIndex];
        int id = Convert.ToInt32(dKey.Values["Id"]);

        Collective.Place.UpdatePlace(id, title, desc, active);

        lblErrMsg.Text = string.Empty;
        grdPlaces.EditIndex = -1;
        CacheNewInputs();
    }

    void grdPlaces_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("InsertNew"))
        {
            string title = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewTitle")).Text.Trim();
            string desc = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewDescription")).Text.Trim();
            bool active = ((CheckBox)grdPlaces.FooterRow.FindControl("cbNewActive")).Checked;

            hfSavedNewTitle.Value = title;
            hfSavedNewDesc.Value = desc;
            hfSavedNewActive.Value = active.ToString();

            if (String.IsNullOrEmpty(title))
            {
                lblErrMsg.Text = ERR_MSG_MISSING_TITLE;
                return;
            }

            Collective.Place.CreatePlace(title, desc, active);

            hfSavedNewTitle.Value = string.Empty;
            hfSavedNewDesc.Value = string.Empty;
            hfSavedNewActive.Value = string.Empty;

            lblErrMsg.Text = String.Empty;
        }
    }

    void grdPlaces_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey dKey = grdPlaces.DataKeys[e.RowIndex];
        int id = Convert.ToInt32(dKey.Values["Id"]);

        Collective.Place.DeletePlace(id);

        CacheNewInputs();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hlBack.NavigateUrl = "http://" + Request.Url.Authority + "/manage/CollectiveAdminHome.aspx";
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        FillPlaceGrid();
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

    }

    private void CacheNewInputs()
    {
        hfSavedNewTitle.Value = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewTitle")).Text;
        hfSavedNewDesc.Value = ((TextBox)grdPlaces.FooterRow.FindControl("tbNewDescription")).Text;
        hfSavedNewActive.Value = ((CheckBox)grdPlaces.FooterRow.FindControl("cbNewActive")).Checked.ToString();
    }
}