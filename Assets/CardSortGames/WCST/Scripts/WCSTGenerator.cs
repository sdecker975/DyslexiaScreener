using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShapeArray
{
    public GameObject[] array;
}

[System.Serializable]
public class ColorArray
{
    public ShapeArray[] array;
}

public class WCSTGenerator : MonoBehaviour {

    //first index is amount, second is color, third is shape
    public ColorArray[] Amount;
    public Card[] topRow;
    public List<GameObject> generatableCards;

    //show correct or incorrect for answer
    public GameObject correct, incorrect;

    Card prevCard;
    Card prevprevCard;
    Card prevprevprevCard;
    Card sortingCard;

    public Transform[] stackPos;
    public GameObject sortingCardPos;

    GameObject sortingCardGO;
    GameObject canvas;

    bool firstCardGen = false;

	// Use this for initialization
	void Start () {
        sortingCard = new Card();
        prevCard = new Card();
        prevprevCard = new Card();
        prevprevprevCard = new Card();
        GenerateFirstCard();
        canvas = GameObject.Find("Canvas");
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void GeneratableCardsGenerator()
    {
        //is wcst 2 or 3
        if(Amount.Length == 1)
        {
            for(int i = 0; i < Amount[0].array.Length; i++)
            {
                for(int j = 0; j < Amount[0].array[i].array.Length; j++)
                {
                    GameObject GO = Amount[0].array[i].array[j];
                    Card GOCard = GO.GetComponent<Card>();
                    bool set = true;
                    for (int k = 0; k < topRow.Length; k++)
                    {
                        if (GOCard.compare(topRow[k], 3))
                        {
                            set = false;
                            break;
                        }
                    }
                    if (set)
                        generatableCards.Add(GO);
                }
            }
        }
        //is wcst 4
        else
        {
            for (int h = 0; h < Amount.Length; h++)
            {
                for (int i = 0; i < Amount[h].array.Length; i++)
                {
                    for (int j = 0; j < Amount[h].array[i].array.Length; j++)
                    {
                        GameObject GO = Amount[h].array[i].array[j];
                        Card GOCard = GO.GetComponent<Card>();
                        bool set = true;
                        for (int k = 0; k < topRow.Length; k++)
                        {
                            if (GOCard.compare(topRow[k], 2))
                            {
                                set = false;
                                break;
                            }
                        }
                        if (set)
                            generatableCards.Add(GO);
                    }
                }
            }
        }
    }

    private void ClearSortingCardChildren()
    {
        foreach (Transform child in sortingCardPos.transform)
            Destroy(child.gameObject);
    }
    //this is where blue star is made first card every time, fix please
    public void GenerateFirstCard()
    {
        ClearSortingCardChildren();

        GeneratableCardsGenerator();

        GenerateCard();

        //WCSTManager.totalRun++;

        //sortingCard.amount = 1;
        //prevCard.amount = 1;
        //prevprevCard.amount = 1;
        //prevprevprevCard.amount = 1;

        //sortingCard.color = Card.Color.Blue;
        //sortingCard.shape = Card.Shape.Star;

        //prevCard.color = Card.Color.Blue;
        //prevCard.shape = Card.Shape.Star;

        //prevprevCard.color = Card.Color.Blue;
        //prevprevCard.shape = Card.Shape.Star;

        //prevprevprevCard.color = Card.Color.Blue;
        //prevprevprevCard.shape = Card.Shape.Star;

        //sortingCardGO = Instantiate(Amount[sortingCard.amount-1].array[(int)Card.Color.Blue].array[(int)Card.Shape.Star], sortingCardPos.transform);

        //sortingCardGO.transform.position = sortingCardPos.transform.position;
        //sortingCardGO.AddComponent<MouseMovement>();
        //WCSTManager.runTimer = true;
        //sortingCard.clone(ref prevCard);
        //prevCard.clone(ref prevprevCard);
        //prevprevCard.clone(ref prevprevprevCard);
    }

    public void GenerateCard()
    {
        ClearSortingCardChildren();

        WCSTManager.totalRun++;
        bool notMade = true;
        int dist = Amount.Length;

        while (notMade)
        {
            int randomShape = Random.Range(0, WCSTManager.shapeCount);
            //fix this, this is assuming amount is 0 when it is 1
            int randomAmount = Random.Range(0, WCSTManager.amountCount);
            int randomColor = Random.Range(0, WCSTManager.colorCount);

            sortingCard.amount = randomAmount + 1;

            sortingCard.color = (Card.ColorTypes)randomColor;
            sortingCard.shape = (Card.ShapeTypes)randomShape;

            bool exists = false;
            Debug.Log(generatableCards.Count);
            for(int i = 0; i < generatableCards.Count; i++)
            {

                //if(dist == 1)
                //{
                if (sortingCard.compare(generatableCards[i].GetComponent<Card>(), 3) && !sortingCard.compare(prevCard, 3))
                {
                    exists = true;
                    break;
                }

                //}
                //else
                //{
                //    if (sortingCard.compare(generatableCards[i].GetComponent<Card>(), 2))
                //    {
                //        exists = true;
                //        break;
                //    }
                //}
            }

            if (!exists)
            {
                continue;
            }
            Debug.Log(sortingCard.color + " " + prevCard.color);

            //if(sortingCard.compare(prevCard, 2) || sortingCard.compare(prevprevCard, 1))
            //    continue;

            //commenting this out so that it only gives completely different cards
            Debug.Log(sortingCard.amount + " " + (int)sortingCard.color + " " + (int)sortingCard.shape);
            sortingCardGO = Instantiate(Amount[sortingCard.amount - 1].array[(int)sortingCard.color].array[(int)sortingCard.shape], sortingCardPos.transform);
            
            //if (sortingCard.shape == Card.Shape.Star)
            //{
            //    if (sortingCard.color == Card.Color.Red)
            //        continue;
            //    else if (sortingCard.color == Card.Color.Blue)
            //        sortingCardGO = Instantiate(blueStar, sortingCardPos.transform);
            //    else if (sortingCard.color == Card.Color.Yellow)
            //        sortingCardGO = Instantiate(yellowStar, sortingCardPos.transform);
            //}
            //else if (sortingCard.shape == Card.Shape.Circle)
            //{
            //    if (sortingCard.color == Card.Color.Red)
            //        sortingCardGO = Instantiate(redCircle, sortingCardPos.transform);
            //    else if (sortingCard.color == Card.Color.Blue)
            //        sortingCardGO = Instantiate(blueCircle, sortingCardPos.transform);
            //    else if (sortingCard.color == Card.Color.Yellow)
            //        continue;
            //}
            //else if (sortingCard.shape == Card.Shape.Square)
            //{
            //    if (sortingCard.color == Card.Color.Red)
            //        sortingCardGO = Instantiate(redSquare, sortingCardPos.transform);
            //    else if (sortingCard.color == Card.Color.Blue)
            //        continue;
            //    else if (sortingCard.color == Card.Color.Yellow)
            //        sortingCardGO = Instantiate(yellowSquare, sortingCardPos.transform);
            //}
            prevCard.clone(ref prevprevCard);
            sortingCard.clone(ref prevCard);
            notMade = false;
        }
        sortingCardGO.transform.position = sortingCardPos.transform.position;
        sortingCardGO.AddComponent<MouseMovement>();
        WCSTManager.runTimer = true;
    }

    public void GenerateResults(bool isCorrect)
    {
        ClearSortingCardChildren();

        //play sound

        if(isCorrect)
            sortingCardGO = Instantiate(correct, sortingCardPos.transform);
        else
            sortingCardGO = Instantiate(incorrect, sortingCardPos.transform);
        sortingCardGO.transform.position = sortingCardPos.transform.position;
        sortingCardGO.AddComponent<ResultsManager>();
    }
}
