using UnityEngine;
using DG.Tweening;
using Lean.Pool;


public class MainCarBehavior : VehicleBehavior {


    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private GameObject goModel = null;
    [SerializeField] private GameObject goModelFaded = null;
    [SerializeField] private LeanGameObjectPool poolExplosion = null;


    protected override void UpdateInvincibilityDisplay(bool isInvincible) {

        //if invincible, deactivate the physics colliders: the main car will pass through the enemies
        //only trigger colliders will be activated to detect collectibles
        foreach (var collider in GetComponentsInChildren<Collider>()) {

            if (!collider.isTrigger) {
                collider.enabled = !isInvincible;
            }
        }

        //disable the renderer instead of deactivating the GameObject to keep the colliders enabled
        goModel.GetComponent<MeshRenderer>().enabled = !isInvincible;
        goModelFaded.SetActive(isInvincible);
    }

    private void HandleDamage() {

        TryLoseLife(1);

        //shake camera
        var camera = Camera.main;
        var initialCamPos = camera.transform.localPosition;
        var tween = camera.DOShakePosition(0.5f, 2, 20);
        tween.OnKill(() => camera.transform.localPosition = initialCamPos);
    }

    protected override void OnCollisionWithVehicle(VehicleBehavior vehicleBehavior) {

        HandleDamage();
    }

    protected override void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior) {

        obstacleBehavior.Explode(transform);

        //removed to have an easier game
        //HandleDamage();
    }

    protected override void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior) {

        if (lifeBehavior.Life < lifeBehavior.MaxLife) {
            collectibleBehavior.Collect();
        }

        //hearts collectibles refills life so update the smoke
        UpdateDamageParticles();
    }

    protected override void OnVehicleExplosionRequired() {

        poolExplosion.Spawn(transform.position, transform.rotation);

        gameObject.SetActive(false);

        //trigger gameover when main car has exploded
        gameManager.StopPlaying();
    }

}
