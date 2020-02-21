using UnityEngine;
using DG.Tweening;


public class MainCarBehavior : VehicleBehavior {


    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject goModel;
    [SerializeField] private GameObject goModelFaded;


    protected override void UpdateInvincibilityDisplay(bool isInvincible) {

        //chage to trigger instead of disabling so that the car will pass through the other
        // cars and obstacles but with still detect the life items
        PhysicsCollider.enabled = !isInvincible;

        goModel.SetActive(!isInvincible);
        goModelFaded.SetActive(isInvincible);
    }

    private void HandleDamage() {

        var hasLostLife = TryLoseLife();
        if (hasLostLife) {
            Camera.main.DOShakePosition(0.5f, 2, 20);
        }
    }

    protected override void OnCollisionWithEnemy(VehicleBehavior vehicleBehavior) {

        HandleDamage();
    }

    protected override void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior) {

        obstacleBehavior.Explode();

        HandleDamage();
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
