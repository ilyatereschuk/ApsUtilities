using AtsUtilities.DpsLookUpParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AtsUtilities.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DpsLookUp.Get(
                "jboesch@abctitle.com",
                "notary123").Wait();
        }
    }
}
