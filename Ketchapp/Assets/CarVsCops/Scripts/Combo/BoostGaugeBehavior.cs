using UnityEngine;
using UnityEngine.UI;


public class BoostGaugeBehavior : MonoBehaviour {


    [SerializeField] private float speedIncrement = 0;
    [SerializeField] private float speedDecrement = 0;

    public GameObject goListener = null;
    private IBoostGaugeBehaviorListener Listener {
        get {
            if (goListener == null) {
                return null;
            }
            return goListener?.GetComponent<IBoostGaugeBehaviorListener>();
        }
    }

    private Image imageGauge;

    public bool IsIncrementing { get; private set; }
    public bool IsDecrementing { get; private set; }


    protected void Awake() {

        imageGauge = GetComponent<Image>();
    }

    protected void OnEnable() {

        IsIncrementing = false;
        IsDecrementing = false;

        imageGauge.fillAmount = 0;
    }

    protected void Update() {

        if (IsIncrementing) {
            AddGaugeAmount(speedIncrement * Time.deltaTime);
        } else if (IsDecrementing) {
            AddGaugeAmount(-speedDecrement * Time.deltaTime);
        }
    }

    public void SetIncrementing() {

        IsIncrementing = true;
        IsDecrementing = false;
    }

    public void SetDecrementing() {

        IsIncrementing = false;
        IsDecrementing = true;
    }

    private void AddGaugeAmount(float value) {

        imageGauge.fillAmount += value;

        Listener?.OnBoostGaugeValueUpdate(imageGauge.fillAmount);
    }

}


public interface IBoostGaugeBehaviorListener {

    void OnBoostGaugeValueUpdate(float percentage);

}