using System;
using UnityEngine;

public class MainCarBehavior : MonoBehaviour, ICarControlsManagerListener {


    private static readonly float ROTATION_ANGLE_DEGREES_PER_SEC = 200f;


    private bool mustRotateLeft;
    private bool mustRotateRight;


    void Update() {

        if (mustRotateLeft || mustRotateRight) {

            var multiplier = mustRotateLeft ? -1 : 1;

            //add rotation by multiplying the quaternions
            transform.localRotation *= Quaternion.Euler(
                0,
                multiplier * ROTATION_ANGLE_DEGREES_PER_SEC * Time.deltaTime,
                0
            );

            mustRotateLeft = false;
            mustRotateRight = false;
        }
    }

    public void onLeftPressed() {

        //udate bool then waitfor next update
        mustRotateLeft = true;
        mustRotateRight = false;
    }

    public void onRightPressed() {

        //udate bool then waitfor next update
        mustRotateRight = true;
        mustRotateLeft = false;
    }

}
