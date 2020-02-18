using UnityEngine;

public class GroundTreadmillBehavior : MonoBehaviour {


    [SerializeField] private Transform trTargetToFollow;
    private MeshRenderer meshRenderer;


    void Start() {

        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update() {

        var scale = meshRenderer.material.mainTextureScale;

        meshRenderer.material.mainTextureOffset = new Vector2(
            -trTargetToFollow.position.x / 10f,// / scale.x,
            -trTargetToFollow.position.z/ 10f// / scale.y
        );
    }

    void LateUpdate() {

        //the ground must be attached to the target
        transform.position = trTargetToFollow.position;
    }

}
