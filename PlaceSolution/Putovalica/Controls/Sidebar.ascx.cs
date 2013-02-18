using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

public partial class Controls_Sidebar : System.Web.UI.UserControl
{
    private Queue<string> qStyles = new Queue<string>();
    private Collective.Category _selCategory;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(rptCategories_ItemDataBound);
    }

    void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Collective.Category cat = e.Item.DataItem as Collective.Category;
            
            HtmlGenericControl liCat = e.Item.FindControl("liCat") as HtmlGenericControl;
            HyperLink hlCat = e.Item.FindControl("hlCat") as HyperLink;

            string style = qStyles.Dequeue();
            if (cat.Id == _selCategory.Id)
            {
                liCat.Attributes.Add("class", style + " active");
            }
            else
            {
                liCat.Attributes.Add("class", style);
            }

            //hlCat.NavigateUrl = PutovalicaUtil.QSAddArgWithValue(Request.Url.AbsoluteUri, "categoryid", cat.Id.ToString());
            hlCat.NavigateUrl = "/" + cat.Translation.UrlTag;

            qStyles.Enqueue(style);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            qStyles.Enqueue("orange");
            qStyles.Enqueue("blue");
            qStyles.Enqueue("purple");
            qStyles.Enqueue("green");
            qStyles.Enqueue("red");
            qStyles.Enqueue("grey");

            UserPageBase pageBase = (UserPageBase)Page;
            List<Collective.Category> lstCats = pageBase.ListCategories();
            _selCategory = pageBase.GetSelectedCategory();

            if(_selCategory == null)
            {
                _selCategory = lstCats.FirstOrDefault();
            }

            this.rptCategories.DataSource = lstCats;
            this.rptCategories.DataBind();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }
}