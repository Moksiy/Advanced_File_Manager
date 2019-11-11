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
    /// Класс с автосвойствами для хранения информации о файлах
    /// </summary>
    public class Data
    {
        #region Поля

        //Полный путь файла
        private string filePath { get; set; } = null;


        //Тип файла
        private string fileType { get; set; }


        //Буфер (при копировании или перемещении) <-помещается полный путь к файлу
        private string buffer { get; set; } = null;


        //Дополнительная информация о файле

        #endregion

        #region Методы взаимодействия

        /// <summary>
        /// Установка пути файла
        /// </summary>
        public void addFilePath(string path)
        {
            this.filePath = path;
        }

        /// <summary>
        /// Получение пути файла
        /// </summary>
        /// <returns></returns>
        public string getFilePath()
        {
            return this.filePath;
        }

        /// <summary>
        /// Установка типа файла
        /// </summary>
        /// <param name="type"></param>
        public void addFileType(string type)
        {
            this.fileType = type;
        }

        /// <summary>
        /// Получение типа файла
        /// </summary>
        /// <returns></returns>
        public string getFileType()
        {
            return this.fileType;
        }

        #endregion
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Экземпляр класса Data, где хранится информация о выделенном файле
        Data data = new Data();

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
                FolderView2.Items.Add(item);
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
                var dirs = Directory.GetDirectories(fullPath);

                //Если путь корректный, добавляем в список
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }

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
        /// Когда в директории есть подпапки, поиск вложенных файлов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded2(object sender, RoutedEventArgs e)
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
                var dirs = Directory.GetDirectories(fullPath);

                //Если путь корректный, добавляем в список
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }

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
                subItem.Expanded += Folder_Expanded2;

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
            var normalizedPath = path.Replace('/', '\\');

            //Поиск последнего слеша в пути
            var lastindex = normalizedPath.LastIndexOf('\\');

            //Если не находим обратный слеш, то возвращаем сам путь
            if (lastindex <= 0)
                return path;

            //Возвращаем имя после обратного слеша
            return path.Substring(lastindex + 1);
        }
        #endregion


        #region Хурма

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolutionTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem SelectedItem = FolderView.SelectedItem as TreeViewItem;
            //Отправка полного пути файла в коллекцию
            if (SelectedItem.Tag.ToString() != null)
            {
                data.addFilePath(SelectedItem.Tag.ToString());
            }

            //Определение типа файла


        }

        #endregion


        #region Вызов контекстного меню
        /// <summary>
        /// Вызов контекстного меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openContextMenu(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ContextMenu cm = FolderView.FindResource("File") as ContextMenu;
                cm.IsOpen = true;
            }
        }

        #endregion


        #region Вставить
        /// <summary>
        /// Вставка файла
        /// </summary>
        /// <param name="sender2"></param>
        /// <param name="e2"></param>
        private void PasteFile(object sender2, RoutedEventArgs e2)
        {
            string copied = @"";                         //Прикрутить передачу пути   <---------------!
            string copyTo = @"";                      //Прикрутить передачу пути   <---------------!
            FileInfo fileInf = new FileInfo(copied);
            if (fileInf.Exists)
            {
                File.Copy(copied, copyTo, true);
            }
        }

        private void PasteDir(object sender2, RoutedEventArgs e2)
        {
            string copied = @"";                         //Прикрутить передачу пути   <---------------!
            string copyTo = @"";                      //Прикрутить передачу пути   <---------------!
            //Ппц тут писать
        }

        #endregion


        #region Копировать
        /// <summary>
        /// Копирование файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyFile(object sender, RoutedEventArgs e)
        {
            //Прикрутить получение пути копируемого файла    <-------------------!
        }

        /// <summary>
        /// Копирование каталога
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyDir(object sender, RoutedEventArgs e)
        {
            //Прикрутить получение пути копируемого каталога    <-------------------!           
        }

        #endregion


        #region Вырезать
        /// <summary>
        /// Перемещение файла   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutFile(object sender, RoutedEventArgs e)
        {
            string cutied = @"";                      //Прикрутить передачу пути   <---------------!
            string cutTo = @"";                      //Прикрутить передачу пути   <---------------!
            FileInfo fileInf = new FileInfo(cutied);
            if (fileInf.Exists)
            {
                File.Move(cutied, cutTo);
            }
        }

        /// <summary>
        /// Перемещение каталога
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutDir(object sender, RoutedEventArgs e)
        {
            string cutied = @"";                      //Прикрутить передачу пути   <---------------!
            string cutTo = @"";                      //Прикрутить передачу пути   <---------------!

        }

        #endregion


        #region Переименовать
        /// <summary>
        /// Переименование файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameFile(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Переименование директории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameDir(object sender, RoutedEventArgs e)
        {

        }

        #endregion


        #region Удалить
        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveFile(object sender, RoutedEventArgs e)
        {
            string path = @"";                      //Прикрутить передачу пути   <---------------!
            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists)
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Удаление папки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDir(object sender, RoutedEventArgs e)
        {
            string dirName = @"C:\SomeFolder";     //Прикрутить передачу пути   <---------------!
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirName);
                dirInfo.Delete(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion


        #region Открыть файл
        /// <summary>
        /// Открытие файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile(object sender, RoutedEventArgs e)
        {

        }


        #endregion


        #region Создать папку
        /// <summary>
        /// Создание каталога
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateDir(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }


}














































































































#region Ficha
//              (__)
//              (oo)
//        /------\/
//       / |   || 
//      * /\---/\
//        ~~   ~~
//..."Have you mooed today?"....
#endregion
