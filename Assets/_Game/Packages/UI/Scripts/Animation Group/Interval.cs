﻿using UnityEngine;

public class Interval : IElement
{
    [SerializeField] float duration = 0.25f;

    WaitForSeconds wait;

    public WaitForSeconds Wait
    {
        get
        {
            if (wait == null)
            {
                wait = new WaitForSeconds(duration);
            }

            return wait;
        }
    }
}