
public enum CELL{
    DEAD,
    ALIVE
}
public partial class Program
{
const int width = 80;
const int height = 50;

static CELL [,] field = new CELL[width,height];
static CELL [,] buffer = new CELL[width,height];
    public static void Main()
    {
        setupField();
        drawField();
        while(true)
        {
            processCells();
            drawField();
            Thread.Sleep(100);
        }
    }
    static CELL GetCellAt(int x,int y)
    {
        if(x < 0) x = width+x;
        if(x >=width) x = 0;
        if(y < 0) y = height+y;
        if(y >= height) y = 0;

        return field[x,y];
    }
    static int GetAliveCellsAround(int x,int y)
    {
        int result = 0;
        CELL[] neighbours = new CELL[8];
        neighbours[0] = GetCellAt(x-1,y-1);
        neighbours[1] = GetCellAt(x,y-1);
        neighbours[2] = GetCellAt(x+1,y-1);
        neighbours[3] = GetCellAt(x+1,y);
        neighbours[4] = GetCellAt(x+1,y+1);
        neighbours[5] = GetCellAt(x,y+1);
        neighbours[6] = GetCellAt(x-1,y+1);
        neighbours[7] = GetCellAt(x-1,y);
        for(int i =0;i<8;i++)
        {
            if(neighbours[i] == CELL.ALIVE) result++;
        }
        return result;
    }
    public static void processCells()
    {
        for(int y = 0;y<height;y++)
        {
            for(int x = 0;x<width;x++)
            {
                int life = GetAliveCellsAround(x,y);
                if(life == 3) buffer[x,y] = CELL.ALIVE;
                if(life < 2) buffer[x,y] = CELL.DEAD;
                if(life >3) buffer[x,y] = CELL.DEAD;
                if(field[x,y] == CELL.ALIVE && (life == 2 || life == 3)) buffer[x,y] = CELL.ALIVE; 
            }
        }
        cleanOnCopyBuffer();
    }
    public static void cleanOnCopyBuffer()
    {
        for(int y = 0;y<height;y++)
        {
            for(int x = 0;x<width;x++)
            {
                field[x,y] = buffer[x,y];
                buffer[x,y] = CELL.DEAD;
            }
        }
    }
    public static void setupField()
    {
        Console.WriteLine("Enter path to proper .life file");
        string path = Console.ReadLine();
        if(!string.IsNullOrEmpty(path))
        {
            if(File.Exists(path))
            {
                List<byte> data = File.ReadAllBytes(path).ToList();
                for(int i =0;i<data.Count;i+=2)
                {
                    field[data[i],data[i+1]] = CELL.ALIVE;
                }
            }
        }

        /*
            . # .
            . . #
            # # #
        
        field[1,0] = CELL.ALIVE;
        field[2,1] = CELL.ALIVE;
        field[0,2] = CELL.ALIVE;
        field[1,2] = CELL.ALIVE;
        field[2,2] = CELL.ALIVE;
        */
        /*                         11  13  15  17  19  21  23  25  27  29  31  33  35
             0 1 2 3 4 5 6 7 8 9 10  12  14  16  18  20  22  24  26  28  30  32  34  36
           0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
           1 . . . . . . . . . . . . . . . . . . . . . . . . . # . . . . . . . . . . .
           2 . . . . . . . . . . . . . . . . . . . . . . . # . # . . . . . . . . . . .
           3 . . . . . . . . . . . . . # # . . . . . . # # . . . . . . . . . . . . # #
           4 . . . . . . . . . . . . # . . . # . . . . # # . . . . . . . . . . . . # #
           5 . # # . . . . . . . . # . . . . . # . . . # # . . . . . . . . . . . . . .
           6 . # # . . . . . . . . # . . . # . # # . . . . # . # . . . . . . . . . . .
           7 . . . . . . . . . . . # . . . . . # . . . . . . . # . . . . . . . . . . .
           8 . . . . . . . . . . . . # . . . # . . . . . . . . . . . . . . . . . . . .
           9 . . . . . . . . . . . . . # # . . . . . . . . . . . . . . . . . . . . . .
        
        field[1,5] = CELL.ALIVE;
        field[2,5] = CELL.ALIVE;
        field[1,6] = CELL.ALIVE;
        field[2,6] = CELL.ALIVE;
        field[11,5] = CELL.ALIVE;
        field[11,6] = CELL.ALIVE;
        field[11,7] = CELL.ALIVE;
        field[12,4] = CELL.ALIVE;
        field[12,8] = CELL.ALIVE;
        field[13,3] = CELL.ALIVE;
        field[13,9] = CELL.ALIVE;
        field[14,3] = CELL.ALIVE;
        field[14,9] = CELL.ALIVE;
        field[15,6] = CELL.ALIVE;
        field[16,4] = CELL.ALIVE;
        field[16,8] = CELL.ALIVE;
        field[17,5] = CELL.ALIVE;
        field[17,6] = CELL.ALIVE;
        field[17,7] = CELL.ALIVE;
        field[18,6] = CELL.ALIVE;
        field[21,3] = CELL.ALIVE;
        field[21,4] = CELL.ALIVE;
        field[21,5] = CELL.ALIVE;
        field[22,3] = CELL.ALIVE;
        field[22,4] = CELL.ALIVE;
        field[22,5] = CELL.ALIVE;
        field[23,2] = CELL.ALIVE;
        field[23,6] = CELL.ALIVE;
        field[25,1] = CELL.ALIVE;
        field[25,2] = CELL.ALIVE;
        field[25,6] = CELL.ALIVE;
        field[25,7] = CELL.ALIVE;
        field[35,3] = CELL.ALIVE;
        field[35,4] = CELL.ALIVE;
        field[36,3] = CELL.ALIVE;
        field[36,4] = CELL.ALIVE;
        */
    }
    public static void drawField()
    {
        Console.Clear();
        Console.WriteLine();
        for(int y = 0;y<height;y++)
        {
            for(int x =0;x<width;x++)
            {
                if(field[x,y] == CELL.ALIVE)
                    Console.Write("██");
                else Console.Write("  ");
            }
            Console.WriteLine();
        }
    }
}