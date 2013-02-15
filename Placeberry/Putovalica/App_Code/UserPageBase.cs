using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Security;

public abstract class UserPageBase : System.Web.UI.Page
{
    public const string USER_SESSION_NAME = "CurrentUser";

    private const int OFFER_LIST_COUNT = 20;

    private List<Collective.Offer> _lstCollOffers;
    private List<Collective.Offer> _lstLinkedOffers;
    private List<Collective.Category> _lstCats;
    private List<Collective.Place> _lstPlaces;

    private Collective.Offer _currOffer;

    protected override void InitializeCulture()
    {
        var selectedCulture = PutovalicaUtil.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    public List<Collective.Offer> ListOffers()
    {
        if (_lstCollOffers == null)
        {
            Collective.Category selcat = GetSelectedCategory();
            int? selcatid = selcat != null ? (int?)selcat.Id : (int?)ListCategories().FirstOrDefault().Id;  // da vrati prvu ako nema selektirane (kad je samo default.aspx)

            Collective.Place selpl = GetSelectedPlace();
            int? selplid = selpl != null ? (int?)selpl.Id : null;

            _lstCollOffers = Collective.Offer.ListOffersForClient(selcatid, selplid, PutovalicaUtil.GetLanguageId(), OFFER_LIST_COUNT);
        }

        return _lstCollOffers;
    }

    public List<Collective.Category> ListCategories()
    {
        if (_lstCats == null)
            _lstCats = Collective.Category.ListCategoriesForUser(PutovalicaUtil.GetLanguageId());

        return _lstCats;
    }

    public List<Collective.Place> ListPlaces()
    {
        if (_lstPlaces == null)
            _lstPlaces = Collective.Place.ListPlaces(true);

        return _lstPlaces;
    }

    public Collective.Category GetSelectedCategory()
    {
        string urlTag = Request.QueryString["cattag"];

        if (!String.IsNullOrEmpty(urlTag))
        {
            return (from c in ListCategories()
                    where c.Translation.UrlTag == urlTag
                    select c).FirstOrDefault();
        }

        return null;
    }

    //public Collective.Category GetSelectedCategory()
    //{
    //    int catid;

    //    if (Int32.TryParse(Request.QueryString["categoryid"], out catid))
    //    {
    //        return (from c in ListCategories()
    //                where c.Id == catid
    //                select c).FirstOrDefault();
    //    }

    //    return null;
    //}

    public Collective.Place GetSelectedPlace()
    {
        string placetag = Request.QueryString["pltag"];

        if (!String.IsNullOrEmpty(placetag))
        {
            return (from p in ListPlaces()
                    where p.UrlTag == placetag
                    select p).FirstOrDefault();
        }

        return null;
    }

    //public Collective.Place GetSelectedPlace()
    //{
    //    int plid;

    //    if (Int32.TryParse(Request.QueryString["placeid"], out plid))
    //    {
    //        return (from p in ListPlaces()
    //                    where p.Id == plid
    //                    select p).FirstOrDefault();
    //    }

    //    return null;
    //}

    public Collective.Offer GetCurrentOffer()
    {
        if (_currOffer == null)
        {
            int offerid;
            if (Int32.TryParse(Request.QueryString["offerid"], out offerid))
            {
                _currOffer = Collective.Offer.GetOffer(offerid, PutovalicaUtil.GetLanguageId());
            }
        }

        return _currOffer;
    }

    public void LoginCollectiveUser(MembershipUser user)
    {
        HttpContext.Current.Session[USER_SESSION_NAME] = Collective.User.GetUser(user);
    }

    public Collective.User GetLoggedCollectiveUser()
    {
        Collective.User loggedUser = HttpContext.Current.Session[USER_SESSION_NAME] as Collective.User;

        if (loggedUser == null)
        {
            if (Membership.GetUser() == null)
                return null;

            loggedUser = Collective.User.GetUser(Membership.GetUser());
            HttpContext.Current.Session[USER_SESSION_NAME] = loggedUser;
        }

        return loggedUser;
    }

    public void LogoutCollectiveUser()
    {
        HttpContext.Current.Session[USER_SESSION_NAME] = null;
    }
}