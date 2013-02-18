using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class admin_CouponsList : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.grdCoupons.RowDataBound += new GridViewRowEventHandler(grdCoupons_RowDataBound);
        this.grdCoupons.RowDeleting += new GridViewDeleteEventHandler(grdCoupons_RowDeleting);
    }

    void grdCoupons_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Collective.Coupon coupon = e.Row.DataItem as Collective.Coupon;


            LinkButton lbDelete = e.Row.FindControl("lbDelete") as LinkButton;
            if (lbDelete != null && coupon != null)
            {
                lbDelete.Attributes.Add("onclick", "javascript:return confirm('Dali ste sigurni da želite obrisati kupon\\nza ponudu: \"" + coupon.OfferName.Replace('\'', ' ') + "\"\\nod korisnika: \"" + coupon.UserEmail +  "\"?');");
            }

            CheckBox cbActive = e.Row.FindControl("cbActive") as CheckBox;
            if (cbActive != null)
            {
                cbActive.Checked = coupon.Active;
                cbActive.Attributes.Add("couponId", coupon.Id.ToString());
            }
        }
    }

    protected void cbActive_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = sender as CheckBox;
        int couponId = Int32.Parse(cb.Attributes["couponId"]);
        Collective.Coupon.ToggleActive(couponId);
    }


    void grdCoupons_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKey dKey = grdCoupons.DataKeys[e.RowIndex];
        int id = Convert.ToInt32(dKey.Values["Id"]);

        Collective.Coupon.DeleteCoupon(id);

        FillCouponsGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillCouponsGrid();
        }
    }

    private void FillCouponsGrid()
    {
        int page = 1;
        int rows = 10;
        int totalPageCount;
        string offerName = Request.QueryString["offername"];
        string email = Request.QueryString["email"];
        string firstName = Request.QueryString["firstname"];
        string lastName = Request.QueryString["lastname"];

        if (!String.IsNullOrEmpty(Request.QueryString["page"]))
        {
            Int32.TryParse(Request.QueryString["page"], out page);
        }
        if (!String.IsNullOrEmpty(Request.QueryString["rows"]))
        {
            Int32.TryParse(Request.QueryString["rows"], out rows);
        }

        List<Collective.Coupon> lst = Collective.Coupon.PagCouponsForAdmin(offerName, email, firstName, lastName, page, rows, out totalPageCount, out page);

        grdCoupons.DataSource = lst;
        grdCoupons.DataBind();


        StringBuilder sbScript = new StringBuilder();
        sbScript.Append("var searchHref = " + "'http://" + Request.Url.Authority + "/admin/CouponsList.aspx';\n");
        
        
        ScriptManager.RegisterStartupScript(this, GetType(), "QuerryStringScript", sbScript.ToString(), true);

        this.tbOfferName.Text = offerName;
        this.tbEmail.Text = email;
        this.tbFirstName.Text = firstName;
        this.tbLastName.Text = lastName;

        CreateNavigationLinks(page, rows, totalPageCount, email, firstName, lastName);
    }

    private void CreateNavigationLinks(int currPage, int numOfRows, int totalPageCount, string email, string firstName, string lastName)
    {
        string basicUrl = "http://" + Request.Url.Authority + "/admin/CouponsList.aspx";
        string searchUrlPart = (!String.IsNullOrEmpty(email) ? "&email=" + email : string.Empty) +
            (!String.IsNullOrEmpty(firstName) ? "&firstname=" + firstName : string.Empty) + (!String.IsNullOrEmpty(lastName) ? "&lastname=" + lastName : string.Empty);

        // paging linovi
        this.hlFirst.NavigateUrl = basicUrl + "?page=" + 1.ToString() + "&rows=" + numOfRows.ToString() + searchUrlPart;
        this.hlLast.NavigateUrl = basicUrl + "?page=" + totalPageCount.ToString() + "&rows=" + numOfRows.ToString() + searchUrlPart;
        this.hlPrev.NavigateUrl = basicUrl + "?page=" + (currPage - 1).ToString() + "&rows=" + numOfRows.ToString() + searchUrlPart;
        this.hlNext.NavigateUrl = basicUrl + "?page=" + (currPage + 1).ToString() + "&rows=" + numOfRows.ToString() + searchUrlPart;

        this.hlPrev.Visible = this.hlFirst.Visible = currPage > 1;
        this.hlNext.Visible = this.hlLast.Visible = currPage < totalPageCount;


        SortedList<int, int> pags = new SortedList<int, int>();
        pags.Add(currPage, currPage);
        int pl = currPage;
        int pr = currPage;

        for (int i = 0; i < 3; ++i)
        {
            if (--pl >= 1)
            {
                pags.Add(pl, pl);
            }
            else if (++pr <= totalPageCount)
            {
                pags.Add(pr, pr);
            }

            if (++pr <= totalPageCount)
            {
                pags.Add(pr, pr);
            }
            else if (--pl >= 1)
            {
                pags.Add(pl, pl);
            }
        }

        if (pags.First().Value > 1)
        {
            phPages.Controls.Add(new Literal() { Text = "... " });
        }

        foreach (KeyValuePair<int, int> kwp in pags)
        {
            if (kwp.Value == currPage)
            {
                phPages.Controls.Add(new Literal() { Text = kwp.Value.ToString() });
            }
            else
            {
                phPages.Controls.Add(new HyperLink()
                {
                    Text = kwp.Value.ToString(),
                    NavigateUrl = basicUrl + "?page=" + kwp.Value.ToString() + "&rows=" + numOfRows.ToString() + searchUrlPart
                });
            }
            phPages.Controls.Add(new Literal() { Text = " " });
        }
        if (pags.Last().Value < totalPageCount)
        {
            phPages.Controls.Add(new Literal() { Text = " ..." });
        }
    }
}