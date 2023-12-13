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

namespace KursovoiWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int id = -1;
        bool admin = false;
        public bool logout = false;
        public MainWindow(int id, bool admin = false)
        {
            InitializeComponent();
            this.id = id;
            this.admin = admin;
        }
        private void frame1_Loaded(object sender, RoutedEventArgs e)
        {
            if (id == 4)
            {
                var page = new PageMenuAdmin(this);
                this.Content = page;
            }
            else
            {
                var pageS = new PageShop(id, this);
                this.Content = pageS;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if(logout == false)
            {
                Application.Current.Shutdown();
            }
        }
        private void frame1_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
