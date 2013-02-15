using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using FuzzyStrings;

namespace Migration
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = @"mssql4.mojsite.com,1555";
            string database = @"mrihr_Placeberry_dev";
            string username = @"mrihr_User";
            string password = @"mriusr123";
            string cityFilePath = @"C:\Users\matija\Documents\Visual Studio 2012\Projects\cities.txt";

            using (SqlConnection conn = CreateConnection(server, database, username, password))
            {
                conn.Open();

                //Console.WriteLine("Creating tables..");
                //CreateTables(conn);

                //Console.WriteLine("Populating term type..");
                //Dictionary<int, int> termIdMap = PopulateTermType(conn);

                //Console.WriteLine("Populating geographical data..");
                //PopulateGeoplace(conn, cityFilePath, termIdMap);

                //Console.WriteLine("Populating constraint terms..");
                //PopulateConstraintTerms(conn, termIdMap);

                using (SqlConnection connUpdate = CreateConnection(server, database, username, password))
                {
                    connUpdate.Open();
                    ParseAdverts(conn, connUpdate);
                    connUpdate.Close();
                }

                conn.Close();
            }

            Console.WriteLine("Done!");
            Console.Read();
        }

        private static void ParseAdverts(SqlConnection conn, SqlConnection connUpdate)
        { 
            String strcmd = @"SELECT Id, Title, Regex, LanguageId, SearchTermTypeId, SearchGeoplaceId FROM SearchTerm 
                                WHERE (SearchTermTypeId = 12 OR SearchTermTypeId in (1, 2, 3, 4, 5, 6, 7) AND SearchGeoPlaceId IS NOT NULL) AND Regex IS NOT NULL AND LanguageId IN (1,2)";
            SqlCommand sqlcmd = new SqlCommand(strcmd, conn);
            SqlDataReader reader = sqlcmd.ExecuteReader();

            Dictionary<int, List<Tuple<Regex, string, int, int>>> geoTerms = new Dictionary<int, List<Tuple<Regex, string, int, int>>>();
            Dictionary<int, List<Tuple<int, Regex>>> accTerms = new Dictionary<int, List<Tuple<int, Regex>>>();

            for (int i = 1; i <= 2; ++i)
            {
                geoTerms[i] = new List<Tuple<Regex, string, int, int>>();
                accTerms[i] = new List<Tuple<int, Regex>>();
            }

            while (reader.Read())
            {
                int id = Int32.Parse(reader["Id"].ToString());
                string title = reader["Title"].ToString();
                Regex regex = new Regex(reader["Regex"].ToString());
                int lid = Int32.Parse(reader["LanguageId"].ToString());
                int tid = Int32.Parse(reader["SearchTermTypeId"].ToString());

                if (tid == 12)
                {
                    accTerms[lid].Add(new Tuple<int, Regex>(id, regex));
                }
                else
                {
                    int gid = Int32.Parse(reader["SearchGeoplaceId"].ToString());
                    geoTerms[lid].Add(new Tuple<Regex, string, int, int>(regex, title, tid, gid));
                }
            }

            reader.Close();
            
            strcmd = @"SELECT Id, AccommodationType, Country, Region, Island, City, LanguageId FROM Advert WHERE (Country LIKE 'Hrvatska' OR Country LIKE 'Croatia') AND LanguageId IN (1, 2)";
            sqlcmd = new SqlCommand(strcmd, conn);
            reader = sqlcmd.ExecuteReader();

            while (reader.Read())
            {
                int id = Int32.Parse(reader["Id"].ToString());
                int lid = Int32.Parse(reader["LanguageId"].ToString());

                int aid = -1;
                Object tmp = reader["AccommodationType"];
                if (tmp != DBNull.Value && !"".Equals((string)tmp.ToString()))
                {
                    string acc = tmp.ToString().ToLower();
                    foreach (var t in accTerms[lid])
                    {
                        if (t.Item2.IsMatch(acc))
                        {
                            aid = t.Item1;
                            break;
                        }
                    }
                }

                int gid = -1;
                string[] red = new string[4] { "City", "Island", "Region", "Country" };
                int[] rad = new int[4] { 2, 1, 5, 4 };

                for (int i = 0; i < 4 && gid == -1; ++i)
                {
                    tmp = reader[red[i]];
                    if (tmp == DBNull.Value || "".Equals((string)tmp.ToString())) continue;
                    string geo = tmp.ToString().ToLower();

                    int bind = -1, bval = -1;
                    foreach (var t in geoTerms[lid])
                    {
                        if (rad[i] == t.Item3 && t.Item1.IsMatch(geo))
                        {
                            int val = LevenshteinDistanceExtensions.LevenshteinDistance(geo, t.Item2);
                            if (bind == -1 || val < bval)
                            {
                                val = bval;
                                bind = t.Item4;
                            }
                        }
                    }

                    if (bind != -1)
                    {
                        gid = bind;
                    }
                }

                if (aid != -1 || gid != -1)
                {
                    string acc = (aid == -1 ? "NULL" : aid.ToString());
                    string geo = (gid == -1 ? "NULL" : gid.ToString());
                    strcmd = String.Format(@"INSERT INTO SearchAdvertInfo (AdvertId, AccommodationTermId, GeoplaceId) VALUES ({0}, {1}, {2})", id, acc, geo);
                    sqlcmd = new SqlCommand(strcmd, connUpdate);
                    sqlcmd.ExecuteNonQuery();
                }
            }

            reader.Close();
        }

        private static Tuple<string, string, int, int, string, int> CreateTuple(string a, string b, int c, int d, string e, int f)
        {
            return new Tuple<string, string, int, int, string, int>(a, b, c, d, e, f);
        }

        private static void PopulateConstraintTerms(SqlConnection conn, Dictionary<int,int> termIdMap)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"INSERT INTO SearchTerm (Title, Regex, LanguageId, SearchTermTypeId, ParseTag, UltimateTableId) VALUES ");

            List<Tuple<string, string, int, int, string, int>> list = new List<Tuple<string,string,int,int,string,int>>();

            // CAPACITY
            list.Add(CreateTuple(@"za x osoba", @"^((za|max|do)\s+)?(?<max>[1-9][0-9]*)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_X_PEOPLE", 701));
            list.Add(CreateTuple(@"for x persons", @"^((for|up to|max)\s+)?(?<max>[1-9][0-9]*)\s+(persons?|people|men|man|women|woman)$", 2, 32, @"CAPACITY_X_PEOPLE", 701));

            list.Add(CreateTuple(@"za jednu osobu", @"^((za|max|do)\s+)?(?<max>jedn[uae]|jednoga?|jednom|jedan)\s+(osob[uae]|[čc]ovjeka?)$", 1, 32, @"CAPACITY_1_PERSON", 706));
            list.Add(CreateTuple(@"for one person", @"^((for|up\s+to|max)\s+)?(?<max>one)\s+(person|man|woman|guy|dude)$", 2, 32, @"CAPACITY_1_PERSON", 706));

            list.Add(CreateTuple(@"za dvoje ljudi", @"^((za|max|do)\s+)?(?<max>dva|dvoje|dvojicu|dvije)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_2_PEOPLE", 705));
            list.Add(CreateTuple(@"for two people", @"^((for|up\s+to|max)\s+)?(?<max>two)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_2_PEOPLE", 705));

            list.Add(CreateTuple(@"za troje ljudi", @"^((za|max|do)\s+)?(?<max>tri|troje|trojicu)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_3_PEOPLE", -1));
            list.Add(CreateTuple(@"for three people", @"^((for|up\s+to|max)\s+)?(?<max>three)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_3_PEOPLE", -1));

            list.Add(CreateTuple(@"za cetvero ljudi", @"^((za|max|do)\s+)?(?<max>(c|č|ch)etiri|(c|č|ch)etri|(c|č|ch)etvero|(c|č|ch)etvoro|(c|č|ch)etvoricu)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_4_PEOPLE", -1));
            list.Add(CreateTuple(@"for four people", @"^((for|up\s+to|max)\s+)?(?<max>four)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_4_PEOPLE", -1));

            list.Add(CreateTuple(@"za petero ljudi", @"^((za|max|do)\s+)?(?<max>pet|petero|petoro|petoricu)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_5_PEOPLE", -1));
            list.Add(CreateTuple(@"for five people", @"^((for|up\s+to|max)\s+)?(?<max>five)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_5_PEOPLE", -1));

            list.Add(CreateTuple(@"za sest ljudi", @"^((za|max|do)\s+)?(?<max>(s|š|sh)est|(s|š|sh)estero|(s|š|sh)estoricu|(s|š|sh)estoro)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_6_PEOPLE", -1));
            list.Add(CreateTuple(@"for six people", @"^((for|up\s+to|max)\s+)?(?<max>six)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_6_PEOPLE", -1));

            list.Add(CreateTuple(@"za sedam ljudi", @"^((za|max|do)\s+)?(?<max>sedam|sedmero|sedmoricu)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_7_PEOPLE", -1));
            list.Add(CreateTuple(@"for seven people", @"^((for|up\s+to|max)\s+)?(?<max>seven)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_7_PEOPLE", -1));

            list.Add(CreateTuple(@"za osam ljudi", @"^((za|max|do)\s+)?(?<max>osam|osmero|osmoricu)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_8_PEOPLE", -1));
            list.Add(CreateTuple(@"for eight people", @"^((for|up\s+to|max)\s+)?(?<max>eight)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_8_PEOPLE", -1));

            list.Add(CreateTuple(@"za devet ljudi", @"^((za|max|do)\s+)?(?<max>devet|devetoricu|devetoro|devetero)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY_9_PEOPLE", -1));
            list.Add(CreateTuple(@"for nine people", @"^((for|up\s+to|max)\s+)?(?<max>nine)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_9_PEOPLE", -1));

            list.Add(CreateTuple(@"za deset ljudi", @"^((za|max|do)\s+)?(?<max>deset|desetoricu|desetoro|desetero)\s+(osob[aue]|[čc]ovjeka?|ljudi)$", 1, 32, @"CAPACITY10_PEOPLE", -1));
            list.Add(CreateTuple(@"for ten people", @"^((for|up\s+to|max)\s+)?(?<max>ten)\s+(people|m[ea]n|wom[ea]n|persons?)$", 2, 32, @"CAPACITY_10_PEOPLE", -1));

            // PRICE
            list.Add(CreateTuple(@"Do n kuna, eura, dolara...", @"^((do|max|maksimalno|maks|najvi[šs]e|ne vi[šs]e od)\s+)?(?<max>[1-9][0-9]*([.,][0-9][0-9]?)?)\s*(?<unit>(kun[ae]|kn|hr?kn|dolara?|[$]|eur[ao]|eur|ojra|funt[iae]|£))$", 1, 33, @"PRICE_MAX_X", 315));
            list.Add(CreateTuple(@"Up to n kuna, eura, dolara...", @"^((up\s+to|max)\s+)?(?<max>[1-9][0-9]*([.,][0-9][0-9]?)?)\s*(?<unit>(kun[ae]s?|kn|hr?kn|dolars?|bucks?|[$]|euros?|eur|pounds?|quids?|£))$", 2, 33, @"PRICE_MAX_X", 315));

            list.Add(CreateTuple(@"Od n kuna", @"^od\s+(?<min>[1-9][0-9]*([.,][0-9][0-9]?)?)\s*(?<unit>(kun[ae]|kn|hr?kn|dolara?|[$]|eur[ao]|eur|[€]|ojra|funt[iae]|£))$", 1, 33, @"PRICE_MIN_X", 320));
            list.Add(CreateTuple(@"from n kuna", @"^from\s+(?<min>[1-9][0-9]*([.,][0-9][0-9]?)?)\s*(?<unit>(kun[ae]s?|kn|hr?kn|dolars?|bucks?|[$]|euros?|eur|[€]|pounds?|quids?|£))$", 2, 33, @"PRICE_MIN_X", 320));
            
            list.Add(CreateTuple(@"Od x do y kuna, eura, dolara...", @"^od\s+(?<min>[1-9][0-9]*([.,][0-9][0-9]?)?)\s*((do|max|maksimalno|maks|najvi[šs]e|ne vi[šs]e od)\s+)?(?<max>[1-9][0-9]*([.,][0-9][0-9]?)?)\s*(?<unit>(kun[ae]|kn|hr?kn|dolara?|[$]|eur[ao]|eur|ojra|funt[iae]|£))$", 1, 33, @"PRICE_MIN_X_MAX_Y", -1));
            list.Add(CreateTuple(@"From x to y kuna, eura, dolara...", @"^from\s+(?<min>[1-9][0-9]*([.,][0-9][0-9]?)?)\s*((up\s+to|max)\s+)?(?<max>[1-9][0-9]*([.,][0-9][0-9]?)?)\s*(?<unit>(kun[ae]s?|kn|hr?kn|dolars?|bucks?|[$]|euros?|eur|pounds?|quids?|£))$", 2, 33, @"PRICE_MIN_X_MAX_Y", -1));

            // DATE
            list.Add(CreateTuple(@"U siječnju", @"^u\s+(?<mth>si?je(c|č|ch)nju)$", 1, 34, @"DATE_JAN", 322));
            list.Add(CreateTuple(@"U veljači", @"^u\s+(?<mth>velja(c|č|ch)i)$", 1, 34, @"DATE_FEB", 323));
            list.Add(CreateTuple(@"u ožujku", @"^u\s+(?<mth>o[zž]ujku)$", 1, 34, @"DATE_MAR", 324));
            list.Add(CreateTuple(@"U travnju", @"^u\s+(?<mth>travnju)$", 1, 34, @"DATE_APR", 325));
            list.Add(CreateTuple(@"U svibnju", @"^u\s+(?<mth>svibnju)$", 1, 34, @"DATE_MAY", 326));
            list.Add(CreateTuple(@"U lipnju", @"^u\s+(?<mth>lipnju)$", 1, 34, @"DATE_JUN", 327));
            list.Add(CreateTuple(@"U srpnju", @"^u\s+(?<mth>srpnju)$", 1, 34, @"DATE_JUL", 328));
            list.Add(CreateTuple(@"U kolovozu", @"^u\s+(?<mth>kolovozu)$", 1, 34, @"DATE_AUG", 329));
            list.Add(CreateTuple(@"U rujnu", @"^u\s+(?<mth>rujnu)$", 1, 34, @"DATE_SEP", 330));
            list.Add(CreateTuple(@"U listopadu", @"^u\s+(?<mth>listopadu)$", 1, 34, @"DATE_OCT", 331));
            list.Add(CreateTuple(@"U studenom", @"^u\s+(?<mth>studenom)$", 1, 34, @"DATE_NOV", 332));
            list.Add(CreateTuple(@"U prosincu", @"^u\s+(?<mth>prosincu)$", 1, 34, @"DATE_DEC", 333));

            list.Add(CreateTuple(@"in january", @"^in\s+(?<mth>january)$", 2, 34, @"DATE_JAN", 322));
            list.Add(CreateTuple(@"in february", @"^in\s+(?<mth>february)$", 2, 34, @"DATE_FEB", 323));
            list.Add(CreateTuple(@"in march", @"^in\s+(?<mth>march)$", 2, 34, @"DATE_MAR", 324));
            list.Add(CreateTuple(@"in april", @"^in\s+(?<mth>april)$", 2, 34, @"DATE_APR", 325));
            list.Add(CreateTuple(@"in may", @"^in\s+(?<mth>may)$", 2, 34, @"DATE_MAY", 326));
            list.Add(CreateTuple(@"in june", @"^in\s+(?<mth>june)$", 2, 34, @"DATE_JUN", 327));
            list.Add(CreateTuple(@"in july", @"^in\s+(?<mth>july)$", 2, 34, @"DATE_JUL", 328));
            list.Add(CreateTuple(@"in august", @"^in\s+(?<mth>august)$", 2, 34, @"DATE_AUG", 329));
            list.Add(CreateTuple(@"in september", @"^in\s+(?<mth>september)$", 2, 34, @"DATE_SEP", 330));
            list.Add(CreateTuple(@"in october", @"^in\s+(?<mth>october)$", 2, 34, @"DATE_OCT", 331));
            list.Add(CreateTuple(@"in november", @"^in\s+(?<mth>november)$", 2, 34, @"DATE_NOV", 332));
            list.Add(CreateTuple(@"in december", @"^in\s+(?<mth>december)$", 2, 34, @"DATE_DEC", 333));

            list.Add(CreateTuple(@"mjesec", @"^(?:u\s+)?(?<mth>[1-9][0-2]|[1-9])[.]?\s*(?:mj?esecu?|misecu?)$", 1, 34, @"DATE_MONTH", 707));
            list.Add(CreateTuple(@"month", @"^(?<mth>[1-9][0-2]|[1-9])[.]?\s*month$", 2, 34, @"DATE_MONTH", 707));

            // ACCOMMODATION

            list.Add(CreateTuple(@"Hotel", @"^hotel\w*$", 1, 21, @"ACC_HOTEL", 300));
            list.Add(CreateTuple(@"Apartman", @"^apartman\w*$", 1, 21, @"ACC_APARTMENT", 301));
            list.Add(CreateTuple(@"Soba", @"^sob\w+$", 1, 21, @"ACC_ROOM", 302));
            list.Add(CreateTuple(@"bungalov", @"^bungalov\w*$", 1, 21, @"ACC_BUNGALOW", 447));
            list.Add(CreateTuple(@"hostel", @"^hostel\w*$", 1, 21, @"ACC_HOSTEL", 448));
            list.Add(CreateTuple(@"jednokrevetna soba", @"^jednokrevetn\w+\s+sob\w+$", 1, 21, @"ACC_SINGLE_ROOM", 449));
            list.Add(CreateTuple(@"kamp", @"^kamp\w*$", 1, 21, @"ACC_CAMP", 450));
            list.Add(CreateTuple(@"pansion", @"^pansion\w*$", 1, 21, @"ACC_PENSION", 451));
            list.Add(CreateTuple(@"privatni smješaj", @"^privatn\w+\s+smje(s|š|sh)aj\w*$", 1, 21, @"ACC_PRIVATE", 452));
            list.Add(CreateTuple(@"vila", @"^vil\w+$", 1, 21, @"ACC_VILLA", 453));
            list.Add(CreateTuple(@"sobe s doruckom", @"^sobe s doruckom\w*$", 1, 21, @"ACC_BREAKFAST", -1));
            list.Add(CreateTuple(@"jedrilica", @"^(jedrilic\w*)$", 1, 21, @"ACC_SAILBOAT", -1));
            list.Add(CreateTuple(@"studio apartman", @"^(studio apartman\w*)$", 1, 21, @"ACC_STUDIO", -1));
            list.Add(CreateTuple(@"seoski turizam", @"^(seosk\w*\s+turiz\w*)$", 1, 21, @"ACC_VILLAGE", -1));
            list.Add(CreateTuple(@"agroturizam", @"^agroturizam\w*$", 1, 21, @"ACC_AGRO", -1));
            list.Add(CreateTuple(@"kuca", @"^ku[čćc]\w*$", 1, 21, @"ACC_HOUSE", -1));
            list.Add(CreateTuple(@"odmaralište", @"^(odmarali(s|š|sh)t\w+)$", 1, 21, @"ACC_REST", 558));

            list.Add(CreateTuple(@"hotel", @"^(hotel)$", 2, 21, @"ACC_HOTEL", 300));
            list.Add(CreateTuple(@"apartment", @"^(apartment)$", 2, 21, @"ACC_APARTMENT", 301));
            list.Add(CreateTuple(@"room", @"^(room)$", 2, 21, @"ACC_ROOM", 302));
            list.Add(CreateTuple(@"bungalow", @"^(bungalow)$", 2, 21, @"ACC_BUNGALOW", 447));
            list.Add(CreateTuple(@"hostel", @"^(hostel)$", 2, 21, @"ACC_HOSTEL", 448));
            list.Add(CreateTuple(@"single room", @"^(single\s+room)$", 2, 21, @"ACC_SINGLE_ROOM", 449));
            list.Add(CreateTuple(@"autocamo, camp", @"^(autocamo|camp)$", 2, 21, @"ACC_CAMP", 450));
            list.Add(CreateTuple(@"pension", @"^pension$", 2, 21, @"ACC_PENSION", -1));
            list.Add(CreateTuple(@"private accommodation", @"^(private\s+accommodation)$", 2, 21, @"ACC_PRIVATE", 313));
            list.Add(CreateTuple(@"villa", @"^(villa)$", 2, 21, @"ACC_VILLA", 453));
            list.Add(CreateTuple(@"bed and breakfast, boarding house", @"^(bed\s+(and)?\s+breakfast|boarding\s+house)$", 2, 21, @"ACC_BREAKFAST", -1));
            list.Add(CreateTuple(@"sailboat", @"^(sailboat\w*)$", 2, 21, @"ACC_SAILBOAT", -1));
            list.Add(CreateTuple(@"studio apartment", @"^(studio apartment\w*)$", 2, 21, @"ACC_STUDIO", -1));
            list.Add(CreateTuple(@"village tourism", @"^(villages\w*\s+tourism\w*)$", 2, 21, @"ACC_VILLAGE", -1));
            list.Add(CreateTuple(@"agrotourism", @"^agrotourism\w*$", 2, 21, @"ACC_AGRO", -1));
            list.Add(CreateTuple(@"house", @"^hous\w*$", 2, 21, @"ACC_HOUSE", -1));

            foreach (var r in list)
            {
                sb.Append(String.Format(@"('{0}', '{1}', {2}, {3}, '{4}', {5}), ", r.Item1, r.Item2.Replace("'", "''"), r.Item3, termIdMap[r.Item4], r.Item5, r.Item6));
            }

            string cstr = sb.ToString();
            SqlCommand command = new SqlCommand(cstr.Substring(0, cstr.Length - 2), conn);
            command.ExecuteNonQuery();
        }

        private static Dictionary<int, int> PopulateTermType(SqlConnection conn)
        {
            string scmd = @"INSERT INTO SearchTermType (Title, Code, GroupCode) VALUES
                                  ('Otok', 'ISLAND', 'GEO'),
                                  ('Grad', 'CITY', 'GEO'),
                                  ('Županija', 'COUNTY', 'GEO'),
                                  ('Država', 'COUNTRY', 'GEO'),
                                  ('Regija', 'REGION', 'GEO'),
                                  ('Podregija', 'SUBREGION', 'GEO'),
                                  ('Poluotok', 'PENINSULA', 'GEO'),
                                  ('Rijeka', 'RIVER', 'GEO2'),
                                  ('Planina', 'MOUNTAIN', 'GEO2'),
                                  ('Jezero', 'LAKE', 'GEO2'),
                                  ('Plaža', 'BEACH', 'GEO2'),
                                  ('Smještaj', 'ACCOMMODATION', 'CONSTRAINT'),
                                  ('Broj osoba', 'CAPACITY', 'CONSTRAINT'),
                                  ('Cijena', 'PRICE', 'CONSTRAINT'),
                                  ('Datum', 'DATE', 'CONSTRAINT')";
            SqlCommand sqlcmd = new SqlCommand(scmd, conn);
            sqlcmd.ExecuteNonQuery();

            Dictionary<int, int> termIdMap = new Dictionary<int, int>();
            termIdMap[-1] = -1;
            termIdMap[1] = 1;
            termIdMap[2] = 2;
            termIdMap[3] = 3;
            termIdMap[4] = 4;
            termIdMap[5] = 5;
            termIdMap[6] = 6;
            termIdMap[11] = 7;
            termIdMap[7] = 8;
            termIdMap[8] = 9;
            termIdMap[9] = 10;
            termIdMap[10] = 11;
            termIdMap[21] = 12;
            termIdMap[32] = 13;
            termIdMap[33] = 14;
            termIdMap[34] = 15;

            return termIdMap;
        }

        static SqlConnection CreateConnection(string server, string db, string uname, string pass)
        {
            SqlConnectionStringBuilder connSB = new SqlConnectionStringBuilder();
            connSB.Add("user id", uname);
            connSB.Add("password", pass);
            connSB.Add("server", server);
            connSB.Add("database", db);
            //Console.WriteLine(connSB.ConnectionString);
            return new SqlConnection(connSB.ConnectionString);
        }

        static void CreateTables(SqlConnection conn)
        {
            string cstr = @"CREATE TABLE dbo.SearchGeoplace (
	                            Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	                            Title VARCHAR(max) NOT NULL,
	                            SearchTermId INT NULL,
	                            Longitude FLOAT NULL,
	                            Latitude FLOAT NULL,
	                            ParentId INT NULL,
	                            SearchTermTypeId INT NULL
                            )
                            CREATE TABLE dbo.SearchTerm (
	                            Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	                            Title VARCHAR(max) NOT NULL,
	                            Regex VARCHAR(max) NOT NULL,
	                            LanguageId INT NOT NULL,
	                            SearchTermTypeId INT NULL,
	                            ParseTag VARCHAR(max) NULL,
	                            SearchGeoplaceId INT NULL,
                                UltimateTableId INT NULL
                            )
                            CREATE TABLE dbo.SearchTermType (
	                            Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	                            Title VARCHAR(max) NOT NULL,
	                            Code VARCHAR(max) NULL,
	                            GroupCode VARCHAR(max) NULL
                            )
                            CREATE TABLE dbo.SearchAdvertInfo (
	                            Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	                            AdvertId INT NOT NULL,
	                            AccommodationTermId INT NULL,
	                            GeoplaceId INT NULL
                            )";

            SqlCommand command = new SqlCommand(cstr, conn);
            command.ExecuteNonQuery();
        }

        static void PopulateGeoplace(SqlConnection conn, String cityFilePath, Dictionary<int, int> termIdMap)
        {
            Console.WriteLine("Reading cities..");
            // Title, Latitude, Longitude
            List<Tuple<String, Double, Double>> cities = new List<Tuple<String, Double, Double>>();
            ReadCities(cityFilePath, cities);

            Console.WriteLine("Reading geographic terms..");
            // UltimateTable.Id, UT.Title, UltimateTable.RegexExpression, UltimateTableRelation.ParentId
            Dictionary<int, Tuple<string, Regex, int, int>> geoTerms = new Dictionary<int, Tuple<string, Regex, int, int>>();
            ReadGeoTerms(conn, geoTerms, termIdMap);

            Console.WriteLine("Matching cities with geographical terms..");
            // Index in 'cities' for each entry in 'geoTerms'.
            Dictionary<int, int> termMatch = new Dictionary<int, int>();
            // Index in 'geoTerms' for each entry in 'cities';
            Dictionary<int, int> cityMatch = new Dictionary<int, int>();
            MatchGeoTerms(geoTerms, cities, termMatch, cityMatch);

            Console.WriteLine("Running DFS for calculating node coordinates..");
            TreeCoordinator coordinator = new TreeCoordinator(geoTerms, termMatch, cities);
            Dictionary<int, Tuple<double, double, int>> coordinates = coordinator.Run();

            Console.WriteLine("Storing geographical terms in DB..");
            Dictionary<int, int> gmap = StoreAll(conn, cities, geoTerms, coordinates);

            Console.WriteLine("Copying geo. terms in other languages..");
            CopyForeignGeo(conn, gmap, termIdMap);
        }

        private static void CopyForeignGeo(SqlConnection conn, Dictionary<int, int> termMatch, Dictionary<int, int> termIdMap)
        {
            string cstr = @"SELECT utt.UltimateTableId utid, utt.LanguageId lid, utt.Title title, utt.Regex regex, ut.ObjectTypeId otype
                                FROM UltimateTableTranslation utt JOIN UltimateTable ut on ut.Id = utt.UltimateTableId
                                WHERE utt.LanguageId <> 1 AND ObjectTypeId in (1, 2, 4, 5, 6, 11)";

            SqlCommand sqlcmd = new SqlCommand(cstr, conn);
            List<Tuple<int, int, string, string, int>> stuff = new List<Tuple<int, int, string, string, int>>();

            SqlDataReader reader = sqlcmd.ExecuteReader();

            while (reader.Read())
            {
                Object tmp = reader["utid"];
                if (tmp == DBNull.Value) continue;
                int id = Int32.Parse(tmp.ToString());

                tmp = reader["lid"];
                if (tmp == DBNull.Value) continue;
                int lid = Int32.Parse(tmp.ToString());

                tmp = reader["title"];
                if (tmp == DBNull.Value) continue;
                string title = tmp.ToString();

                tmp = reader["regex"];
                if (tmp == DBNull.Value) continue;
                string regex = tmp.ToString();

                tmp = reader["otype"];
                if (tmp == DBNull.Value) tmp = -1;
                int otype = termIdMap[Int32.Parse(tmp.ToString())];   

                //if (!termMatch.ContainsKey(id)) continue;

                stuff.Add(new Tuple<int, int, string, string, int>(id, lid, title, regex, otype));
            }

            reader.Close();

            foreach (var g in stuff)
            {
                string type = g.Item5 == -1 ? "NULL" : g.Item5.ToString();

                if (termMatch.ContainsKey(g.Item1))
                {
                    cstr = String.Format(
                            @"INSERT INTO SearchTerm (Title, Regex, LanguageId, SearchGeoPlaceId, UltimateTableId, SearchTermTypeId) 
                                VALUES ('{0}', '{1}', {2}, {3}, {4}, {5})",
                            g.Item3.Replace("'", "''"), g.Item4.Replace("'", "''"), g.Item2, termMatch[g.Item1], g.Item1, type);
                }
                else
                {
                    cstr = String.Format(
                            @"INSERT INTO SearchTerm (Title, Regex, LanguageId, UltimateTableId, SearchTermTypeId) 
                                VALUES ('{0}', '{1}', {2}, {3}, {4})",
                            g.Item3.Replace("'", "''"), g.Item4.Replace("'", "''"), g.Item2, g.Item1, type);
                }

                sqlcmd = new SqlCommand(cstr, conn);
                sqlcmd.ExecuteNonQuery();
            }
        }

        static bool ReadCities(String cityFilePath, List<Tuple<String, Double, Double>> cities)
        {
            try
            {
                using (StreamReader sr = new StreamReader(cityFilePath))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        int p1 = line.IndexOf('\t');
                        int p2 = line.IndexOf('\t', p1 + 1);
                        Double x = Double.Parse(line.Substring(0, p1));
                        Double y = Double.Parse(line.Substring(p1 + 1, p2 - p1 - 1));
                        String name = line.Substring(p2 + 1).ToLower();
                        cities.Add(new Tuple<String, Double, Double>(name, x, y));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        static void ReadGeoTerms(SqlConnection conn, Dictionary<int, Tuple<string, Regex, int, int>> geoTerms, Dictionary<int, int> termIdMap)
        {
            string cstr = @"SELECT ut.id id, ut.title title, ut.regexexpression regex, utr.parentid pid, ut.objecttypeid otype
	                            FROM UltimateTable ut 
	                            LEFT JOIN UltimateTableRelation utr ON ut.id = utr.childId
	                            WHERE ut.objecttypeid in (1, 2, 4, 5, 6, 11)";

            SqlCommand command = new SqlCommand(cstr, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = Int32.Parse(reader["id"].ToString());

                string title = reader["title"].ToString();

                object tmp = reader["regex"];
                if (tmp == DBNull.Value) continue;
                Regex regex = new Regex(tmp.ToString());

                tmp = reader["pid"];
                if (tmp == DBNull.Value) tmp = -1;
                int pid = Int32.Parse(tmp.ToString());

                tmp = reader["otype"];
                if (tmp == DBNull.Value) tmp = -1;
                int otype = termIdMap[Int32.Parse(tmp.ToString())];            

                geoTerms.Add(id, new Tuple<string, Regex, int, int>(title, regex, pid, otype));
            }
            reader.Close();
        }

        static void MatchGeoTerms(
            Dictionary<int, Tuple<string, Regex, int, int>> geoTerms, 
            List<Tuple<string, double, double>> cities, 
            Dictionary<int, int> termMatch, 
            Dictionary<int, int> cityMatch)
        {
            foreach (var g in geoTerms)
            {
                // Matching is only for cities at the moment.
                if (g.Value.Item4 != 2) continue;

                //Console.WriteLine(g.Value.Item1);
                //Console.Read();


                int bind = -1, bval = -1, i = 0;

                foreach (var c in cities)
                {
                    int val = LevenshteinDistanceExtensions.LevenshteinDistance(g.Value.Item1, c.Item1);

                    //if (val < 5)
                    //{
                    //    Console.WriteLine(c.Item1 + " " + g.Value.Item1 + " " + g.Value.Item2.ToString());
                    //}
                    if (g.Value.Item2.IsMatch(c.Item1))
                    {
                        val = LevenshteinDistanceExtensions.LevenshteinDistance(g.Value.Item1, c.Item1);
                        if (bind == -1 || val < bval)
                        {
                            bind = i;
                            bval = val;
                        }
                    }

                    ++i;
                }

                if (bind != -1)
                {
                    cityMatch[bind] = g.Key;
                    termMatch[g.Key] = bind;
                }
            }
        }

        private static Dictionary<int, int> StoreAll(
            SqlConnection conn,
            List<Tuple<string, double, double>> cities, 
            Dictionary<int, Tuple<string, Regex, int, int>> geoTerms,
            Dictionary<int, Tuple<double, double, int>> coordinates)
        {
            
            int gind = 1, tind = 1;
            Dictionary<int, int> gmap = new Dictionary<int,int>();
            Dictionary<int, int> tmap = new Dictionary<int,int>();

            foreach (var g in geoTerms)
            {
                if (coordinates.ContainsKey(g.Key) && coordinates[g.Key].Item1 > -1000)
                {
                    gmap[g.Key] = gind;
                    ++gind;
                }

                tmap[g.Key] = tind;
                ++tind;
            }

            SqlCommand identityCmd = new SqlCommand(@"SET IDENTITY_INSERT SearchGeoplace ON", conn);
            identityCmd.ExecuteNonQuery();

            gind = tind = 1;
            foreach (var g in geoTerms)
            {
                string cmd; 
                SqlCommand sqlcmd;

                if (coordinates.ContainsKey(g.Key) && coordinates[g.Key].Item1 > -1000)
                {
                    string parent = gmap.ContainsKey(g.Value.Item3) ? gmap[g.Value.Item3].ToString() : "NULL";
                    string type = g.Value.Item4 == -1 ? "NULL" : g.Value.Item4.ToString();

                    cmd = String.Format(
                        "INSERT INTO SearchGeoplace (Id, Title, SearchTermId, Latitude, Longitude, ParentId, SearchTermTypeId) VALUES ({0}, '{1}', {2}, {3}, {4}, {5}, {6})",
                        gind, g.Value.Item1, tind, coordinates[g.Key].Item1, coordinates[g.Key].Item2, parent, type);
                    sqlcmd = new SqlCommand(cmd, conn);
                    sqlcmd.ExecuteNonQuery();

                    ++gind;
                }

                ++tind;
            }

            identityCmd = new SqlCommand(@"SET IDENTITY_INSERT SearchGeoplace OFF", conn);
            identityCmd.ExecuteNonQuery();
            identityCmd = new SqlCommand(@"SET IDENTITY_INSERT SearchTerm ON", conn);
            identityCmd.ExecuteNonQuery();

            tind = gind = 1;
            foreach (var g in geoTerms) {
                string cmd;
                SqlCommand sqlcmd;

                if (coordinates.ContainsKey(g.Key) && coordinates[g.Key].Item1 > -1000)
                {
                    string type = g.Value.Item4 == -1 ? "NULL" : g.Value.Item4.ToString();

                    cmd = String.Format(
                        @"INSERT INTO SearchTerm (Id, Title, Regex, LanguageId, SearchGeoplaceId, UltimateTableId, SearchTermTypeId) 
                                VALUES ({0}, '{1}', '{2}', {3}, {4}, {5}, {6})",
                        tind, g.Value.Item1, g.Value.Item2.ToString(), 1, gind, g.Key, type);
                    sqlcmd = new SqlCommand(cmd, conn);
                    sqlcmd.ExecuteNonQuery();

                    ++gind;
                    ++tind;
                }
                else
                {
                    string type = g.Value.Item4 == -1 ? "NULL" : g.Value.Item4.ToString();

                    cmd = String.Format(
                        @"INSERT INTO SearchTerm (Id, Title, Regex, LanguageId, UltimateTableId, SearchTermTypeId) VALUES ({0}, '{1}', '{2}', {3}, {4}, {5})",
                        tind, g.Value.Item1, g.Value.Item2.ToString(), 1, g.Key, type);
                    sqlcmd = new SqlCommand(cmd, conn);
                    sqlcmd.ExecuteNonQuery();

                    ++tind;
                }
            }

            identityCmd = new SqlCommand(@"SET IDENTITY_INSERT SearchTerm OFF", conn);
            identityCmd.ExecuteNonQuery();

            return gmap;
        }
    }

    class TreeCoordinator
    {
        Dictionary<int, List<int>> forest = new Dictionary<int, List<int>>();
        Dictionary<int, Tuple<double, double, int>> data = new Dictionary<int, Tuple<double, double, int>>();
        List<int> roots = new List<int>();
        //List<int> updated = new List<int>();
        Dictionary<int, Tuple<string, Regex, int, int>> terms;

        public TreeCoordinator(Dictionary<int, Tuple<string, Regex, int, int>> terms, Dictionary<int, int> match, List<Tuple<string, double, double>> cities) {
            this.terms = terms;

            foreach (var t in terms)
            {
                int id = t.Key;
                int parentId = t.Value.Item3;

                double lat, lon;
                if (match.ContainsKey(t.Key))
                {
                    //Console.WriteLine("ima");
                    lat = cities[match[id]].Item2;
                    lon = cities[match[id]].Item3;
                    data.Add(id, new Tuple<Double, Double, int>(lat, lon, 1));
                }

                if (parentId == -1)
                {
                    roots.Add(id);
                }
                else
                {
                    if (!forest.ContainsKey(parentId))
                        forest.Add(parentId, new List<int>());

                    forest[parentId].Add(id);
                }
            }
        }
        
        public Dictionary<int, Tuple<double, double, int>> Run()
        {
            foreach (Int32 root in roots)
            {
                DFS(root);
            }

            //foreach (var d in data)
            //{
            //    Console.WriteLine(terms[d.Key].Item1 + ": " + d.Value.Item1 + " " + d.Value.Item2);
            //}

            return data;
        }

        private void DFS(Int32 node)
        {
            if (!forest.ContainsKey(node)) return;

            int count = 0;
            double lat = 0;
            double lon = 0;

            foreach (Int32 child in forest[node])
            {
                DFS(child);
                if (data.ContainsKey(child) && data[child].Item1 > -1000)
                {
                    Tuple<Double, Double, int> dete = data[child];
                    lat += dete.Item1 * dete.Item3;
                    lon += dete.Item2 * dete.Item3;
                    count += dete.Item3;
                }
            }

            //Console.WriteLine(terms[node].Item1 + ": " + count + " " + lat + " " + lon);

            if (count != 0)
            {
                lat /= count;
                lon /= count;

                data[node] = new Tuple<double, double, int>(lat, lon, count);
            }
        }
    }
}
