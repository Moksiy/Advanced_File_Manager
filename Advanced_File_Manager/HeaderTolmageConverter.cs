using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Advanced_File_Manager
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderTolmageConverter : IValueConverter
    {
        public static HeaderTolmageConverter Instance = new HeaderTolmageConverter();

        /// <summary>
        /// Конвертация полного пути в специальную иконку
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Получение полного пути
            var path = (string)value;

            //Если путь null, игнор
            if (path == null)
                return null;

            //Получение имени элемента
            var name = MainWindow.GetFileFolderName(path);

            //Устанавливаем дефолтное значение пути
            var image = "Images/File.png";

            //Если имя пустое, то это диск
            if (string.IsNullOrEmpty(name))
                image = "Images/Disk.png";
            //Скрытая папка
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Hidden))
                image = "Images/ClosedFolder.png";
            //Директория
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "Images/OpenFolder.png";   
            
            if(!string.IsNullOrEmpty(name))
            {
                switch (name.Substring(name.LastIndexOf('.') +1))
                {
                    //Документ
                    case "docx":
                        image = "Images/Document.png";
                        break;
                    //Изображения
                    case "jpg":
                    case "png":
                    case "jpeg":
                        image = "Images/Image.png";
                        break;
                    case "":
                        image = "Images/OpenFolder.png";
                        break;
                }
            }
                return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
