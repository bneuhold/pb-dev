using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using UltimateDC;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Collections;


public partial class manage_UltimateTable : System.Web.UI.Page
{
    const int ULTGRID_MAX_ITEMS = 15;

    UltimateDataContext dc = null;
    protected UltimateTable ultimateTable = null;

    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        dc = new UltimateDataContext();

        string action = Request.QueryString["action"] ?? string.Empty;

        switch (action)
        {
            case "newentry":
                NewEntry();
                break;
            case "editentry":
                EditEntry();
                break;
            default:
                EditEntry();
                break;
        }

        if (!IsPostBack)
        {
            RefreshUltimateTableGrid();

            var querystring = HttpUtility.ParseQueryString(Request.Url.Query);
            querystring["action"] = "newentry";
            aNewUltimate.HRef = String.Format("/manage/ultimatetable.aspx?{0}", querystring.ToString());
        }

        GetShowStatusMessage();
    }

    private void NewEntry()
    {
        if (!IsPostBack)
        {

            UltimateTableNew();
            plhSelectedUltimateTable.Visible = true;

        }
    }
    private void EditEntry()
    {
        int ultimateid = 0;
        if (int.TryParse(Request.QueryString["ultimateid"], out ultimateid))
        {
            ultimateTable = dc.UltimateTables.Where(i => i.Id == ultimateid).SingleOrDefault();
        }

        if (!IsPostBack)
        {
            if (ultimateTable != null)
            {
                UltimateTableSelected();
                plhSelectedUltimateTable.Visible = true;
            }
            else
            {
                plhSelectedUltimateTable.Visible = false;
            }
        }
    }

    private void UltimateTableSelected()
    {
        mvwTitle.ActiveViewIndex = 0;
        plhTranslationsChildrenParents.Visible = true;

        ltlId.Text = ultimateTable.Id.ToString();
        lbtnDeleteUltimateTable.CommandArgument = ultimateTable.Id.ToString();

        FillUltimateTableDropDowns();

        ddlLanguageId.SelectedValue = ultimateTable.LanguageId.HasValue ? ultimateTable.LanguageId.ToString() : "-1";
        ddlObjectTypeId.SelectedValue = ((int)ultimateTable.ObjectTypeId).ToString();

        tbxTitle.Text = ultimateTable.Title ?? string.Empty;
        tbxRegexExpression.Text = ultimateTable.RegexExpression ?? string.Empty;
        tbxRegexExpressionExtended.Text = ultimateTable.RegexExpressionExtended ?? string.Empty;
        tbxDescription.Text = ultimateTable.Description ?? string.Empty;
        tbxAccuweather.Text = ultimateTable.Accuweather ?? string.Empty;

        chbxIsIgnored.Checked = ultimateTable.IsIgnored;
        chbxActive.Checked = ultimateTable.Active;

        btnSubmitAll.CommandName = "saveedit";


        grdvwTranslations.DataSource = ultimateTable.UltimateTableTranslations;
        grdvwTranslations.DataBind();

        RefreshRelations();
    }
    private void UltimateTableNew()
    {
        mvwTitle.ActiveViewIndex = 1;
        plhTranslationsChildrenParents.Visible = false;

        FillUltimateTableDropDowns();

        btnSubmitAll.CommandName = "savenew";
    }

    private void RefreshUltimateTableGrid()
    {
        int page = 1;
        int.TryParse(Request.QueryString["page"], out page);
        string sortBy = Request.QueryString["sort"] ?? string.Empty;

        var ultimates = dc.UltimateTables;

        int count = ultimates.Count();
        int pages = count / ULTGRID_MAX_ITEMS;
        if (count % ULTGRID_MAX_ITEMS > 0) pages++;

        if (page < 1) page = 1;
        else if (page > pages) page = pages;

        if (pages > 1)
        {
            CreatePagingControl(pages);
        }

        IQueryable<UltimateTable> sortUltimates = null;
        switch (sortBy)
        {
            case "title":
                sortUltimates = from p in ultimates
                                orderby p.Title
                                select p;
                break;
            case "desc":
                sortUltimates = from p in ultimates
                                orderby p.Description
                                select p;
                break;
            default:
                sortUltimates = from p in ultimates
                                orderby p.Id
                                select p;
                break;
        }

        int startAt = (page - 1) * ULTGRID_MAX_ITEMS;
        int endAt = ULTGRID_MAX_ITEMS;

        var selecUltimates = sortUltimates.Skip(startAt).Take(endAt);

        repUltimateTableGrid.DataSource = selecUltimates;
        repUltimateTableGrid.DataBind();

    }
    private void CreatePagingControl(int pages)
    {
        var querystring = HttpUtility.ParseQueryString(Request.Url.Query);
        querystring.Remove("ultimateid");
        querystring.Remove("action");

        for (int i = 1; i <= pages; i++)
        {
            string pagenum = i.ToString();
            querystring["page"] = pagenum;

            HtmlAnchor anchor = new HtmlAnchor();
            anchor.HRef = String.Format("/manage/ultimatetable.aspx?{0}", querystring.ToString());
            anchor.InnerText = pagenum;

            Literal spacer = new Literal();
            spacer.Text = "&nbsp;";

            plhUltimateGridPaging.Controls.Add(anchor);
            plhUltimateGridPaging.Controls.Add(spacer);
        }
    }

    private void FillUltimateTableDropDowns()
    {
        var empty = new { Id = -1, Title = string.Empty };

        var types = (from p in dc.UltimateTableObjectTypes
                     select new
                     {
                         Id = (int)p.Id,
                         Title = p.Code
                     }).ToList();

        types.Insert(0, empty);
        ddlObjectTypeId.DataSource = types;
        ddlObjectTypeId.DataValueField = "Id";
        ddlObjectTypeId.DataTextField = "Title";
        ddlObjectTypeId.DataBind();

        var languages = (from p in dc.Languages
                         select new
                         {
                             Id = p.Id,
                             Title = p.Title
                         }).ToList();

        languages.Insert(0, empty);
        ddlLanguageId.DataSource = languages;
        ddlLanguageId.DataValueField = "Id";
        ddlLanguageId.DataTextField = "Title";
        ddlLanguageId.DataBind();
    }
    private void FillNewTransLang()
    {
        var languages = from p in dc.Languages
                        where !ultimateTable.UltimateTableTranslations.Select(i => i.Language).Contains(p)
                        select new { Id = p.Id, Title = p.Title };

        ddlNewTransLang.DataSource = languages;
        ddlNewTransLang.DataValueField = "Id";
        ddlNewTransLang.DataTextField = "Title";
        ddlNewTransLang.DataBind();
    }

    private void RefreshParentsView()
    {
        var parent = (from p in dc.UltimateTableRelations
                      where p.Child == ultimateTable && p.Parent.UltimateTableObjectType.GroupCode == "GEO"
                      select p.Parent).Take(1).SingleOrDefault();
        if (parent != null)
        {
            mvwParents.ActiveViewIndex = 0;
            //afda
            ltlParentId.Text = parent.Id.ToString();
            aParentTitle.HRef = LinkToUltimateTable(parent.Id);
            aParentTitle.InnerText = parent.Title;
            ltlParentActive.Text = parent.Active.ToString();
            lbtnRemoveParent.CommandArgument = parent.Id.ToString();
        }
        else
        {
            mvwParents.ActiveViewIndex = 1;

            tbxSetNewParentTitle.Text = string.Empty;
            hfSetNewParentId.Value = string.Empty;
        }
    }
    private void RefreshChildrenView()
    {
        var children = from p in dc.UltimateTableRelations
                       where p.Parent == ultimateTable && p.Child.UltimateTableObjectType.GroupCode == "GEO"
                       orderby p.Child.Title ascending
                       select new
                       {
                           Id = p.Child.Id,
                           Title = p.Child.Title,
                           ObjectType = p.Child.UltimateTableObjectType.Code,
                           Link = LinkToUltimateTable(p.Child.Id),
                           Active = p.Active
                       };
        if (children.Any())
        {
            mvwChildren.ActiveViewIndex = 0;

            repChildren.DataSource = children;
            repChildren.DataBind();
        }
        else
        {
            mvwChildren.ActiveViewIndex = 1;
        }

        tbxSetNewChildTitle.Text = string.Empty;
        hfSetNewChildId.Value = string.Empty;
    }
    private void RefreshNotGeoParentsView()
    {
        var query = from p in dc.UltimateTableRelations
                    where p.Child == ultimateTable
                    select p;

        if (ultimateTable.UltimateTableObjectType.GroupCode == "GEO")
        {
            query = from p in query
                    where p.Parent.UltimateTableObjectType.GroupCode != "GEO" || p.Parent.UltimateTableObjectType.GroupCode == null
                    select p;
        }

        var parents = from p in query
                      orderby p.Parent.Title ascending
                      select new
                      {
                          Id = p.Parent.Id,
                          Title = p.Parent.Title,
                          ObjectType = p.Parent.UltimateTableObjectType.Code,
                          Link = LinkToUltimateTable(p.Parent.Id),
                          Active = p.Active
                      };

        if (parents.Any())
        {
            mvwNotGeoParents.ActiveViewIndex = 0;

            repNotGeoParents.DataSource = parents;
            repNotGeoParents.DataBind();
        }
        else
        {
            mvwNotGeoParents.ActiveViewIndex = 1;
        }

        tbxSetNewNotGeoParentTitle.Text = string.Empty;
        hfSetNewNotGeoParentId.Value = string.Empty;
    }
    private void RefreshNotGeoChildrenView()
    {
        var query = from p in dc.UltimateTableRelations
                    where p.Parent == ultimateTable
                    select p;

        if (ultimateTable.UltimateTableObjectType.GroupCode == "GEO")
        {
            query = from p in query
                    where p.Child.UltimateTableObjectType.GroupCode != "GEO" || p.Child.UltimateTableObjectType.GroupCode == null
                    select p;
        }

        var children = from p in query
                       orderby p.Child.Title ascending
                       select new
                       {
                           Id = p.Child.Id,
                           Title = p.Child.Title,
                           ObjectType = p.Child.UltimateTableObjectType.Code,
                           Link = LinkToUltimateTable(p.Child.Id),
                           Active = p.Active
                       };

        if (children.Any())
        {
            mvwNotGeoChildren.ActiveViewIndex = 0;

            repNotGeoChildren.DataSource = children;
            repNotGeoChildren.DataBind();
        }
        else
        {
            mvwNotGeoChildren.ActiveViewIndex = 1;
        }

        tbxSetNewNotGeoChildTitle.Text = string.Empty;
        hfSetNewNotGeoChildId.Value = string.Empty;
    }
    private void RefreshRelations()
    {
        if (ultimateTable.UltimateTableObjectType.GroupCode == "GEO")
        {
            plhGeoRelations.Visible = true;
            RefreshParentsView();
            RefreshChildrenView();
        }
        else
        {
            plhGeoRelations.Visible = false;
        }

        RefreshNotGeoParentsView();
        RefreshNotGeoChildrenView();
    }

    private string LinkToUltimateTable(int id)
    {
        return String.Format("/manage/ultimatetable.aspx?action=editentry&ultimateid={0}", id);
    }
    private void SetStatusMessage(string message)
    {
        HttpContext.Current.Items["statusmessage"] = message;
    }
    private void GetShowStatusMessage()
    {
        string message = (string)HttpContext.Current.Items["statusmessage"] ?? string.Empty;
        if (!string.IsNullOrEmpty(message))
        {
            Common.JavascripAlert(message, this);
        }
    }

    protected void UltimateGridCommand(object sender, CommandEventArgs e)
    {
        string command = e.CommandName;
        if (command == "selectrow")
        {
            int ultimateid = int.Parse((string)e.CommandArgument);

            var querystring = HttpUtility.ParseQueryString(Request.Url.Query);
            querystring["ultimateid"] = ultimateid.ToString();
            querystring["action"] = "editentry";

            Response.Redirect(String.Format("/manage/ultimatetable.aspx?{0}", querystring.ToString()));
        }
        else if (command == "deleterow")
        {
            int ultimateid = int.Parse((string)e.CommandArgument);

            var toDelete = dc.UltimateTables.Where(i => i.Id == ultimateid).SingleOrDefault();

            dc.UltimateTables.DeleteOnSubmit(toDelete);
            dc.SubmitChanges();

            SetStatusMessage(String.Format("Pojam id:<b>{0}</b>, Title:<b>{1}</b> uspješno pobrisan!", toDelete.Id.ToString(), toDelete.Title));

            RefreshUltimateTableGrid();
            plhSelectedUltimateTable.Visible = false;
        }
    }
    protected void SubmitChangesCommand(object sender, CommandEventArgs e)
    {
        string command = e.CommandName;
        Validate("ultimatetable");
        if (IsValid)
        {
            if (command == "saveedit" || command == "savenew")
            {
                if (command == "savenew")
                {
                    ultimateTable = new UltimateTable();
                    dc.UltimateTables.InsertOnSubmit(ultimateTable);
                }
                ultimateTable.Title = tbxTitle.Text.Trim();
                ultimateTable.RegexExpression = tbxRegexExpression.Text.Trim();
                ultimateTable.RegexExpressionExtended = tbxRegexExpressionExtended.Text.Trim();
                ultimateTable.Description = tbxDescription.Text.Trim();
                ultimateTable.Accuweather = tbxAccuweather.Text.Trim();

                ultimateTable.IsIgnored = chbxIsIgnored.Checked;
                ultimateTable.Active = chbxActive.Checked;

                ultimateTable.LanguageId = ddlLanguageId.SelectedValue == "-1" ? (int?)null : int.Parse(ddlLanguageId.SelectedValue);
                ultimateTable.ObjectTypeId = int.Parse(ddlObjectTypeId.SelectedValue);

                dc.SubmitChanges();


                string statusmessage = "Promjene spremljene";

                var querystring = HttpUtility.ParseQueryString(Request.Url.Query);
                querystring.Remove("ultimateid");
                querystring.Remove("action");

                if (command == "savenew")
                {
                    statusmessage = "Uspješan unos";
                    querystring["action"] = "editentry";
                    querystring["ultimateid"] = ultimateTable.Id.ToString();
                }

                SetStatusMessage(statusmessage);
                Server.Transfer(String.Format("/manage/ultimatetable.aspx?{0}", querystring.ToString()));
            }
        }
    }
    protected void SetRemoveParentCommand(object sender, CommandEventArgs e)
    {
        string command = e.CommandName;
        if (command == "removeparent")
        {
            int parentid = int.Parse((string)e.CommandArgument);
            dc.UltimateTableRelations.DeleteAllOnSubmit(dc.UltimateTableRelations.Where(i => i.ParentId == parentid && i.ChildId == ultimateTable.Id));
            dc.SubmitChanges();

            RefreshParentsView();
        }
        else if (command == "setnewparent")
        {
            var relation = new UltimateTableRelation();
            relation.Child = ultimateTable;
            relation.Active = true;
            try
            {
                relation.ParentId = int.Parse(hfSetNewParentId.Value);
                dc.UltimateTableRelations.InsertOnSubmit(relation);
                dc.SubmitChanges();
            }
            catch (Exception) { }

            RefreshParentsView();
        }
    }
    protected void SetRemoveChildCommand(object sender, CommandEventArgs e)
    {
        string command = e.CommandName;
        if (command == "removeparent")
        {
            int parentid = int.Parse((string)e.CommandArgument);
            dc.UltimateTableRelations.DeleteAllOnSubmit(dc.UltimateTableRelations.Where(i => i.ParentId == parentid && i.ChildId == ultimateTable.Id));
            dc.SubmitChanges();

            RefreshParentsView();
        }
        else if (command == "setnewparent")
        {
            var relation = new UltimateTableRelation();
            relation.Child = ultimateTable;
            relation.Active = true;
            try
            {
                relation.ParentId = int.Parse(hfSetNewParentId.Value);
                dc.UltimateTableRelations.InsertOnSubmit(relation);
                dc.SubmitChanges();
            }
            catch (Exception) { }

            RefreshParentsView();
        }
    }
    protected void SetRemoveNotGeoParentCommand(object sender, CommandEventArgs e)
    {
        string command = e.CommandName;
        if (command == "removeparent")
        {
            int parentid = int.Parse((string)e.CommandArgument);
            dc.UltimateTableRelations.DeleteAllOnSubmit(dc.UltimateTableRelations.Where(i => i.ParentId == parentid && i.ChildId == ultimateTable.Id));
            dc.SubmitChanges();

            RefreshNotGeoParentsView();
        }
        else if (command == "setnewparent")
        {
            var relation = new UltimateTableRelation();
            relation.Child = ultimateTable;
            relation.Active = true;
            try
            {
                relation.ParentId = int.Parse(hfSetNewNotGeoParentId.Value);
                dc.UltimateTableRelations.InsertOnSubmit(relation);
                dc.SubmitChanges();
            }
            catch (Exception) { }

            RefreshNotGeoParentsView();
        }
    }
    protected void SetRemoveNotGeoChildCommand(object sender, CommandEventArgs e)
    {
        string command = e.CommandName;
        if (command == "removechild")
        {
            int childid = int.Parse((string)e.CommandArgument);
            dc.UltimateTableRelations.DeleteAllOnSubmit(dc.UltimateTableRelations.Where(i => i.ParentId == ultimateTable.Id && i.ChildId == childid));
            dc.SubmitChanges();

            RefreshNotGeoChildrenView();
        }
        else if (command == "setnewchild")
        {
            var relation = new UltimateTableRelation();
            relation.Parent = ultimateTable;
            relation.Active = true;
            try
            {
                relation.ChildId = int.Parse(hfSetNewNotGeoChildId.Value);
                dc.UltimateTableRelations.InsertOnSubmit(relation);
                dc.SubmitChanges();
            }
            catch (Exception) { }

            RefreshNotGeoChildrenView();
        }
    }

    protected bool SelectRow(object id)
    {
        bool selected = false;
        int ultimateTableId = (int)id;
        if (ultimateTable != null)
        {
            if (ultimateTable.Id == ultimateTableId) selected = true;
        }
        return selected;
    }
    protected void grdvwTranslations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView gw = sender as GridView;

        if (gw.EditIndex >= 0 && gw.EditIndex == e.Row.RowIndex)
        {
            DropDownList ddl = e.Row.FindControl("ddlTransLang") as DropDownList;
            HtmlButton btn = e.Row.FindControl("btnEdtTransAuto") as HtmlButton;
            TextBox tbxT = e.Row.FindControl("tbxTransTitle") as TextBox;
            TextBox tbxR = e.Row.FindControl("tbxTransRegex") as TextBox;

            string langTitle = (string)DataBinder.Eval(e.Row.DataItem, "Language.Title");
            int langId = (int)DataBinder.Eval(e.Row.DataItem, "LanguageId");

            ddl.Items.Add(new ListItem(langTitle, langId.ToString()));
            ddl.SelectedIndex = 0;

            if (ddl != null && btn != null && tbxT != null && tbxR != null)
            {
                btn.Attributes["value"] = String.Format("{0};{1};{2}", tbxT.ClientID, tbxR.ClientID, ddl.ClientID);
            }
        }
    }
    protected void grdvwTranslations_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdvwTranslations.EditIndex = e.NewEditIndex;

        grdvwTranslations.DataSource = ultimateTable.UltimateTableTranslations;
        grdvwTranslations.DataBind();
    }
    protected void grdvwTranslations_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdvwTranslations.EditIndex = -1;

        grdvwTranslations.DataSource = ultimateTable.UltimateTableTranslations;
        grdvwTranslations.DataBind();
    }
    protected void grdvwTranslations_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Validate("translationedit");
        if (IsValid)
        {
            int index = grdvwTranslations.EditIndex;
            GridViewRow row = grdvwTranslations.Rows[index];

            TextBox tbxTitle = row.FindControl("tbxTransTitle") as TextBox;
            TextBox tbxRegex = row.FindControl("tbxTransRegex") as TextBox;
            CheckBox cbxActive = row.FindControl("chbxTransActive") as CheckBox;

            int id = (int)grdvwTranslations.DataKeys[e.RowIndex].Value;

            var trans = dc.UltimateTableTranslations.Where(i => i.Id == id).SingleOrDefault();

            if (trans != null)
            {
                trans.Title = tbxTitle.Text.Trim();
                trans.Regex = tbxRegex.Text.Trim();
                trans.Active = chbxActive.Checked;

                dc.SubmitChanges();
            }

            grdvwTranslations.EditIndex = -1;

            grdvwTranslations.DataSource = ultimateTable.UltimateTableTranslations;
            grdvwTranslations.DataBind();
        }
    }
    protected void grdvwTranslations_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = (int)grdvwTranslations.DataKeys[e.RowIndex].Value;

        dc.UltimateTableTranslations.DeleteOnSubmit(dc.UltimateTableTranslations.Where(i => i.Id == id).SingleOrDefault());
        dc.SubmitChanges();

        grdvwTranslations.DataSource = ultimateTable.UltimateTableTranslations;
        grdvwTranslations.DataBind();

        string oldvalue = ddlNewTransLang.SelectedValue;
        FillNewTransLang();
        try
        {
            ddlNewTransLang.SelectedValue = oldvalue;
        }
        catch (Exception) { }
    }
    protected void grdvwTranslations_Insert(object sender, CommandEventArgs e)
    {
        //grdvwTranslations.EditIndex = e.NewEditIndex;
        string command = e.CommandName;

        if (command == "showinsert")
        {
            tbxNewTransTitle.Text = string.Empty;
            tbxNewTransRegex.Text = string.Empty;
            chbxNewTransActive.Checked = true;

            FillNewTransLang();

            if (ddlNewTransLang.Items.Count > 0)
            {
                plhInsertTranslation.Visible = true;
                btnInsertTranslation.Visible = false;
            }
        }
        else if (command == "saveinsert")
        {
            Validate("translationnew");
            if (IsValid)
            {
                UltimateTableTranslation translation = new UltimateTableTranslation();
                dc.UltimateTableTranslations.InsertOnSubmit(translation);

                translation.UltimateTableId = ultimateTable.Id;
                translation.Title = tbxNewTransTitle.Text.Trim();
                translation.Regex = tbxNewTransRegex.Text.Trim();

                translation.LanguageId = int.Parse(ddlNewTransLang.SelectedValue);
                translation.Active = chbxNewTransActive.Checked;

                dc.SubmitChanges();

                plhInsertTranslation.Visible = false;
                btnInsertTranslation.Visible = true;

                grdvwTranslations.DataSource = ultimateTable.UltimateTableTranslations;
                grdvwTranslations.DataBind();
            }
        }
        else if (command == "cancelinsert")
        {
            plhInsertTranslation.Visible = false;
            btnInsertTranslation.Visible = true;
        }
    }

    protected void ImportRelations(object sender, EventArgs e)
    {
        int ultimateId = 0;
        if (int.TryParse(hfImportRelations.Value, out ultimateId))
        {
            var query = from p in dc.UltimateTableRelations
                            where p.ChildId == ultimateId || p.ParentId == ultimateId
                            select p;

            var georelations = from p in query
                               where p.Child.UltimateTableObjectType.GroupCode == "GEO" && p.Parent.UltimateTableObjectType.GroupCode == "GEO"
                               select p;

            //geo relacije maknemo
            var withoutgeo = (from p in query
                             where !georelations.Contains(p)
                             select p).ToList();

            //dosadašnje relacije vezane za ovaj pojam
            var oldrelations = (from p in ultimateTable.UltimateTableRelations.Union(ultimateTable.UltimateTableRelations1)
                                select new { p.ChildId, p.ParentId }).ToList();


            List<UltimateTableRelation> newrelations = new List<UltimateTableRelation>();
            foreach (var rel in withoutgeo)
            {
                int childId = rel.ChildId == ultimateId ? ultimateTable.Id : rel.ChildId;
                int parentId = rel.ParentId == ultimateId ? ultimateTable.Id : rel.ParentId;

                if (!oldrelations.Any(i => i.ChildId == childId && i.ParentId == parentId))
                {
                    newrelations.Add(new UltimateTableRelation()
                    {
                        ChildId = childId,
                        ParentId = parentId,
                        Type = rel.Type,
                        Priority = rel.Priority,
                        Active = rel.Active
                    });
                }
            }

            dc.UltimateTableRelations.InsertAllOnSubmit(newrelations);
            dc.SubmitChanges();
            
            Common.JavascripAlert("Uspješno preneseni relationsi!", this);

            RefreshNotGeoParentsView();
            RefreshNotGeoChildrenView();
        }


    }

    [WebMethod]
    public static string GetAutoRegex(string term, int languageid)
    {
        string regex = string.Empty;
        using (UltimateDataContext dc = new UltimateDataContext())
        {
            dc.GetAutoRegex(term, languageid, ref regex);
        }
        return regex;
    }
    [WebMethod]
    public static IEnumerable GetUltimateTableSearchSuggestions(string term, int maxresults)
    {
        IEnumerable results = null;
        using (UltimateDataContext dc = new UltimateDataContext())
        {
            results = (from p in dc.UltimateTables
                       where p.Title.Contains(term)
                       orderby p.Title ascending
                       select new
                       {
                           Id = p.Id,
                           Title = String.Format("{0} [id={1}, type={2}]", p.Title, p.Id, p.UltimateTableObjectType.Code.ToLower()),
                           Url = String.Format("/manage/ultimatetable.aspx?ultimateid={0}", p.Id)
                       }).Take(maxresults).ToList();
        }

        return results;
    }

    [WebMethod]
    public static IEnumerable GetAutoParentSuggestions(string term, int childtypeid, int maxresults)
    {
        IEnumerable results = null;

        if (childtypeid > 0)
        {
            ObjectType childtype = (ObjectType)childtypeid;

            using (UltimateDataContext dc = new UltimateDataContext())
            {
                IQueryable<UltimateTable> query = null;
                bool noquery = false;
                switch (childtype)
                {
                    case ObjectType.ISLAND:
                        query = from p in dc.UltimateTables
                                where (p.ObjectTypeId == (int)ObjectType.SUBREGION ||
                                         p.ObjectTypeId == (int)ObjectType.REGION) && p.Title.Contains(term)
                                select p;
                        break;
                    case ObjectType.CITY:
                        query = from p in dc.UltimateTables
                                where (p.ObjectTypeId == (int)ObjectType.SUBREGION ||
                                         p.ObjectTypeId == (int)ObjectType.REGION ||
                                         p.ObjectTypeId == (int)ObjectType.ISLAND) && p.Title.Contains(term)
                                select p;
                        break;
                    case ObjectType.COUNTRY:
                        noquery = true;
                        break;
                    case ObjectType.REGION:
                        query = from p in dc.UltimateTables
                                where p.ObjectTypeId == (int)ObjectType.COUNTRY && p.Title.Contains(term)
                                select p;
                        break;
                    case ObjectType.SUBREGION:
                        query = from p in dc.UltimateTables
                                where p.ObjectTypeId == (int)ObjectType.REGION && p.Title.Contains(term)
                                select p;
                        break;
                    default:
                        noquery = true;
                        break;
                }
                if (!noquery)
                {
                    results = (from p in query
                               orderby p.Title ascending
                               select new
                               {
                                   Id = p.Id,
                                   Title = String.Format("{0} [id={1}, type={2}]", p.Title, p.Id, p.UltimateTableObjectType.Code.ToLower())
                               }).Take(maxresults).ToList();
                }
            }
        }
        return results;
    }
    [WebMethod]
    public static IEnumerable GetAutoChildSuggestions(string term, int parentid, int parenttypeid, int maxresults)
    {
        IEnumerable results = null;

        if (parenttypeid > 0)
        {
            ObjectType parentype = (ObjectType)parenttypeid;

            using (UltimateDataContext dc = new UltimateDataContext())
            {
                IQueryable<UltimateTable> query = null;
                bool noquery = false;
                switch (parentype)
                {
                    case ObjectType.ISLAND:
                        query = from p in dc.UltimateTables
                                where p.ObjectTypeId == (int)ObjectType.CITY && p.Title.Contains(term)
                                select p;
                        break;
                    case ObjectType.CITY:
                        noquery = true;
                        break;
                    case ObjectType.COUNTRY:
                        query = from p in dc.UltimateTables
                                where (p.ObjectTypeId == (int)ObjectType.REGION ||
                                    p.ObjectTypeId == (int)ObjectType.SUBREGION) && p.Title.Contains(term)
                                select p;
                        break;
                    case ObjectType.REGION:
                        query = from p in dc.UltimateTables
                                where (p.ObjectTypeId == (int)ObjectType.SUBREGION ||
                                    p.ObjectTypeId == (int)ObjectType.ISLAND ||
                                    p.ObjectTypeId == (int)ObjectType.CITY) && p.Title.Contains(term)
                                select p;
                        break;
                    case ObjectType.SUBREGION:
                        query = from p in dc.UltimateTables
                                where (p.ObjectTypeId == (int)ObjectType.ISLAND ||
                                    p.ObjectTypeId == (int)ObjectType.CITY) && p.Title.Contains(term)
                                select p;
                        break;
                    default:
                        noquery = true;
                        break;
                }
                if (!noquery)
                {
                    var check = from p in dc.UltimateTableRelations
                                where p.Parent.UltimateTableObjectType.GroupCode == "GEO"
                                select p.Child;

                    var children = from p in dc.UltimateTableRelations
                                   where p.ParentId == parentid && p.Child.UltimateTableObjectType.GroupCode == "GEO"
                                   select p.Child;

                    results = (from p in query
                               where !check.Contains(p) && !children.Contains(p)
                               orderby p.Title ascending
                               select new
                               {
                                   Id = p.Id,
                                   Title = String.Format("{0} [id={1}, type={2}]", p.Title, p.Id, p.UltimateTableObjectType.Code.ToLower())
                               }).Take(maxresults).ToList();
                }

            }
        }
        return results;
    }

    [WebMethod]
    public static IEnumerable GetAutoNotGeoParentSuggestions(string term, int childid, int maxresults)
    {
        IEnumerable results = null;

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            var child = (from p in dc.UltimateTables
                         where p.Id == childid
                         select p).SingleOrDefault();

            var parents = from p in dc.UltimateTableRelations
                          where p.Child == child
                          select p.Child;

            var query = from p in dc.UltimateTables
                        where !parents.Contains(p)
                                && p != child
                                && p.Title.Contains(term)
                        select p;

            if (child.UltimateTableObjectType.GroupCode == "GEO")
            {
                query = from p in query
                        where p.UltimateTableObjectType.GroupCode != "GEO" || p.UltimateTableObjectType.GroupCode == null
                        select p;
            }

            results = (from p in query
                       orderby p.Title ascending
                       select new
                       {
                           Id = p.Id,
                           Title = String.Format("{0} [id={1}, type={2}]", p.Title, p.Id, p.UltimateTableObjectType.Code.ToLower())
                       }).Take(maxresults).ToList();
        }

        return results;
    }
    [WebMethod]
    public static IEnumerable GetAutoNotGeoChildSuggestions(string term, int parentid, int maxresults)
    {
        IEnumerable results = null;

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            var parent = (from p in dc.UltimateTables
                          where p.Id == parentid
                          select p).SingleOrDefault();

            var children = from p in dc.UltimateTableRelations
                           where p.Parent == parent
                           select p.Child;

            var query = from p in dc.UltimateTables
                        where !children.Contains(p)
                                && p != parent
                                && p.Title.Contains(term)
                        select p;

            if (parent.UltimateTableObjectType.GroupCode == "GEO")
            {
                query = from p in query
                        where p.UltimateTableObjectType.GroupCode != "GEO" || p.UltimateTableObjectType.GroupCode == null
                        select p;
            }

            results = (from p in query
                       orderby p.Title ascending
                       select new
                       {
                           Id = p.Id,
                           Title = String.Format("{0} [id={1}, type={2}]", p.Title, p.Id, p.UltimateTableObjectType.Code.ToLower())
                       }).Take(maxresults).ToList();
        }

        return results;
    }


}