﻿using UnityEngine;

public abstract class CollectibleBehavior : MonoBehaviour {


    [SerializeField] private ItemDestructorBehavior itemDestructorBehavior = null;


    public void Collect() {

        Destroy();

        OnCollected();
    }

    public void Destroy() {

        itemDestructorBehavior.DestroyCurrentItem();
    }

    protected abstract void OnCollected();

}
