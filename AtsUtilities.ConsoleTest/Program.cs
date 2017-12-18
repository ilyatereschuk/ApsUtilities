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


            /*
              the email is: jboesch@abctitle.com the password is: notary123
            the VIN Number you can test with is: 2GCEC13T861244730
            The license plate number you can test with is: YLL248
            Please also test failed conditions. Thanks. VM LM
             */
            //vimnumber / licensenumber

            String res = DpsLookUp.GetDpsLookupHtmlResult(
                "jboesch@abctitle.com",
                "notary123",
                "vinnumber",
                "2GCEC13T861244730",
                (String s) => { },
                (Int32 p) => { });

            Console.WriteLine("\n\n\n\n\n\n" + res);
        }
    }
}
