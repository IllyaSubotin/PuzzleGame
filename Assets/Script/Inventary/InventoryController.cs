using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Controller
{
    private InventoryView _view;
    private InventoryModel _model;

    public GameObject panel;

    public InventoryController(InventoryView view, InventoryModel model)
    {
        _view = view;
        _model = model;
        panel = _view.panel;


    }   

    public override void Init() 
    {
    
    }
}
