using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
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
using Turbo.turbo;

namespace Turbo
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        static InsertWindow SubWindow;

        public MainWindow()
        {
            InitializeComponent();

            //Console.WriteLine(String.Format(CultureInfo.CurrentCulture, "{0}", DateTime.Now));
            Console.WriteLine(String.Format(new CultureInfo("ko-KR"), "{0}", DateTime.Now));
        }

        // 검색
        public void Button_Click(object sender, RoutedEventArgs e)
        {

            var collection = ControllDB.db.GetCollection<PayVO>("turbo");

            var filter = Builders<PayVO>.Filter.Regex("Worker", new BsonRegularExpression(this.SearchName.Text));
            var filter2 = Builders<PayVO>.Filter.Regex("CompNum", new BsonRegularExpression(this.SearchCompNum.Text));

            var parsingList = collection.Find(filter & filter2).ToList();

            List<PayVO> list = new List<PayVO>();

            DataTable DTable = new DataTable();

            var i = 1;
            foreach (var vo in parsingList)
            {
                vo.No = i++;

                //Console.WriteLine(vo.Id);
                list.Add(vo);

            }
            PayList.ItemsSource = list;

        }

        // 작성
        public void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SubWindow == null )
            {
                SubWindow = new InsertWindow(this);
                SubWindow.Closed += SecondWindowClosed;
                SubWindow.Show();
            }
            else
            {
                SubWindow.Focus();
                SubWindow.BtnSave.Visibility = Visibility.Visible;
                SubWindow.BtnModify.Visibility = Visibility.Collapsed;
            }
            
        }

        public void SecondWindowClosed(object sender, System.EventArgs e)
        {
            SubWindow = null;
        }

        // 목록 클릭
        //private void PayList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    DataGrid dg = sender as DataGrid;
        //    if (dg.SelectedItems.Count > 0) {
        //        PayVO vo = (PayVO)dg.SelectedItems[0];

        //        //var collection = ControllDB.db.GetCollection<PayVO>("turbo");
        //        //collection.Find(x => x.Id == vo.Id).First();
        //        this.Button_Click_1(null, null);

        //        SubWindow.Worker.Text = vo.Worker;
        //        SubWindow.CompNum.Text = vo.CompNum;
        //        SubWindow.Amt.Text = vo.Amt.ToString();
        //        SubWindow.Company.Text = vo.Company;

        //        SubWindow.BtnSave.Visibility = Visibility.Collapsed;
        //        SubWindow.BtnModify.Visibility = Visibility.Visible;

        //        Console.WriteLine(vo.Id);
        //    }
        //}

        private void PayList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg.SelectedItems.Count > 0)
            {
                PayVO vo = (PayVO)dg.SelectedItems[0];

                //var collection = ControllDB.db.GetCollection<PayVO>("turbo");
                //collection.Find(x => x.Id == vo.Id).First();
                this.Button_Click_1(null, null);

                SubWindow.Worker.Text = vo.Worker;
                SubWindow.CompNum.Text = vo.CompNum;
                SubWindow.Amt.Text = vo.Amt.ToString();
                SubWindow.Company.Text = vo.Company;
                SubWindow.PayDt.SelectedDate = DateTime.Parse(vo.PayDt);

                SubWindow.BtnSave.Visibility = Visibility.Collapsed;
                SubWindow.BtnModify.Visibility = Visibility.Visible;

                //Console.WriteLine(vo.Id);
            }
        }
    }
}
