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
                var item = new TreeViewItem()
                {
                    //Установка имени
                    Header = drive,
                    //Установка полного пути
                    Tag = drive
                }; 

                item.Items.Add(null);
                item.Expanded += Folder_Expanded;

                //Добавление элемента в tree-view
                FolderView.Items.Add(item);
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            //Если в item содержатся некорректные данные
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            //Очистка
            item.Items.Clear();

            //Получаем полный путь к папке
            var fullPath = (string)item.Tag;

            //Список путей
            var directories = new List<string>();


            try
            {
                //Получаем полный путь к директории
                var dirs= Directory.GetDirectories(fullPath);

                //Если путь корректный, добавляем в список
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch{}

            //Для директории ищем вложенные файлы
            directories.ForEach(directoryPath =>
            {
                var subItem = new TreeViewItem();


            });
        }
        #endregion
    }
}
