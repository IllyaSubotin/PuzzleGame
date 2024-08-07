using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThreeRowObject", menuName = "ScriptableObjects/new ThreeRowObject", order = 1)]
public class ThreeRowObject : ScriptableObject 
{
    public int BoardSize;
    public bool IsMapCreated;
    public int[,] Map; 
}

