using System;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;


public class ThreeRowModel: Model
{
    public int BoardSize;
    public int[,] Map;
    private bool IsMapCreated;

    public ThreeRowModel(ThreeRowObject threeRowObject) 
    {
        IsMapCreated = threeRowObject.IsMapCreated;

        if (IsMapCreated)
        {
            Map = threeRowObject.Map;
            BoardSize = Map.Length; //тут щас неправильно, треба передивитись по довжині одного массива
        }
        else
        {
            BoardSize = threeRowObject.BoardSize;
            Map = CreateMap(BoardSize);
        }
    }


    private int[,] CreateMap(int size)
    {
        var map = new int[size, size];
        for (int a = 0; a < size; a++)
        {
            for (int b = 0; b < size; b++)
            {
                CreateRandomBoll(a, b);
            }
        }
        return map;

        void CreateRandomBoll(int a, int b)
        {
            bool reroll = true;
            while (reroll) 
            {
                map[a, b] = UnityEngine.Random.Range(1, 7);
                if (DeleteLine(a, b, -1, 0) != 0 || DeleteLine(a, b, 0, -1) != 0)
                {
                    reroll = true;
                }
                else
                {
                    reroll = false;
                }
            }
           
        }

        int DeleteLine(int x0, int y0, int sx, int sy)
        {
            int ball = map[x0, y0];
            int count = 0;

            if (ball == 0)
            {
                return 0;
            }

            for (int x = x0, y = y0; GetMap(x, y) == ball; x += sx, y += sy)
            {
                count++;
            }

            if (count < 3)
            {
                return 0;
            }

            return count;
        }

        int GetMap(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < BoardSize && y < BoardSize)
            {
                return map[x, y];
            }
            else
            {
                return 0;
            }
        }



    }

 

 


}
