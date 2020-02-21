using System;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;


public class BaseGeneratorBehavior : MonoBehaviour {


    [SerializeField] private Transform trDropPoint = null;
    [SerializeField] private float minGenerationDistance = 0;
    [SerializeField] private float maxGenerationDistance = 0;


    public List<GameObject> SpawnObjects(int count, LeanGameObjectPool pool, bool mustFaceGenerationCenter, IItemDestructorBehaviorListener listener) {

        if (count <= 0) {
            throw new ArgumentException("Can't generate 0 or less items");
        }

        var res = new List<GameObject>();

        for (int i = 0; i < count; i++) {

            var go = SpawnObject(pool, mustFaceGenerationCenter, listener);
            res.Add(go);
        }

        return res;
    }

    public GameObject SpawnObject(LeanGameObjectPool pool, bool mustFaceGenerationCenter, IItemDestructorBehaviorListener listener) {

        if (pool == null) {
            throw new ArgumentException("Can't generate item without pool");
        }

        //turn the generator with a random angle
        transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

        //move the point locally to have a generation distance from the center
        var distance = minGenerationDistance;
        if (minGenerationDistance < maxGenerationDistance) {
            distance = UnityEngine.Random.Range(minGenerationDistance, maxGenerationDistance);
        }
        trDropPoint.transform.localPosition = new Vector3(0, 0, distance);

        //get the global position of the drop point to generate the item
        var goItem = pool.Spawn(trDropPoint.position, Quaternion.identity);

        //inverse the rotation of the drop point to make it face the center
        if (mustFaceGenerationCenter) {
            goItem.transform.rotation = Quaternion.Inverse(trDropPoint.rotation);
        }

        //if the item has a destructor behavior, init it
        goItem.GetComponent<ItemDestructorBehavior>()?.Init(pool, trDropPoint, listener);

        return goItem;
    }

}
