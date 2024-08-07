using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPoint : MonoBehaviour
{
    [SerializeField]
    private ThreeRowView ThreeObjectsView;

    [SerializeField]
    private ThreeRowObject[] ThreeObjects;

    private void Start()
    {
        var model = new ThreeRowModel(ThreeObjects[0]);
        var threeObjectsController = new ThreeRowControler(ThreeObjectsView, model);
        threeObjectsController.Init();
    }
}
