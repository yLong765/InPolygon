using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Polygon
{
    public string name;
    public List<GameObject> points;
    public Polygon(string _name)
    {
        name = _name;
        points = new List<GameObject>();
    }
}
