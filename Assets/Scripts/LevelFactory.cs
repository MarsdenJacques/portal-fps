using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//maybe use a seed

public class LevelFactory : MonoBehaviour
{
    // Start is called before the first frame update
    public int teleporterCount = 2;
    public int size = 25;
    public int ammoPillCount = 4;
    public int ghostWidth = 5;
    public int ghostHeight = 3;
    int ghostRow;
    int ghostColumn;
    void Start()
    {
        //Debug.Log(GameManager.manager.RandomVal());
        //ghostRow = ((size - 1) / 2) - (ghostHeight / 2);
        //ghostColumn = ((size - 1) / 2) - (ghostWidth / 2);
        //GenLevel2D();
    }
    public Level CreateLevel()
    {
        return new Level();
    }
    private int[,] GenLevel2D()
    {
        int[,] board = GenLevel2DSetupBoard();
        //do dfs from 4 corners to center, or do dfs from start on bottom enter to bototm corners, then bottom corners to top corners, then to center?
        int startX = (size - 1) / 2;
        int startY = ghostRow - 1;
        //start dfs
        return board;
    }
    private int[,] GenLevel2DSetupBoard()
    {
        int[,] board = new int[size, size];
        /*Chunk[,] built = new Chunk[size,size];
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                Chunk nextChunk = new Chunk(size, x, y);
                Chunk[] neighbors = new Chunk[4];
                neighbors[0] = built[nextChunk.left, y];
                neighbors[1] = built[x, nextChunk.up];
                neighbors[2] = built[nextChunk.right, y];
                neighbors[3] = built[x, nextChunk.down];
                nextChunk.InitChunk(neighbors);
            }
        }
        return new int[0,0];*/
        //new try
        //0 = unassigned, 1 = path, 2 = inside wall, 3 = outerwall, 4 = ghostspawn
        //start gen at ghostrow - 1 and middle column
        int numZeros = size * size;
        for(int x = ghostRow; x < ghostRow + ghostHeight; x++)
        {
            for(int y = ghostColumn; y < ghostColumn + ghostWidth; y++)
            {
                board[x, y] = 4;
                numZeros--;
            }
        }
        for(int x = 0; x < size; x++)
        {
            int y = 0;
            int y2 = size - 1;
            board[x, y] = 3;
            board[x, y2] = 3;
            numZeros -= 2;
        }
        for(int y = 0; y < size; y++)
        {
            int x = 0;
            int x2 = size - 1;
            board[x, y] = 3;
            board[x2, y] = 3;
            numZeros -= 2;
        }
        for(int x = 0; x < size; x++)
        {
            string line = "";
            for(int y = 0; y < size; y++)
            {
                line += board[x, y];
            }
            Debug.Log(line);
        }
        return board;
    }
}

//"chunks"
//dfs within chunk
//center size?
//odd ^2 value only
//size should be in number of chunks per row
//enter/exit coords always set for center so no need to calc (just build around it)

//multiple enters and exits per chunk??? if so, gen those first? so get known sides, assign doors, then make doors for unknown sides (make teleporters on left side then give to right?)
//gen top left - bottom left - left to right
//have to send "center chunks" and "type" to constructor

//chunk generation: get neighbor chunk's doors, make doors, dfs until every empty slot is filled and every door as connection?

class Chunk
{
    public int size;
    public string[,] piece;
    public int xPos;
    public int yPos;
    public string[,] doors;
    public int left;
    public int right;
    public int up;
    public int down;
    public Chunk(int size, int xPos, int yPos)
    {
        this.size = size;
        this.xPos = xPos;
        this.yPos = yPos;
        piece = new string[size, size];
        doors = new string[4, size];
        left = xPos - 1 < 0 ? size - 1 : xPos - 1;
        right = xPos + 1 >= size ? 0 : xPos + 1;
        up = yPos - 1 < 0 ? size - 1 : yPos - 1;
        down = yPos + 1 >= size ? 0 : yPos + 1;
    }
    public void InitChunk(Chunk[] neighbors)
    {
        GetNeighboringDoors(neighbors);
        DoorsToPiece();
        GenPiece();
        Debug.Log("new piece");
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Debug.Log(piece[i, j]);
            }
        }
    }
    void GenPiece()
    {
        for(int i = 1; i < size - 1; i++)
        {
            for(int j = 1; j < size - 1; j++)
            {
                piece[i, j] = "default";
            }
        }
    }
    void DoorsToPiece()
    {
        for(int pos = 0; pos < size; pos++)
        {
            piece[0, pos] = doors[0, pos];
        }
        for (int pos = 0; pos < size; pos++)
        {
            piece[size - 1, pos] = doors[2, pos];
        }
        for (int pos = 0; pos < size; pos++)
        {
            piece[pos, 0] = doors[1, pos];
        }
        for (int pos = 0; pos < size; pos++)
        {
            piece[pos, size - 1] = doors[3, pos];
        }
    }
    void GetNeighboringDoors(Chunk[] neighbors)
    {
        int neighborSide = -1;
        List<int> noNeighbors = new List<int>();
        for (int side = 0; side < 4; side++)
        {
            if (neighbors[side] != null)
            {
                neighborSide = side + 2;
                if (neighborSide >= 4)
                {
                    neighborSide -= 4;
                }
                for (int door = 0; door < neighbors[side].doors.GetLength(neighborSide); door++) //change this to be flipped later
                {
                    string doorStr = neighbors[side].doors[neighborSide, door];
                    if(doorStr[0] == 'd')
                    {
                        char[] array = doorStr.ToCharArray();
                        array[1] = (char)side;
                        doorStr = new string(array);
                    }
                    doors[side, door] = doorStr;
                }
            }
            else
            {
                noNeighbors.Add(side);
            }
        }
        if(noNeighbors.Count > 0)
        {
            GenDoors(noNeighbors);
        }
    }
    void GenDoors(List<int> noNeighbors)
    {
        int doorCount = (size - 2) / 2;
        for (int i = 0; i < noNeighbors.Count; i++)
        {
            doors[noNeighbors[i], 0] = "";
            doors[noNeighbors[i], size - 1] = "";
            bool addedDoor = false;
            for(int j = 1; j < size - 1; j++)
            {
                if(!addedDoor && doorCount > 0)
                {
                    if(GameManager.manager.RandomVal() < 0.5f)
                    {
                        doors[noNeighbors[i], j] = "d" + noNeighbors[i]; //change to also have direction after, all doors should have d followed by door direction
                        addedDoor = true;
                        doorCount--;
                    }
                    {
                        doors[noNeighbors[i], j] = "";
                    }
                }
                else
                {
                    doors[noNeighbors[i], j] = "";
                    addedDoor = false;
                }
            }
        }
    }
}
