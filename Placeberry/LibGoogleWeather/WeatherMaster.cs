using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LibGoogleWeather
{
    public class WeatherMaster
    {
        private const int waitTime = 20 * 1000;

        private static List<Place> places = new List<Place>();
        private static List<Forecast> forecasts = new List<Forecast>();

        public static List<Place> Places
        {
            get
            {
                return places;
            }
        }
        public static List<Forecast> Forecasts
        {
            get
            {
                return forecasts;
            }
        }


        public static void BeginFetchForecasts(List<Place> mjesta)
        {        
            //prođemo sva mjesta tako da instanciramo forecast objekta samo ako još nema ni jedne instance s istim parametrom
            foreach (var mjesto in mjesta)
            {
                var forecastold = forecasts.Where(i => i.Param == mjesto.Param);
                if (forecastold.Any())
                {
                    mjesto.Forecast = forecastold.Take(1).SingleOrDefault();
                }
                else
                {
                    Forecast forecast = new Forecast(mjesto.Id, mjesto.Title, mjesto.Param);
                    mjesto.Forecast = forecast;
                    forecasts.Add(forecast);
                }

                var placeold = places.Where(i => i.Id == mjesto.Id).Take(1).SingleOrDefault();
                if (placeold != null)
                {
                    if (placeold.Param.ToLower().Trim() != mjesto.Param.ToLower().Trim())
                    {
                        placeold.Param = mjesto.Param;
                        placeold.Forecast = mjesto.Forecast;
                    }
                    if (placeold.Title.ToLower().Trim() != mjesto.Title.ToLower().Trim())
                    {
                        placeold.Title = mjesto.Title;
                        placeold.Forecast = mjesto.Forecast;
                    }
                }
                else
                {
                    places.Add(mjesto);
                }
            }

            BeginRefreshForecasts(forecasts);
        }
        public static void BeginRefreshForecasts(List<Forecast> items)
        {
            if (items.Any())
            {
                System.Threading.Thread thread = new Thread(i =>
                {
                    //Najviše 3 pokušaja
                    List<Forecast> fails = FailSafeTry(items);

                    if (fails.Any())
                    {
                        Thread.Sleep(waitTime);
                        List<Forecast> epicFails = FailSafeTry(fails);

                        if (epicFails.Any())
                        {
                            Thread.Sleep(waitTime);
                            FailSafeTry(epicFails);
                        }
                    }

                });

                thread.Start();
            }
        }

        private static List<Forecast> FailSafeTry(List<Forecast> items)
        {
            List<Forecast> fails = new List<Forecast>();

            items.ForEach(i =>
            {
                try
                {
                    i.FetchForecast();
                }
                catch (Exception ex)
                {
                    //throw new ApplicationException(ex.Message);
                    //neznam kaj se radi s exceptionima, neki log ili mail di se to može slati?
                    fails.Add(i);
                }
            });

            return fails;
        }
    }
}
