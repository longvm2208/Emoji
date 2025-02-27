﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] BigItem bigItem;
    [SerializeField] List<Collider2D> colliders;
    [SerializeField] UnityEvent[] onTriggerEnters;

    Collider2D currentCollider;

    private void OnValidate()
    {
        bigItem = GetComponent<BigItem>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliders.Contains(collision) && currentCollider == null) currentCollider = collision;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (colliders.Contains(collision) && currentCollider == collision) currentCollider = null;
    }

    public void CheckCollision()
    {
        if (currentCollider == null) return;
        if (!colliders.Contains(currentCollider)) return;
        int id = colliders.IndexOf(currentCollider);
        onTriggerEnters[id]?.Invoke();
        currentCollider = null;
        if (bigItem != null)
        {
            bigItem.OnCheckCollisionCompleted();
        }
    }
}
