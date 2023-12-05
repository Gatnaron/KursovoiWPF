using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
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
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageBasket : Page
    {
        public static SmartStoreEntities1 DataEntities { get; set; } = new SmartStoreEntities1();
        public ObservableCollection<BasketItem> ListProduct { get; set; }
        public PageBasket(int ID_Prof, PageShop ps)
        {
            InitializeComponent();
            DataEntities = new SmartStoreEntities1();
            ListProduct = new ObservableCollection<BasketItem>();
            this.ID_Prof = ID_Prof;
            this.ps = ps;
            this.mw = ps.mw;
            AllPrice.Text = GetSum;
        }

        public MainWindow mw;
        public PageShop ps;
        public int ID_Prof;
        public bool isDirty = true;
        public static bool canSave = true;

        public string GetSum { get { return "Сумма: " + SumPrice(); } }

        public void GetProduct()
        {
            ListProduct.Clear();
            var queryProduct = (from product in DataEntities.BasketItem where product.ID_Profiles == ID_Prof orderby product.ID_P select product).ToList();
            foreach (BasketItem prod in queryProduct)
            {
                ListProduct.Add(prod);
            }
            DataGridProd.ItemsSource = ListProduct;
            AllPrice.Text = GetSum;
        }
        private void RewriteProduct()
        {
            DataEntities = new SmartStoreEntities1();
            ListProduct.Clear();
            GetProduct();
        }
        
        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BasketItem product = DataGridProd.SelectedItem as BasketItem;
            MessageBoxResult result = MessageBox.Show("Удалить данные", "Предупреждение", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {

                DataEntities.BasketItem.Remove(product);
                DataEntities.SaveChanges();
                DataGridProd.SelectedIndex = DataGridProd.SelectedIndex == 0 ? 1 : DataGridProd.SelectedIndex - 1;
                ListProduct.Remove(product);

                DataEntities.SaveChanges();
                AllPrice.Text = GetSum;
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
            FindByName(this.TextBox1.Text, this.TextBox2.Text, this.TextBox3.Text);
            isDirty = true;


        }
        private void FindByName(string name, string brand, string code)
        {/*
            DataEntities = new SmartStoreEntities();
            ListProduct.Clear();
            var queryEmployee = (from product in DataEntities.BasketItem
                                 where product.Name.Contains(name)
                                 where product.Brand.Contains(brand)
                                 where product.code.Contains(code)
                                 select product).ToList();
            foreach (BasketItem pd in queryEmployee)
            {
                ListProduct.Add(pd);
            }
            if (ListProduct.Count > 0)
            {
                DataGridProd.ItemsSource = ListProduct;
            }
            else
            {
                MessageBox.Show("Товыры не найдены", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                GetProduct();
            }*/
        }
        private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetProduct();
            DataGridProd.SelectedIndex = 0;
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            Orders orders = new Orders();
            orders.ID_Profile = ID_Prof;
            orders.Adress = TextBoxAdress.Text;
            orders.Date = System.DateTime.Now;
            DataEntities.Orders.Add(orders);
            DataEntities.SaveChanges();
            var queryOrder = (from product in DataEntities.Orders where product.ID_Profile == ID_Prof orderby product.ID_Orders select product).ToList();
            var order = queryOrder.Last();
            var queryProduct = (from product in DataEntities.BasketItem where product.ID_Profiles == ID_Prof orderby product.ID_P select product).ToList();
            foreach(var product in queryProduct)
            {
                var OI = new OrderItem();
                OI.ID_Orders = order.ID_Orders;
                OI.ID_P = product.ID_P;
                OI.Price = product.Product.Price;
                DataEntities.OrderItem.Add(OI);
                DataEntities.BasketItem.Remove(product);
            }
            DataEntities.SaveChanges();
            GetProduct();
        }
        private int SumPrice()
        {
            int sum = 0;
            foreach(var i in ListProduct)
            {
                sum +=(int) i.Product.Price;
            }
            return sum;
        }
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            ps.mw.Content = ps;
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            var pb = new PageHistoryUser(this);
            mw.Content = pb;
        }
    }
}
