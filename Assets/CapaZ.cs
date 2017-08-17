using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

[ExecuteInEditMode]
public class CapaZ : MonoBehaviour
{

    public float capaZ;
    public SpriteMeshInstance render;


    private void Awake()
    {
        render = GetComponent<SpriteMeshInstance>();
        
    }

    private void Update()
    {
        render.m_SortingOrder = (int)capaZ;
    }

    void OnValidate()
    {
        render.m_SortingOrder = (int)capaZ;
    }
}
