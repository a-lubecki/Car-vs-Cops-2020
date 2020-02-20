﻿using System;
using UnityEngine;


///Increase the speed of an enemy when it's too far from the target
public class EnemySpeedBehavior : MonoBehaviour {


    [SerializeField] private float distanceFromTargetToActivateMaxSpeed;
    [SerializeField] private float maxSpeed;

    private Transform trTargetToFollow;
    private ConstantForce force;
    private float originalForceValue;


    void Awake() {

        force = GetComponent<ConstantForce>();
        originalForceValue = force.relativeForce.z;
    }

    public void InitTargetToFollow(Transform trTargetToFollow) {

        this.trTargetToFollow = trTargetToFollow ?? throw new ArgumentException();
    }

    void Update() {

        if (trTargetToFollow == null) {
            //no target to follow
            return;
        }

        var distanceFromTarget = Vector2.Distance(
            new Vector2(trTargetToFollow.position.x, trTargetToFollow.position.z),
            new Vector2(transform.position.x, transform.position.z)
        );

        if (distanceFromTarget > distanceFromTargetToActivateMaxSpeed) {
            UpdateConstantForce(maxSpeed);
        } else {
            UpdateConstantForce(originalForceValue);
        }
    }

    private void UpdateConstantForce(float value) {

        var relativeForce = force.relativeForce;
        relativeForce.z = value;
        force.relativeForce = relativeForce;
    }

}