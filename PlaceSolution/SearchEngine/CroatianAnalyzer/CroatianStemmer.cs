using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CroatianAnalyzer
{
    public class CroatianStemmer
    {
        private List<Regex> rules;
        private List<String> transFrom;
        private List<String> transTo;

        public CroatianStemmer()
        {
            rules = new List<Regex>();
            transFrom = new List<string>();
            transTo = new List<string>();
            Init();
        }

        public String Stem(String term)
        {
            term = term.ToLower();


            for (int i = 0; i < transFrom.Count; ++i) 
            {
                if (term.EndsWith(transFrom[i]))
                {
                    term = term.Substring(0, term.Length - transFrom[i].Length) + transTo[i];
                    break;
                }
            }

            foreach (Regex r in rules) 
            {
                Match m = r.Match(term);
                if (m.Success)
                {
                    String g1 = m.Groups[1].ToString();
                    if (HasVowel(g1) && g1.Length > 1) 
                    {
                        return g1;
                    }
                }
            }

            return term;
        }

        private bool HasVowel(String term)
        {
            return Regex.IsMatch(Regex.Replace(term, @"(^|[^aeiou])r($|[^aeiou])", new MatchEvaluator(ReplaceR)), "[aeiouR]");
        }

        private String ReplaceR(Match match)
        {
            return match.Groups[1].ToString() + "R" + match.Groups[2].ToString();
        }

        private void Init() 
        {
            rules.Add(new Regex(@"^(.+(s|š)k)(ijima|ijega|ijemu|ijem|ijim|ijih|ijoj|ijeg|iji|ije|ija|oga|ome|omu|ima|og|om|im|ih|oj|i|e|o|a|u)$"));
            rules.Add(new Regex(@"^(.+(s|š)tv)(ima|om|o|a|u)$"));
            rules.Add(new Regex(@"^(.+(t|m|p|r|g)anij)(ama|ima|om|a|u|e|i| )$"));
            rules.Add(new Regex(@"^(.+an)(inom|ina|inu|ine|ima|in|om|u|i|a|e| )$"));
            rules.Add(new Regex(@"^(.+in)(ima|ama|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+on)(ovima|ova|ove|ovi|ima|om|a|e|i|u| )$"));
            rules.Add(new Regex(@"^(.+n)(ijima|ijega|ijemu|ijeg|ijem|ijim|ijih|ijoj|iji|ije|ija|iju|ima|ome|omu|oga|oj|om|ih|im|og|o|e|a|u|i| )$"));
            rules.Add(new Regex(@"^(.+(a|e|u)ć)(oga|ome|omu|ega|emu|ima|oj|ih|om|eg|em|og|uh|im|e|a)$"));
            rules.Add(new Regex(@"^(.+ugov)(ima|i|e|a)$"));
            rules.Add(new Regex(@"^(.+ug)(ama|om|a|e|i|u|o)$"));
            rules.Add(new Regex(@"^(.+log)(ama|om|a|u|e| )$"));
            rules.Add(new Regex(@"^(.+[^eo]g)(ovima|ama|ovi|ove|ova|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+(rrar|ott|ss|ll)i)(jem|ja|ju|o| )$"));
            rules.Add(new Regex(@"^(.+uj)(ući|emo|ete|mo|em|eš|e|u| )$"));
            rules.Add(new Regex(@"^(.+(c|č|ć|đ|l|r)aj)(evima|evi|eva|eve|ama|ima|em|a|e|i|u| )$"));
            rules.Add(new Regex(@"^(.+(b|c|d|l|n|m|ž|g|f|p|r|s|t|z)ij)(ima|ama|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+[^z]nal)(ima|ama|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+ijal)(ima|ama|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+ozil)(ima|om|a|e|u|i| )$"));
            rules.Add(new Regex(@"^(.+olov)(ima|i|a|e)$"));
            rules.Add(new Regex(@"^(.+ol)(ima|om|a|u|e|i| )$"));
            rules.Add(new Regex(@"^(.+lem)(ama|ima|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+ram)(ama|om|a|e|i|u|o)$"));
            rules.Add(new Regex(@"^(.+(a|d|e|o)r)(ama|ima|om|u|a|e|i| )$"));
            rules.Add(new Regex(@"^(.+(e|i)s)(ima|om|e|a|u)$"));
            rules.Add(new Regex(@"^(.+(t|n|j|k|j|t|b|g|v)aš)(ama|ima|om|em|a|u|i|e| )$"));
            rules.Add(new Regex(@"^(.+(e|i)š)(ima|ama|om|em|i|e|a|u| )$"));
            rules.Add(new Regex(@"^(.+ikat)(ima|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+lat)(ima|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+et)(ama|ima|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+(e|i|k|o)st)(ima|ama|om|a|e|i|u|o| )$"));
            rules.Add(new Regex(@"^(.+išt)(ima|em|a|e|u)$"));
            rules.Add(new Regex(@"^(.+ova)(smo|ste|hu|ti|še|li|la|le|lo|t|h|o)$"));
            rules.Add(new Regex(@"^(.+(a|e|i)v)(ijemu|ijima|ijega|ijeg|ijem|ijim|ijih|ijoj|oga|ome|omu|ima|ama|iji|ije|ija|iju|im|ih|oj|om|og|i|a|u|e|o| )$"));
            rules.Add(new Regex(@"^(.+[^dkml]ov)(ijemu|ijima|ijega|ijeg|ijem|ijim|ijih|ijoj|oga|ome|omu|ima|iji|ije|ija|iju|im|ih|oj|om|og|i|a|u|e|o| )$"));
            rules.Add(new Regex(@"^(.+(m|l)ov)(ima|om|a|u|e|i| )$"));
            rules.Add(new Regex(@"^(.+el)(ijemu|ijima|ijega|ijeg|ijem|ijim|ijih|ijoj|oga|ome|omu|ima|iji|ije|ija|iju|im|ih|oj|om|og|i|a|u|e|o| )$"));
            rules.Add(new Regex(@"^(.+(a|e|š)nj)(ijemu|ijima|ijega|ijeg|ijem|ijim|ijih|ijoj|oga|ome|omu|ima|iji|ije|ija|iju|ega|emu|eg|em|im|ih|oj|om|og|a|e|i|o|u)$"));
            rules.Add(new Regex(@"^(.+čin)(ama|ome|omu|oga|ima|og|om|im|ih|oj|a|u|i|o|e| )$"));
            rules.Add(new Regex(@"^(.+roši)(vši|smo|ste|še|mo|te|ti|li|la|lo|le|m|š|t|h|o)$"));
            rules.Add(new Regex(@"^(.+oš)(ijemu|ijima|ijega|ijeg|ijem|ijim|ijih|ijoj|oga|ome|omu|ima|iji|ije|ija|iju|im|ih|oj|om|og|i|a|u|e| )$"));
            rules.Add(new Regex(@"^(.+(e|o)vit)(ijima|ijega|ijemu|ijem|ijim|ijih|ijoj|ijeg|iji|ije|ija|oga|ome|omu|ima|og|om|im|ih|oj|i|e|o|a|u| )$"));
            rules.Add(new Regex(@"^(.+ast)(ijima|ijega|ijemu|ijem|ijim|ijih|ijoj|ijeg|iji|ije|ija|oga|ome|omu|ima|og|om|im|ih|oj|i|e|o|a|u| )$"));
            rules.Add(new Regex(@"^(.+k)(ijemu|ijima|ijega|ijeg|ijem|ijim|ijih|ijoj|oga|ome|omu|ima|iji|ije|ija|iju|im|ih|oj|om|og|i|a|u|e|o| )$"));
            rules.Add(new Regex(@"^(.+(e|a|i|u)va)(jući|smo|ste|jmo|jte|ju|la|le|li|lo|mo|na|ne|ni|no|te|ti|še|hu|h|j|m|n|o|t|v|š| )$"));
            rules.Add(new Regex(@"^(.+ir)(ujemo|ujete|ujući|ajući|ivat|ujem|uješ|ujmo|ujte|avši|asmo|aste|ati|amo|ate|aju|aše|ahu|ala|alo|ali|ale|uje|uju|uj|al|an|am|aš|at|ah|ao)$"));
            rules.Add(new Regex(@"^(.+ač)(ismo|iste|iti|imo|ite|iše|eći|ila|ilo|ili|ile|ena|eno|eni|ene|io|im|iš|it|ih|en|i|e)$"));
            rules.Add(new Regex(@"^(.+ača)(vši|smo|ste|smo|ste|hu|ti|mo|te|še|la|lo|li|le|ju|na|no|ni|ne|o|m|š|t|h|n)$"));
            rules.Add(new Regex(@"^(.+n)(uvši|usmo|uste|ući|imo|ite|emo|ete|ula|ulo|ule|uli|uto|uti|uta|em|eš|uo|ut|e|u|i)$"));
            rules.Add(new Regex(@"^(.+ni)(vši|smo|ste|ti|mo|te|mo|te|la|lo|le|li|m|š|o)$"));
            rules.Add(new Regex(@"^(.+((a|r|i|p|e|u)st|[^o]g|ik|uc|oj|aj|lj|ak|ck|čk|šk|uk|nj|im|ar|at|et|št|it|ot|ut|zn|zv)a)(jući|vši|smo|ste|jmo|jte|jem|mo|te|je|ju|ti|še|hu|la|li|le|lo|na|no|ni|ne|t|h|o|j|n|m|š)$"));
            rules.Add(new Regex(@"^(.+ur)(ajući|asmo|aste|ajmo|ajte|amo|ate|aju|ati|aše|ahu|ala|ali|ale|alo|ana|ano|ani|ane|al|at|ah|ao|aj|an|am|aš)$"));
            rules.Add(new Regex(@"^(.+(a|i|o)staj)(asmo|aste|ahu|ati|emo|ete|aše|ali|ući|ala|alo|ale|mo|ao|em|eš|at|ah|te|e|u| )$"));
            rules.Add(new Regex(@"^(.+(b|c|č|ć|d|e|f|g|j|k|n|r|t|u|v)a)(lama|lima|lom|lu|li|la|le|lo|l)$"));
            rules.Add(new Regex(@"^(.+(t|č|j|ž|š)aj)(evima|evi|eva|eve|ama|ima|em|a|e|i|u| )$"));
            rules.Add(new Regex(@"^(.+([^o]m|ič|nč|uč|b|c|ć|d|đ|h|j|k|l|n|p|r|s|š|v|z|ž)a)(jući|vši|smo|ste|jmo|jte|mo|te|ju|ti|še|hu|la|li|le|lo|na|no|ni|ne|t|h|o|j|n|m|š)$"));
            rules.Add(new Regex(@"^(.+(a|i|o)sta)(dosmo|doste|doše|nemo|demo|nete|dete|nimo|nite|nila|vši|nem|dem|neš|deš|doh|de|ti|ne|nu|du|la|li|lo|le|t|o)$"));
            rules.Add(new Regex(@"^(.+ta)(smo|ste|jmo|jte|vši|ti|mo|te|ju|še|la|lo|le|li|na|no|ni|ne|n|j|o|m|š|t|h)$"));
            rules.Add(new Regex(@"^(.+inj)(asmo|aste|ati|emo|ete|ali|ala|alo|ale|aše|ahu|em|eš|at|ah|ao)$"));
            rules.Add(new Regex(@"^(.+as)(temo|tete|timo|tite|tući|tem|teš|tao|te|li|ti|la|lo|le)$"));
            rules.Add(new Regex(@"^(.+(elj|ulj|tit|ac|ič|od|oj|et|av|ov)i)(vši|eći|smo|ste|še|mo|te|ti|li|la|lo|le|m|š|t|h|o)$"));
            rules.Add(new Regex(@"^(.+(tit|jeb|ar|ed|uš|ič)i)(jemo|jete|jem|ješ|smo|ste|jmo|jte|vši|mo|še|te|ti|ju|je|la|lo|li|le|t|m|š|h|j|o)$"));
            rules.Add(new Regex(@"^(.+(b|č|d|l|m|p|r|s|š|ž)i)(jemo|jete|jem|ješ|smo|ste|jmo|jte|vši|mo|lu|še|te|ti|ju|je|la|lo|li|le|t|m|š|h|j|o)$"));
            rules.Add(new Regex(@"^(.+luč)(ujete|ujući|ujemo|ujem|uješ|ismo|iste|ujmo|ujte|uje|uju|iše|iti|imo|ite|ila|ilo|ili|ile|ena|eno|eni|ene|uj|io|en|im|iš|it|ih|e|i)$"));
            rules.Add(new Regex(@"^(.+jeti)(smo|ste|še|mo|te|ti|li|la|lo|le|m|š|t|h|o)$"));
            rules.Add(new Regex(@"^(.+e)(lama|lima|lom|lu|li|la|le|lo|l)$"));
            rules.Add(new Regex(@"^(.+i)(lama|lima|lom|lu|li|la|le|lo|l)$"));
            rules.Add(new Regex(@"^(.+at)(ijega|ijemu|ijima|ijeg|ijem|ijih|ijim|ima|oga|ome|omu|iji|ije|ija|iju|oj|og|om|im|ih|a|u|i|e|o| )$"));
            rules.Add(new Regex(@"^(.+et)(avši|ući|emo|imo|em|eš|e|u|i)$"));
            rules.Add(new Regex(@"^(.+)(ajući|alima|alom|avši|asmo|aste|ajmo|ajte|ivši|amo|ate|aju|ati|aše|ahu|ali|ala|ale|alo|ana|ano|ani|ane|am|aš|at|ah|ao|aj|an)$"));
            rules.Add(new Regex(@"^(.+)(anje|enje|anja|enja|enom|enoj|enog|enim|enih|anom|anoj|anog|anim|anih|eno|ovi|ova|oga|ima|ove|enu|anu|ena|ama)$"));
            rules.Add(new Regex(@"^(.+)(nijega|nijemu|nijima|nijeg|nijem|nijim|nijih|nima|niji|nije|nija|niju|noj|nom|nog|nim|nih|an|na|nu|ni|ne|no)$"));
            rules.Add(new Regex(@"^(.+)(om|og|im|ih|em|oj|an|u|o|i|e|a)$"));

            transFrom.Add(@"lozi");
            transTo.Add(@"loga");
            transFrom.Add(@"lozima");
            transTo.Add(@"loga");
            transFrom.Add(@"pjesi");
            transTo.Add(@"pjeh");
            transFrom.Add(@"pjesima");
            transTo.Add(@"pjeh");
            transFrom.Add(@"vojci");
            transTo.Add(@"vojka");
            transFrom.Add(@"bojci");
            transTo.Add(@"bojka");
            transFrom.Add(@"jaci");
            transTo.Add(@"jak");
            transFrom.Add(@"jacima");
            transTo.Add(@"jak");
            transFrom.Add(@"čajan");
            transTo.Add(@"čajni");
            transFrom.Add(@"ijeran");
            transTo.Add(@"ijerni");
            transFrom.Add(@"laran");
            transTo.Add(@"larni");
            transFrom.Add(@"ijesan");
            transTo.Add(@"ijesni");
            transFrom.Add(@"anjac");
            transTo.Add(@"anjca");
            transFrom.Add(@"ajac");
            transTo.Add(@"ajca");
            transFrom.Add(@"ajaca");
            transTo.Add(@"ajca");
            transFrom.Add(@"ljaca");
            transTo.Add(@"ljca");
            transFrom.Add(@"ljac");
            transTo.Add(@"ljca");
            transFrom.Add(@"ejac");
            transTo.Add(@"ejca");
            transFrom.Add(@"ejaca");
            transTo.Add(@"ejca");
            transFrom.Add(@"ojac");
            transTo.Add(@"ojca");
            transFrom.Add(@"ojaca");
            transTo.Add(@"ojca");
            transFrom.Add(@"ajaka");
            transTo.Add(@"ajka");
            transFrom.Add(@"ojaka");
            transTo.Add(@"ojka");
            transFrom.Add(@"šaca");
            transTo.Add(@"šca");
            transFrom.Add(@"šac");
            transTo.Add(@"šca");
            transFrom.Add(@"inzima");
            transTo.Add(@"ing");
            transFrom.Add(@"inzi");
            transTo.Add(@"ing");
            transFrom.Add(@"tvenici");
            transTo.Add(@"tvenik");
            transFrom.Add(@"tetici");
            transTo.Add(@"tetika");
            transFrom.Add(@"teticima");
            transTo.Add(@"tetika");
            transFrom.Add(@"nstava");
            transTo.Add(@"nstva");
            transFrom.Add(@"nicima");
            transTo.Add(@"nik");
            transFrom.Add(@"ticima");
            transTo.Add(@"tik");
            transFrom.Add(@"zicima");
            transTo.Add(@"zik");
            transFrom.Add(@"snici");
            transTo.Add(@"snik");
            transFrom.Add(@"kuse");
            transTo.Add(@"kusi");
            transFrom.Add(@"kusan");
            transTo.Add(@"kusni");
            transFrom.Add(@"kustava");
            transTo.Add(@"kustva");
            transFrom.Add(@"dušan");
            transTo.Add(@"dušni");
            transFrom.Add(@"antan");
            transTo.Add(@"antni");
            transFrom.Add(@"bilan");
            transTo.Add(@"bilni");
            transFrom.Add(@"tilan");
            transTo.Add(@"tilni");
            transFrom.Add(@"avilan");
            transTo.Add(@"avilni");
            transFrom.Add(@"silan");
            transTo.Add(@"silni");
            transFrom.Add(@"gilan");
            transTo.Add(@"gilni");
            transFrom.Add(@"rilan");
            transTo.Add(@"rilni");
            transFrom.Add(@"nilan");
            transTo.Add(@"nilni");
            transFrom.Add(@"alan");
            transTo.Add(@"alni");
            transFrom.Add(@"ozan");
            transTo.Add(@"ozni");
            transFrom.Add(@"rave");
            transTo.Add(@"ravi");
            transFrom.Add(@"stavan");
            transTo.Add(@"stavni");
            transFrom.Add(@"pravan");
            transTo.Add(@"pravni");
            transFrom.Add(@"tivan");
            transTo.Add(@"tivni");
            transFrom.Add(@"sivan");
            transTo.Add(@"sivni");
            transFrom.Add(@"atan");
            transTo.Add(@"atni");
            transFrom.Add(@"cenata");
            transTo.Add(@"centa");
            transFrom.Add(@"denata");
            transTo.Add(@"denta");
            transFrom.Add(@"genata");
            transTo.Add(@"genta");
            transFrom.Add(@"lenata");
            transTo.Add(@"lenta");
            transFrom.Add(@"menata");
            transTo.Add(@"menta");
            transFrom.Add(@"jenata");
            transTo.Add(@"jenta");
            transFrom.Add(@"venata");
            transTo.Add(@"venta");
            transFrom.Add(@"tetan");
            transTo.Add(@"tetni");
            transFrom.Add(@"pletan");
            transTo.Add(@"pletni");
            transFrom.Add(@"šave");
            transTo.Add(@"šavi");
            transFrom.Add(@"manata");
            transTo.Add(@"manta");
            transFrom.Add(@"tanata");
            transTo.Add(@"tanta");
            transFrom.Add(@"lanata");
            transTo.Add(@"lanta");
            transFrom.Add(@"sanata");
            transTo.Add(@"santa");
            transFrom.Add(@"ačak");
            transTo.Add(@"ačka");
            transFrom.Add(@"ačaka");
            transTo.Add(@"ačka");
            transFrom.Add(@"ušak");
            transTo.Add(@"uška");
            transFrom.Add(@"atak");
            transTo.Add(@"atka");
            transFrom.Add(@"ataka");
            transTo.Add(@"atka");
            transFrom.Add(@"atci");
            transTo.Add(@"atka");
            transFrom.Add(@"atcima");
            transTo.Add(@"atka");
            transFrom.Add(@"etak");
            transTo.Add(@"etka");
            transFrom.Add(@"etaka");
            transTo.Add(@"etka");
            transFrom.Add(@"itak");
            transTo.Add(@"itka");
            transFrom.Add(@"itaka");
            transTo.Add(@"itka");
            transFrom.Add(@"itci");
            transTo.Add(@"itka");
            transFrom.Add(@"otak");
            transTo.Add(@"otka");
            transFrom.Add(@"otaka");
            transTo.Add(@"otka");
            transFrom.Add(@"utak");
            transTo.Add(@"utka");
            transFrom.Add(@"utaka");
            transTo.Add(@"utka");
            transFrom.Add(@"utci");
            transTo.Add(@"utka");
            transFrom.Add(@"utcima");
            transTo.Add(@"utka");
            transFrom.Add(@"eskan");
            transTo.Add(@"eskna");
            transFrom.Add(@"tičan");
            transTo.Add(@"tični");
            transFrom.Add(@"ojsci");
            transTo.Add(@"ojska");
            transFrom.Add(@"esama");
            transTo.Add(@"esma");
            transFrom.Add(@"metara");
            transTo.Add(@"metra");
            transFrom.Add(@"centar");
            transTo.Add(@"centra");
            transFrom.Add(@"centara");
            transTo.Add(@"centra");
            transFrom.Add(@"istara");
            transTo.Add(@"istra");
            transFrom.Add(@"istar");
            transTo.Add(@"istra");
            transFrom.Add(@"ošću");
            transTo.Add(@"osti");
            transFrom.Add(@"daba");
            transTo.Add(@"dba");
            transFrom.Add(@"čcima");
            transTo.Add(@"čka");
            transFrom.Add(@"čci");
            transTo.Add(@"čka");
            transFrom.Add(@"mac");
            transTo.Add(@"mca");
            transFrom.Add(@"maca");
            transTo.Add(@"mca");
            transFrom.Add(@"naca");
            transTo.Add(@"nca");
            transFrom.Add(@"nac");
            transTo.Add(@"nca");
            transFrom.Add(@"voljan");
            transTo.Add(@"voljni");
            transFrom.Add(@"anaka");
            transTo.Add(@"anki");
            transFrom.Add(@"vac");
            transTo.Add(@"vca");
            transFrom.Add(@"vaca");
            transTo.Add(@"vca");
            transFrom.Add(@"saca");
            transTo.Add(@"sca");
            transFrom.Add(@"sac");
            transTo.Add(@"sca");
            transFrom.Add(@"naca");
            transTo.Add(@"nca");
            transFrom.Add(@"nac");
            transTo.Add(@"nca");
            transFrom.Add(@"raca");
            transTo.Add(@"rca");
            transFrom.Add(@"rac");
            transTo.Add(@"rca");
            transFrom.Add(@"aoca");
            transTo.Add(@"alca");
            transFrom.Add(@"alaca");
            transTo.Add(@"alca");
            transFrom.Add(@"alac");
            transTo.Add(@"alca");
            transFrom.Add(@"elaca");
            transTo.Add(@"elca");
            transFrom.Add(@"elac");
            transTo.Add(@"elca");
            transFrom.Add(@"olaca");
            transTo.Add(@"olca");
            transFrom.Add(@"olac");
            transTo.Add(@"olca");
            transFrom.Add(@"olce");
            transTo.Add(@"olca");
            transFrom.Add(@"njac");
            transTo.Add(@"njca");
            transFrom.Add(@"njaca");
            transTo.Add(@"njca");
            transFrom.Add(@"ekata");
            transTo.Add(@"ekta");
            transFrom.Add(@"ekat");
            transTo.Add(@"ekta");
            transFrom.Add(@"izam");
            transTo.Add(@"izma");
            transFrom.Add(@"izama");
            transTo.Add(@"izma");
            transFrom.Add(@"jebe");
            transTo.Add(@"jebi");
            transFrom.Add(@"baci");
            transTo.Add(@"baci");
            transFrom.Add(@"ašan");
            transTo.Add(@"ašni");
        }
    }
}
