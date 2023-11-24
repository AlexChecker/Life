using System.Data.Common;

namespace MapBuilder
{
    public enum CELL{
        DEAD ,
        ALIVE
    }

    public struct Cursor{
        public int x;
        public int y;
        public Cursor()
        {
            x = 0;
            y = 0;
        }
    }
    public class Map
    {
        CELL[] field = new CELL[width*height];
        bool end = false;
        public List<byte> bindata = new List<byte>();

        Cursor cursor = new Cursor();
        const int width = 80;
        const int height = 50;

        public Map()
        {

        }

        public Map(List<byte> data)
        {
            for(int i =0;i<data.Count;i+=2)
            {
                field[data[i+1]*width+data[i]] = CELL.ALIVE;
            }
        }

        CELL getCellAt(int x,int y)
        {
            return field[y*width+x];
        }

        public void drawMap()
        {
            Console.Clear();
            for(int y = 0;y<height;y++)
            {
                for(int x =0;x<width;x++)
                {
                    Console.ResetColor();
                    if(x == cursor.x && y == cursor.y)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    if(getCellAt(x,y) == CELL.ALIVE) Console.Write("# ");
                    else Console.Write(". ");
                }
                Console.WriteLine();
            }
        }

        public void processInput()
        {
            var key = Console.ReadKey();
            switch(key.Key)
            {
                case ConsoleKey.UpArrow:
                    cursor.y--;
                    if(cursor.y<0) cursor.y = height-1;
                break;
                case ConsoleKey.DownArrow:
                    cursor.y ++;
                    if(cursor.y >= height) cursor.y = 0;
                break;
                case ConsoleKey.RightArrow:
                    cursor.x ++;
                    if(cursor.x >= width) cursor.x = 0;
                break;
                case ConsoleKey.LeftArrow:
                    cursor.x --;
                    if(cursor.x<0) cursor.x = width-1;
                break;
                case ConsoleKey.Spacebar:
                    CELL data = getCellAt(cursor.x,cursor.y);
                    if(data == CELL.DEAD) field[cursor.y*width+cursor.x] = CELL.ALIVE;
                    else field[cursor.y*width+cursor.x] = CELL.DEAD;
                break;
                case ConsoleKey.Escape:
                    end = true;
                break;
            }
        }

        public void saveBinaryData()
        {
            for(int y =0;y<height;y++)
            {
                for(int x = 0;x< width;x++)
                {
                    if(getCellAt(x,y) == CELL.ALIVE)
                    {
                        bindata.Add(Convert.ToByte(x));
                        bindata.Add(Convert.ToByte(y));
                    }
                }
            }
        }

        public List<byte> edit()
        {
            while(!end)
            {
                drawMap();
                processInput();
            }
            saveBinaryData();
            return bindata;
        }

        


    }
}