using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Threading;
using System.Globalization;


namespace LibWeather
{
    public class LocalWeather : IEqualityComparer<LocalWeather>
    {
        //Primjer linka
        //accuweather.com/en-gb/hr/dubrovacko-neretvanska/dubrovnik/forecast.aspx?cityid=112593
        private const int WAIT_TIME = 20 * 1000;

        private string urlWeekThis = "http://www.accuweather.com/en-gb/hr/{0}/{1}/forecast.aspx?{2}";
        private string urlWeekNext = "http://www.accuweather.com/en-gb/hr/{0}/{1}/forecast2.aspx?{2}";
        private string urlHourly = "http://www.accuweather.com/en-gb/hr/{0}/{1}/hourly.aspx?{2}";
        private DateTime timeFetched;

        private string parameters;
        private List<Weather> allWeather;

        public string Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }
        public DateTime TimeFetched
        {
            get
            {
                return this.timeFetched;
            }
        }
        public Weather TodaysWeather
        {
            get
            {
                return this.allWeather.SingleOrDefault(i => i.Date.Date == DateTime.Today);
            }
        }
        public List<Weather> WeekThis
        {
            get
            {
                return this.allWeather.Take(7).ToList();
            }
        }
        public List<Weather> WeekNext
        {
            get
            {
                return this.allWeather.Skip(7).Take(7).ToList();
            }
        }
        public List<Weather> AllWeather
        {
            get { return this.allWeather; }
        }

        /// <summary>
        /// Kontruktor
        /// </summary>
        /// <param name="place">Naziv mjesta</param>
        /// <param name="parameters">Parametri oblika dubrovacko-neretvanska;dubrovnik;cityid=112593</param>
        /// <returns></returns>
        /// 
        public LocalWeather(string parameters)
        {
            //accuweather.com/en-gb/hr/dubrovacko-neretvanska/dubrovnik/quick-look.aspx?cityid=112593

            this.parameters = parameters;

            var param = this.parameters.Split(';');

            this.urlWeekThis = string.Format(urlWeekThis, param[0], param[1], param[2]);
            this.urlWeekNext = string.Format(urlWeekNext, param[0], param[1], param[2]);
            this.urlHourly = string.Format(urlHourly, param[0], param[1], param[2]);

            allWeather = new List<Weather>();
        }


        public void FetchWeather()
        {
            GetWeather();
        }


        private void GetWeather()
        {
            Page pageWeekThis = new Page { Url = urlWeekThis };
            //Page pageWeekNext = new Page { Url = urlWeekNext };
            Page pageHourly = new Page { Url = urlHourly };

            ManualResetEvent doneWeekThis = new ManualResetEvent(false);
            //ManualResetEvent doneWeekNext = new ManualResetEvent(false);
            ManualResetEvent doneHourly = new ManualResetEvent(false);

            GetHtml(doneWeekThis, pageWeekThis);
            //GetHtml(doneWeekNext, pageWeekNext);
            GetHtml(doneHourly, pageHourly);

            doneWeekThis.WaitOne();
            //doneWeekNext.WaitOne();
            doneHourly.WaitOne();

            DoRegexGetWeekWeather(pageWeekThis.Html, allWeather);
            //DoRegexGetWeekWeather(pageWeekNext.Html, allWeather);
            DoRegexGetHourlyWeather(pageHourly.Html);

            timeFetched = DateTime.Now;
        }

        private void DoRegexGetWeekWeather(string input, List<Weather> weekWeather)
        {
            Regex clear = new Regex(@"<div >\s*<div class=""fltLeft"" >(?<day>.*?)<div class=""fltLeft"" >(?<night>.*?)<div class=""fltLeft"" >", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex day = new Regex(@"<div style=""margin-bottom: 10px;"" >.*?<div style=""clear: both;"" ></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Regex regDate = new Regex(@"<[^>]*lblDate"">[a-z ]*?(?<value>[0-9/]*)</span>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex regIcon = new Regex(@"<[^>]*imgIcon[^>]*src=""(?<value>[^""]*)""", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex regDesc = new Regex(@"<[^>]*lblDesc"">(?<value>[^<]*)</span>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex regFeel = new Regex(@"<[^>]*lblRealFeelValue"">(?<value>[^<]*)</span>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex regTemp = new Regex(@"<[^>]*lblHigh"">(?<value>[^<]*)</span>", RegexOptions.Singleline | RegexOptions.IgnoreCase);


            Match daynight = clear.Match(input);

            if (daynight.Success)
            {
                string daysGroup = daynight.Groups["day"].Value;
                string nightsGroup = daynight.Groups["night"].Value;

                MatchCollection days = day.Matches(daysGroup);
                MatchCollection nights = day.Matches(nightsGroup);

                if (days.Count > 0 && nights.Count > 0)
                {
                    foreach (Match m in days)
                    {
                        Match mDate = regDate.Match(m.Value);
                        Match mIcon = regIcon.Match(m.Value);
                        Match mDesc = regDesc.Match(m.Value);
                        Match mFeel = regFeel.Match(m.Value);
                        Match mTemp = regTemp.Match(m.Value);

                        DateTime date = DateTime.ParseExact(mDate.Groups["value"].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        Weather weather = weekWeather.SingleOrDefault(i => i.Date == date);
                        if (weather == null)
                        {
                            weather = new Weather
                            {
                                Date = date,
                                IconDay = mIcon.Groups["value"].Value,
                                DescriptionDay = mDesc.Groups["value"].Value,
                                RealFeelDay = mFeel.Groups["value"].Value,
                                TemperatureDay = mTemp.Groups["value"].Value
                            };
                            weekWeather.Add(weather);
                        }
                        else
                        {
                            weather.IconDay = mIcon.Groups["value"].Value;
                            weather.DescriptionDay = mDesc.Groups["value"].Value;
                            weather.RealFeelDay = mFeel.Groups["value"].Value;
                            weather.TemperatureDay = mTemp.Groups["value"].Value;
                        }
                    }

                    foreach (Match m in nights)
                    {
                        Match mDate = regDate.Match(m.Value);
                        Match mIcon = regIcon.Match(m.Value);
                        Match mDesc = regDesc.Match(m.Value);
                        Match mFeel = regFeel.Match(m.Value);
                        Match mTemp = regTemp.Match(m.Value);

                        DateTime date = DateTime.ParseExact(mDate.Groups["value"].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        Weather weather = weekWeather.SingleOrDefault(i => i.Date == date);
                        if (weather == null)
                        {
                            weather = new Weather
                            {
                                Date = date,
                                IconNight = mIcon.Groups["value"].Value,
                                DescriptionNight = mDesc.Groups["value"].Value,
                                RealFeelNight = mFeel.Groups["value"].Value,
                                TemperatureNight = mTemp.Groups["value"].Value
                            };
                            weekWeather.Add(weather);
                        }
                        else
                        {
                            weather.IconNight = mIcon.Groups["value"].Value;
                            weather.DescriptionNight = mDesc.Groups["value"].Value;
                            weather.RealFeelNight = mFeel.Groups["value"].Value;
                            weather.TemperatureNight = mTemp.Groups["value"].Value;
                        }
                    }

                    weekWeather = (from p in weekWeather
                                   orderby p.Date ascending
                                   select p).ToList();

                }
                else throw new ApplicationException("No matches for regex 'day'");

            }
            else throw new ApplicationException("No matches for regex 'clear'");



        }
        private void DoRegexGetHourlyWeather(string input)
        {
            Regex clear = new Regex(@"<div id=""hbhBox"">(?<value>.*?)<div id=""hbhSevereWxPotetial"">", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowHour = new Regex(@"<!-- Hourly Labels /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowIcon = new Regex(@"<!-- Weather Icons /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowDesc = new Regex(@"<!-- Weather Text /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowTemp = new Regex(@"<!-- Temperature /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowFeel = new Regex(@"<!-- RealFeel /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowHumi = new Regex(@"<!-- Humidity /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowDewP = new Regex(@"<!-- Dew Point /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowWDir = new Regex(@"<!-- Wind Direction /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rowWind = new Regex(@"<!-- Wind Speed /-->.*?<div class=""hbhItemLabel"">(?<value>.*?)<div class=""clear""></div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Regex rowValues = new Regex(@"<div class=""hbhItem[^""]*"">\s*(?<value>.*?)\s*</div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Match clearMatch = clear.Match(input);

            if (clearMatch.Success)
            {
                string clearedInput = clearMatch.Groups["value"].Value;

                string[] hours = GetRowValues(rowHour, rowValues, clearedInput);
                string[] descs = GetRowValues(rowDesc, rowValues, clearedInput);
                string[] temps = GetRowValues(rowTemp, rowValues, clearedInput);
                string[] feels = GetRowValues(rowFeel, rowValues, clearedInput);
                string[] humis = GetRowValues(rowHumi, rowValues, clearedInput);
                string[] dewPs = GetRowValues(rowDewP, rowValues, clearedInput);
                string[] wDirs = GetRowValues(rowWDir, rowValues, clearedInput);
                string[] winds = GetRowValues(rowWind, rowValues, clearedInput);

                var todayWeather = (from p in allWeather
                                    where p.Date == DateTime.Today
                                    select p).SingleOrDefault();
                if (todayWeather == null)
                {
                    todayWeather = new Weather { Date = DateTime.Today };
                    allWeather.Add(todayWeather);
                }

                var todayHourly = todayWeather.HourlyWeather;

                int na = hours.Count();
                for (int i = 0; i < na; i++)
                {
                    DateTime time;
                    if (DateTime.TryParseExact(hours[i], "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out time))
                    {
                        time = new DateTime(todayWeather.Date.Year, todayWeather.Date.Month, todayWeather.Date.Day, time.Hour, time.Minute, time.Second);

                        var hourWeather = todayHourly.SingleOrDefault(th => th.Hour == time);
                        if (hourWeather == null)
                        {
                            todayHourly.Add(new HourlyWeather
                            {
                                Hour = time,
                                Description = descs[i],
                                Temperature = temps[i],
                                RealFeel = feels[i],
                                Humidity = humis[i],
                                WindDirection = wDirs[i],
                                WindSpeed = winds[i]
                            });
                        }
                        else
                        {
                            hourWeather.Description = descs[i];
                            hourWeather.Temperature = temps[i];
                            hourWeather.RealFeel = feels[i];
                            hourWeather.Humidity = humis[i];
                            hourWeather.WindDirection = wDirs[i];
                            hourWeather.WindSpeed = winds[i];
                        }
                    }
                }


                int now = DateTime.Now.Hour;

                todayHourly = (from p in todayHourly
                               where p.Hour.Hour >= now
                               orderby p.Hour ascending
                               select p).ToList();


            }

            else throw new ApplicationException("No matches for regex 'clear' in DoRegexGetHourlyWeather");


        }
        private string[] GetRowValues(Regex regex, Regex valuesRegex, string input)
        {
            Match match = regex.Match(input);
            if (match.Success)
            {
                MatchCollection matches = valuesRegex.Matches(match.Groups["value"].Value);
                if (matches.Count > 0)

                    return (from Match p in matches
                            select p.Groups["value"].Value).ToArray();

                else throw new ApplicationException(String.Format("No matches for regex:  {0}", valuesRegex.ToString()));
            }
            else throw new ApplicationException(String.Format("No matches for regex:  {0}", regex.ToString()));
        }
        private void GetHtml(ManualResetEvent done, Page page)
        {
            HttpWebRequest request = HttpWebRequest.Create(page.Url) as HttpWebRequest;

            IAsyncResult requestAR = request.BeginGetResponse(i =>
            {
                Page p = i.AsyncState as Page;
                var response = request.EndGetResponse(i);

                StreamReader reader = new StreamReader(response.GetResponseStream());
                p.Html = reader.ReadToEnd();

                reader.Close();
                response.Close();

                done.Set();

            }, page);

            ThreadPool.RegisterWaitForSingleObject(requestAR.AsyncWaitHandle, (state, timedOut) =>
            {
                if (timedOut)
                {
                    HttpWebRequest r = state as HttpWebRequest;
                    if (r != null)
                    {
                        r.Abort();
                    }
                }

            }, request, WAIT_TIME, true);
        }



        public bool Equals(LocalWeather left, LocalWeather right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            return left.parameters == right.parameters;
        }

        public int GetHashCode(LocalWeather lw)
        {
            return (lw.parameters).GetHashCode();
        }
    }



}


