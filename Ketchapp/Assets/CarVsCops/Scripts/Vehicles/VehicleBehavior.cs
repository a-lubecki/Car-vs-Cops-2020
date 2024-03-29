﻿using UnityEngine;
using System.Collections;


public abstract class VehicleBehavior : MonoBehaviour {


    [SerializeField] protected LifeBehavior lifeBehavior = null;
    [SerializeField] protected DamageParticlesBehavior damageParticlesBehavior = null;
    [SerializeField] protected float invincibilityDurationSec = 1;


    protected void OnEnable() {

        GetComponent<Rigidbody>().isKinematic = false;

        //set invincible and fake display until InitLife is called
        lifeBehavior.isInvincible = true;
        UpdateInvincibilityDisplay(false);
    }

    protected void OnDisable() {

        //stop invincibility coroutines to avoid bugs after pooling
        StopAllCoroutines();
    }

    protected void Update() {

        //freezing position / rotation in the rigidbody constraints are not enough : bugs can occur with collisions
        var pos = transform.localPosition;
        pos.y = 0;
        transform.localPosition = pos;

        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
    }

    public void InitVehicle() {

        SetInvincible(false);

        lifeBehavior.Life = lifeBehavior.MaxLife;

        UpdateDamageParticles();
    }

    ///try to lose 1 life if not invincible then return true if the vehicle really lost 1 life
    protected void TryLoseLife(int value, bool forceLoseLife = false) {

        if (forceLoseLife) {
            SetInvincible(false);
        }

        if (lifeBehavior.isInvincible) {
            //nothing happens to the vehicle if invincible
            return;
        }

        lifeBehavior.TryDecrementLife(value);

        if (lifeBehavior.IsDead()) {
            Explode();
        } else {
            StartCoroutine(SetInvincibleForDuration());
        }

        UpdateDamageParticles();
    }

    private IEnumerator SetInvincibleForDuration() {

        SetInvincible(true);

        //set invincible for 3sec
        yield return new WaitForSeconds(invincibilityDurationSec);

        SetInvincible(false);
    }

    private void SetInvincible(bool invincible) {

        lifeBehavior.isInvincible = invincible;
        UpdateInvincibilityDisplay(lifeBehavior.isInvincible);
    }

    protected abstract void UpdateInvincibilityDisplay(bool isInvincible);

    protected virtual void UpdateDamageParticles() {

        //show particles depending on life value
        var life = lifeBehavior.Life;
        if (life == 2) {
            damageParticlesBehavior.ActivateSmoke();
        } else if (life == 1) {
            damageParticlesBehavior.ActivateFire();
        } else {
            damageParticlesBehavior.Deactivate();
        }
    }

    protected void Explode() {

        GetComponent<Rigidbody>().isKinematic = true;

        OnVehicleExplosionRequired();
    }

    protected void OnCollisionEnter(Collision collision) {

        var goOther = collision.gameObject;

        var vehicleBehavior = goOther.GetComponentInParent<VehicleBehavior>();
        if (vehicleBehavior != null) {
            OnCollisionWithVehicle(vehicleBehavior);
        }

        var obstacleBehavior = goOther.GetComponent<ObstacleBehavior>();
        if (obstacleBehavior != null) {
            OnCollisionWithObstacle(obstacleBehavior);
        }

        var collectibleBehavior = goOther.GetComponent<CollectibleBehavior>();
        if (collectibleBehavior != null) {
            OnCollisionWithCollectible(collectibleBehavior);
        }
    }

    protected void OnTriggerEnter(Collider collider) {

        //only the main car can detect collectibles with trigger (when the car is invincible)
        var collectibleBehavior = collider.GetComponent<CollectibleBehavior>();
        if (collectibleBehavior != null) {
            OnCollisionWithCollectible(collectibleBehavior);
        }

        var obstacleBehavior = collider.GetComponent<ObstacleBehavior>();
        if (obstacleBehavior != null) {
            OnCollisionWithObstacle(obstacleBehavior);
        }
    }

    protected abstract void OnCollisionWithVehicle(VehicleBehavior vehicleBehavior);

    protected abstract void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior);

    protected abstract void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior);

    protected abstract void OnVehicleExplosionRequired();

}
