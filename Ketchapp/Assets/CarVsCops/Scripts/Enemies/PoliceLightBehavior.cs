using UnityEngine;


public class PoliceLightBehavior : MonoBehaviour {


    private static readonly float displayPeriodSec = 0.6f;

    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private float timeOffsetPercentage = 0;

    private Renderer lightRenderer;


    protected void Awake() {

        lightRenderer = GetComponent<Renderer>();
    }

    protected void Start() {

        lightRenderer.material.color = lightColor;
    }

    protected void Update() {

        //align object with camera for better display
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);

        //change display synchronized with time, offset is here to alternate blue and red
        var timeInPeriod = (Time.timeSinceLevelLoad + (timeOffsetPercentage * displayPeriodSec)) % displayPeriodSec;

        //the light is enabled from 0% to 50% and disabled from 50% to 100% of the period
        lightRenderer.enabled = (timeInPeriod / displayPeriodSec <= 0.5f);
    }

}
