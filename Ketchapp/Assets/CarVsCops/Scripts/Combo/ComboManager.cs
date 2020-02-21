using UnityEngine;
using System;


public class ComboManager : MonoBehaviour {


    private int comboMultiplier;
    public int ComboMultiplier {
        get {
            return comboMultiplier;
        }
        set {
            if (value < 0) {
                throw new ArgumentException();
            }

            comboMultiplier = value;
        }
    }

    public bool IsComboEnabled() {
        return comboMultiplier > 0;
    }

}
