using System;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;


public class BaseGeneratorBehavior : MonoBehaviour {


    [SerializeField] private Transform trDropPoint;
    [SerializeField] private float generationDistance;


    public List<GameObject> SpawnObjects(int count, LeanGameObjectPool pool, bool mustFaceGenerationCenter) {

        if (count <= 0) {
            throw new ArgumentException("Can't generate 0 or less items");
        }

        var res = new List<GameObject>();

        for (int i = 0; i < count; i++) {

            var go = SpawnObject(pool, mustFaceGenerationCenter);
            res.Add(go);
        }

        return res;
    }

    public GameObject SpawnObject(LeanGameObjectPool pool, bool mustFaceGenerationCenter) {

        if (pool == null) {
            throw new ArgumentException("Can't generate item without pool");
        }

        //turn the generator with a random angle
        transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

        //move the point locally to have a generation distance from the center
        trDropPoint.transform.localPosition = new Vector3(0, 0, generationDistance);

        //get the global position of the drop point to generate the item
        var goItem = pool.Spawn(trDropPoint.position, Quaternion.identity);

        //inverse the rotation of the drop point to make it face the center
        if (mustFaceGenerationCenter) {
            goItem.transform.rotation = Quaternion.Inverse(trDropPoint.rotation);
        }

        //if the item has a destructor behavior, init it
        goItem.GetComponent<ItemDestructorBehavior>()?.Init(pool, trDropPoint);

        return goItem;
    }

}
