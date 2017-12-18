using AtsUtilities.DpsLookUp;
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
        static void Main(string[] arguments)
        {
            //Run it by launching TestInConsole.bat

            String 
                userName = arguments[0],
                passWord = arguments[1],
                searchBy = arguments[2],
                query = arguments[3];


            try
            { 
                DpsLookUpParser.GetDpsLookupHtmlResult(
                    userName,
                    passWord,
                    searchBy,
                    query,
                    (String stepChangedMessage, Int32 progressChangedValue) =>
                    {
                        Console.WriteLine("{0} ({1}%)", stepChangedMessage, progressChangedValue);
                    },
                    (String htmlResult) =>
                    {
                        Console.WriteLine(htmlResult);
                        Console.WriteLine("Press ENTER to exit");
                        Console.ReadLine();
                    }
                );
            }
            catch(Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
            }
        }
    }
}
