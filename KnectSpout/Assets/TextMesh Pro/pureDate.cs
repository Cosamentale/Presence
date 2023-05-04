using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class pureDate : MonoBehaviour
{
    public TextMeshPro textMesh;
    private string date;
    void Start()
    {
        
    }
    void Update()
    {
        DateTime dt = DateTime.Now;
        date = dt.ToString("MM / dd / yyyy   ddd   HH : mm : ss").ToUpper();
        UpdateText();
    }
    void UpdateText()
    {
        textMesh.text = date;
    }
}
