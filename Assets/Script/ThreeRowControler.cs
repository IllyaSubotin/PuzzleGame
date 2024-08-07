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

    private bool DeleteLines() 
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
            return map[x, y];
        }
        else
        {
            return 0;
        }
    } 
}


