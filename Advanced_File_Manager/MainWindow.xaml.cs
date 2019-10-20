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
using System.IO;

namespace Advanced_File_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Конструктор
        /// <summary>
        /// Дефолтный конструктор
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Запуск
        /// <summary>
        /// Первый запуск приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Получение всех жестких дисков компьютера
            foreach (var drive in Directory.GetLogicalDrives())
            {
                //Создание нового элемента для этого
                var item = new TreeViewItem();

                //Установка заголовка и пути
                item.Header = drive;
                item.Tag = drive;

                item.Items.Add(null);

                //Добавление элемента в tree-view
                FolderView.Items.Add(item);
            }
        }
        #endregion
    }
}
