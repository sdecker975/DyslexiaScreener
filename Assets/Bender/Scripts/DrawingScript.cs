using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawingScript : MonoBehaviour
{
    private LineRenderer line;
    private bool isMousePressed;
    public List<Vector3> pointsList;
    private Vector3 mousePos;
    public static bool isFirst = false;

    // Structure for line points
    struct myLine
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
    };

    void Awake()
    {
        if(!isFirst)
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Diffuse"));
            line.SetVertexCount(0);
            line.SetWidth(0.1f, 0.1f);
            line.SetColors(Color.green, Color.green);
            line.useWorldSpace = true;
            isMousePressed = false;
            pointsList = new List<Vector3>();
            isFirst = true;
        }
    }
    void Update()
    {
        if (isMousePressed)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (!pointsList.Contains(mousePos))
            {
                pointsList.Add(mousePos);
                line.SetVertexCount(pointsList.Count);
                line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
            }
        }
    }

    void OnMouseDown()
    {
        isMousePressed = true;
        line.SetVertexCount(0);
        pointsList.RemoveRange(0, pointsList.Count);
        line.SetColors(Color.green, Color.green);
    }

    void OnMouseUp()
    {
        isMousePressed = false;
        print("hit");
        GameObject o = Instantiate(gameObject);
        Destroy(o.GetComponent<DrawingScript>());
        Destroy(o.GetComponent<BoxCollider2D>());
        o.name = "prevLine";
    }

    void OnMouseExit()
    {
        isMousePressed = false;
        GameObject o = Instantiate(gameObject);
        Destroy(o.GetComponent<DrawingScript>());
        Destroy(o.GetComponent<BoxCollider2D>());
        o.name = "prevLine";
    }

    void OnMouseEnter()
    {
        if(Input.GetMouseButton(0))
        {
            isMousePressed = true;
            line.SetVertexCount(0);
            pointsList.RemoveRange(0, pointsList.Count);
            line.SetColors(Color.green, Color.green);
        }
    }

    //eventually this will call something to save the lines to a file
    public void ClearLines()
    {
        line.SetVertexCount(0);
        pointsList.RemoveRange(0, pointsList.Count);

        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "prevLine")
                Destroy(go);
        }
    }
}