using UnityEngine;


public class TargetTrackerBehavior : MonoBehaviour {


    [SerializeField] private Transform trTargetToFollow = null;


    protected void Update() {

        //track the target
        transform.position = trTargetToFollow.position;
    }

}
