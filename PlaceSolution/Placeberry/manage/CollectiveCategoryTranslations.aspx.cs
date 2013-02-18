using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class manage_CollectiveCategoryTranslations : System.Web.UI.Page
{
    private const string ERR_MSG_MISSING_CATEGORY_NAME = "Obavezan unos naziva kategorije.";
    private const string ERR_MSG_MISSING_TITLE = "Obavezan unos naslova.";
    private const string ERR_MSG_MISSING_DESCRIPTION = "Obavezan unos opisa.";
    private const string ERR_MSG_ERROR_CREATING_TRANSLATION = "Došlo je do greške pri kreiranju prijevoda. Dali postoji već prijevod za ovaj jezik?";


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.grdTrans.RowDataBound += new GridViewRowEventHandler(grdTrans_RowDataBound);
        this.grdTrans.RowEditing += new GridViewEditEventHandler(grdTrans_RowEditing);
        this.grdTrans.RowCancelingEdit += new GridViewCancelEditEventHandler(grdTrans_RowCancelingEdit);
        this.grdTrans.RowUpdating += new GridViewUpdateEventHandler(grdTrans_RowUpdating);
        this.grdTrans.RowCommand += new GridViewCommandEventHandler(grdTrans_RowCommand);
        this.grdTrans.RowDeleting += new GridViewDeleteEventHandler(grdTrans_RowDeleting);
    }

    void grdTrans_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        List<Collective.Language> allLangs = GetLanguages();

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Collective.CategoryTranslation catTrans = e.Row.DataItem as Collective.CategoryTranslation;

            Collective.Language currLang = (from l in allLangs
                                            where l.Id == catTrans.LanguageId
                                            select l).FirstOrDefault();


            Label lblLang = e.Row.FindControl("lblLang") as Label;
            if (lblLang != null && currLang != null)    // currLang moze bit null ako je dummy element
            {
                lblLang.Text = currLang.Title;
            }

            LinkButton lbDelete = e.Row.FindControl("lbDelete") as LinkButton;
            if (lbDelete != null && currLang != null)
            {
                lbDelete.Attributes.Add("onclick", "javascript:return confirm('Dali ste sigurni da želite obrisati " + currLang.Title + " prijevod?');");
            }


            //edit
            Label lblLangEdit = e.Row.FindControl("lblLangEdit") as Label;
            if (lblLangEdit != null && currLang != null)    // currLang moze bit null ako je dummy element
            {
                lblLangEdit.Text = currLang.Title;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlNewLang = e.Row.FindControl("ddlNewLang") as DropDownList;

            foreach (Collective.Language l in allLangs)
            {
                ddlNewLang.Items.Add(new ListItem(l.Title, l.Id.ToString()));
            }
        }
    }

    void grdTrans_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        grdTrans.EditIndex = e.NewEditIndex;
        CacheNewInputs();
    }

    void grdTrans_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        grdTrans.EditIndex = -1;
        CacheNewInputs();
    }

    void grdTrans_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string title = ((TextBox)grdTrans.Rows[e.RowIndex].FindControl("tbTitle")).Text.Trim();
        if (String.IsNullOrEmpty(title))
        {
            lblErrMsg.Text = ERR_MSG_MISSING_TITLE;
            return;
        }

        string desc = ((TextBox)grdTrans.Rows[e.RowIndex].FindControl("tbDescription")).Text.Trim();
        if (String.IsNullOrEmpty(desc))
        {
            lblErrMsg.Text = ERR_MSG_MISSING_DESCRIPTION;
            return;
        }

        DataKey dKey = grdTrans.DataKeys[e.RowIndex];
        int catId = Convert.ToInt32(dKey.Values["CategoryId"]);
        int langId = Convert.ToInt32(dKey.Values["LanguageId"]); ;

        Collective.CategoryTranslation.UpdateCategoryTranslaton(catId, langId, title, desc);

        lblErrMsg.Text = string.Empty;
        grdTrans.EditIndex = -1;
        CacheNewInputs();
    }

    void grdTrans_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("InsertNew"))
        {
            string title = ((TextBox)grdTrans.FooterRow.FindControl("tbNewTitle")).Text.Trim();
            string desc = ((TextBox)grdTrans.FooterRow.FindControl("tbNewDescription")).Text.Trim();
            int langId = Int32.Parse(((DropDownList)grdTrans.FooterRow.FindControl("ddlNewLang")).SelectedValue);

            hfSavedNewTitle.Value = title;
            hfSavedNewDesc.Value = desc;
            hfSavedNewLangId.Value = langId.ToString();

            if (String.IsNullOrEmpty(title))
            {
                lblErrMsg.Text = ERR_MSG_MISSING_TITLE;
                return;
            }


            if (String.IsNullOrEmpty(desc))
            {
                lblErrMsg.Text = ERR_MSG_MISSING_DESCRIPTION;
                return;
            }

            if (Collective.CategoryTranslation.CreateCategoryTranslaton(GetCategory().Id, langId, title, desc) == null)
            {
                lblErrMsg.Text = ERR_MSG_ERROR_CREATING_TRANSLATION;
                return;
            }

            hfSavedNewTitle.Value = string.Empty;
            hfSavedNewDesc.Value = string.Empty;
            hfSavedNewLangId.Value = string.Empty;

            lblErrMsg.Text = String.Empty;
        }
    }

    void grdTrans_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey dKey = grdTrans.DataKeys[e.RowIndex];
        int catId = Convert.ToInt32(dKey.Values["CategoryId"]);
        int langId = Convert.ToInt32(dKey.Values["LanguageId"]);

        Collective.CategoryTranslation.DeleteCategoryTranslaton(catId, langId);

        CacheNewInputs();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hlBackToCatLst.NavigateUrl = "http://" + Request.Url.Authority + "/manage/CollectiveCategoryList.aspx";

            if (GetCategory() == null)
            {
                lblErrMsg.Text = "Pogrešno uneseni parametri.";
                return;
            }

            lblCatName.Text = GetCategory().Name;
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (GetCategory() != null)
        {
            FillTransGrid(GetCategory().Id);
        }
    }


    private Collective.Category _category = null;

    private Collective.Category GetCategory()
    {
        if (_category == null)
        {
            int catId;
            if (Int32.TryParse(Request.QueryString["catid"], out catId) || Int32.TryParse(this.hfCreatedCategoryId.Value, out catId))
            {
                _category = Collective.Category.GetCategory(catId);
            }
        }

        return _category;
    }

    private List<Collective.Language> _lstLang;

    private List<Collective.Language> GetLanguages()
    {
        if (_lstLang == null)
        {
            _lstLang = Collective.Language.ListLanguages(true);
        }

        return _lstLang;
    }

    private void FillTransGrid(int catId)
    {
        List<Collective.CategoryTranslation> lst = Collective.CategoryTranslation.ListCategoryTranslations(catId);

        if (lst.Count > 0)
        {
            grdTrans.DataSource = lst;
            grdTrans.DataBind();
        }
        else
        {
            List<Collective.CategoryTranslation> lstDummy = new List<Collective.CategoryTranslation>();
            lstDummy.Add(new Collective.CategoryTranslation());    // add dummy item

            grdTrans.DataSource = lstDummy;
            grdTrans.DataBind();

            int totalColumns = grdTrans.Rows[0].Cells.Count;
            grdTrans.Rows[0].Cells.Clear();
            grdTrans.Rows[0].Cells.Add(new TableCell());
            grdTrans.Rows[0].Cells[0].ColumnSpan = totalColumns;
            grdTrans.Rows[0].Cells[0].Text = "Nema dodjeljenih prijevoda.";
            grdTrans.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            grdTrans.Rows[0].Cells[0].CssClass = "grid_item";
            grdTrans.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Gray;
        }

        SetCachedNewInputs();
    }

    private void SetCachedNewInputs()
    {
        if (!String.IsNullOrEmpty(hfSavedNewTitle.Value))
        {
            ((TextBox)grdTrans.FooterRow.FindControl("tbNewTitle")).Text = hfSavedNewTitle.Value;
            hfSavedNewTitle.Value = String.Empty;
        }
        if (!String.IsNullOrEmpty(hfSavedNewDesc.Value))
        {
            ((TextBox)grdTrans.FooterRow.FindControl("tbNewDescription")).Text = hfSavedNewDesc.Value;
            hfSavedNewDesc.Value = String.Empty;
        }
        if (!String.IsNullOrEmpty(hfSavedNewLangId.Value))
        {
            ((DropDownList)grdTrans.FooterRow.FindControl("ddlNewLang")).SelectedValue = hfSavedNewLangId.Value;
            hfSavedNewLangId.Value = String.Empty;
        }
    }

    private void CacheNewInputs()
    {
        hfSavedNewTitle.Value = ((TextBox)grdTrans.FooterRow.FindControl("tbNewTitle")).Text;
        hfSavedNewDesc.Value = ((TextBox)grdTrans.FooterRow.FindControl("tbNewDescription")).Text;
        hfSavedNewLangId.Value = ((DropDownList)grdTrans.FooterRow.FindControl("ddlNewLang")).SelectedValue.ToString();
    }
}