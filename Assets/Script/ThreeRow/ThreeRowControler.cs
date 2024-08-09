using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThreeRowControler: Controller
{
    private CellComponent[,] cellComponent; 
    private int[,] map;
    private bool[,] mark;

    private int Size = 5;
    private int Bolls = 7;

    private int FromX, FromY;
    private bool IsSelected;

    private ThreeRowView _view;
    private ThreeRowModel _model; 
    
    public GameObject componentPrefab;
    public GameObject panel;
    public Sprite[] sprites;

    public ThreeRowControler(ThreeRowView view, ThreeRowModel model)
    {
        _model = model;
        _view = view;

        map = _model.Map;
        Size = _model.BoardSize;

        componentPrefab = _view.componentPrefab;
        panel = _view.panel;
        sprites = _view.sprites;

        mark = new bool[Size, Size];    
        IsSelected = false;
    }

    public override void Init()
    {
        InitCellComponent();
        SetClickOnButtons();
        SetCurrentSprites();
        while (SearchPosibleTurn())
        {
            AddRandomBolls();
        }
    }

    private void InitCellComponent()
    {
        cellComponent = new CellComponent[Size, Size];

        for (int a = 0; a < cellComponent.Length; a++)
        {
            var cell = GameObject.Instantiate(componentPrefab);
            cell.transform.SetParent(panel.transform, false);
            cellComponent[a % Size, a / Size] = cell.GetComponent<CellComponent>();
            cellComponent[a % Size, a / Size].Index = a;
        }
    }

    private void SetClickOnButtons()
    {
        for (int a = 0; a < cellComponent.Length; a++)
        {
            var temp = a;
            cellComponent[a % Size, a / Size].Button.onClick.AddListener(() =>
            {
                Click(temp);
            });
        }
    }

    private void SetCurrentSprites()
    {
        for (int a = 0; a < cellComponent.Length; a++)
        { 
            cellComponent[a % Size, a / Size].Image.sprite = sprites[map[a % Size, a / Size]]; 
        }
    }

    private void ClearMap()
    {
        for (int a = 0; a < Size; a++)
        {
            for (int b = 0; b < Size; b++)
            {
                 SetMap(a, b, 0);
            }
        }
    }

    private void AddRandomBolls()
    {
        for (int a = 0; a < Size; a++)
        {
            for (int b = 0; b < Size; b++)
            {
                AddRandomBoll(a, b);
            }
        }
    }

    private void AddRandomBoll(int a, int b)
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

    private void SetMap(int a, int b, int Image)
    {
        map[a, b] = Image;
        cellComponent[a, b].Image.sprite = sprites[Image];
    }


    private void Click(int index)
    { 
        int x = index % Size;
        int y = index / Size; 

        if (IsSelected)
        {
            MoveBoll(x, y);
        }
        else
        {
            TakeBoll(x, y);
        }
    }

    private void TakeBoll(int x, int y)
    {
        FromX = x;
        FromY = y;
        IsSelected = true;

    }

    private void MoveBoll(int x, int y)
    {
        if (!IsSelected) return;
        if (PosibleMove(x, y)) 
        {     
            int swap1 = map[x, y];
            int swap2 = map[FromX, FromY];  

            SetMap(x, y, map[FromX, FromY]);
            SetMap(FromX, FromY, swap1);  

            //Тут анімация отмени

            if(DeleteLines())
            {
                while (DeleteLines()){}                  
                while (SearchPosibleTurn() != true) 
                {
                    AddRandomBolls(); //кінець гри
                }
            }
            else 
            {
                SetMap(x, y, swap1);
                SetMap(FromX, FromY, swap2);
            }


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

    private bool DeleteLines() 
    {
        bool Deleteline = false;
        for(int x = 0; x < Size; x++) 
        {
            for (int y = 0; y < Size; y++)
            {
                if (DeleteLine(x, y, 1, 0) + DeleteLine(x, y, 0, 1) + DeleteLine(x, y, -1, 0) + DeleteLine(x, y, 0, -1) > 0)
                {
                    Deleteline = true;
                }
                
            }
        }

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (mark[x, y]) 
                {
                    SetMap(x, y, 0);
                    CreateNewBall(x, y);
                    mark[x, y] = false;
                }
                
            }
        }
        
        return Deleteline;
                 
    }

    private void CreateNewBall(int x, int y)
    {
        for (; GetMap(x, y - 1, map) > 0; y--) 
        {
            SetMap(x, y, map[x, y - 1]);
        }

        if (GetMap(x, y - 1, map) == 0)
        {
            AddRandomBoll(x, y);
        }
    }

   
    private int DeleteLine(int x0, int y0, int sx, int sy)
    {
        int ball = map[x0, y0];
        int count = 0;

        if (ball == 0) 
        {
            return 0;
        }        

        for (int x = x0, y = y0; GetMap(x, y, map) == ball; x += sx, y += sy)  
        {
            count++;
        }
        
        if (count < 3)
        {
            return 0;
        }


        for (int x = x0, y = y0; GetMap(x, y, map) == ball; x += sx, y += sy)
        {
            mark[x, y] = true;
        }


        if (count < 3)
        {
            return 0;
        }
        else 
        {
            return 1;
        }      
    }

    private int GetMap(int x, int y, int[,] Map)
    {
        if (x >= 0 && y >= 0 && x < Size && y < Size)
        { 
            return Map[x, y];
        }
        else
        {
            return 0;
        }
    }


    private bool SearchPosibleTurn()
    {
        var copyMap = map;
        int DeathMap = 0;

        for (int a = 0; a < Size; a++)
        {
            for (int b = 0; b < Size; b++)
            {

                DeathMap += SetCopyMap(a, b, a, b + 1, copyMap) + SetCopyMap(a, b, a + 1, b, copyMap);
               
            }
        }

        if (DeathMap == 0) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int SetCopyMap(int a0, int b0, int a1, int b1, int[,] copyMap)
    {
        if (CGetMap(a1, b1, copyMap) != 0)
        {
            int swap0 = copyMap[a0, b0];
            int swap1 = copyMap[a1, b1];
            int ret = 0;

            copyMap[a0, b0] = swap1;
            copyMap[a1, b1] = swap0;

            ret += CDeleteLine(a0, b0, 1, 0, copyMap);
            ret += CDeleteLine(a0, b0, 0, 1, copyMap);

            copyMap[a0, b0] = swap0;
            copyMap[a1, b1] = swap1;

            return ret;
        }
        else 
        {
            return 0;
        }
    }

    private int CDeleteLine(int x0, int y0, int sx, int sy,  int[,] copyMap)
    {
        int ball = copyMap[x0, y0];
        int count = 0;

        if (ball == 0)
        {
            return 0;
        }

        for (int x = x0, y = y0; CGetMap(x, y, copyMap) == ball; x += sx, y += sy)
        {
            count++;
        }

        if (count < 3)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    private int CGetMap(int x, int y, int[,] copyMap)
    {
        if (x >= 0 && y >= 0 && x < Size && y < Size)
        {
            return copyMap[x, y];
        }
        else
        {
            return 0;
        }
    }

}


