using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public enum ShapeTypes { Star, Square, Circle, Triangle };
    public enum ColorTypes { Blue, Red, Yellow, Green };
    public int amount;
    public ShapeTypes shape;
    public ColorTypes color;

    public void clone(ref Card dest)
    {
        dest.amount = amount;
        dest.shape = shape;
        dest.color = color;
    }

    public bool compareWithoutAmount(Card c, int dist)
    {
        bool r;
        if (dist == 2)
            r = c.shape == shape && c.color == color;
        else
            r = c.shape == shape || c.color == color;
        return r;
    }

    public bool compare(Card c, int dist)
    {
        bool r;
        if (dist == 1)
            r = c.shape == shape || c.color == color || c.amount == amount;
        else if (dist == 2)
            r = (c.shape == shape && c.color == color) ||
                (c.shape == shape && c.amount == amount) ||
                (c.color == color && c.amount == amount);
        else
            r = c.shape == shape && c.color == color && c.amount == amount;
        return r;
    }

}
