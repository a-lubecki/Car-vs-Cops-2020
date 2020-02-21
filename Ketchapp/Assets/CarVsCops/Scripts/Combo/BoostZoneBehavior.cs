using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BoostZoneBehavior : MonoBehaviour {


    [SerializeField] private BoostGaugeBehavior boostGaugeBehavior = null;

    private Image imageZone;
    private Collider zoneCollider;

    ///the retained objects in the zone, useful as Unity doesn't provide the inverse method of OnCOllisionStay
    private HashSet<GameObject> objectsInZone = new HashSet<GameObject>();


    protected void Awake() {

        imageZone = GetComponent<Image>();
        zoneCollider = GetComponent<Collider>();
    }

    protected void OnEnable() {

        //deactivate gauge display when the car appear
        SetGaugeActivated(false, false);
    }

    protected void OnDisable() {

        objectsInZone.Clear();
    }

    protected void LateUpdate() {

        //when an object is in the zone and deactivated, the OnTriggerExit won't be called so
        // if it happen, remove the object and try deactivating gauge as if OnTriggerExitd would be called
        HashSet<GameObject> objectsToRemove = null;

        foreach (var go in objectsInZone) {

            if (go.activeSelf && AreCollidersInZone(go)) {
                //the object can still call OnTriggerExit
                continue;
            }

            //lazy init for optimization
            if (objectsToRemove == null) {
                objectsToRemove = new HashSet<GameObject>();
            }

            //mark as removable
            objectsToRemove.Add(go);
        }

        if (objectsToRemove != null) {

            //iterate over objectsToRemove instead of objectsInZone to avoid modifing while iterating
            foreach (var go in objectsToRemove) {
                objectsInZone.Remove(go);
            }

            TryDeactivateGauge();
        }
    }

    private bool AreCollidersInZone(GameObject go) {

        if (!go.activeSelf) {
            return false;
        }

        foreach (var c in go.GetComponents<Collider>()) {

            if (c.isTrigger && zoneCollider.bounds.Intersects(c.bounds)) {
                //object is still in zone
                return true;
            }
        }

        return false;
    }

    protected void OnTriggerEnter(Collider collider) {

        if (collider.GetComponent<EnemyBehavior>() == null) {
            //not an enemy
            return;
        }

        //keep track of the entered colliders to know when there are no more triggering colliders (to disable boost)
        objectsInZone.Add(collider.gameObject);

        //when an enemy enter, the gauge is activated
        SetGaugeActivated(true, true);
    }

    protected void OnTriggerExit(Collider collider) {

        if (collider.GetComponent<EnemyBehavior>() == null) {
            //not an enemy
            return;
        }

        objectsInZone.Remove(collider.gameObject);

        TryDeactivateGauge();
    }

    private void TryDeactivateGauge() {

        //when all the enemies are not in the zone any more, the gauge is deactivated
        if (objectsInZone.Count <= 0) {
            SetGaugeActivated(false, true);
        }
    }

    private void SetGaugeActivated(bool isActive, bool animated) {

        if (isActive) {
            boostGaugeBehavior.SetIncrementing();
        } else {
            boostGaugeBehavior.SetDecrementing();
        }

        imageZone.DOFade(
            isActive ? 1 : 0,
            animated ? 0.5f : 0
        );
    }

}
