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
        public PageUsers()
        {
            InitializeComponent();
            DataEntities = new SmartStoreEntities1();
            ListProfiles = new ObservableCollection<Profiles>();
        }
        public bool isDirty = true;
        public static bool canSave = true;
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
        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Сохранено");
            DataEntities.SaveChanges();

            canSave = true;
            DataGridUsers.IsReadOnly = true;
        }
        private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = canSave;

        }
        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Profiles profiles = DataGridUsers.SelectedItem as Profiles;
            MessageBoxResult result = MessageBox.Show("Удалить данные", "Предупреждение", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {

                DataEntities.Profiles.Remove(profiles);
                DataGridUsers.SelectedIndex = DataGridUsers.SelectedIndex == 0 ? 1 : DataGridUsers.SelectedIndex - 1;
                ListProfiles.Remove(profiles);
            }
            else
            {
                MessageBox.Show("Выберете строку для удаления");
            }
        }
        private void DeleteCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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
    }
}
