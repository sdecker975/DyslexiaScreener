using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is the base Test Item Object
 * All test items will have events
 * All test items will inherit from this
 */
[System.Serializable]
public class TestItem {
    public EventSystem.EventObject[] events;
    public EventSystem.EventObject currentEvent;
    public int eventNumber = 0;
}
