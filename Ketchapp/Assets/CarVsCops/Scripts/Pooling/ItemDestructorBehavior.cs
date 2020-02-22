using UnityEngine;
using Lean.Pool;


public class ItemDestructorBehavior : MonoBehaviour {


    [SerializeField] private float distanceFromCenter = 500;
    private LeanGameObjectPool pool;
    private Transform trDropPoint;

    ///a boolean indicating that the object is destroying and can't call the destroy method again
    private bool isDestroying;

    public IItemDestructorBehaviorListener listener;

    private LeanGameObjectPool poolExplosion;


    public void Init(LeanGameObjectPool pool, Transform trDropPoint, IItemDestructorBehaviorListener listener) {

        this.pool = pool;
        this.trDropPoint = trDropPoint;
        this.listener = listener;
    }

    public void InitPoolExplosion(LeanGameObjectPool poolExplosion) {

        this.poolExplosion = poolExplosion;
    }

    protected void OnDisable() {

        isDestroying = false;
        listener = null;
    }

    protected void Update() {

        //destroy item if too far from the center
        if (Vector3.Distance(transform.position, trDropPoint.position) > distanceFromCenter) {
            DestroyCurrentItem();
        }
    }

    public void ExplodeCurrentItem(Quaternion? overridenRotation = null) {

        //save position and rotation now as the item will be pooled
        var initialPos = transform.position;
        var initialRot = overridenRotation.HasValue ? overridenRotation.Value : transform.rotation;

        bool hasDestroyed = TryDestroyCurrentItem(true);

        if (hasDestroyed) {
            poolExplosion.Spawn(initialPos, initialRot);
        }
    }

    public void DestroyCurrentItem() {

        TryDestroyCurrentItem(false);
    }

    ///try to destroy then return true if it has been well destroyed
    private bool TryDestroyCurrentItem(bool hasExploded) {

        if (!gameObject.activeSelf) {
            //can't destroy if not active
            return false;
        }
        if (isDestroying) {
            //the DestroyCurrentItem() has already been called, it can be cause by a recursion loop due to the previous listener notify
            return false;
        }

        isDestroying = true;

        //notify the listener before despawning to be able to get info about the object in the OnItemDestroyed()
        listener?.OnItemDestroyed(gameObject, hasExploded);

        //avoid destroying if the OnDisable has been called by the listener notify
        if (gameObject.activeSelf && isDestroying) {
            pool.Despawn(gameObject);
        }

        return true;
    }

}


public interface IItemDestructorBehaviorListener {

    void OnItemDestroyed(GameObject goItem, bool hasExploded);

}
