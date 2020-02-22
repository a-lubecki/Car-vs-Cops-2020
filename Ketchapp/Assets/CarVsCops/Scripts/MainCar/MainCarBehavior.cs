using UnityEngine;
using DG.Tweening;
using Lean.Pool;


public class MainCarBehavior : VehicleBehavior {


    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private GameObject goModel = null;
    [SerializeField] private GameObject goModelFaded = null;
    [SerializeField] private LeanGameObjectPool poolExplosion = null;


    protected override void UpdateInvincibilityDisplay(bool isInvincible) {

        //chage to trigger instead of disabling so that the car will pass through the other
        // cars and obstacles but with still detect the life items
        PhysicsCollider.enabled = !isInvincible;

        goModel.SetActive(!isInvincible);
        goModelFaded.SetActive(isInvincible);
    }

    private void HandleDamage() {

        TryLoseLife(1);

        var camera = Camera.main;
        var initialCamPos = camera.transform.localPosition;
        var tween = camera.DOShakePosition(0.5f, 2, 20);
        tween.OnKill(() => camera.transform.localPosition = initialCamPos);///TODO TEST
    }

    protected override void OnCollisionWithVehicle(VehicleBehavior vehicleBehavior) {

        HandleDamage();
    }

    protected override void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior) {

        obstacleBehavior.Explode(transform);

        HandleDamage();
    }

    protected override void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior) {

        collectibleBehavior.Collect();

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
