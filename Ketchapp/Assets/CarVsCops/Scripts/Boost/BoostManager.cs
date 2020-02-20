using UnityEngine;
using System;


public class BoostManager : MonoBehaviour {


    private int boostMultiplier;
    public int BoostMultiplier {
        get {
            return boostMultiplier;
        }
        set {
            if (value < 0) {
                throw new ArgumentException();
            }

            boostMultiplier = value;
        }
    }

    public bool IsBoostEnabled() {
        return boostMultiplier > 0;
    }

}
