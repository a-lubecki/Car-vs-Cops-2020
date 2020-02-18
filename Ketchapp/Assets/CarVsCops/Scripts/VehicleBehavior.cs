using System;
using UnityEngine;

public class VehicleBehavior : MonoBehaviour {


    public void Update() {

        //freezing position / rotation are not necessary : bugs can occur with collisions
        var pos = transform.localPosition;
        pos.y = 0;
        transform.localPosition = pos;

        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
    }

    public void Show() {

        gameObject.SetActive(true);
    }

    public void Explode() {

        if (!gameObject.activeSelf) {
            return;
        }

        gameObject.SetActive(false);

        ///TODO explosion
    }

}
