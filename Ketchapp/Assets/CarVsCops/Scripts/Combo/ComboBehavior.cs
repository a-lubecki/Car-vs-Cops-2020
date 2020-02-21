using System;
using UnityEngine;


public class ComboBehavior : MonoBehaviour {


    private static readonly float COMBO_DURATION_SEC = 4;


    public GameObject goListener;
    private IComboBehaviorListener Listener {
        get {
            if (goListener == null) {
                return null;
            }
            return goListener?.GetComponent<IComboBehaviorListener>();
        }
    }

    public bool IsComboEnabled { get; private set; }
    public int ComboMultiplier { get; private set; }

    ///the time when the combo must be disabled
    private float? disablingTime;


    protected void Update() {

        if (disablingTime.HasValue && Time.timeSinceLevelLoad > disablingTime.Value) {
            //time has elapsed, the combo must be disabled
            disablingTime = null;
            SetComboEnabled(false);
        }
    }

    protected void OnDisable() {

        StopAllCoroutines();
    }

    public void SetComboEnabled(bool enabled) {

        if (enabled != IsComboEnabled) {

            IsComboEnabled = enabled;

            //reset multiplier if state change
            ComboMultiplier = 1;

            if (IsComboEnabled) {
                Listener?.OnComboEnabled();
            } else {
                Listener?.OnComboDisabled();
            }
        }

        if (IsComboEnabled) {
            //set the time to disable at now + some seconds
            disablingTime = Time.timeSinceLevelLoad + COMBO_DURATION_SEC;
        }
    }

    public void SetComboMultiplier(int multiplier) {

        if (multiplier <= 0) {
            throw new ArgumentException();
        }
        if (!IsComboEnabled) {
            throw new InvalidOperationException("You must enable the combo before changing the multiplier");
        }

        if (multiplier == ComboMultiplier) {
            //no changes
            return;
        }

        ComboMultiplier = multiplier;

        Listener?.OnComboMultiplierChange();
    }

}


public interface IComboBehaviorListener {

    void OnComboEnabled();
    void OnComboDisabled();
    void OnComboMultiplierChange();

}
