using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using UltimateDC;
using System.Web.Script.Services;

/// <summary>
/// Summary description for PlaceInteractionService
/// </summary>
[WebService(Namespace = "http://www.placeberry.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PlaceInteractionService : System.Web.Services.WebService
{

    public PlaceInteractionService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod]
    public IEnumerable SuggestCountry(string term, int maxresults)
    {
        IEnumerable countries = null;
        term = term.ToLower();

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            countries = (from p in dc.UltimateTables
                         where p.ObjectTypeId == (int)ObjectType.COUNTRY && p.Title.ToLower().Contains(term)
                         orderby p.Title ascending
                         select new
                         {
                             Id = p.Id,
                             Title = p.Title
                         }).Take(maxresults).ToList();
        }

        return countries;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public IEnumerable SuggestCity(string term, int maxresults)
    {
        IEnumerable cities = null;
        term = term.ToLower();

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            cities = (from p in dc.UltimateTables
                      where p.UltimateTableObjectType.Id == (int)ObjectType.CITY && p.Title.ToLower().Contains(term)
                      orderby p.Title ascending
                      select new
                      {
                          Id = p.Id,
                          Title = p.Title
                      }).Take(maxresults).ToList();
        }

        return cities;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public IEnumerable GetParentCities(int countryId, int? languageId)
    {
        IEnumerable cities = null;

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            cities = (from p in dc.GetParentCities(countryId, languageId)
                      orderby p.ChildTitle ascending
                      select new
                      {
                          Id = p.ChildId.Value,
                          Title = p.ChildTitle,
                          DisplayTitle = String.Format("{0}, {1}", p.ChildTitle, p.ParentTitle)
                      }).ToList();
        }

        return cities;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public IEnumerable GetParents(string childTitle, int? childId, int? languageId)
    {
        IEnumerable parents = null;

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            parents = (from p in dc.GetParents(null, childId, languageId)
                       orderby p.ParentTitle ascending
                       select new
                       {
                           Id = p.ParentId,
                           Title = p.ParentTitle,
                           Type = p.ObjectTypeCode
                       }).ToList();
        }

        return parents;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public IEnumerable GetChildrenByType(int? parentId, int? childTypeId, int? languageId)
    {
        IEnumerable children = null;

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            children = (from p in dc.GetParentChildsByType(parentId, childTypeId, languageId)
                       orderby p.ParentTitle ascending
                       select new
                       {
                           Id = p.ChildId,
                           Title = p.ChildTitle
                       }).ToList();
        }

        return children;
    }

}
