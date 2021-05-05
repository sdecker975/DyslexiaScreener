using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public int orderNumber;
    bool isMouseDown = false;
    public bool inBox = false;
    EventSystem.typeOfEvent e;

    void Update()
    {
        e = Camera.main.GetComponent<SeqLangTestHandler>().backEndItem.currentEvent.type;

        if (isMouseDown && !inBox)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -1;
            transform.position = pos;
        }
    }

    public void ToggleCollider()
    {
        GetComponent<BoxCollider2D>().enabled = !GetComponent<BoxCollider2D>().enabled;
    }

    void OnMouseDown()
    {
        isMouseDown = e == EventSystem.typeOfEvent.Mouse;
    }

    void OnMouseUp()
    {
        isMouseDown = false;
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }
}
