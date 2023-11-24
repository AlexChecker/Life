public class Menu
{
    public List<string> items;
    int selected = 0;

    public Menu(List<string> items)
    {
        this.items = items;
    }

    public Menu(){
        items = new List<string>();
    }

    public int drawMenu()
    {
        while(true)
        {
        Console.Clear();
            for(int i =0;i<items.Count;i++)
            {
                if(i == selected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }

                Console.WriteLine($"{items[i]}");

                Console.BackgroundColor = ConsoleColor.Black;
            }
            var key = Console.ReadKey();
            switch(key.Key)
            {
                case ConsoleKey.UpArrow:
                    selected--;
                    if(selected < 0) selected = items.Count - 1;
                break;
                case ConsoleKey.DownArrow:
                    selected ++;
                    if(selected > items.Count -1) selected = 0;
                break;
                case ConsoleKey.Enter:
                    return selected+1;
                
            }

        }
    }
}