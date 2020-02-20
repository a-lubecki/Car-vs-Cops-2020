using UnityEngine;


public class TargetTrackerBehavior : MonoBehaviour {


    [SerializeField] private Transform trTargetToFollow;


    void Update() {

        //track the target
        transform.position = trTargetToFollow.position;
        //transform.rotation = trTargetToFollow.rotation;
    }

}
