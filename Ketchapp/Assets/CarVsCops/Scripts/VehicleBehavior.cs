using UnityEngine;
using System.Collections;


public abstract class VehicleBehavior : MonoBehaviour {


    [SerializeField] protected GameObject goModel;
    [SerializeField] protected LifeBehavior lifeBehavior;


    void OnDisable() {

        //stop invincibility coroutines to avoid bugs after pooling
        StopAllCoroutines();
    }

    public void Update() {

        //freezing position / rotation are not necessary : bugs can occur with collisions
        var pos = transform.localPosition;
        pos.y = 0;
        transform.localPosition = pos;

        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
    }

    public void Show() {

        goModel.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void TryLoseLife() {

        if (lifeBehavior.isInvincible) {
            //nothing happens to the vehicle if invincible
            return;
        }

        lifeBehavior.DecrementLife();

        if (lifeBehavior.IsDead()) {
            Explode();
        } else {
            StartCoroutine(SetInvincibleForDuration());
        }
    }

    private IEnumerator SetInvincibleForDuration() {

        lifeBehavior.isInvincible = true;

        UpdateInvincibilityDisplay();

        //set invincible for 3sec
        yield return new WaitForSeconds(3);

        lifeBehavior.isInvincible = false;

        UpdateInvincibilityDisplay();
    }

    protected abstract void UpdateInvincibilityDisplay();

    public void Explode() {

        if (!gameObject.activeSelf) {
            return;
        }

        goModel.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = true;

        ///TODO explosion

        OnVehicleExplode();
    }

    void OnCollisionEnter(Collision collision) {

        var goOther = collision.gameObject;

        var enemyBehavior = goOther.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null) {
            OnCollisionWithEnemy(enemyBehavior);
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

    protected virtual void OnCollisionWithEnemy(VehicleBehavior vehicleBehavior) {

        TryLoseLife();
    }

    protected virtual void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior) {

        obstacleBehavior.Explode();

        TryLoseLife();
    }

    protected virtual void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior) {
        //override this
    }

    protected virtual void OnVehicleExplode() {
        //override this
    }

}
