using System;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine;


public delegate void ShowBox(int x, int y, int Image);
public class Model 
{
    public const int Size = 5;
    public const int Bolls = 4;


    ShowBox showBox;
    public Model(ShowBox showBox) 
    { 
        this.showBox = showBox;
       
    
    }

    public void Start()
    {
        
    }
   
  
}
