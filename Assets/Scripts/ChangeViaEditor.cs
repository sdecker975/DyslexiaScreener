using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeViaEditor : MonoBehaviour
{
    public WISTestHandler primary;

    public void Change()
    {
 
        foreach (WISTestItem lw in primary.testItemFrontEnd)
        {
            if(lw.displayedText.Length > 0)
            {
                lw.LetterButtons[0] = primary.testItemFrontEnd[1].LetterButtons[0];
                lw.LetterButtons[1] = primary.testItemFrontEnd[1].LetterButtons[0];
                lw.LetterButtons[2] = primary.testItemFrontEnd[1].LetterButtons[0];
                lw.LetterButtons[3] = primary.testItemFrontEnd[1].LetterButtons[0];
            }
        }
    }
    
}