using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;


public class ItemGeneratorBehavior : BaseGeneratorBehavior {


    [SerializeField] private LeanGameObjectPool poolPoliceCar = null;
    [SerializeField] private LeanGameObjectPool poolObstacles = null;
    [SerializeField] private LeanGameObjectPool poolHearts = null;


    public List<GameObject> SpawnPoliceCars(int count, IItemDestructorBehaviorListener listener, GameObject goMainCar) {

        var res = SpawnObjects(count, poolPoliceCar, true, listener);

        //additional inits
        foreach (var go in res) {
            go.GetComponent<EnemyTurnBehavior>()?.InitTargetToFollow(goMainCar.transform);
            go.GetComponent<EnemySpeedBehavior>()?.InitTargetToFollow(goMainCar.transform);
            go.GetComponent<VehicleBehavior>()?.InitLife();
        }

        return res;
    }

    public List<GameObject> SpawnObstacles(int count, IItemDestructorBehaviorListener listener) {

        return SpawnObjects(count, poolObstacles, false, listener);
    }

    public List<GameObject> SpawnNewHearts(int count, IItemDestructorBehaviorListener listener, LifeBehavior lifeBehavior) {

        var res = SpawnObjects(count, poolHearts, false, listener);

        //additional inits
        foreach (var go in res) {
            go.GetComponent<HeartBehavior>()?.Init(lifeBehavior);
        }

        return res;
    }

    public void DespawnAll() {

        poolPoliceCar.DespawnAll();
        poolObstacles.DespawnAll();
        poolHearts.DespawnAll();
    }

}
