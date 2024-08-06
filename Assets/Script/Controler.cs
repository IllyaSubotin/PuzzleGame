using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controler 
{
    Button[,] button;
    Image[] image;
    public int[,] map;
    public bool[,] mark;

    public int Size = 5;
    public int Bolls = 7;

    int FromX, FromY;
    bool IsSelected;


    public Controler()
    {
        map = new int[Size, Size];
        mark = new bool[Size, Size];    
        IsSelected = false;
    }

    public void InitButton()
    {
        button = new Button[Size, Size];

        for (int a = 0; a < button.Length; a++)
        {
            button[a % Size, a / Size] = GameObject.Find($"Button ({a})").GetComponent<Button>();
        }
    }

    public void InitImage()
    {
        image = new Image[Bolls];
        for (int a = 0; a < Bolls; a++)
        {
            image[a] = GameObject.Find($"Image ({a})").GetComponent<Image>();
        }
    }
       
    public void ClearMap()
    {
        for (int a = 0; a < Size; a++)
        {
            for (int b = 0; b < Size; b++)
            {
                 SetMap(a, b, 0);
            }
        }
    }

    public void AddRandomBolls()
    {
        for (int a = 0; a < Size; a++)
        {
            for (int b = 0; b < Size; b++)
            {
                AddRandomBoll(a, b);
            }
        }
    }

    public void AddRandomBoll(int a, int b)
    {
        bool reroll = true;

        while (reroll) 
        {
            map[a, b] = UnityEngine.Random.Range(1, Bolls);
            if (DeleteLine(a, b, -1, 0) != 0 || DeleteLine(a, b, 0, -1) != 0 || DeleteLine(a, b, 1, 0) != 0 || DeleteLine(a, b, 0, 1) != 0)
            {
                reroll = true;
            }
            else 
            {
                reroll = false; 
            }
        }
        SetMap(a, b, map[a, b]);
    }

    public void SetMap(int a, int b, int Image)
    {
        map[a, b] = Image;
        button[a, b].GetComponent<Image>().sprite = image[Image].sprite;
    }

  
    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int Number = Num(name);
        int x = Number % Size;
        int y = Number / Size;
        Debug.Log($"{name} {x} {y}");

        if (IsSelected)
        {
            MoveBoll(x, y);
        }
        else
        {
            TakeBoll(x, y);
        }
    }

    public void TakeBoll(int x, int y)
    {
        FromX = x;
        FromY = y;
        IsSelected = true;

    }

    public void MoveBoll(int x, int y)
    {
        if (!IsSelected) return;
        if (PosibleMove(x, y)) 
        {        
        int swap = map[x, y];

        SetMap(x, y, map[FromX, FromY]);
        SetMap(FromX, FromY, swap);
        DeleteLines();

        FromX = 0;
        FromY = 0;
        IsSelected = false;
        }
        else
        {
            TakeBoll(x, y);
        }
    }
    private bool PosibleMove(int x, int y)
    {

        if (x == FromX && y == FromY + 1 || x == FromX && y == FromY - 1)
        {
            return true;
        }
        if (x == FromX + 1 && y == FromY || x == FromX - 1 && y == FromY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DeleteLines() 
    {
        int Deleteline = 0;
        for(int x = 0; x < Size; x++) 
        {
            for (int y = 0; y < Size; y++)
            {
                Deleteline = DeleteAllLine(Deleteline, x, y);
            }
        }

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (mark[x, y]) 
                {
                    SetMap(x, y, 0);
                    AddRandomBoll(x, y);
                    mark[x, y] = false;
                }
                
            }
        }
        if (Deleteline >= 0) 
        {
            return true;
        }
        else 
        {
            return false;
        }                
    }

    private int DeleteAllLine(int Deleteline, int x, int y)
    {
        Deleteline += DeleteLine(x, y, 1, 0);
        Deleteline += DeleteLine(x, y, 0, 1);

        return Deleteline;

    }

    private int DeleteLine(int x0, int y0, int sx, int sy)
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

        for (int x = x0, y = y0; GetMap(x, y) == ball; x += sx, y += sy)
        {
            mark[x, y] = true;
        }

        return count;
    }

    private int GetMap(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Size && y < Size)
        {
            Debug.Log($"{x} {y}");
            return map[x, y];
        }
        else
        {
            return 0;
        }
    }




   private int Num(string name)
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        Match match = regex.Match(name);
        if (!match.Success)
            throw new System.Exception("not");
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    }

}


