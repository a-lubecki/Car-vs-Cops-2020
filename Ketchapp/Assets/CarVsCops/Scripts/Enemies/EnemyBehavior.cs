using System;
using UnityEngine;


public class EnemyBehavior : VehicleBehavior {


    [SerializeField] private ItemDestructorBehavior itemDestructorBehavior;


    protected override void UpdateInvincibilityDisplay(bool isInvincible) {
        //no specific update
    }


    protected override void OnCollisionWithEnemy(VehicleBehavior vehicleBehavior) {

        TryLoseLife();
    }

    protected override void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior) {

        obstacleBehavior.Explode();

        Explode();
    }

    protected override void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior) {

        collectibleBehavior.Destroy();
    }

    protected override void OnVehicleExplode() {

        itemDestructorBehavior.DestroyCurrentItem();
    }

}
