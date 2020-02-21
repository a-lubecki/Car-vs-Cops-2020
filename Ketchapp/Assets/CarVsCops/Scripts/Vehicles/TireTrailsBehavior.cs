using System.Collections;
using UnityEngine;


public class TireTrailsBehavior : MonoBehaviour {


    [SerializeField] private TrailRenderer[] tireTrailRenderers = null;


    protected void Awake() {

        //disable trail renderers to avoid glitches during init position setting
        SetTrailsEnabled(false);
    }

    protected void OnEnable() {

        //delay the display of the trails to let the init processing
        StartCoroutine(SetTrailsEnabledAfterDelay());
    }

    protected void OnDisable() {

        SetTrailsEnabled(false);

        StopAllCoroutines();
    }

    private IEnumerator SetTrailsEnabledAfterDelay() {

        yield return new WaitForSeconds(1);

        SetTrailsEnabled(true);
    }

    private void SetTrailsEnabled(bool enabled) {

        foreach (var r in tireTrailRenderers) {
            r.enabled = enabled;
        }
    }

}
