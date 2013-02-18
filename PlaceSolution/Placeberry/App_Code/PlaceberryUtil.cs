using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

public static class PlaceberryUtil
{
    public static string GetConnectionString()
    {
        //System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/Placeberry");        
        //return rootWebConfig.ConnectionStrings.ConnectionStrings["Placeberry_CS"].ConnectionString;

        return ConfigurationManager.ConnectionStrings["Placeberry_CS"].ConnectionString;
    }
}