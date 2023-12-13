using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace KursovoiWPF
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {

        public AuthWindow()
        {
            InitializeComponent();
            DoubleAnimation btnAnimation = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 230;
            btnAnimation.Duration = TimeSpan.FromSeconds(3);
            loginButton.BeginAnimation(Button.WidthProperty, btnAnimation);
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            AuthLog auth = new AuthLog();
            Profiles profiles = new Profiles();

            if (regLoginBox.Text.Trim() != "" &&
               regPasswordBox1.Text.Trim() != "" &&
               regEmailBox.Text.Trim() != "") 
            { 
                if (regPasswordBox1.Text.Trim() == regPasswordBox2.Text.Trim())
                { 
                    profiles.Email = regEmailBox.Text.Trim();
                    profiles.Login = regLoginBox.Text.Trim();
                    profiles.Password = BCrypt.Net.BCrypt.HashPassword(regPasswordBox1.Text.Trim());

                    auth.Register(profiles);
                    MessageBox.Show("Пользователь успешно зарегестрирован!");
                }
                else MessageBox.Show("Пароли не совпадают!");
            }
            else MessageBox.Show("Данные не заполнены!");
        }
        private void loginButton_Click(Object sender, RoutedEventArgs e)
        {
            AuthLog auth = new AuthLog();
            int id = auth.Login(logLoginBox.Text.Trim(), logPasswordBox.Password.Trim());
            if (id != -1) 
            {
                MessageBox.Show("Добро пожаловать " + logLoginBox.Text.Trim());
                MainWindow mw = new MainWindow(id);
                this.Hide();
                mw.ShowDialog();
                this.Show();
            }
            else MessageBox.Show("Неверный логин или пароль!");
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            DoubleAnimation btnAnim = new DoubleAnimation();
            btnAnim.From = 142;
            btnAnim.To = 225;
            btnAnim.Duration = TimeSpan.FromSeconds(0.1);
            button.BeginAnimation(Button.WidthProperty, btnAnim);
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            DoubleAnimation btnAnim = new DoubleAnimation();
            btnAnim.From = button.Width;
            btnAnim.To = 142;
            btnAnim.Duration = TimeSpan.FromSeconds(0.1);
            button.BeginAnimation(Button.WidthProperty, btnAnim);
        }
    }
}
