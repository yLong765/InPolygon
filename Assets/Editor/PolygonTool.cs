using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PolygonData))]
public class PolygonTool : Editor
{
    PolygonData polygonData;

    Vector2 ploygonScr;
    Vector2 ploygonPointScr;
    int currentPolygon;

    private void OnEnable()
    {
        polygonData = target as PolygonData;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        DrawLeft();
        DrawRight();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawLeft()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(100));

        if (GUILayout.Button("Create Polygon"))
        {
            polygonData.ploygons.Add(new Polygon("polygon" + polygonData.ploygons.Count.ToString()));
        }

        ploygonScr = EditorGUILayout.BeginScrollView(ploygonScr);
        for (int i = 0; i < polygonData.ploygons.Count; ++i)
        {
            if (GUILayout.Button(polygonData.ploygons[i].name, GUILayout.Height(30)))
            {
                currentPolygon = i;
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    private void DrawRight()
    {
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Create Point"))
        {
            GameObject go = new GameObject(polygonData.ploygons[currentPolygon].points.Count.ToString());
            go.transform.position = Vector3.zero;
            go.transform.SetParent(polygonData.transform);
            polygonData.ploygons[currentPolygon].points.Add(go);
        }

        if (polygonData.ploygons.Count > 0)
        {
            ploygonPointScr = EditorGUILayout.BeginScrollView(ploygonPointScr);
            for (int i = 0; i < polygonData.ploygons[currentPolygon].points.Count; ++i)
            {
                //polygonData.ploygons[currentPolygon].points[i].transform.position = EditorGUILayout.Vector3Field(i.ToString(), polygonData.ploygons[currentPolygon].points[i].transform.positio);
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnSceneGUI()
    {
        if (polygonData.ploygons.Count > 0)
        {
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < polygonData.ploygons[currentPolygon].points.Count; ++i)
            {
                points.Add(polygonData.ploygons[currentPolygon].points[i].transform.position);
            }
            Handles.DrawPolyLine(points.ToArray());
        }
    }
}
