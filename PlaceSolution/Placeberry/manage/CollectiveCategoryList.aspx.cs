using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class manage_CollectiveCategoryList : System.Web.UI.Page
{
    private const string ERR_MSG_MISSING_NAME = "Obavezan unos naziva.";
    private const string ERR_MSG_WRONG_PRIORITY_FORMAT = "Krivo unesen format prioriteta.";

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.grdCategories.RowDataBound += new GridViewRowEventHandler(grdCategories_RowDataBound);
        this.grdCategories.RowEditing += new GridViewEditEventHandler(grdCategories_RowEditing);
        this.grdCategories.RowCancelingEdit += new GridViewCancelEditEventHandler(grdCategories_RowCancelingEdit);
        this.grdCategories.RowUpdating += new GridViewUpdateEventHandler(grdCategories_RowUpdating);
        this.grdCategories.RowCommand += new GridViewCommandEventHandler(grdCategories_RowCommand);
        this.grdCategories.RowDeleting += new GridViewDeleteEventHandler(grdCategories_RowDeleting);
    }

    void grdCategories_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Collective.Category cat = e.Row.DataItem as Collective.Category;

            LinkButton lbDelete = e.Row.FindControl("lbDelete") as LinkButton;
            if (lbDelete != null && cat != null)
            {
                lbDelete.Attributes.Add("onclick", "javascript:return confirm('Dali ste sigurni da želite obrisati kategoriju: " + cat.Name + "?');");
            }

            HyperLink hlEditTrans = e.Row.FindControl("hlEditTrans") as HyperLink;
            if (hlEditTrans != null)
            {
                string url = "http://" + Request.Url.Authority + "/manage/CollectiveCategoryTranslations.aspx?catid=" + cat.Id;
                hlEditTrans.NavigateUrl = url;
            }
        }
    }

    void grdCategories_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        grdCategories.EditIndex = e.NewEditIndex;
        CacheNewInputs();
    }

    void grdCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        grdCategories.EditIndex = -1;
        CacheNewInputs();
    }

    void grdCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string name = ((TextBox)grdCategories.Rows[e.RowIndex].FindControl("tbName")).Text.Trim();
        if (String.IsNullOrEmpty(name))
        {
            lblErrMsg.Text = ERR_MSG_MISSING_NAME;
            return;
        }

        int priority;
        TextBox tbPriority = (TextBox)grdCategories.Rows[e.RowIndex].FindControl("tbPriority");
        if (String.IsNullOrEmpty(tbPriority.Text))
        {
            priority = 0;
        }
        else if (!Int32.TryParse(tbPriority.Text, out priority))
        {
            lblErrMsg.Text = ERR_MSG_WRONG_PRIORITY_FORMAT;
            return; 
        }

        bool active = ((CheckBox)grdCategories.Rows[e.RowIndex].FindControl("cbActive")).Checked;

        DataKey dKey = grdCategories.DataKeys[e.RowIndex];
        int id = Convert.ToInt32(dKey.Values["Id"]);

        Collective.Category.UpdateCategory(id, name, priority, active);

        lblErrMsg.Text = string.Empty;
        grdCategories.EditIndex = -1;
        CacheNewInputs();
    }

    void grdCategories_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("InsertNew"))
        {
            string name = ((TextBox)grdCategories.FooterRow.FindControl("tbNewName")).Text.Trim();
            bool active = ((CheckBox)grdCategories.FooterRow.FindControl("cbNewActive")).Checked;
            TextBox tbNewPriority = (TextBox)grdCategories.FooterRow.FindControl("tbNewPriority");

            hfSavedNewName.Value = name;
            hfSavedPriority.Value = tbNewPriority.Text;
            hfSavedNewActive.Value = active.ToString();

            if (String.IsNullOrEmpty(name))
            {
                lblErrMsg.Text = ERR_MSG_MISSING_NAME;
                return;
            }

            int priority;
            if (String.IsNullOrEmpty(tbNewPriority.Text))
            {
                priority = 0;
            }
            else if (!Int32.TryParse(tbNewPriority.Text, out priority))
            {
                lblErrMsg.Text = ERR_MSG_WRONG_PRIORITY_FORMAT;
                return;
            }

            Collective.Category.CreateCategory(name, priority, active);

            hfSavedNewName.Value = string.Empty;
            hfSavedPriority.Value = string.Empty;
            hfSavedNewActive.Value = string.Empty;

            lblErrMsg.Text = String.Empty;
        }
    }

    void grdCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey dKey = grdCategories.DataKeys[e.RowIndex];
        int id = Convert.ToInt32(dKey.Values["Id"]);

        Collective.Category.DeleteCategory(id);

        CacheNewInputs();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        FillCategoriesGride();
    }

    private void FillCategoriesGride()
    {
        List<Collective.Category> lst = Collective.Category.ListCategories(null);

        if (lst.Count > 0)
        {
            grdCategories.DataSource = lst;
            grdCategories.DataBind();
        }
        else
        {
            List<Collective.Category> lstDummy = new List<Collective.Category>();
            lstDummy.Add(new Collective.Category());    // add dummy item

            grdCategories.DataSource = lstDummy;
            grdCategories.DataBind();

            int totalColumns = grdCategories.Rows[0].Cells.Count;
            grdCategories.Rows[0].Cells.Clear();
            grdCategories.Rows[0].Cells.Add(new TableCell());
            grdCategories.Rows[0].Cells[0].ColumnSpan = totalColumns;
            grdCategories.Rows[0].Cells[0].Text = "Nema postojećih kategorija.";
            grdCategories.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            grdCategories.Rows[0].Cells[0].CssClass = "grid_item";
            grdCategories.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Gray;
        }

        SetCachedNewInputs();
    }

    private void SetCachedNewInputs()
    {
        if (!String.IsNullOrEmpty(hfSavedNewName.Value))
        {
            ((TextBox)grdCategories.FooterRow.FindControl("tbNewName")).Text = hfSavedNewName.Value;
            hfSavedNewName.Value = String.Empty;
        }

        if (!String.IsNullOrEmpty(hfSavedPriority.Value))
        {
            ((TextBox)grdCategories.FooterRow.FindControl("tbNewPriority")).Text = hfSavedPriority.Value;
        }

        if (!String.IsNullOrEmpty(hfSavedNewActive.Value))
        {
            ((CheckBox)grdCategories.FooterRow.FindControl("cbNewActive")).Checked = Boolean.Parse(hfSavedNewActive.Value);
            hfSavedNewActive.Value = String.Empty;
        }

    }

    private void CacheNewInputs()
    {
        hfSavedNewName.Value = ((TextBox)grdCategories.FooterRow.FindControl("tbNewName")).Text;
        hfSavedPriority.Value = ((TextBox)grdCategories.FooterRow.FindControl("tbNewPriority")).Text;
        hfSavedNewActive.Value = ((CheckBox)grdCategories.FooterRow.FindControl("cbNewActive")).Checked.ToString();
    }

}