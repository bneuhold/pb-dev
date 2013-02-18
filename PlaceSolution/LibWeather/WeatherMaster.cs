using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Threading;
using System.Timers;

namespace LibWeather
{
    public static class WeatherMaster
    {
        private const int waitTime = 20 * 1000;

        private static List<Place> places = new List<Place>();
        private static List<LocalWeather> localWeathers = new List<LocalWeather>();

        public static List<Place> Places
        {
            get
            {
                return places;
            }
        }
        public static List<LocalWeather> LocalWeathers
        {
            get
            {
                return localWeathers;
            }
        }


        public static void BeginFetchLocalWeathers(List<Place> gradovi)
        {
            if (places == null) places = new List<Place>();
            if (localWeathers == null) localWeathers = new List<LocalWeather>();

            if (gradovi == null ? false : gradovi.Any())
            {
                var mjesta = gradovi.Distinct().ToList();

                var parameters = (from p in mjesta
                                  select p.Parameters).Distinct().ToList();

                //Napravimo po jedan LocalWeather objekt za svaki distinct parametar
                parameters.ForEach(param =>
                {
                    if (!localWeathers.Where(i => i.Parameters == param).Any())
                    {
                        //Ovaj try-catch je dodan za svaki slučaj da se ne skrši pozivanje konstruktora (ako su parametri takvi da skrše jer se ne mogu splitati)
                        try
                        {
                            LocalWeather lw = new LocalWeather(param);
                            localWeathers.Add(lw);
                        }
                        catch (Exception)
                        {
                            //throw new ArgumentException(String.Format("Pozvan konstruktor sa neispravnim parametrima: ", param));
                            //neznam kaj se radi s exceptionima, neki log ili mail di se to može slati?
                            throw;
                        }
                    }
                });

                //Napravimo po jedan Place objekt za svaki grad/mjesto i pridružimo mu njegovu prognozu
                mjesta.ForEach(mjesto =>
                {
                    if (!places.Where(i => i.Id == mjesto.Id).Any())
                    {
                        mjesto.LocalWeather = localWeathers.SingleOrDefault(i => i.Parameters == mjesto.Parameters);
                        if (mjesto.LocalWeather != null)
                        {
                            places.Add(mjesto);
                        }
                    }
                    else
                    {
                        if (mjesto.LocalWeather == null)
                        {
                            mjesto.LocalWeather = localWeathers.SingleOrDefault(i => i.Parameters == mjesto.Parameters);
                        }
                        else if (mjesto.LocalWeather.Parameters != mjesto.Parameters)
                        {
                            mjesto.LocalWeather = localWeathers.SingleOrDefault(i => i.Parameters == mjesto.Parameters);
                        }

                        //Ako je i dalje null na neku foru onda maknem to mjesto iz popisa mjesta koja imaju prognoze
                        if (mjesto.LocalWeather == null)
                        {
                            places.RemoveAll(i => i.Id == mjesto.Id);
                        }
                    }
                });
            }

            BeginRefreshLocalWeathers(localWeathers);
        }
        public static void BeginRefreshLocalWeathers(List<LocalWeather> items)
        {
            if (items.Any())
            {
                System.Threading.Thread thread = new Thread(i =>
                {
                    //Najviše 3 pokušaja
                    List<LocalWeather> fails = FailSafeTry(items);

                    if (fails.Any())
                    {
                        Thread.Sleep(waitTime);
                        List<LocalWeather> epicFails = FailSafeTry(fails);

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

        private static List<LocalWeather> FailSafeTry(List<LocalWeather> items)
        {
            List<LocalWeather> fails = new List<LocalWeather>();

            items.ForEach(i =>
            {
                try
                {
                    i.FetchWeather();
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
