using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableByTongue : MonoBehaviour
{
    [SerializeField]
    private int collectingWeight = 1;
    public int CollectingWeight
    {
        get => collectingWeight;
    }
    
    void Start()
    {
        this.tag = "TongueCollectable";
    }
}
