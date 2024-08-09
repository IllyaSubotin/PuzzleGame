using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CellComponent : MonoBehaviour
{
    public int Index;
    public Button Button;
    public Image Image;
    
    public void ChangeBallAnimation(Sprite sprite)
    {
        transform.DOScale(Vector3.zero, 1f).OnComplete(() => { 
            Image.sprite = sprite;
            transform.DOScale(Vector3.one, 1f);
        });
    }

    public void CreacteBallAnimation()
    {
        transform.DOScale(Vector3.one, 2f);
    }
}
