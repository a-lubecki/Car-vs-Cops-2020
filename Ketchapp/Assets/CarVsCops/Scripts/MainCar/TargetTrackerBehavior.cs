using UnityEngine;


public class TargetTrackerBehavior : MonoBehaviour {


    [SerializeField] private Transform trTargetToFollow = null;
    [SerializeField] private bool mustTrackRotation = false;


    protected void Update() {

        //track the target
        transform.position = trTargetToFollow.position;

        if (mustTrackRotation) {
            transform.localRotation = trTargetToFollow.localRotation;
        }
    }

}
