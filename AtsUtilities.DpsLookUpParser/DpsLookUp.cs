
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AtsUtilities.DpsLookUpParser
{
    public class DpsLookUp
    {
        public static String GetDpsLookupHtmlResult(
            String userName,
            String passWord,
            String searchBy, //vimnumber / licensenumber
            String query
            )
        {
            //http://www.autotitleservice.com/DPS/Account/Login?ReturnUrl=%2fDPS

            /*
              the email is: jboesch@abctitle.com the password is: notary123
            the VIN Number you can test with is: 2GCEC13T861244730
            The license plate number you can test with is: YLL248
            Please also test failed conditions. Thanks. VM LM
             */


            var requestUri = "http://www.autotitleservice.com/DPS/Account/Login?ReturnUrl=%2fDPS";

            

            var _driver = new PhantomJSDriver();

            _driver.Navigate().GoToUrl(
                "http://www.autotitleservice.com/DPS/Account/Login?ReturnUrl=%2fDPS"
            );

            _driver.FindElement(By.Name("UserName")).SendKeys(userName);
            _driver.FindElement(By.Name("Password")).SendKeys(passWord);
            _driver.FindElement(By.Id("btnLogin")).Submit();

            Console.WriteLine("\nSUBMITTED LOGIN\n");

           

            IWebElement element =
                (new WebDriverWait(_driver, new TimeSpan(0, 0, 10)))
                .Until(ExpectedConditions.ElementExists(By.Name("searchData")));

            element.SendKeys("2GCEC13T861244730");

            Console.WriteLine("\nSUBMITTED DATA\n");

            var checkboxes = _driver.FindElements(By.Name("searchtype"));
            checkboxes[0].Click();



            _driver.FindElement(By.Name("search_now")).Submit();

            Console.WriteLine("\nSUBMITTED SEARCH\n");

            //table lookup-table

            
            IWebElement tableElement = 
                (new WebDriverWait(_driver, new TimeSpan(0, 0, 10)))
                .Until(ExpectedConditions.ElementExists(By.TagName("table")));


            Console.WriteLine("\n\n\n\nElement: " + tableElement.Text);
            



            /*
             WebDriver driver = new FirefoxDriver();
driver.get("http://somedomain/url_that_delays_loading");
WebElement myDynamicElement = (new WebDriverWait(driver, 10))
  .until(ExpectedConditions.presenceOfElementLocated(By.Name("searchData")));
             */

            /*
            Assert.AreEqual(
              "Andy",
              _driver.FindElement(By.ClassName("sidebar-text5")).Text
            );*/


        }
    }
}
