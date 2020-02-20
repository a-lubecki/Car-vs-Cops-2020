using UnityEngine;
using DG.Tweening;


public class MainCarBehavior : VehicleBehavior {


    [SerializeField] private GameManager gameManager;
    [SerializeField] protected GameObject goModel;
    [SerializeField] protected GameObject goModelFaded;


    protected override void UpdateInvincibilityDisplay(bool isInvincible) {

        //chage to trigger instead of disabling so that the car will pass through the other
        // cars and obstacles but with still detect the life items
        physicsCollider.enabled = !isInvincible;

        goModel.SetActive(!isInvincible);
        goModelFaded.SetActive(isInvincible);
    }

    protected override void OnCollisionWithEnemy(VehicleBehavior vehicleBehavior) {

        TryLoseLife();
    }

    protected override void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior) {

        obstacleBehavior.Explode();

        TryLoseLife();
    }

    protected override void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior) {

        collectibleBehavior.Collect();
    }

    protected override void OnVehicleExplode() {

        gameObject.SetActive(false);

        //trigger game over when main car has exploded
        gameManager.StopPlaying();
    }

}
