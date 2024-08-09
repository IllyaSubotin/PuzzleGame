using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class EnterPoint : MonoBehaviour
{
    [SerializeField]
    private ThreeRowView ThreeObjectsView;

    [SerializeField]
    private InventoryView InventoryObjectsView;

    [SerializeField]
    private ThreeRowObject[] ThreeObjects;

    [SerializeField]
    private Button GameStartButton;

    [SerializeField]
    private Button InventoryStartButton;


    public void Start()
    {
        InventoryStartButton.onClick.AddListener(() =>
        {
            InventoryStart();
        });


        GameStartButton.onClick.AddListener(() =>
        {
            GameStart(); 
        });
    }

    private void InventoryStart()
    {
        var model = new InventoryModel();
        var controller = new InventoryController(InventoryObjectsView, model);
        controller.Init();

        GameStartButton.gameObject.SetActive(false);
    }

    private void GameStart()
    {  
        var model = new ThreeRowModel(ThreeObjects[0]);
        var controller = new ThreeRowControler(ThreeObjectsView, model);

        controller.Init();
        
        GameStartButton.gameObject.SetActive(false);
        InventoryStartButton.gameObject.SetActive(false);
    }
}
