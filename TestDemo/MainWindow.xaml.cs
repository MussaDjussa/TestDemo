using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TestDemo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Product> collection = new ObservableCollection<Product>();
        public ObservableCollection<ProductType> collection2 = new ObservableCollection<ProductType>();

        

        public ICollectionView collectionView;
        public MainWindow()
        {

            foreach (var item in App.db.Product)
            {
                collection.Add(item);
            }

            foreach (var item in App.db.ProductType)
            {
                collection2.Add(item);
            }

            InitializeComponent();
            list.ItemsSource = collection;

            collectionView = CollectionViewSource.GetDefaultView(collection);
            collectionView.Filter = Method;

            typeProduct.ItemsSource = collection2;
            
            typeProduct.DisplayMemberPath = "Title";

            collection2.Insert(0, new ProductType() { Title = "Все типы"});
        }

        private bool Method(object obj)
        {
            var value = obj as Product;
            if(Regex.IsMatch(value.Title, textBox.Text))
            {
                return true;
            }
            return false;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            collectionView.Refresh();
        }

        private void sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sort.SelectedIndex == 0)
            {
                collectionView.SortDescriptions.Clear();
                collectionView.Refresh();

            }
            if (sort.SelectedIndex == 1)
            {
                collectionView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                collectionView.Refresh();
            }
            if (sort.SelectedIndex == 2)
            {
                collectionView.SortDescriptions.Add(new SortDescription("ProductionPersonCount", ListSortDirection.Ascending));
                collectionView.Refresh();
            }
            if (sort.SelectedIndex == 3)
            {
                collectionView.SortDescriptions.Add(new SortDescription("MinCostForAgent", ListSortDirection.Ascending));
                collectionView.Refresh();
            }
        }

        private void typeProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = typeProduct.SelectedItem as ProductType;

            if (selectedItem != null)
            {
                list.ItemsSource = collection.Where(q => q.ProductTypeID == selectedItem.ID);
                collectionView = CollectionViewSource.GetDefaultView(list.Items) ;
                collectionView.Filter = Method;
                collectionView.Refresh();
            }
        }
    }
}
