using UnityEngine;
using System.Collections;

public class CheckQuit : MonoBehaviour
{
    ////this stops from quitting, deprecated for 2018
    public static bool canQuit = false;

    void Awake()
    {
        // This game object needs to survive multiple levels
        Object.DontDestroyOnLoad(this.gameObject);
        Application.wantsToQuit += WantsToQuit;
    }
    static bool WantsToQuit()
    {
        if (!canQuit)
        {
            return false;
        }else
        return true;
    }
}
