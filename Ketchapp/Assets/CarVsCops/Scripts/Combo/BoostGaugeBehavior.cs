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

        var previousAmount = imageGauge.fillAmount;

        imageGauge.fillAmount += value;

        var amount = imageGauge.fillAmount;
        if (amount != previousAmount) {
            Listener?.OnBoostGaugeValueChange(amount, previousAmount);
        }
    }

}


public interface IBoostGaugeBehaviorListener {

    void OnBoostGaugeValueChange(float percentage, float previousPercentage);

}