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
    /// Логика взаимодействия для PageMenuAdmin.xaml
    /// </summary>
    public partial class PageMenuAdmin : Page
    {
        public PageMenuAdmin(MainWindow mainWindow)
        {
            InitializeComponent();
            mw = mainWindow;
        }
        public MainWindow mw;

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            Page Users = new PageUsers(mw);
            mw.Content = Users;
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            Page Shop = new PageMain(mw);
            mw.Content = Shop;
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            Page History = new PageHistory(mw);
            mw.Content = History;
        }
    }
}
