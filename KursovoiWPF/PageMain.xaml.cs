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
    public partial class PageMain : Page
    {
        public static SmartStoreEntities1 DataEntities { get; set; } = new SmartStoreEntities1();
        public ObservableCollection<Product> ListProduct { get; set; }
        public PageMain(MainWindow mainWindow)
        {
            InitializeComponent();
            DataEntities = new SmartStoreEntities1();
            ListProduct = new ObservableCollection<Product>();
            mw = mainWindow;
        }
        public MainWindow mw;
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
        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Сохранено");
            DataEntities.SaveChanges();

            canSave = true;
            DataGridProd.IsReadOnly = true;
        }
        private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = canSave;

        }
        private void EditCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGridProd.IsReadOnly = false;
            DataGridProd.BeginEdit();
            isDirty = true;
        }
        private void EditCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Product product = DataGridProd.SelectedItem as Product;
            MessageBoxResult result = MessageBox.Show("Удалить данные", "Предупреждение", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {

                DataEntities.Product.Remove(product);
                DataEntities.SaveChanges();
                DataGridProd.SelectedIndex = DataGridProd.SelectedIndex == 0 ? 1 : DataGridProd.SelectedIndex - 1;
                ListProduct.Remove(product);
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
        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Product product = new Product();
            product.Name = "Название";
            product.Price = 0;
            product.Brand = "Фирма";
            product.ComProtocol = "ПротоколС";
            product.code = "КодТ";
            try
            {
                DataEntities.Product.AddOrUpdate(product);
                ListProduct.Add(product);
                MessageBox.Show("Создание");
                isDirty = false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Ошибка добавления данных" + ex.ToString());
            }
        }
        private void NewCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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
        private void CutCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGridProd.IsReadOnly = false;
            DataGridProd.BeginEdit();
            MessageBox.Show("Редактирование");
            isDirty = true;
        }
        private void CutCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetProduct();
            DataGridProd.SelectedIndex = 0;
        }

        

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Page Menu = new PageMenuAdmin(mw);
            mw.Content = Menu;
        }

        
    }
}
