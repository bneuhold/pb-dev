using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PB_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = "config.xml";
                if (args.Length >= 1)
                    config = args[0];

                new Init(config);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
