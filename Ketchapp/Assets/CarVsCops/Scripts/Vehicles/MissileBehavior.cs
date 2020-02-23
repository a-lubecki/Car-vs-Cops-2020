using System.Collections;
using UnityEngine;


public class MissileBehavior : EnemyBehavior {


    protected override void UpdateDamageParticles() {

        //show particles depending on life value
        var life = lifeBehavior.Life;
        if (life >= 2) {
            damageParticlesBehavior.ActivateSmoke();
        } else if (life == 1) {
            damageParticlesBehavior.ActivateFire();
        } else {
            damageParticlesBehavior.Deactivate();
        }
    }

    protected override void OnCollisionWithVehicle(VehicleBehavior vehicleBehavior) {

        if (vehicleBehavior is MainCarBehavior) {
            //explode after delay to let the main car detect the collision
            StartCoroutine(ExplodeAfterDelay());
        } else {
            base.OnCollisionWithVehicle(vehicleBehavior);
        }
    }

    private IEnumerator ExplodeAfterDelay() {

        yield return new WaitForEndOfFrame();

        Explode();
    }

}
