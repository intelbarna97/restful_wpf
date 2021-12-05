using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_RESTful.entity;

namespace WPF_RESTful
{
    /// <summary>
    /// Interaction logic for Goods.xaml
    /// </summary>
    public partial class Goods : Window
    {
        String URL = "http://localhost/wpfapi";
        String ROUTE = "goods.php";
        List<Good> goodsList = new List<Good>();
        MainWindow mainWindow;
        public static Good selectedGood;
        public Goods(MainWindow main)
        {
            mainWindow = main;
            InitializeComponent();
            Username_TextBlock.Text = MainWindow.activeUser.Username;
            listOwn();
            if(MainWindow.activeUser.isAdmin())
            {
                AdminCount_TextBlock.Visibility = Visibility.Visible;
                AdminName_TextBlock.Visibility = Visibility.Visible;
                AdminPrice_TextBlock.Visibility = Visibility.Visible;
                AdminGoodsPrice_TextBox.Visibility = Visibility.Visible;
                AdminGoodsName_TextBox.Visibility = Visibility.Visible;
                AdminGoodsCount_TextBox.Visibility = Visibility.Visible;
                AdminModify_Button.Visibility = Visibility.Visible;
                DeleteGoods_Button.Visibility = Visibility.Visible;
                Highlighted_TextBlock.Visibility = Visibility.Visible;
                ID_TextBlock.Visibility = Visibility.Visible;
                Username_TextBlock.Foreground = new SolidColorBrush(Color.FromRgb(186, 36, 2));
            }
        }

        private void ListAllProducts_Button_Click(object sender, RoutedEventArgs e)
        {
            List_All();
        }

        public void List_All()
        {
            var client = new RestClient(URL);
            string route = ROUTE + "?list_all=try";
            var request = new RestRequest(route, Method.GET);

            var response = client.Execute(request);
            goodsList = new JsonSerializer().Deserialize<List<Good>>(response);

            Goods_Datagrid.ItemsSource = goodsList;
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Show();
        }

        private void Goods_Datagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                selectedGood = Goods_Datagrid.SelectedItems[0] as Good;
                Highlighted_TextBlock.Text = selectedGood.Id.ToString();
                AdminGoodsName_TextBox.Text = selectedGood.Name;
                AdminGoodsCount_TextBox.Text= selectedGood.Count.ToString();
                AdminGoodsPrice_TextBox.Text = selectedGood.Price.ToString();
            }
            catch 
            {
                Highlighted_TextBlock.Text = "NaN";
            }
        }

        private void DeleteGoods_Button_Click(object sender, RoutedEventArgs e)
        {
            if(selectedGood != null)
            {
                var client = new RestClient(URL);
                string route = ROUTE + "?delete=" + selectedGood.Id;
                var request = new RestRequest(route, Method.DELETE);
                request.AddJsonBody(new User
                { Id = MainWindow.activeUser.Id });
                var response = client.Execute(request);
                if (response.Content.Contains("Delete successful!"))
                {
                    Highlighted_TextBlock.Text = "";
                }
                selectedGood = null;
                List_All();
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if(GoodsName_TextBox.Text !="" && GoodsCount_TextBox.Text !="" && GoodsPrice_TextBox.Text != "")
            {
                var client = new RestClient(URL);
                string route = ROUTE + "?add=" + MainWindow.activeUser.Id;
                var request = new RestRequest(route, Method.POST);
                request.AddJsonBody(new Good
                {
                    Name = GoodsName_TextBox.Text,
                    Count = Convert.ToInt32(GoodsCount_TextBox.Text),
                    Price = Convert.ToInt32(GoodsPrice_TextBox.Text)
                }
                );
                var response = client.Execute(request);
                if (response.Content.Contains("POST successful!"))
                {
                    GoodsName_TextBox.Text = "";
                    GoodsCount_TextBox.Text = "";
                    GoodsPrice_TextBox.Text = "";
                }
                listOwn();
            }
            else
            {
                MessageBox.Show("Empty field!");
            }
            
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AdminModify_Button_Click(object sender, RoutedEventArgs e)
        {
            if(AdminGoodsPrice_TextBox.Text != "" && AdminGoodsName_TextBox.Text != "" && AdminGoodsCount_TextBox.Text != "" && selectedGood!=null)
            {
                var client = new RestClient(URL);
                string route = ROUTE + "?modify=" + MainWindow.activeUser.Id;
                var request = new RestRequest(route, Method.PUT);
                request.AddJsonBody(new Good
                {
                    Id = selectedGood.Id,
                    Name = AdminGoodsName_TextBox.Text,
                    Count = Convert.ToInt32(AdminGoodsCount_TextBox.Text),
                    Price = Convert.ToInt32(AdminGoodsPrice_TextBox.Text)
                }
                );
                var response = client.Execute(request);
                if(response.Content.Contains("PUT successful!"))
                {
                    AdminGoodsName_TextBox.Text = "";
                    AdminGoodsCount_TextBox.Text = "";
                    AdminGoodsPrice_TextBox.Text = "";
                    selectedGood = null;
                    List_All();
                }
                else
                {
                    MessageBox.Show(response.Content);
                }
            }
        }

        private void Goods_Editor_Closed(object sender, EventArgs e)
        {
            mainWindow.Show();
        }

        public void listOwn()
        {
            var client = new RestClient(URL);
            string route = ROUTE + "?list_own=" + MainWindow.activeUser.Id;
            var request = new RestRequest(route, Method.GET);

            var response = client.Execute(request);
            goodsList = new JsonSerializer().Deserialize<List<Good>>(response);

            Goods_Datagrid.ItemsSource = goodsList;
        }

        private void ListSelf_Button_Click(object sender, RoutedEventArgs e)
        {
            listOwn();
        }
    }
}
