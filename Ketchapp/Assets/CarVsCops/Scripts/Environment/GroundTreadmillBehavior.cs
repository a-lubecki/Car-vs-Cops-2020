using UnityEngine;


public class GroundTreadmillBehavior : MonoBehaviour {


    [SerializeField] private Transform trTargetToFollow = null;

    private MeshRenderer meshRenderer;


    protected void Start() {

        meshRenderer = GetComponent<MeshRenderer>();
    }

    protected void Update() {

        //move the texture like the target to make the illusion of a static ground
        meshRenderer.material.mainTextureOffset = new Vector2(
            -trTargetToFollow.position.x / 10f,
            -trTargetToFollow.position.z / 10f
        );

        //attached to the target
        transform.position = trTargetToFollow.position;
    }

}
