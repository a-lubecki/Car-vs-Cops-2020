using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;


public class ItemGeneratorBehavior : BaseGeneratorBehavior {


    [SerializeField] private LeanGameObjectPool poolPoliceCar;
    [SerializeField] private LeanGameObjectPool poolObstacles;


    public List<GameObject> SpawnPoliceCars(int count, IItemDestructorBehaviorListener listener, GameObject goMainCar) {

        var res = SpawnObjects(count, poolPoliceCar, true, listener);

        //for all generated cars, init the main car object as the target to follow
        foreach (var goCar in res) {

            goCar.GetComponent<EnemyTurnBehavior>()?.InitTargetToFollow(goMainCar.transform);
            goCar.GetComponent<EnemySpeedBehavior>()?.InitTargetToFollow(goMainCar.transform);
            goCar.GetComponent<EnemyBehavior>().Show();
        }

        return res;
    }

    public List<GameObject> SpawnObstacles(int count, IItemDestructorBehaviorListener listener) {

        return SpawnObjects(count, poolObstacles, false, listener);
    }

    public void DespawnAll() {

        poolPoliceCar.DespawnAll();
        poolObstacles.DespawnAll();
    }

}
