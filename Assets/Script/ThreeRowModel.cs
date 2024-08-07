using System;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine;


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
            map[a, b] = UnityEngine.Random.Range(1, 7);
        }
    }
}
