using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;


public class ItemGeneratorBehavior : BaseGeneratorBehavior {


    [SerializeField] private LeanGameObjectPool poolPoliceCar;


    public List<GameObject> SpawnPoliceCars(int count, GameObject goMainCar) {

        var res = SpawnObjects(count, poolPoliceCar, true);

        //for all generated cars, init the main car object as the target to follow
        foreach (var goCar in res) {
            
            goCar.GetComponent<EnemyTurnBehavior>()?.InitTargetToFollow(goMainCar.transform);
            goCar.GetComponent<VehicleBehavior>()?.Show();
        }

        return res;
    }

    public void DespawnAll() {

        poolPoliceCar.DespawnAll();
    }

}
