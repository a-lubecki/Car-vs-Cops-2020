using System;
using UnityEngine;


public class EnemyTurnBehavior : BaseTurnBehavior {


    private Transform trTargetToFollow;

    //we can consider the enemy is aligned with the target if it's in the alignment range (the range is an angle)
    private float alignmentRangeDegree = 10;


    public void OnEnable() {

        //min range is 10 degrees to avoid snaking and more to add more random on the enemy group trajectories
        alignmentRangeDegree = UnityEngine.Random.Range(10, 90);
    }


    public void InitTargetToFollow(Transform trTargetToFollow) {

        this.trTargetToFollow = trTargetToFollow ?? throw new ArgumentException();
    }

    protected override void Update() {

        if (trTargetToFollow == null) {
            //no target to follow
            return;
        }

        //calculate the angle between the current moving direction of the car and the line formed by the car and its target
        var direction = trTargetToFollow.position - transform.position;

        var angleBetweenCars = Vector2.SignedAngle(
            new Vector2(direction.x, direction.z),
            Vector2.down
        );
        var angle = transform.rotation.eulerAngles.y - angleBetweenCars;

        //normalize angle
        angle = angle % 360;
        if (angle < 0) {
            angle += 360;
        }

        //check if must rotate left or right depending of the calculated angle
        //cut 10 degrees to avoid snaking
        var halfRange = 0.5 * alignmentRangeDegree;
        if (180 + halfRange <= angle && angle <= 360) {
            MustRotateLeft = true;
        } else if (0 <= angle && angle <= 180 - halfRange) {
            MustRotateRight = true;
        }

        base.Update();
    }

}
