using System;
using UnityEngine;
using Lean.Pool;


public class ItemGeneratorBehavior : BaseGeneratorBehavior {


    [SerializeField] private LeanGameObjectPool poolPoliceCar;


    public void GeneratePoliceCars(int count) {
        GenerateItems(count, poolPoliceCar, true);
    }

}
