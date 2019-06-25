using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        IWebDriver driver;

        public MainWindow()
        {
            InitializeComponent();
            ChromeOptions op = new ChromeOptions();
            //op.AddArgument("--headless");
            driver = new ChromeDriver(op);

            var firingDriver = new EventFiringWebDriver(driver);
            firingDriver.ElementClicked += new EventHandler<WebElementEventArgs>(delegate (object sender, WebElementEventArgs e)
            {
                try
                {
                    System.Console.Write("Clicked on: " + e.Element.ToString());
                    System.Console.WriteLine("Page title: " + e.Driver.Title);
                }
                catch (Exception)
                {
                    //System.Console.WriteLine(e.StackTrace);
                    //throw;
                }
                
            });

            firingDriver.Navigated += new EventHandler<WebDriverNavigationEventArgs>(delegate (object sender, WebDriverNavigationEventArgs e)
            {
                try
                {
                    System.Console.Write("Navigated to: " + e.Driver.Url);
                    System.Console.WriteLine("Page title: " + e.Driver.Title);
                }
                catch (Exception)
                {
                    //throw;
                }
            });

            //firingDriver.ExceptionThrown += new EventHandler<WebDriverExceptionEventArgs>(firingDriver_ExceptionThrown);

            driver = firingDriver;

            driver.Navigate().GoToUrl("https://spib.wooribank.com/pib/Dream?withyou=CMLGN0001");
            //driver.Navigate().GoToUrl("http://www.khphub.com/bbs/board.php?bo_table=g5_tip&page=");
            Thread.Sleep(1000);

            while (true)
            {
                try
                {
                    driver.FindElement(By.CssSelector(".pr_log_2")).Click();
                    break;
                }
                catch (UnhandledAlertException)
                {
                    IAlert alert = driver.SwitchTo().Alert();
                    Console.WriteLine(alert.Text);
                    alert.Dismiss();
                    Thread.Sleep(1000);
                    //throw;
                }
                catch (ElementClickInterceptedException e)
                {
                    Console.WriteLine(e.StackTrace);
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Thread.Sleep(1000);
                }
            }

            Thread.Sleep(1000);
            ((IJavaScriptExecutor)driver).ExecuteScript("$('#USER_ID').val('" + "keehyun2" + "');");
            Thread.Sleep(1000);
            ((IJavaScriptExecutor)driver).ExecuteScript("$('#PWD').val('" + "asdfdg93" + "');");
            ((IJavaScriptExecutor)driver).ExecuteScript("$('#PWD').trigger({type: 'keypress', which: 39, keyCode: 39});");

            Thread.Sleep(1000);
            driver.FindElement(By.Id("PWD")).Click();
            //driver.FindElement(By.Id("PWD")).SendKeys(Keys.ArrowRight);
            //driver.FindElement(By.Id("id_login")).Click();
            Console.WriteLine("패스워드 입력완료");

            //Utill.FindElement(driver, By.CssSelector(".pr_log_2"), 3).Click();
            //Thread.Sleep(1000);
            //Utill.FindElement(driver, By.Id("USER_ID"), 3).SendKeys("keehyun2");
            //Thread.Sleep(1000);
            //Utill.FindElement(driver, By.Id("PWD"), 3).SendKeys("asdfdg93");

            //var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            //var clickableElement = wait.
            
        }

    }

    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
    }

    public class BankAccount
    {
        public String Num { get; set; } // 계좌번호
        public int Balance { get; set; } // 잔액

        // 조회시간
        // 입금건수
        // 입금금액
        // 출금건수
        // 출금금액
        // 건수합계
        // 금액합계
        // 처리건수 
    }
}
