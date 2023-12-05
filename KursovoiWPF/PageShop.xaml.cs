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
    /// Логика взаимодействия для PageShop.xaml
    /// </summary>
    public partial class PageShop : Page
    {
        public static SmartStoreEntities1 DataEntities { get; set; } = new SmartStoreEntities1();
        public ObservableCollection<Product> ListProduct { get; set; }
        public PageShop(int ID_P, MainWindow mainWindow)
        {
            InitializeComponent();
            DataEntities = new SmartStoreEntities1();
            ListProduct = new ObservableCollection<Product>();
            ID_Prof = ID_P;
            mw = mainWindow;
        }
        public MainWindow mw;
        public int ID_Prof;
        public bool isDirty = true;
        public static bool canSave = true;

        public void GetProduct()
        {
            ListProduct.Clear();
            var queryProduct = (from product in DataEntities.Product orderby product.ID_P select product).ToList();
            foreach (Product prod in queryProduct)
            {
                ListProduct.Add(prod);
            }
            DataGridProd.ItemsSource = ListProduct;
        }
        private void RewriteProduct()
        {
            DataEntities = new SmartStoreEntities1();
            ListProduct.Clear();
            GetProduct();
        }
        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteProduct();
            DataGridProd.IsReadOnly = true;
            isDirty = true;
        }
        private void UndoCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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
        {
            DataEntities = new SmartStoreEntities1();
            ListProduct.Clear();
            var queryEmployee = (from product in DataEntities.Product
                                 where product.Name.Contains(name)
                                 where product.Brand.Contains(brand)
                                 where product.code.Contains(code)
                                 select product).ToList();
            foreach (Product pd in queryEmployee)
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
            }
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

        private void AddToBasket_Click(object sender, RoutedEventArgs e)
        {
            var basketitem = new List<BasketItem>();
            var h1 = DataGridProd.SelectedItems;
            var bi =new List<Product>();
            foreach (var h in h1) 
            {
                bi.Add(h as Product);
            }
            
            
            for (int i = 0; i < bi.Count; i++) { 
                var temp = new BasketItem();
                temp.ID_Profiles = ID_Prof;
                temp.ID_P = bi[i].ID_P;
                basketitem.Add(temp);
            }
            try
            {
                foreach(var i in basketitem)
                    DataEntities.BasketItem.AddOrUpdate(i);
                MessageBox.Show("Добавили в корзину");
                isDirty = false;
                DataEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Ошибка добавления товара" + ex.ToString());
            }
        }

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            Page Basket = new PageBasket(ID_Prof, this);
            mw.Content = Basket;
        }
    }
}