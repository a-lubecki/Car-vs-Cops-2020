using System.Collections;
using UnityEngine;
using Lean.Pool;

public class ParticleExplosionBehavior : MonoBehaviour {


    protected void OnEnable() {

        StartCoroutine(DisappearAfterDelay());
    }

    private IEnumerator DisappearAfterDelay() {

        yield return new WaitForSeconds(3);

        LeanPool.Despawn(this);
    }

}
