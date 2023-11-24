using MapBuilder;
using System.IO;

public partial class Program
{
    int width = 80;
    int height =50;
    static string filename;
    public static void Main()
    {
        Menu mainMenu = new Menu();
        mainMenu.items.Add("New map");
        mainMenu.items.Add("Open map");
        mainMenu.items.Add("Exit");

        int action = mainMenu.drawMenu();
        switch(action)
        {
            case 1:
                createNewMap();
            break;
            case 2:
                List<string> files = Directory.GetFiles("./").ToList();
                Menu fileOpen = new Menu();
                foreach(var item in files)
                {
                    if(item.Contains(".life"))
                    {
                        fileOpen.items.Add(item);
                    }
                }
                int choice = fileOpen.drawMenu();
                filename = fileOpen.items[choice-1];
                List<byte> data = File.ReadAllBytes(fileOpen.items[choice-1]).ToList();
                editMap(data);
                
            break;
            case 3:
            return;
        }
    }

    public static void editMap(List<byte> data)
    {
        Map map = new Map(data);
        data = map.edit();
        File.WriteAllBytes(filename,data.ToArray());
    }
    public static void createNewMap()
    {
        Console.WriteLine("Enter map name: ");
        filename = Console.ReadLine();
        if(!string.IsNullOrEmpty(filename))
        {
            Map map = new Map();
            List<byte> data = map.edit();
            File.Create(filename+".life").Close();
            
            File.WriteAllBytes(filename+".life",data.ToArray());
            
        }
    }
}