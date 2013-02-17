using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PAB;
using System.Configuration;


public partial class admin_OfferImages : System.Web.UI.Page
{
    private string OFFER_IMAGE_UPLOAD_PATH = ConfigurationManager.AppSettings["OfferImagesPath"].ToString();
    private const int IMAGE_QUALITY = 100;
    private const int IMAGE_MAX_H = 800;
    private const int IMAGE_MAX_W = 600;

    private const string BTN_IMG_EDIT_TEXT = "Izmijeni";
    private const string BTN_IMG_SAVE_TEXT = "Spremi";

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.btnUploadNewImagePreview.Click += new EventHandler(btnUploadNewImagePreview_Click);
        this.btnSaveNewImage.Click += new EventHandler(btnSaveNewImage_Click);
        this.rptOfferImages.ItemDataBound += new RepeaterItemEventHandler(rptOfferImages_ItemDataBound);
    }

    void rptOfferImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            TextBox tbEditTitle = e.Item.FindControl("tbEditTitle") as TextBox;
            TextBox tbEditDesc = e.Item.FindControl("tbEditDesc") as TextBox;
            TextBox tbOrder = e.Item.FindControl("tbOrder") as TextBox;
            Label lblTitle = e.Item.FindControl("lblTitle") as Label;
            Label lblDesc = e.Item.FindControl("lblDesc") as Label;
            Label lblOrder = e.Item.FindControl("lblOrder") as Label;
            LinkButton lbEdit = e.Item.FindControl("lbEdit") as LinkButton;
            LinkButton lbSave = e.Item.FindControl("lbSave") as LinkButton;
            LinkButton lbDelete = e.Item.FindControl("lbDelete") as LinkButton;
            LinkButton lbCancel = e.Item.FindControl("lbCancel") as LinkButton;
            CheckBox cbActive = e.Item.FindControl("cbActive") as CheckBox;

            tbEditTitle.Visible = tbEditDesc.Visible = lbSave.Visible = lbCancel.Visible = false;
            lblTitle.Visible = lblDesc.Visible = lbEdit.Visible = lbDelete.Visible = true;

            Collective.OfferImage img = e.Item.DataItem as Collective.OfferImage;

            lblTitle.Text = tbEditTitle.Text = img.Title;
            lblDesc.Text = tbEditDesc.Text = img.Description;
            lblOrder.Text = tbOrder.Text = img.Order.ToString();

            lbEdit.Attributes.Add("rptItemIndex", e.Item.ItemIndex.ToString());

            lbSave.Attributes.Add("rptItemIndex", e.Item.ItemIndex.ToString());
            lbSave.Attributes.Add("imgId", img.ImageId.ToString());

            lbDelete.Attributes.Add("imgId", img.ImageId.ToString());
            lbDelete.Attributes.Add("imgSrc", img.Src);
            lbDelete.Attributes.Add("onclick", "javascript:return confirm('Dali ste sigurni da želite obrisati sliku: " + img.Title + "?');");

            cbActive.Checked = img.Active;
            cbActive.Enabled = false;
        }
    }

    void lbSave_Click(object sender, EventArgs e)
    {
        LinkButton lbSave = sender as LinkButton;
        RepeaterItem item = this.rptOfferImages.Items[Int32.Parse(lbSave.Attributes["rptItemIndex"])];

        TextBox tbEditTitle = item.FindControl("tbEditTitle") as TextBox;
        TextBox tbEditDesc = item.FindControl("tbEditDesc") as TextBox;
        TextBox tbOrder = item.FindControl("tbOrder") as TextBox;
        Label lblOrder = item.FindControl("lblOrder") as Label;
        CheckBox cbActive = item.FindControl("cbActive") as CheckBox;

        int order = 0;
        if (!String.IsNullOrEmpty(tbOrder.Text))
        {
            if (!Int32.TryParse(tbOrder.Text, out order))
            {
                order = Int32.Parse(lblOrder.Text);
            }
        }

        Collective.OfferImage.UpdateImage(Int32.Parse(lbSave.Attributes["imgId"]), tbEditTitle.Text, tbEditDesc.Text, order, cbActive.Checked);
        FillGrid();

        if (lblOrder.Text != order.ToString())
        {
            this.upListImages.Update();
            this.hfLastEditItemIndex.Value = this.hfEditItemIndex.Value = string.Empty;
        }
        else
        {
            this.hfLastEditItemIndex.Value = this.hfEditItemIndex.Value;
            this.hfEditItemIndex.Value = string.Empty;
        }
    }

    void lbCancel_Click(object sender, EventArgs e)
    {
        this.hfLastEditItemIndex.Value = this.hfEditItemIndex.Value;
        this.hfEditItemIndex.Value = string.Empty;
    }

    void lbEdit_Click(object sender, EventArgs e)
    {
        LinkButton lbEdit = sender as LinkButton;
        this.hfLastEditItemIndex.Value = this.hfEditItemIndex.Value;
        this.hfEditItemIndex.Value = lbEdit.Attributes["rptItemIndex"];
    }

    void lbDelete_Click(object sender, EventArgs e)
    {
        LinkButton lbDelete = sender as LinkButton;
        Collective.OfferImage.DeleteImage(Int32.Parse(lbDelete.Attributes["imgId"]));
        FillGrid();
        this.upListImages.Update();
        this.hfEditItemIndex.Value = this.hfLastEditItemIndex.Value = string.Empty;

        string imgPath = Server.MapPath(lbDelete.Attributes["imgSrc"]);
        if (File.Exists(imgPath))
        {
            File.Delete(imgPath);
        }
    }

    void btnSaveNewImage_Click(object sender, EventArgs e)
    {
        this.lblErrMsg.Text = string.Empty;

        int order = 0;
        if (!String.IsNullOrEmpty(tbNewOrder.Text))
        {
            if (!Int32.TryParse(tbNewOrder.Text, out order))
            {
                this.lblErrMsg.Text = "Redni broj mora biti broj.";
                return;
            }
        }

        object sesobj = Session["imageuploadpreview"];
        Validate("uploadimage");

        if (Page.IsValid && sesobj != null)
        {
            Session.Remove("imageuploadpreview");

            byte[] buffer = (byte[])sesobj;
            MemoryStream ms = new MemoryStream(buffer);
            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);

            var cropinfo = (from p in hfCrop.Value.Split(',')
                            select int.Parse(p)).ToArray();

            if (cropinfo[2] != 0 && cropinfo[3] != 0)
            {
                bmp = bmp.Clone(new System.Drawing.Rectangle(cropinfo[0], cropinfo[1], cropinfo[2], cropinfo[3]), bmp.PixelFormat);
            }

            ImageResizer resizer = new ImageResizer();
            resizer.OutputFormat = ImageFormat.Jpeg;
            resizer.ImgQuality = IMAGE_QUALITY;
            resizer.MaxHeight = IMAGE_MAX_H;
            resizer.MaxWidth = IMAGE_MAX_W;

            byte[] imgRes = resizer.Resize(bmp);

            bmp.Dispose();
            ms.Dispose();

            string ext = ".jpg";    // ImageResizer zapravo sve onak postavlja na jpg
            string title = this.tbNewTitle.Text.Trim();
            string desc = this.tbNewDesc.Text.Trim();
            string alt = GetOffer().OfferName + " " + title;

            Collective.OfferImage newImg = Collective.OfferImage.CreateImage(title, desc, alt, GetOffer().OfferId, order, cbNewActive.Checked,
            (OFFER_IMAGE_UPLOAD_PATH.Last() == '/' ? OFFER_IMAGE_UPLOAD_PATH : OFFER_IMAGE_UPLOAD_PATH + "/"), ext);

            string saveLocation = Server.MapPath(newImg.Src);
            if (!Directory.Exists(saveLocation)) Directory.CreateDirectory(Path.GetDirectoryName(saveLocation));

            File.WriteAllBytes(saveLocation, imgRes);

            imgThumb.Visible = false;
            this.tbNewTitle.Text = this.tbNewDesc.Text = this.tbNewOrder.Text = string.Empty;
            this.cbNewActive.Checked = false;

            FillGrid();
            this.upListImages.Update();
            this.hfEditItemIndex.Value = this.hfLastEditItemIndex.Value = string.Empty;
        }
    }

    void btnUploadNewImagePreview_Click(object sender, EventArgs e)
    {
        Session.Remove("imageuploadpreview");

        if (this.fuAddNewImage.HasFile)
        {
            var file = fuAddNewImage.PostedFile;
            string type = file.ContentType;

            if (type == "image/jpeg" || type == "image/gif" || type == "image/png")
            {
                BinaryReader br = new BinaryReader(file.InputStream);
                byte[] buffer = br.ReadBytes(file.ContentLength);
                Session.Add("imageuploadpreview", buffer);

                imgThumb.Src = String.Format("/admin/picpreview.aspx?what={0}&content={1}", "imageuploadpreview", Server.UrlEncode(type));
                imgThumb.Alt = "Nema odabrane slike";

                hfCrop.Value = string.Empty;
                imgThumb.Visible = true;
                reqNewImage.Visible = false;
                reqNewImage.Enabled = false;

                this.lblErrMsg.Text = string.Empty;
            }
            else
            {
                this.lblErrMsg.Text = "Slika mora biti u jpg, gif ili png formatu.";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Remove("imageuploadpreview");

            FillGrid();
        }

        AddRepeaterHandlers();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.btnSaveNewImage.Enabled = Session["imageuploadpreview"] != null;

        int rptItemId;  // update panel unutar repeatera koji se trenutno mjenja
        if (Int32.TryParse(this.hfEditItemIndex.Value, out rptItemId))
        {
            RepeaterItem item = this.rptOfferImages.Items[rptItemId];

            SetEditForm(item, true);
        }

        int lastRptItemId;  // update panel unutar repeatera koji se prethodno mjenjao
        if (Int32.TryParse(this.hfLastEditItemIndex.Value, out lastRptItemId))
        {
            this.hfLastEditItemIndex.Value = string.Empty;

            RepeaterItem item = this.rptOfferImages.Items[lastRptItemId];

            SetEditForm(item, false);
        }
    }

    private void SetEditForm(RepeaterItem item, bool isEdit)
    {
        TextBox tbEditTitle = item.FindControl("tbEditTitle") as TextBox;
        TextBox tbEditDesc = item.FindControl("tbEditDesc") as TextBox;
        TextBox tbOrder = item.FindControl("tbOrder") as TextBox;
        Label lblTitle = item.FindControl("lblTitle") as Label;
        Label lblDesc = item.FindControl("lblDesc") as Label;
        Label lblOrder = item.FindControl("lblOrder") as Label;
        LinkButton lbEdit = item.FindControl("lbEdit") as LinkButton;
        LinkButton lbSave = item.FindControl("lbSave") as LinkButton;
        LinkButton lbDelete = item.FindControl("lbDelete") as LinkButton;
        LinkButton lbCancel = item.FindControl("lbCancel") as LinkButton;
        CheckBox cbActive = item.FindControl("cbActive") as CheckBox;

        tbEditTitle.Visible = tbEditDesc.Visible = tbOrder.Visible = lbSave.Visible = lbCancel.Visible = isEdit;
        lblTitle.Visible = lblDesc.Visible = lblOrder.Visible = lbEdit.Visible = lbDelete.Visible = !isEdit;

        cbActive.Enabled = isEdit;

        UpdatePanel upPan = item.FindControl("UpdatePanel1") as UpdatePanel;
        upPan.Update();
    }

    private void AddRepeaterHandlers()
    {
        foreach (RepeaterItem it in this.rptOfferImages.Items)
        {
            if (it.ItemType == ListItemType.Item || it.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbEdit = it.FindControl("lbEdit") as LinkButton;
                LinkButton lbSave = it.FindControl("lbSave") as LinkButton;
                LinkButton lbDelete = it.FindControl("lbDelete") as LinkButton;
                LinkButton lbCancel = it.FindControl("lbCancel") as LinkButton;

                lbEdit.Click += new EventHandler(lbEdit_Click);
                lbCancel.Click += new EventHandler(lbCancel_Click);
                lbSave.Click += new EventHandler(lbSave_Click);
                lbDelete.Click += new EventHandler(lbDelete_Click);
            }
        }
    }

    private void FillGrid()
    {
        this.rptOfferImages.DataSource = Collective.OfferImage.ListOfferImages(GetOffer().OfferId);
        this.rptOfferImages.DataBind();
    }

    private Collective.Offer _offer;

    protected Collective.Offer GetOffer()
    {
        if (_offer == null)
        {
            int offerId;
            if (Int32.TryParse(Request.QueryString["offerid"], out offerId))
            {
                _offer = Collective.Offer.GetOffer(offerId, null);
            }
        }

        return _offer;
    }
}