using UnityEngine;


public class TargetTrackerBehavior : MonoBehaviour {


    [SerializeField] private Transform trTargetToFollow = null;

    [SerializeField] private bool mustTrackPosition = false;
    [SerializeField] private bool mustTrackRotation = false;
    [SerializeField] private bool mustChildrenTrackActiveSelf = false;


    protected void Update() {

        if (mustTrackPosition) {
            transform.position = trTargetToFollow.position;
        }

        if (mustTrackRotation) {
            transform.rotation = trTargetToFollow.rotation;
        }

        if (mustChildrenTrackActiveSelf) {

            var isTargetActive = trTargetToFollow.gameObject.activeSelf;

            foreach (Transform t in transform) {
                t.gameObject.SetActive(isTargetActive);
            }
        }

    }

}
