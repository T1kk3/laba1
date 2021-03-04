using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.Tasks;


namespace laba1
{
  class User
  {
    public string Name { get; set; }
    public int Age { get; set; }
    public string Company { get; set; }
  }

  class Person
  {
    public string Name { get; set; }
    public int Age { get; set; }
  }

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Добрый день! \nДанные диска(-ов): \n");
      DriveInfo[] drives = DriveInfo.GetDrives();

      foreach (DriveInfo drive in drives)
      {
        Console.WriteLine($"Название: {drive.Name}");
        Console.WriteLine($"Тип: {drive.DriveType}");
        if (drive.IsReady)
        {
          Console.WriteLine($"Объем диска: {drive.TotalSize}");
          Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
          Console.WriteLine($"Метка: {drive.VolumeLabel}");
        }
        Console.WriteLine();
      }

      // Каталог для файла
      string path = @"D:\testpath";
      DirectoryInfo dirInfo = new DirectoryInfo(path);
      if (!dirInfo.Exists)
      {
        dirInfo.Create();
      }

      // Запись в файл
      Console.WriteLine("Введите строку для записи в файл:");
      string text = Console.ReadLine();
      using (FileStream fstream = new FileStream($"{path}/testfile.txt", FileMode.OpenOrCreate))
      {
        byte[] array = System.Text.Encoding.Default.GetBytes(text);
        fstream.Write(array, 0, array.Length);
        Console.WriteLine("Текст записан в файл");
      }

      // Чтение из файла
      using (FileStream fstream = File.OpenRead($"{path}/testfile.txt"))
      {
        byte[] array = new byte[fstream.Length];
        fstream.Read(array, 0, array.Length);
        string textFromFile = System.Text.Encoding.Default.GetString(array);
        Console.WriteLine($"Текст из файла: {textFromFile}");
      }

      // Удаление файла
      Console.WriteLine("Нажмите '1', чтобы удалить файл!");
      int delButton = int.Parse(Console.ReadLine());
      if (delButton == 1)
      {
        FileInfo fileInf = new FileInfo($"{path}/testfile.txt");
        if (fileInf.Exists)
        {
          File.Delete($"{path}/testfile.txt");
          Console.WriteLine("Готово.");
        }
      }
      else
      {
        Console.WriteLine("Ошибка! Завершаю работу...");
      }

      // JSON, сериализация:

      static async Task MainJSON(string[] args)
      {
        // сохранение данных
        using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
        {
          Person tom = new Person() { Name = "Tom", Age = 35 };
          await JsonSerializer.SerializeAsync<Person>(fs, tom);
          Console.WriteLine("Data has been saved to file");
        }

        // чтение данных
        using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
        {
          Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
          Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
        }
      }

      Console.WriteLine("Удаление...");
      FileInfo fileInfoJSON = new FileInfo("user.json");
      if (fileInfoJSON.Exists)
      {
        File.Delete("user.json");
        Console.WriteLine("Готово.");
      }
      // ------------------------------------------------------------------     xDoc.Load($"{path}/file.xml");

      List<User> users = new List<User>();

      XmlDocument xDoc = new XmlDocument();
      xDoc.Load($"{path}/file.xml");
      XmlElement xRoot = xDoc.DocumentElement;

      XmlElement userElem = xDoc.CreateElement("user");
      // создаем атрибут name
      XmlAttribute nameAttr = xDoc.CreateAttribute("name");
      // создаем элементы company и age
      XmlElement companyElem = xDoc.CreateElement("company");
      XmlElement ageElem = xDoc.CreateElement("age");
      // создаем текстовые значения для элементов и атрибута
      XmlText nameText = xDoc.CreateTextNode("Mark Zuckerberg");
      XmlText companyText = xDoc.CreateTextNode("Facebook");
      XmlText ageText = xDoc.CreateTextNode("30");

      //добавляем узлы
      nameAttr.AppendChild(nameText);
      companyElem.AppendChild(companyText);
      ageElem.AppendChild(ageText);
      userElem.Attributes.Append(nameAttr);
      userElem.AppendChild(companyElem);
      userElem.AppendChild(ageElem);
      xRoot.AppendChild(userElem);
      xDoc.Save($"{path}/file.xml");
      foreach (XmlElement xnode in xRoot)
      {
        User user = new User();
        XmlNode attr = xnode.Attributes.GetNamedItem("name");
        if (attr != null)
          user.Name = attr.Value;

        foreach (XmlNode childnode in xnode.ChildNodes)
        {
          if (childnode.Name == "company")
            user.Company = childnode.InnerText;

          if (childnode.Name == "age")
            user.Age = Int32.Parse(childnode.InnerText);
        }
        users.Add(user);
      }
      foreach (User u in users)
      {
        Console.WriteLine($"{u.Name} ({u.Company}) - {u.Age}");
      }

      // Удаление

      Console.WriteLine("Удаление...");
      FileInfo fileInfoXML = new FileInfo($"{path}/file.xml");
      if (fileInfoXML.Exists)
      {
        File.Delete($"{path}/file.xml");
        Console.WriteLine("Готово.");
      }


      // Работа с Zip:

      string sourceFolder = "D://test/"; // исходная папка
      string zipFile = "D://test.zip"; // сжатый файл
      string targetFolder = "D://newtest"; // папка, куда распаковывается файл

      ZipFile.CreateFromDirectory(sourceFolder, zipFile);
      Console.WriteLine($"Папка {sourceFolder} архивирована в файл {zipFile}");
      ZipFile.ExtractToDirectory(zipFile, targetFolder);

      Console.WriteLine($"Файл {zipFile} распакован в папку {targetFolder}");

      Console.WriteLine("Удаление...");
      FileInfo fileInfoZip = new FileInfo($"{path}/test.zip");
      if (fileInfoZip.Exists)
      {
        File.Delete($"{path}/test.zip");
        Console.WriteLine("Готово.");
      }

    }
  }
}
