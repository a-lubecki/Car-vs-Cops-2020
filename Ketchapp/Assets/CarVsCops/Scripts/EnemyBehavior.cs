using System;
using UnityEngine;


public class EnemyBehavior : VehicleBehavior {


    [SerializeField] private GameManager gameManager;
    [SerializeField] private ItemDestructorBehavior itemDestructorBehavior;


    protected override void UpdateInvincibilityDisplay() {
        //no specific update
    }

    public void InitGameManager(GameManager gameManager) {

        this.gameManager = gameManager ?? throw new ArgumentException();
    }

    protected override void OnCollisionWithEnemy(VehicleBehavior vehicleBehavior) {
        base.OnCollisionWithEnemy(vehicleBehavior);

        ///TODO
    }

    protected override void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior) {
        base.OnCollisionWithObstacle(obstacleBehavior);

        ///TODO
    }

    protected override void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior) {
        base.OnCollisionWithCollectible(collectibleBehavior);

        ///TODO
    }

    protected override void OnVehicleExplode() {
        base.OnVehicleExplode();

        ///TODO delay of the explosion before destroying

        itemDestructorBehavior.DestroyCurrentItem();

        gameManager.OnEnemyExploded();
    }

}
