using UnityEngine;

public abstract class BaseTurnBehavior : MonoBehaviour {


    [SerializeField] private float maxRotationAnglePerSec = 200f;

    public bool MustRotateLeft { get; protected set; }
    public bool MustRotateRight { get; protected set; }


    protected virtual void Update() {

        if (MustRotateLeft || MustRotateRight) {

            var multiplier = MustRotateLeft ? -1 : 1;

            //add rotation by multiplying the quaternions
            transform.localRotation *= Quaternion.Euler(
                0,
                multiplier * maxRotationAnglePerSec * Time.deltaTime,
                0
            );

            MustRotateLeft = false;
            MustRotateRight = false;
        }
    }

}
