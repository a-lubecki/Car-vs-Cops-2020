using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;


public class ItemGeneratorBehavior : BaseGeneratorBehavior {


    [SerializeField] private LeanGameObjectPool poolPoliceCar;


    public List<GameObject> GeneratePoliceCars(int count, GameObject goMainCar) {

        var res = GenerateItems(count, poolPoliceCar, true);

        //for all generated cars, init the main car object as the target to follow
        foreach (var goCar in res) {
            goCar.GetComponent<EnemyCarBehavior>()?.InitTargetToFollow(goMainCar.transform);
        }

        return res;
    }

}
