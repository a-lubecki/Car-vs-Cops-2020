using UnityEngine;
using Lean.Pool;


public class ItemDestructorBehavior : MonoBehaviour {


    [SerializeField] private float distanceFromCenter = 500;
    private LeanGameObjectPool pool;
    private Transform trDropPoint;

    ///a boolean indicating that the object is destroying and can't call the destroy method again
    private bool isDestroying;

    public IItemDestructorBehaviorListener listener;


    public void Init(LeanGameObjectPool pool, Transform trDropPoint, IItemDestructorBehaviorListener listener) {

        this.pool = pool;
        this.trDropPoint = trDropPoint;
        this.listener = listener;
    }

    void OnDisable() {

        isDestroying = false;
        listener = null;
    }

    void Update() {

        //destroy item if too far from the center
        if (Vector3.Distance(transform.position, trDropPoint.position) > distanceFromCenter) {
            DestroyCurrentItem();
        }
    }

    public void DestroyCurrentItem() {

        if (isDestroying) {
            //the DestroyCurrentItem() has already been called, it can be cause by a recursion loop due to the previous listener notify
            return;
        }

        isDestroying = true;

        //notify the listener before despawning to be able to get info about the object in the OnItemDestroyed()
        listener?.OnItemDestroyed(gameObject);

        //avoid destroying if the OnDisable has been called by the listener notify
        if (isDestroying) {
            pool.Despawn(gameObject);
        }
    }

}


public interface IItemDestructorBehaviorListener {

    void OnItemDestroyed(GameObject goItem);

}
