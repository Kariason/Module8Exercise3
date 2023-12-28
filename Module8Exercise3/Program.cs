using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module8Exercise3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь до папки");
            string path = Console.ReadLine();
            DateTime LastWriteTime = Directory.GetLastAccessTime(path);
            TimeSpan timeSpan = TimeSpan.FromSeconds(1);
            DateTime thresholdTime = DateTime.Now.Subtract(timeSpan);


            if (LastWriteTime < thresholdTime)
            {
                Console.WriteLine($"Исходный размер папки: {FolderSize(path)}");
                Console.WriteLine($"Места освобождено: {FolderSize(path)}");
                DelDir(path);
                Console.WriteLine($"Текущий размер папки: {FolderSize(path)}");
            }
            else
            {
                Console.WriteLine("Ошибка удаления: Последние изменения в папке были менее 30мин назад");
            } 
        }

        public static void DelDir(string path)
        {
            
            int quantityFiles = 0;
            DirectoryInfo newDir = new DirectoryInfo(path);
            DirectoryInfo[] folder = newDir.GetDirectories();
            FileInfo[] newFile = newDir.GetFiles();

            if (Directory.Exists(path))
            {
                try
                {
                    foreach (FileInfo f in newFile)
                    {
                        quantityFiles++;
                        f.Delete();
                    }

                    foreach (DirectoryInfo df in folder)
                    {
                        DelDir(df.FullName);
                    }

                    foreach (DirectoryInfo dir in newDir.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine("Директория не найдена. Ошибка: " + ex.Message);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Отсутствует доступ. Ошибка: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: " + ex.Message);
                }

                Console.WriteLine($"В папке {path} файлов удалено: {quantityFiles}");
            }
        }

        public static long FolderSize(string path)
        {
            long lengthFiles = 0;
            DirectoryInfo newDir = new DirectoryInfo(path);
            DirectoryInfo[] folder = newDir.GetDirectories();
            FileInfo[] newFile = newDir.GetFiles();
            if (Directory.Exists(path))
            {
                try
                {
                    foreach (FileInfo f in newFile)
                    {
                        lengthFiles += f.Length;
                    }

                    for (int j = 0; j < folder.Length; j++)
                    {
                        lengthFiles += FolderSize(path + "\\" + folder[j].Name);
                    }
                    return lengthFiles;
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine("Директория не найдена. Ошибка: " + ex.Message);
                    return 0;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Отсутствует доступ. Ошибка: " + ex.Message);
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: " + ex.Message);
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}


