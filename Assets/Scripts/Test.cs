using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class polygonData
{
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;
    public List<Vector3> points;

    public polygonData()
    {
        maxX = 0;
        maxY = 0;
        minX = 0;
        minY = 0;
        points = new List<Vector3>();
    }

    private List<Vector2> V3ToV2()
    {
        List<Vector2> V2 = new List<Vector2>();
        for (int i = 0; i < points.Count; ++i)
        {
            V2.Add(V3ToV2(points[i]));
        }
        return V2;
    }

    public Vector2 V3ToV2(Vector3 V3)
    {
        return new Vector2(V3.x, V3.z);
    }

    public Vector3[] GetDrawList()
    {
        List<Vector3> drawPoints = points;
        drawPoints.Add(drawPoints[0]);
        return drawPoints.ToArray();
    }

    public List<Vector2> GetPointsV2()
    {
        return V3ToV2();
    }

    public void SetMaxMinXY()
    {
        maxX = points[0].x;
        maxY = points[0].y;
        minX = points[0].x;
        minY = points[0].y;

        if (points.Count == 1)
        {
            return;
        }

        for (int i = 0; i < points.Count; ++i)
        {
            maxX = Mathf.Max(maxX, points[i].x);
            maxY = Mathf.Max(maxY, points[i].z);
            minX = Mathf.Min(minX, points[i].x);
            minY = Mathf.Min(minY, points[i].z);
        }
    }
}

public class Test : MonoBehaviour
{
    public List<GameObject> points = new List<GameObject>();
    public LineRenderer lineRenderer;

    private void Update()
    {
        if (points.Count > 0)
        {
            polygonData polygon = new polygonData();
            for (int i = 0; i < points.Count; i++)
            {
                polygon.points.Add(points[i].transform.position);
            }
            polygon.SetMaxMinXY();
            lineRenderer.positionCount = polygon.GetDrawList().Length;
            lineRenderer.SetPositions(polygon.GetDrawList());

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(pnpoly(polygon.V3ToV2(hitInfo.point), polygon) + " " + hitInfo.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject go = new GameObject(points.Count.ToString());
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            points.Add(go);
        }
    }

    private bool pnpoly(Vector2 point, polygonData polygon)
    {
        if (polygon.maxX < point.x || polygon.maxY < point.y || polygon.minX > point.x || polygon.minY > point.y)
        {
            Debug.Log(polygon.maxX + " " + polygon.maxY + " " + polygon.minX + " " + polygon.minY);
            return false;
        }

        List<Vector2> points = polygon.GetPointsV2();
       
        bool isIn = false;
        for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
        {
            if (((points[i].y > point.y) != (points[j].y > point.y)) &&
                (point.x < (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
            {
                isIn = !isIn;
            }
        }
        return isIn;
    }
}
