using System;
using UnityEngine;
using Lean.Pool;


public class ItemDestructorBehavior : MonoBehaviour {


    private LeanGameObjectPool pool;
    private Transform trDropPoint;
    [SerializeField] private float distanceFromCenter = 1000;


    public void Init(LeanGameObjectPool pool, Transform trDropPoint) {

        this.pool = pool;
        this.trDropPoint = trDropPoint;
    }

    void Update() {

        //destroy item if too far from the center
        if (Vector3.Distance(transform.position, trDropPoint.position) > distanceFromCenter) {
            pool?.Despawn(gameObject);
        }
    }

}
