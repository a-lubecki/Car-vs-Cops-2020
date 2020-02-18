using UnityEngine;

public class BaseMainCarBehavior : MonoBehaviour {


    [SerializeField] private float maxRotationAnglePerSec = 200f;

    public bool mustRotateLeft { get; protected set; }
    public bool mustRotateRight { get; protected set; }


    protected virtual void Update() {

        if (mustRotateLeft || mustRotateRight) {

            var multiplier = mustRotateLeft ? -1 : 1;

            //add rotation by multiplying the quaternions
            transform.localRotation *= Quaternion.Euler(
                0,
                multiplier * maxRotationAnglePerSec * Time.deltaTime,
                0
            );

            mustRotateLeft = false;
            mustRotateRight = false;
        }
    }

}
