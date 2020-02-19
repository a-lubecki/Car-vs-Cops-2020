﻿using UnityEngine;
using DG.Tweening;


public class MainCarBehavior : VehicleBehavior {


    [SerializeField] private GameManager gameManager;


    protected override void UpdateInvincibilityDisplay() {

        //chage to trigger instead of disabling so that the car will pass through the other
        // cars and obstacles but with still detect the life items
        goModel.GetComponent<Collider>().isTrigger = lifeBehavior.isInvincible;

        ///TODO shader alpha
        var alpha = lifeBehavior.isInvincible ? 0.2f : 1;
        goModel.GetComponent<Renderer>().material.DOFade(alpha, 0.5f);
    }

    void OnTriggerEnter(Collider collider) {

        //only the main car can detect collectibles with trigger (when the car is invincible)
        var collectibleBehavior = collider.GetComponent<CollectibleBehavior>();
        if (collectibleBehavior != null) {
            OnCollisionWithCollectible(collectibleBehavior);
        }
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

        collectibleBehavior.Collect();
    }

    protected override void OnVehicleExplode() {
        base.OnVehicleExplode();

        //trigger game over when main car has exploded
        gameManager.StopPlaying();
    }

}
