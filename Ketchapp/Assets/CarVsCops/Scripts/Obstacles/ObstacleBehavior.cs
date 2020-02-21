using UnityEngine;

public class ObstacleBehavior : MonoBehaviour {


    [SerializeField] private ItemDestructorBehavior itemDestructorBehavior = null;


    public void Explode() {

        ///TODO explosion

        itemDestructorBehavior.DestroyCurrentItem();
    }

}
