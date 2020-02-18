using UnityEngine;

public class ObstacleBehavior : MonoBehaviour {


    public void Explode() {

        if (!gameObject.activeSelf) {
            return;
        }

        gameObject.SetActive(false);

        ///TODO explosion

        ///TODO POOL
    }

}
