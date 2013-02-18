using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class admin_OfferTranslationCreateEdit : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
    }

    void btnSubmit_Click(object sender, EventArgs e)
    {
        this.lblErrMsg.Text = this.lblSuccessMsg.Text = string.Empty;


        if (tbContentShort.Text.Length > 250)
        {
            this.lblErrMsg.Text = "Content Short nesmije sadržavati više od 250 znakova.";
            return;
        }

        string urlTag = tbUrlTag.Text.Trim();

        if (String.IsNullOrEmpty(urlTag))
        {
            lblErrMsg.Text = "Obavezan unos UrlTag-a";
            return;
        }

        if (!Regex.Match(urlTag, @"^[a-zA-Z\s\-]+$", RegexOptions.IgnoreCase).Success)
        {
            lblErrMsg.Text = "URL TAG sadrži nedopuštene znakove.";
            return;
        }

        urlTag = (urlTag.Trim()).Replace(' ', '-');

        tbUrlTag.Text = urlTag;

        int langid;
        if (Int32.TryParse(Request.QueryString["langid"], out langid))
        {
            Collective.OfferTranslation trans = Collective.OfferTranslation.UpdateOfferTranslaton(GetOffer().OfferId, langid,
                tbTitle.Text, tbContentShort.Text, ckeContentText.Text, ckeReservationText.Text, tbMetaDesc.Text, tbMetaKeywords.Text, urlTag);

            if (trans == null)
            {
                this.lblErrMsg.Text = "Došlo do greške prilikom izmjene prijevoda.";
            }
            else
            {
                this.lblSuccessMsg.Text = "Prijevod uspješno izmijenjen.";
            }
        }
        else
        {
            Collective.OfferTranslation trans = Collective.OfferTranslation.CreateOfferTranslaton(GetOffer().OfferId, Int32.Parse(this.ddlLaguages.SelectedValue),
                tbTitle.Text, tbContentShort.Text, ckeContentText.Text, ckeReservationText.Text, tbMetaDesc.Text, tbMetaKeywords.Text, urlTag);

            if (trans == null)
            {
                this.lblErrMsg.Text = "Došlo do greške prilikom kreiranja prijevoda. Dali prijevod za ovaj jezik već postoji?";
            }
            else
            {
                this.lblSuccessMsg.Text = "Prijevod uspješno kreiran.";
                Response.Redirect("http://" + Request.Url.Authority + "/admin/OfferTranslationCreateEdit.aspx?offerid=" + trans.OfferId.ToString() + "&langid=" + trans.LanguageId.ToString() + "&createmsg=success");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (GetOffer() == null)
            {
                this.lblErrMsg.Text = "Pogrešno uneseni parametri.";
                return;
            }

            this.lblOfferName.Text = GetOffer().OfferName;

            int langid;
            if (Int32.TryParse(Request.QueryString["langid"], out langid))
            {
                // UREDIVAJE

                if (Request.QueryString["createmsg"] == "success")
                {
                    this.lblSuccessMsg.Text = "Prijevod uspješno kreiran.";
                }

                lblOperationName.Text = "Uređivanje prijevoda.";
                if (GetOfferTranslation() == null)
                {
                    this.lblErrMsg.Text = "Pogrešno uneseni parametri.";
                    return;
                }

                this.phCreate.Visible = false;
                this.phEdit.Visible = true;

                Collective.Language currLang = (from l in GetLanguages()
                                                where l.Id == langid
                                                select l).FirstOrDefault();

                this.lblLanguage.Text = currLang.Title;

                Collective.OfferTranslation trans = GetOfferTranslation();

                this.tbTitle.Text = trans.Title;
                this.tbContentShort.Text = trans.ContentShort;
                this.ckeContentText.Text = trans.ContentText;
                this.ckeReservationText.Text = trans.ReservationText;
                this.tbMetaDesc.Text = trans.MetaDesc;
                this.tbMetaKeywords.Text = trans.MetaKeywords;
                this.tbUrlTag.Text = trans.UrlTag;
            }
            else
            {
                // KREIRANJE

                lblOperationName.Text = "Dodavanje prijevoda.";

                this.phCreate.Visible = true;
                this.phEdit.Visible = false;

                foreach (Collective.Language lang in GetLanguages())
                {
                    this.ddlLaguages.Items.Add(new ListItem(lang.Title, lang.Id.ToString()));
                }
            }

        }
    }


    private Collective.Offer _offer;

    protected Collective.Offer GetOffer()
    {
        if (_offer == null)
        {
            int offerid;
            if (Int32.TryParse(Request.QueryString["offerid"], out offerid))
            {
                _offer = Collective.Offer.GetOffer(offerid, null);
            }
        }

        return _offer;
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

    private Collective.OfferTranslation _offerTranslation;

    private Collective.OfferTranslation GetOfferTranslation()
    {
        if (_offerTranslation == null)
        {
            int langid;
            if (Int32.TryParse(Request.QueryString["langid"], out langid) && GetOffer() != null)
            {
                _offerTranslation = Collective.OfferTranslation.GetOfferTranslation(GetOffer().OfferId, langid);
            }
        }

        return _offerTranslation;
    }
}