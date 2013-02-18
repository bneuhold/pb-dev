
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;



namespace UltimateDC
{

    public enum ObjectType
    {
        ISLAND = 1,
        CITY = 2,
        COUNTY = 3,
        COUNTRY = 4,
        REGION = 5,
        SUBREGION = 6,
        L = 17,
        NULL = 18,
        GROUP = 19,
        SUBGROUP = 20,
        ACCOMM = 21,
        SUBACCOMM = 22,
        VACATION = 23,
        PATTERN = 24,
        OPERATOR = 25,
        PRICE_DESC = 26,
        IGNORE = 27,
        VALUE = 28,
        CONECT = 29,
    }


    public partial class UltimateDataContext
    {

        public string GetTraslation(int? ultimateTableId, int languageId)
        {
            var result = (from p in this.UltimateTables
                          join q in this.UltimateTableTranslations on p.Id equals q.UltimateTableId into trans
                          where p.Id == ultimateTableId
                          from r in trans.Where(i=>i.LanguageId == languageId).DefaultIfEmpty()
                          select r != null ? r.Title : p.Title).SingleOrDefault();

            return result;
        }


    }
}
