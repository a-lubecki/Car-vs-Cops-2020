using UnityEngine;


public class EnemyBehavior : VehicleBehavior {


    [SerializeField] private ItemDestructorBehavior itemDestructorBehavior = null;


    protected override void UpdateInvincibilityDisplay(bool isInvincible) {
        //no specific update
    }


    protected override void OnCollisionWithVehicle(VehicleBehavior vehicleBehavior) {

        if (vehicleBehavior is MainCarBehavior) {
            //an enemy can't lose life if touching the main car
            return;
        }

        TryLoseLife(1);
    }

    protected override void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior) {

        obstacleBehavior.Explode();

        //more damages with an obstacle for more strategies + the enemy lose invincibility
        TryLoseLife(2, true);
    }

    protected override void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior) {

        collectibleBehavior.Destroy();
    }

    protected override void OnVehicleExplosionRequired() {

        itemDestructorBehavior.ExplodeCurrentItem();
    }

}
