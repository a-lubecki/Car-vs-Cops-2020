using System;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;


public class ItemGeneratorBehavior : BaseGeneratorBehavior {


    [SerializeField] private LeanGameObjectPool[] poolsEnemies = null;
    [SerializeField] private LeanGameObjectPool poolObstacle = null;
    [SerializeField] private LeanGameObjectPool poolHeart = null;
    [SerializeField] private LeanGameObjectPool PoolExplosion = null;


    public List<GameObject> SpawnEnemy(EnemyType enemyType, int count, IItemDestructorBehaviorListener listener, GameObject goMainCar) {

        var res = SpawnObjects(count, FindEnemyPool(enemyType), true, listener);

        //additional inits
        foreach (var go in res) {
            go.GetComponent<ItemDestructorBehavior>()?.InitPoolExplosion(PoolExplosion);
            go.GetComponent<EnemyTurnBehavior>()?.InitTargetToFollow(goMainCar.transform);
            go.GetComponent<EnemySpeedBehavior>()?.InitTargetToFollow(goMainCar.transform);
            go.GetComponent<VehicleBehavior>()?.InitVehicle();
        }

        return res;
    }

    private LeanGameObjectPool FindEnemyPool(EnemyType enemyType) {

        foreach (var pool in poolsEnemies) {

            //check if pool prefab contains the same enemy type
            var enemyBehavior = pool.Prefab.GetComponent<EnemyBehavior>();
            if (enemyBehavior == null) {
                throw new ArgumentException("The found prefab is not an enemy (no EnemyBehavior)");
            }

            if (enemyBehavior.EnemyType == enemyType) {
                //pool found
                return pool;
            }
        }

        //not found
        throw new NotSupportedException("Enemy pool not defined yet in item generator : " + enemyType);
    }

    public List<GameObject> SpawnObstacles(int count, IItemDestructorBehaviorListener listener) {

        var res = SpawnObjects(count, poolObstacle, false, listener);

        //additional inits
        foreach (var go in res) {
            go.GetComponent<ItemDestructorBehavior>()?.InitPoolExplosion(PoolExplosion);
        }

        return res;
    }

    public List<GameObject> SpawnNewHearts(int count, IItemDestructorBehaviorListener listener, LifeBehavior lifeBehavior) {

        var res = SpawnObjects(count, poolHeart, false, listener);

        //additional inits
        foreach (var go in res) {
            go.GetComponent<HeartBehavior>()?.Init(lifeBehavior);
        }

        return res;
    }

    public void DespawnAll() {

        foreach (var pool in poolsEnemies) {
            pool.DespawnAll();
        }

        poolObstacle.DespawnAll();
        poolHeart.DespawnAll();
        PoolExplosion.DespawnAll();
    }

}
