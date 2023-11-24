#include <stdlib.h>
#include <time.h>
#include <stdio.h>
#include <string.h>
#define WIDTH 80
#define HEIGHT 50
enum CELL
{
    DEAD ,
    ALIVE
};

enum CELL field[WIDTH*HEIGHT];
enum CELL buf[WIDTH*HEIGHT];

void setupFields()
{
    for(int y = 0;y<HEIGHT;y++)
    {
        for(int x =0;x<WIDTH;x++)
        {
            field[y*WIDTH+x] = DEAD;
            buf[y*WIDTH+x] = DEAD;
        }
    }
}

void drawField()
{
    for(int y = 0;y< HEIGHT;y++)
    {
        for(int x =0;x<WIDTH;x++)
        {
            if(field[y*WIDTH+x] == ALIVE) printf("██");
            else printf(". ");
        }
        printf("\n");
    }
}

enum CELL getCellAt(int x,int y)
{
    int xr =x,xy =y;
    if(x >= WIDTH) xr = 0;
    if(x < 0) xr = WIDTH -1;
    if(y >= HEIGHT) xy = 0;
    if(y< 0) xy = HEIGHT -1;

    return field[xy*WIDTH+xr];
}

int getLifeAround(int x,int y)
{
    int result = 0;
    enum CELL neighbours[8];
    neighbours[0] = getCellAt(x-1,y-1);
    neighbours[1] = getCellAt(x,y-1);
    neighbours[2] = getCellAt(x+1,y-1);
    neighbours[3] = getCellAt(x+1,y);
    neighbours[4] = getCellAt(x+1,y+1);
    neighbours[5] = getCellAt(x,y+1);
    neighbours[6] = getCellAt(x-1,y+1);
    neighbours[7] = getCellAt(x-1,y);
    for(int i =0;i<8;i++)
    {
        if(neighbours[i] == ALIVE) result++;
    }
    return result;
}

void copyAtClean()
{
    for(int y = 0;y< HEIGHT;y++)
    {
        for(int x = 0;x<WIDTH;x++)
        {
         field[y*WIDTH+x] = buf[y*WIDTH+x];
         buf[y*WIDTH+x] = DEAD;   
        }
    }

}

void processCells()
{
    for(int y =0;y<HEIGHT;y++)
    {
        for(int x =0;x< WIDTH;x++)
        {
            int life = getLifeAround(x,y);
            if(life == 3) buf[y*WIDTH+x] = ALIVE;
            if(life >3 || life <2) buf[y*WIDTH+x] = DEAD;
            if(getCellAt(x,y) == ALIVE && (life == 2 || life ==3)) buf[y*WIDTH+x] = ALIVE;
        }
    }
    copyAtClean();
}

void setupData(char* data,long length)
{
    for(long i = 0;i<length;i+=2)
    {
        field[data[i+1]*WIDTH+data[i]] = ALIVE;
    }
    /*
        field[1] = ALIVE;
        field[1*WIDTH+2] = ALIVE;
        field[2*WIDTH] = ALIVE;
        field[2*WIDTH+1] = ALIVE;
        field[2*WIDTH+2] = ALIVE;
        */
}

void main(int argc, char** argv)
{
    setupFields();
    if(argc == 3)
    {
        if(strcmp(argv[1],"--file") == 0)
        {
            printf("opening file\n");
            char* file = argv[2];
            FILE* fileptr =fopen(file,"rb");
            fseek(fileptr,0,SEEK_END);
            long filelen = ftell(fileptr);
            rewind(fileptr);
            char* data = (char*) malloc(filelen*sizeof(char));
            fread(data,filelen,1,fileptr);
            fclose(fileptr);
            setupData(data,filelen);
            struct timespec time;
            time.tv_nsec = 100000000;
            time.tv_sec = 0;
            drawField();
            while(1)
            {
                nanosleep(&time,NULL);
                processCells();
                drawField();
            }
        }
    }
}