using UnityEngine;
using System.Collections;


public abstract class VehicleBehavior : MonoBehaviour {


    [SerializeField] private LifeBehavior lifeBehavior;

    protected Collider PhysicsCollider { get; private set; }
    protected Collider TriggerCollider { get; private set; }


    void Awake() {

        PhysicsCollider = GetComponents<Collider>()[0];
        TriggerCollider = GetComponents<Collider>()[1];
    }

    void OnEnable() {

        lifeBehavior.isInvincible = false;

        UpdateInvincibilityDisplay(lifeBehavior.isInvincible);
    }

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

    public void Init() {

        GetComponent<Rigidbody>().isKinematic = false;
    }

    protected void TryLoseLife() {

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

        UpdateInvincibilityDisplay(lifeBehavior.isInvincible);

        //set invincible for 3sec
        yield return new WaitForSeconds(3);

        lifeBehavior.isInvincible = false;

        UpdateInvincibilityDisplay(lifeBehavior.isInvincible);
    }

    protected abstract void UpdateInvincibilityDisplay(bool isInvincible);

    public void Explode() {

        if (!gameObject.activeSelf) {
            return;
        }

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

    void OnTriggerEnter(Collider collider) {

        //only the main car can detect collectibles with trigger (when the car is invincible)
        var collectibleBehavior = collider.GetComponent<CollectibleBehavior>();
        if (collectibleBehavior != null) {
            OnCollisionWithCollectible(collectibleBehavior);
        }
    }

    protected abstract void OnCollisionWithEnemy(VehicleBehavior vehicleBehavior);

    protected abstract void OnCollisionWithObstacle(ObstacleBehavior obstacleBehavior);

    protected abstract void OnCollisionWithCollectible(CollectibleBehavior collectibleBehavior);

    protected abstract void OnVehicleExplode();

}
