using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class View : MonoBehaviour
{
    Controler controler;
    
    // Start is called before the first frame update

    void EnterPoint() 
    {
        controler = new Controler();
    }

    void Start()
    {
        EnterPoint();
        controler.InitButton();
        controler.InitImage();        
        controler.ClearMap();
        controler.AddRandomBolls();
    }

    // Update is called once per frame
    

    public void Click()
    {
        controler.Click();
    }
   
}
