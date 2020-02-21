using UnityEngine;


public class DamageParticlesBehavior : MonoBehaviour {


    [SerializeField] private GameObject goParticlesSmoke = null;
    [SerializeField] private GameObject goParticlesFire = null;


    public void ActivateSmoke() {

        goParticlesSmoke.SetActive(true);
        goParticlesFire.SetActive(false);
    }

    public void ActivateFire() {

        goParticlesSmoke.SetActive(false);
        goParticlesFire.SetActive(true);
    }

    public void Deactivate() {

        goParticlesSmoke.SetActive(false);
        goParticlesFire.SetActive(false);
    }

}
