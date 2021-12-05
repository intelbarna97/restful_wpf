using System;
using System.Collections.Generic;
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
using RestSharp;
using WPF_RESTful.entity;
using JsonSerializer = RestSharp.Serialization.Json.JsonSerializer;

namespace WPF_RESTful
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static String URL = "http://localhost/wpfapi";
        static String ROUTE = "login.php";
        public static bool isLoggedIn = false;
        public static User activeUser = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login();
            if (isLoggedIn)
            {
                MainLogin_Button.Visibility = Visibility.Hidden;
                MainLogout_Button.Visibility = Visibility.Visible;
                Uname_TextBlock.Visibility = Visibility.Hidden;
                Pwd_TextBlock.Visibility = Visibility.Hidden;
                TextBox_username.Visibility = Visibility.Hidden;
                PasswordBox_password.Visibility = Visibility.Hidden;
                TextBox_username.Text = "";
                PasswordBox_password.Password = "";
                Welcome_TextBlock.Text = "Welcome " + activeUser.Username + "!";
                Welcome_TextBlock.Visibility = Visibility.Visible;
                Manage_Goods_Button.Visibility = Visibility.Visible;
                Username_Border.Visibility = Visibility.Hidden;
                Password_Border.Visibility = Visibility.Hidden;

                if (activeUser.isAdmin())
                {
                    Welcome_TextBlock.Text = Welcome_TextBlock.Text + ("(admin)");
                }
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Logout();
            if (!isLoggedIn)
            {
                MainLogout_Button.Visibility = Visibility.Hidden;
                MainLogin_Button.Visibility = Visibility.Visible;
                Uname_TextBlock.Visibility = Visibility.Visible;
                Pwd_TextBlock.Visibility = Visibility.Visible;
                TextBox_username.Visibility = Visibility.Visible;
                PasswordBox_password.Visibility = Visibility.Visible;
                Welcome_TextBlock.Visibility = Visibility.Hidden;
                Manage_Goods_Button.Visibility= Visibility.Hidden;
                Username_Border.Visibility= Visibility.Visible;
                Password_Border.Visibility= Visibility.Visible;
                Welcome_TextBlock.Text = "";
                TextBox_username.Text = "";
                PasswordBox_password.Password = "";
            }
        }

        private void Login()
        {
            if(TextBox_username.Text=="" || PasswordBox_password.Password=="")
            {
                MessageBox.Show("Enter username and password");
            }
            else
            {
                var client = new RestClient(URL);
                string route = ROUTE + "?login=try";
                var request = new RestRequest(route, Method.POST);
                request.AddJsonBody(new User
                {
                    Username = TextBox_username.Text,
                    Password = PasswordBox_password.Password
                });

                var response = client.Execute(request);

                if(response.Content.Contains("\"username\":\""+TextBox_username.Text+"\""))
                {
                    isLoggedIn = true;
                    activeUser = new JsonSerializer().Deserialize<User>(response);
                }
                else
                {
                    MessageBox.Show(response.Content);
                }


            }

        }

        public static void Logout()
        {
            if(isLoggedIn)
            {
                var client = new RestClient(URL);
                string route = ROUTE + "?logout="+activeUser.Id;
                var request = new RestRequest(route, Method.POST);

                var response = client.Execute(request);

                if (response.Content.Contains("Logout successful!"))
                {
                    isLoggedIn = false;
                    activeUser = null;
                }
            }
        }

        private void Manage_Goods_Button_Click(object sender, RoutedEventArgs e)
        {
            if(isLoggedIn)
            {
                Goods good = new Goods(this);
                good.Show();
                this.Hide();
            }
        }

        private void ExitApp_Button_Click(object sender, RoutedEventArgs e)
        {
            Logout();
            this.Close();
        }
    }
}
