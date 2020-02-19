using System;
using UnityEngine;


public class EnemyTurnBehavior : BaseTurnBehavior {


    private Transform trTargetToFollow;


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
        if (185 <= angle && angle <= 360) {
            mustRotateLeft = true;
        } else if (0 <= angle && angle <= 175) {
            mustRotateRight = true;
        }

        base.Update();
    }

}
