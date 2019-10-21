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
        #endregion

        #region Вложенность
        /// <summary>
        /// Когда в директории есть подпапки, поиск вложенных файлов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Инициализация проверок
            var item = (TreeViewItem)sender;
            //Если в item содержатся некорректные данные
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            //Очистка
            item.Items.Clear();

            //Получаем полный путь к папке
            var fullPath = (string)item.Tag;

            #endregion

            #region Получение директорий
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
                //Создание элемента папки
                var subItem = new TreeViewItem()
                {
                    //Установка Header как имя папки
                    Header = GetFileFolderName(directoryPath),
                    //И tag как полный путь
                    Tag = directoryPath
                };

                //Добавляем пустой элемент чтобы 
                subItem.Items.Add(null);

                //Добавляем для ручного "расширения" директории
                subItem.Expanded += Folder_Expanded;

                //Добавление элемента  
                item.Items.Add(subItem);
            });
            #endregion

            #region Получение файлов

            //Список путей
            var files = new List<string>();


            try
            {
                //Получаем полный путь к файлу
                var fs = Directory.GetFiles(fullPath);

                //Если путь корректный, добавляем в список
                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch { }

            //Для фаайла ищем вложенные файлы
            files.ForEach(filePath =>
            {
                //Создание элемента файла
                var subItem = new TreeViewItem()
                {
                    //Установка Header как имя файла
                    Header = GetFileFolderName(filePath),
                    //И tag как полный путь
                    Tag = filePath
                };

                //Добавление элемента  
                item.Items.Add(subItem);
            });

            #endregion
        }

        /// <summary>
        /// Поиск имени папки или файла по полному пути
        /// </summary>
        /// <param name="path"> Полный путь</param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            //Если нет пути, то возвращаем пустую строку
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            //Меняем слеши на обратные
            var normalizedPath = path.Replace('/','\\');

            //Поиск последнего слеша в пути
            var lastindex = normalizedPath.LastIndexOf('\\');

            //Если не находим обратный слеш, то возвращаем сам путь
            if (lastindex <= 0)
                return path;

            //Возвращаем имя после обратного слеша
            return path.Substring(lastindex + 1);
        }
        #endregion
    }
}
