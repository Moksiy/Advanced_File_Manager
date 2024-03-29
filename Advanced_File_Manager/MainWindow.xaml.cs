﻿using System;
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
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace Advanced_File_Manager
{

    /// <summary>
    /// Класс с автосвойствами для хранения информации о файлах
    /// </summary>
    public class Data
    {
        #region Поля

        //Полный путь файла
        private string filePath { get; set; } = "";


        //Тип файла
        private string fileType { get; set; } = "";


        //Буфер (при копировании или перемещении) <-помещается полный путь к файлу
        private string buffer { get; set; } = "";


        //Тип файла, находящегося в буффере
        private string bufferFileType { get; set; } = "";


        //Имя файла, находящегося в буффере
        private string bufferFileName { get; set; } = "";

        public bool isUpdate { get; set; } = false;


        //Имя файла для создания
        private string createFileName { get; set; } = "";


        //Булева переменная для определения действия над файлом скопировать\вырезать
        private bool isCut { get; set; } = false;



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

        /// <summary>
        /// Добавление пути копируемого файла в буфер
        /// </summary>
        /// <param name="path"></param>
        public void addPathToBuffer(string path)
        {
            this.buffer = path;
        }

        /// <summary>
        /// Возвращение пути копируемого файла из буффера
        /// </summary>
        /// <returns></returns>
        public string getPathFromBuffer()
        {
            return this.buffer;
        }

        /// <summary>
        /// Добавление типа копируемого файла
        /// </summary>
        /// <param name="type"></param>
        public void addFileTypeInBuffer(string type)
        {
            this.bufferFileType = type;
        }

        /// <summary>
        /// Получение типа копируемого файла
        /// </summary>
        /// <returns></returns>
        public string getFileTypeInBuffer()
        {
            return this.bufferFileType;
        }

        /// <summary>
        /// Добавление имени копируемого файла
        /// </summary>
        /// <param name="name"></param>
        public void addBufferFileName(string name)
        {
            this.bufferFileName = name;
        }

        /// <summary>
        /// Получение имени копируемого файла
        /// </summary>
        /// <returns></returns>
        public string getBufferFileName()
        {
            return this.bufferFileName;
        }

        /// <summary>
        /// Добавление имени добавляемой папки
        /// </summary>
        /// <param name="name"></param>
        public void addCreateFileName(string name)
        {
            this.createFileName = name;
        }

        /// <summary>
        /// Получение имени добавляемой папки
        /// </summary>
        /// <returns></returns>
        public string getCreateFileName()
        {
            return createFileName;
        }

        /// <summary>
        /// Добавление результата
        /// </summary>
        /// <param name="cut"></param>
        public void addIsCut(bool cut)
        {
            this.isCut = cut;
        }

        /// <summary>
        /// Возвращение переменной
        /// </summary>
        /// <returns></returns>
        public bool getIsCut()
        {
            return this.isCut;
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
            if (data.isUpdate) { item.Items.Clear(); }

            //Если в item содержатся некорректные данные
            if ((item.Items.Count != 1 || item.Items[0] != null) && data.isUpdate == false)
            {
                return;
            }
            else { data.isUpdate = false; }

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
            if (item.Tag.ToString().Length > 4) { data.isUpdate = true; }
            if (data.isUpdate) { item.Items.Clear(); }

            //Если в item содержатся некорректные данные
            if ((item.Items.Count != 1 || item.Items[0] != null) && data.isUpdate == false)
            {
                return;
            }
            else { data.isUpdate = false; }

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

        public static string GetFilePath(string path)
        {
            //Если нет пути, то возвращаем пустую строку
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            //Поиск последнего слеша в пути
            var lastindex = path.LastIndexOf('\\');

            string result = path.Substring(0, lastindex);

            result += "\\";

            return result;
        }

        public static string GetExpansion(string path)
        {
            //Если нет пути, то возвращаем пустую строку
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            //Поиск последнего слеша в пути
            var lastindex = path.LastIndexOf('.');

            if (lastindex < 1) { return ""; }

            string result = path.Substring(lastindex);

            return result;
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

            string path = "";

            try { path = SelectedItem.Tag.ToString(); } catch { }

            //Отправка полного пути файла в коллекцию
            if (path != "")
            {
                data.addFilePath(SelectedItem.Tag.ToString());

                //Полный путь к файлу
                path = SelectedItem.Tag.ToString();

                //Имя файла
                var nameOfFile = MainWindow.GetFileFolderName(path);


                //
                //Определение типа файла
                //

                //Если имя пустое, то это диск
                if (string.IsNullOrEmpty(nameOfFile))
                    data.addFileType("disk");

                //Скрытая папка
                else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Hidden))
                    data.addFileType("hidden");

                //Директория
                else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                    data.addFileType("directory");

                //Файл
                else data.addFileType("file");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolutionTree_SelectedItemChanged2(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem SelectedItem = FolderView2.SelectedItem as TreeViewItem;

            string path = "";

            //Полный путь к файлу
            try { path = SelectedItem.Tag.ToString(); } catch { }

            //Отправка полного пути файла в коллекцию
            if (path != "")
            {
                data.addFilePath(SelectedItem.Tag.ToString());

                //Имя файла
                var nameOfFile = MainWindow.GetFileFolderName(path);


                //
                //Определение типа файла
                //

                //Если имя пустое, то это диск
                if (string.IsNullOrEmpty(nameOfFile))
                    data.addFileType("disk");

                //Скрытая папка
                else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Hidden))
                    data.addFileType("hidden");

                //Директория
                else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                    data.addFileType("directory");

                //Файл
                else data.addFileType("file");


            }
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
                //Логика выбора контекстного меню

                ContextMenu cm = null;

                if (data.getPathFromBuffer() == "" && data.getFileType() == "directory")
                    cm = FolderView.FindResource("folder") as ContextMenu;
                else if (data.getPathFromBuffer() == "" && data.getFileType() == "file")
                    cm = FolderView.FindResource("File") as ContextMenu;
                else if (data.getPathFromBuffer() != "" && data.getFileType() == "directory")
                    cm = FolderView.FindResource("folderCopied") as ContextMenu;
                else if (data.getPathFromBuffer() != "" && data.getFileType() == "file")
                    cm = FolderView.FindResource("FileCoppied") as ContextMenu;

                if (cm != null)
                {
                    //Вызов
                    cm.IsOpen = true;
                }
            }
            else { Functional(sender, e); }
        }

        /// <summary>
        /// Вызов контекстного меню для 2 окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openContextMenu2(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Логика выбора контекстного меню
                ContextMenu cm = null;

                if (data.getPathFromBuffer() == "" && data.getFileType() == "directory")
                    cm = FolderView2.FindResource("folder") as ContextMenu;
                else if (data.getPathFromBuffer() == "" && data.getFileType() == "file")
                    cm = FolderView2.FindResource("File") as ContextMenu;
                else if (data.getPathFromBuffer() != "" && data.getFileType() == "directory")
                    cm = FolderView2.FindResource("folderCopied") as ContextMenu;
                else if (data.getPathFromBuffer() != "" && data.getFileType() == "file")
                    cm = FolderView2.FindResource("FileCoppied") as ContextMenu;

                if (cm != null)
                {
                    //Вызов
                    cm.IsOpen = true;
                }
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
            if (data.getIsCut() == false)
            {
                if (data.getFileTypeInBuffer() == "file" && data.getFileType() == "directory")
                {
                    string copied = data.getPathFromBuffer();
                    string copyTo = data.getFilePath() + "\\" + data.getBufferFileName();
                    FileInfo fileInf = new FileInfo(copied);
                    if (fileInf.Exists)
                    {
                        try
                        {
                            File.Copy(copied, copyTo, true);
                        }
                        catch (System.IO.IOException) //Если файл уже содержится
                        {
                            MessageBox.Show("Файл уже содержится");
                        }
                    }
                }
                else if (data.getFileTypeInBuffer() == "directory")
                {
                    //доделат
                }
                //Вырезать
            }
            else if (data.getIsCut() == true)
            {   //Файл
                if (data.getFileTypeInBuffer() == "file")
                {
                    CutFile(null, null);
                    //Директория
                }
                else if (data.getFileTypeInBuffer() == "directory")
                {
                    CutFile(null, null);
                }
            }
            //Обнуление после перемещения
            if (data.getIsCut() == true)
            {
                data.addPathToBuffer("");
                data.addIsCut(false);
            }
            data.isUpdate = true;
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
            //Передача в буффер пути выбранного файла
            data.addPathToBuffer(data.getFilePath());

            //Передача типа копируемого файла
            data.addFileTypeInBuffer(data.getFileType());

            //Передача имени копируемого файла
            data.addBufferFileName(MainWindow.GetFileFolderName(data.getFilePath()));

            //Передача типа перемещение файла
            data.addIsCut(false);
        }

        private void CutFile_(object sender, RoutedEventArgs e)
        {
            //Передача в буффер пути выбранного файла
            data.addPathToBuffer(data.getFilePath());

            //Передача типа копируемого файла
            data.addFileTypeInBuffer(data.getFileType());

            //Передача имени копируемого файла
            data.addBufferFileName(MainWindow.GetFileFolderName(data.getFilePath()));

            //Передача типа перемещение файла
            data.addIsCut(true);
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
            if (data.getFileTypeInBuffer() == "file")
            {
                string cutied = data.getPathFromBuffer();
                string cutTo = data.getFilePath() + "\\" + data.getBufferFileName();
                FileInfo fileInf = new FileInfo(cutied);
                if (fileInf.Exists)
                {
                    File.Move(cutied, cutTo);
                }
            }
            else if (data.getFileTypeInBuffer() == "directory")
            {
                string oldPath = data.getPathFromBuffer();
                string newPath = data.getFilePath() + "\\" + data.getBufferFileName();
                DirectoryInfo dirInfo = new DirectoryInfo(oldPath);
                if (dirInfo.Exists && Directory.Exists(newPath) == false)
                {
                    dirInfo.MoveTo(newPath);
                }
            }
            data.addPathToBuffer("");
            data.addIsCut(false);
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
            string name = Microsoft.VisualBasic.Interaction.InputBox("Введите новое название файла:");
            if (name != null && name != "") { data.addCreateFileName(name); }
            string path = data.getFilePath();
            string newpath = GetFilePath(path) + data.getCreateFileName();
            if (data.getFileType() == "directory")
            {
                try
                {
                    System.IO.Directory.Move(path, newpath);
                }
                catch { MessageBox.Show("Ошибка переименования каталога"); }
            }
            else if (data.getFileType() == "file")
            {
                try
                {
                    string expansion = GetExpansion(path);
                    if (expansion != "")
                        newpath += expansion;
                    System.IO.File.Move(path, newpath);
                }
                catch { MessageBox.Show("Ошибка переименования файла"); }
            }
            data.isUpdate = true;
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
            if (data.getFileType() == "file")
            {
                string path = data.getFilePath();
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                {
                    File.Delete(path);
                }
            }
            else if (data.getFileType() == "directory")
            {
                string path = data.getFilePath();
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                dirInfo.Delete(true);
            }
            data.isUpdate = true;
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
            if (data.getFileType() == "file")
            {
                Process.Start(data.getFilePath());
            }
            data.isUpdate = true;
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
            if (data.getFileType() == "directory")
            {
                string name = Microsoft.VisualBasic.Interaction.InputBox("Введите название папки:");
                if (name != null && name != "") { data.addCreateFileName(name); }
                string path = data.getFilePath();
                string subpath = data.getCreateFileName();
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    data.isUpdate = true;
                }
                try
                {
                    dirInfo.CreateSubdirectory(subpath);
                }
                catch { MessageBox.Show("Ошибка создания каталога"); }
                data.isUpdate = true;
                data.addCreateFileName("");
            }
        }

        #endregion

        /// <summary>
        /// Взаимодействие с помощью клавиш F
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Functional(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                //Открыть файл
                case Key.F3:
                    OpenFile(null, null);
                    break;

                //Вставить файл
                case Key.F4:
                    if (data.getPathFromBuffer() != "")
                        PasteFile(null, null);
                    break;

                //Копировать файл
                case Key.F5:
                    CopyFile(null, null);
                    break;

                //Переименовать файл
                case Key.F6:
                    if (data.getFileType() == "file" || data.getFileType() == "directory")
                        RenameFile(null, null);
                    break;

                //Создать каталог
                case Key.F7:
                    CreateDir(null, null);
                    break;

                //Удалить файл
                case Key.F8:
                    RemoveFile(null, null);
                    break;
            }
        }

        /// <summary>
        /// Кнопочка помощи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help(object sender, RoutedEventArgs e)
        {
            if (HelpMenu.Text == "")
            {
                HelpMenu.Text =
                    "F3 - Открыть" +
                    new string(' ', 19) +
                    "F4 - Вставить" +
                    new string(' ', 19) +
                    "F5 - Копировать" +
                    new string(' ', 19) +
                    "F6 - Переименовать" +
                    new string(' ', 19) +
                    "F7 - Создать" +
                    new string(' ', 19) +
                    "F8 - Удалить";
            }
            else { HelpMenu.Text = ""; }
        }
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
