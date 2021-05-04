using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour {
    float x, y;
    float deltaX, deltaY;
    bool isDown = false;
    bool inColl = false;
    bool correct = false;
    bool prevCorrect = false;
    WCSTGenerator gen;
    public GameObject collidingGO;

    void Start()
    {
        gen = Camera.main.GetComponent<WCSTGenerator>();
    }

    void Update()
    {
        x = Input.mousePosition.x;
        y = Input.mousePosition.y;

        if (!isDown && inColl && collidingGO)
        {
            WCSTManager.Rule sortRule = WCSTManager.Rule.None;
            
            Debug.Log("Check cards " + WCSTManager.currRule);

            Card collCard = collidingGO.GetComponent<Card>();

            //this sets sortRule to the sorted by rule, this may or may not be the same as the correct rule
            correct = collCard.amount == GetComponent<Card>().amount;
            if (correct && gen.Amount.Length == 4)
                sortRule = WCSTManager.Rule.Amount;

            correct = collCard.color == GetComponent<Card>().color;
            if (correct)
                sortRule = WCSTManager.Rule.Color;

            correct = collCard.shape == GetComponent<Card>().shape;
            if (correct)
                sortRule = WCSTManager.Rule.Shape;

            //this chunk of if statements checks what the rule is then sets correct to whether they sorted by the correct rule
            if (WCSTManager.currRule == WCSTManager.Rule.Color)
                correct = collCard.color == GetComponent<Card>().color;
            else if(WCSTManager.currRule == WCSTManager.Rule.Shape)
                correct = collCard.shape == GetComponent<Card>().shape;
            else if (WCSTManager.currRule == WCSTManager.Rule.Amount)
                correct = collCard.amount == GetComponent<Card>().amount;

            //this chunk checks if they sorted by the previous rule
            if (WCSTManager.prevRule == WCSTManager.Rule.Color)
                prevCorrect = collCard.color == GetComponent<Card>().color;
            else if (WCSTManager.prevRule == WCSTManager.Rule.Shape)
                prevCorrect = collCard.shape == GetComponent<Card>().shape;
            else if(WCSTManager.prevRule == WCSTManager.Rule.Amount)
                prevCorrect = collCard.amount == GetComponent<Card>().amount;

            correct = WCSTManager.HandleScore(correct, prevCorrect, sortRule);

            gen.GenerateResults(correct);
        }
    }

    void OnMouseDrag()
    {
        if(!GameObject.Find("Canvas"))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10.0f));
            if (GetComponent<BoxCollider2D>().OverlapPoint(worldPoint) || isDown)
            {
                if (!isDown)
                {
                    deltaX = transform.position.x - worldPoint.x;
                    deltaY = transform.position.y - worldPoint.y;
                    isDown = true;
                }
                transform.position = new Vector3(worldPoint.x + deltaX, worldPoint.y + deltaY, 10.0f);
            }
        }
        
        //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //pos.z = 0;
        //transform.position = pos;
        //isDown = true;
    }

    void OnMouseUp()
    {
        isDown = false;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.gameObject.Equals(collidingGO))
        {
            inColl = true;
            collidingGO = coll.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.Equals(collidingGO))
        {
            inColl = false;
            collidingGO = null;
        }
    }
}
