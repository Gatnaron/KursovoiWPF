using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для PageUsers.xaml
    /// </summary>
    public partial class PageUsers : Page
    {
        public static SmartStoreEntities1 DataEntities { get; set; } = new SmartStoreEntities1();
        public ObservableCollection<Profiles> ListProfiles { get; set; }
        public PageUsers(MainWindow mainWindow)
        {
            InitializeComponent();
            DataEntities = new SmartStoreEntities1();
            ListProfiles = new ObservableCollection<Profiles>();
            mw = mainWindow;
        }
        public bool isDirty = true;
        public static bool canSave = true;
        public MainWindow mw;
        public void GetProduct()
        {
            ListProfiles.Clear();
            var queryProduct = (from profiles in DataEntities.Profiles orderby profiles.ID_Profiles select profiles).ToList();
            foreach (Profiles prof in queryProduct)
            {
                ListProfiles.Add(prof);
            }
            DataGridUsers.ItemsSource = ListProfiles;
        }
        private void RewriteProfiles()
        {
            DataEntities = new SmartStoreEntities1();
            ListProfiles.Clear();
            GetProduct();
        }
        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteProfiles();
            DataGridUsers.IsReadOnly = true;
            isDirty = true;
        }
        private void UndoCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        
        private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Поиск по фильтру(ам)");
            FindByName(this.TextBox2.Text, this.TextBox3.Text);
            isDirty = true;


        }
        private void FindByName(string login, string email)
        {
            DataEntities = new SmartStoreEntities1();
            ListProfiles.Clear();
            var queryEmployee = (from profiles in DataEntities.Profiles
                                 where profiles.Login.Contains(login)
                                 where profiles.Email.Contains(email)
                                 select profiles).ToList();
            foreach (Profiles pf in queryEmployee)
            {
                ListProfiles.Add(pf);
            }
            if (ListProfiles.Count > 0)
            {
                DataGridUsers.ItemsSource = ListProfiles;
            }
            else
            {
                MessageBox.Show("Товыры не найдены", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                GetProduct();
            }
        }
        private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetProduct();
            DataGridUsers.SelectedIndex = 0;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Page Menu = new PageMenuAdmin(mw);
            mw.Content = Menu;
        }
        private void TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox2.Text != "")
            {
                TextBox2Placeholder.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBox2Placeholder.Visibility = Visibility.Visible;
            }
        }

        private void TextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox3.Text != "")
            {
                TextBox3Placeholder.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBox3Placeholder.Visibility = Visibility.Visible;
            }
        }
    }
}
