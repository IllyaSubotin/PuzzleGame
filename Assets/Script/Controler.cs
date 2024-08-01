using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controler : MonoBehaviour
{
    Button[,] button;
    Image[] image;
    Model model;

    public int[,] map;
    int FromX, FromY;
    bool IsSelected;
    void Start()
    {

        model = new Model(ShowBox);
        
        InitButton();
        InitImage();
        map = new int[Model.Size, Model.Size];
        ClearMap();
        AddRandomBolls();
        
        IsSelected = false;
    }

    public void ShowBox(int x, int y, int Image)
    {
        button[x, y].GetComponent<Image>().sprite = image[Image].sprite;
    }

    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int Number = Num(name);
        int x = Number % Model.Size;
        int y = Number / Model.Size;
        Debug.Log($"{name} {x} {y}");
        
        if(IsSelected)
        {
            MoveBoll(x, y);
        }
        else 
        {
            TakeBoll(x, y);
        }
        
        
        
       
    }

  
    
    private void InitButton()
    {
        button = new Button[Model.Size, Model.Size];
        for (int a = 0; a < button.Length; a++)
        {
            button[a % Model.Size, a / Model.Size] = GameObject.Find($"Button ({a})").GetComponent<Button>();
        }
    }

    private void InitImage() 
    {
        image = new Image[Model.Bolls];
        for (int a = 0; a < Model.Bolls; a++)
        {
            image[a] = GameObject.Find($"Image ({a})").GetComponent<Image>();
        }
    }

    private int Num(string name) 
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        Match match = regex.Match(name);
        if(!match.Success) 
            throw new System.Exception("not");
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    }

 

    

    public void ClearMap()
    {
        for (int a = 0; a < Model.Size; a++)
        {
            for (int b = 0; b < Model.Size; b++)
            {
                 SetMap(a, b, 0);
            }
        }
    }

    public void SetMap(int a, int b, int balls)
    {
        map[a, b] = balls;
        ShowBox(a, b, map[a, b]);
    }
    public void AddRandomBolls()
    {
        for (int a = 0; a < Model.Size; a++)
        {
            for (int b = 0; b < Model.Size; b++)
            {
                AddRandomBoll(a, b);
            }
        }
    }

    public void AddRandomBoll(int a, int b)
    {

        map[a, b] = UnityEngine.Random.Range(1, Model.Bolls);
        SetMap(a, b, map[a, b]);

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
        int swap = map[x, y];
        SetMap(x, y, map[FromX, FromY]);
        SetMap(FromX, FromY, swap);
        FromX = 0;
        FromY = 0;
        IsSelected = false;


    }
}
    

