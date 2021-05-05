using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChecker : MonoBehaviour {

    public GameObject[] checkers;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCheckers(GameObject[] c)
    {
        checkers = c;
    }

    public void ClearCheckers()
    {
        for (int i = 0; i < checkers.Length; i++)
        {
            checkers[i].GetComponent<SeqCardChecker>().collidingGO = null;
        }
    }

    public bool AllFilled()
    {
        for (int i = 0; i < checkers.Length; i++)
        {
            if (!checkers[i].GetComponent<SeqCardChecker>().collidingGO || checkers[i].GetComponent<SeqCardChecker>().isMouseDown)
                return false;
        }
        return true;
    }

    public bool AllFilledArrow()
    {
        for (int i = 0; i < checkers.Length; i++)
        {
            if (!checkers[i].GetComponent<SeqCardChecker>().collidingGO)
                return false;
        }
        return true;
    }

    public bool CheckTestCorrectness()
    {
        for (int i = 0; i < checkers.Length; i++)
        {
            if (!checkers[i].GetComponent<SeqCardChecker>().isCorrect())
                return false;
        }
        return true;
    }
}
