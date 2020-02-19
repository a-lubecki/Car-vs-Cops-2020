using System;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour {


    [SerializeField] private ItemDestructorBehavior itemDestructorBehavior;


    public void Explode() {

        ///TODO explosion

        itemDestructorBehavior.DestroyCurrentItem();
    }

}
