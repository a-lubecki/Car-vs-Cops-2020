using UnityEngine;


public class EnemyCarBehavior : BaseMainCarBehavior {


    [SerializeField] private Transform trTargetToFollow;


    public void InitTargetToFollow(Transform trTargetToFollow) {

        this.trTargetToFollow = trTargetToFollow;
    }

    protected override void Update() {
        base.Update();

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
        if (angle > 180) {
            mustRotateLeft = true;
        } else if (angle < 180) {
            mustRotateRight = true;
        }
    }

}
