using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
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
    /// Логика взаимодействия для PageHistory.xaml
    /// </summary>
    public partial class PageHistoryUser : Page
    {
        public static SmartStoreEntities1 DataEntities { get; set; } = new SmartStoreEntities1();
        public ObservableCollection<OrderItem> ListOrderItem { get; set; }
        public ObservableCollection<Orders> ListOrders { get; set; }
        PageBasket pb;
        public PageHistoryUser(PageBasket page)
        {
            InitializeComponent();
            DataEntities = new SmartStoreEntities1();
            ListOrderItem = new ObservableCollection<OrderItem>();
            ListOrders = new ObservableCollection<Orders>();
            DatePicker1.SelectedDate = DateTime.Now;
            pb = page;
        }

        public bool isDirty = true;
        public static bool canSave = true;
        public void GetOrderItem(int ID_Orders)
        {
            ListOrderItem.Clear();
            var queryProduct = (from profiles in DataEntities.OrderItem where profiles.ID_Orders == ID_Orders orderby profiles.ID_OI select profiles).ToList();
            foreach (OrderItem prof in queryProduct)
            {
                ListOrderItem.Add(prof);
            }
            DataGridOrderItem.ItemsSource = ListOrderItem;
        }
        public void GetOrders() 
        {
            ListOrders.Clear();
            var queryOrders = (from profiles in DataEntities.Orders where profiles.ID_Profile == pb.ID_Prof orderby profiles.ID_Orders select profiles).ToList();
            foreach (Orders prof in queryOrders)
            {
                ListOrders.Add(prof);
            }
            DataGridOrders.ItemsSource = ListOrders;
        }
        private void RewriteProfiles()
        {
            DataEntities = new SmartStoreEntities1();
            ListOrderItem.Clear();
            GetOrders();
        }
        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            pb.mw.Content = pb;
        }
        private void UndoCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        
        private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Поиск по фильтру(ам)");
            FindByName((DateTime)this.DatePicker1.SelectedDate , this.TextBox1.Text);
            isDirty = true;


        }
        
        private void FindByName(DateTime date, string adress)
        {
            DataEntities = new SmartStoreEntities1();
            ListOrders.Clear();
            var date2 = date;
            
            date2=date2.AddDays(0.5);
            date = date.AddDays(-0.5);
            Console.WriteLine(date.ToString() + " " + date2.ToString());
            var queryEmployee = (from orders in DataEntities.Orders
                                 where orders.Date >= date && orders.Date <= date2
                                 where orders.Adress.Contains(adress)
                                 where orders.ID_Profile == pb.ID_Prof
                                 select orders).ToList();
            foreach (Orders pf in queryEmployee)
            {
                ListOrders.Add(pf);
            }
            if (ListOrders.Count > 0)
            {
                DataGridOrders.ItemsSource = ListOrders;
            }
            else
            {
                MessageBox.Show("Товыры не найдены", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                GetOrders();
            }
            ListOrderItem.Clear();
        }
        private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetOrders();
            DataGridOrderItem.SelectedIndex = 0;
        }

        private void DataGridOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridOrders.SelectedItem != null)
            {
                GetOrderItem(((Orders)DataGridOrders.SelectedItem).ID_Orders);
            }
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                TextBox1Placeholder.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBox1Placeholder.Visibility = Visibility.Visible;
            }
        }
    }
}
