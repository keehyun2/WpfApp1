using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using WpfApp1.khphub;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        EventFiringWebDriver blindDriver;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DriverControl dc = new DriverControl();
            EventFiringWebDriver eventDriver = dc.GetDriver(); // 드라이버 객체 생성 및 클릭 이벤트 세팅

            eventDriver.Navigate().GoToUrl("https://spib.wooribank.com/pib/Dream?withyou=CMLGN0001");
            Thread.Sleep(1000);

            Thread.Sleep(1000);
            BringToFront();
            MessageBox.Show("로그인 해주세요.");
            Thread.Sleep(1000);

            // 로그인 될때까지 대기
            while (true)
            {
                try
                {
                    if (eventDriver.FindElement(By.CssSelector(".login-name")).Displayed)
                    {
                        System.Console.WriteLine("로그인 성공");
                        break;
                    }
                }
                catch (Exception)
                {
                    //Console.WriteLine($": '{e}'");
                }
                Thread.Sleep(2000);
            }

            blindDriver = dc.GetDriver(new String[] { "--headless", "window-size=1920x1080", "disable-gpu", "user - agent = Mozilla / 5.0(Macintosh; Intel Mac OS X 10_12_6) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 61.0.3163.100 Safari / 537.36", "lang =ko_KR" });
            // blindDriver = dc.GetDriver();

            blindDriver.Navigate().GoToUrl("https://www.wooribank.com/");

            ReadOnlyCollection<Cookie> _cookies = eventDriver.Manage().Cookies.AllCookies;
            eventDriver.ExecuteScript("$('body').append('<div id=\"k-shield\" style=\"position: fixed; color: white; width: 100%;height: 100%;background: rgba(255, 0, 0, 0.6);top: 0;text-align: center; padding-top:250px; font-size: 30px; font-weight: bold; z-index: 1000;\" >기다려주세요</div>');");

            foreach (Cookie item in _cookies)
            {
                blindDriver.Manage().Cookies.AddCookie(item);
            }
            Thread.Sleep(1000);
            blindDriver.Navigate().GoToUrl("https://spib.wooribank.com/pib/Dream?withyou=PSINQ0013"); // 게좌 화면 
            Thread.Sleep(1000);
            eventDriver.Quit();

            AccountList.ItemsSource = dc.SetAccountList(blindDriver);
            
            BringToFront();

            MessageBox.Show("계좌를 클릭해주세요.");
        }

        // 창 앞으로 땡기는 함수 
        public void BringToFront() {
            if (!IsVisible)
            {
                Show();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }

            Activate();
            Topmost = true;  // important
            Topmost = false; // important
            Focus();         // important
        }

        // 계좌 선택 
        private void AccountList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            blindDriver.Navigate().GoToUrl("https://spib.wooribank.com/pib/Dream?withyou=PSINQ0028"); // 상세조회 화면 
            Thread.Sleep(1000);
            blindDriver.ExecuteScript("beforeAddDate('4');");

            try
            {
                while (blindDriver.FindElement(By.CssSelector(".pop-content")).Displayed)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("조회중");
            }

            List<PayVO> list = new List<PayVO>();

            IReadOnlyCollection<IWebElement> elList = blindDriver.FindElements(By.CssSelector(".gridHeaderTableDefault .grid_body_row"));

            foreach (IWebElement el in elList)
            {
                PayVO payVO = new PayVO();
                payVO.No = el.FindElement(By.CssSelector("td:nth-child(1)")).Text;
                payVO.PayDt = el.FindElement(By.CssSelector("td:nth-child(2)")).Text;
                payVO.Briefs = el.FindElement(By.CssSelector("td:nth-child(3)")).Text;
                payVO.Memo = el.FindElement(By.CssSelector("td:nth-child(4)")).Text;
                payVO.OutAmt = el.FindElement(By.CssSelector("td:nth-child(5)")).Text;
                payVO.InAmt = el.FindElement(By.CssSelector("td:nth-child(6)")).Text;
                payVO.Balance = el.FindElement(By.CssSelector("td:nth-child(7)")).Text;
                payVO.Point = el.FindElement(By.CssSelector("td:nth-child(8)")).Text;

                list.Add(payVO);
            }

            PayList.ItemsSource = list;
        }

    } // MainWindow class 끝 
} // namespace 끝
