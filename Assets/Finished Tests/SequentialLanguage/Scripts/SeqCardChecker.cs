using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeqCardChecker : MonoBehaviour
{
    public static int currentActionNumber = 0;

    public bool isChecker = false;
    public int shapeNumber;
    public int inputNumber;

    public int userShapeNumber;
    public int userInputNumber;

    public bool usesOrder = false;
    public bool isColliding = false;
    public bool isMouseDown = false;

    public GameObject collidingGO;

    //TODO: Bug in dropping card after a card is already there, locks previous card
    // Update is called once per frame
    void Update()
    {
        EventSystem.typeOfEvent e = Camera.main.GetComponent<SeqLangTestHandler>().backEndItem.currentEvent.type;

        isMouseDown = Input.GetMouseButton(0);

        if (!isMouseDown && isColliding && collidingGO && e == EventSystem.typeOfEvent.Mouse && !collidingGO.GetComponent<CardObject>().inBox)
        {
            collidingGO.transform.position = new Vector2(transform.position.x, transform.position.y);
            collidingGO.GetComponent<CardObject>().inBox = true;
            userInputNumber = currentActionNumber;
            currentActionNumber++;
            userShapeNumber = collidingGO.GetComponent<CardObject>().orderNumber;
        }
    }

    public bool isCorrect()
    {
        if (shapeNumber == -1 && !collidingGO)
            return true;
        else if (!collidingGO)
            return false;
        return shapeNumber == collidingGO.GetComponent<CardObject>().orderNumber;
    }

    //TODO: Bug when moving cards around:
    //      If you place a card on a spot, add a card to another spot then move that card to an already used spot it messes up the cards and locks one down

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!collidingGO)
        {
            isColliding = true;
            collidingGO = coll.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.Equals(collidingGO))
        {
            isColliding = false;
            collidingGO = null;
        }
    }
}
