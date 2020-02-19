using UnityEngine;


public class HeartRotationBehavior : MonoBehaviour {


    [SerializeField] private float rotationSpeed;


    void Update() {

        //spin the heart
        var newValue = transform.rotation.eulerAngles.y + rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, newValue, 0);
    }

}
