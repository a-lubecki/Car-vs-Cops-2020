using System;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour {


    [SerializeField] private ItemDestructorBehavior itemDestructorBehavior;


    public void Explode() {

        if (!gameObject.activeSelf) {
            return;
        }

        gameObject.SetActive(false);

        ///TODO explosion

        itemDestructorBehavior.DestroyCurrentItem();
    }

}
