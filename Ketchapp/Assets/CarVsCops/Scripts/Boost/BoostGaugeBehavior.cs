using UnityEngine;
using UnityEngine.UI;


public class BoostGaugeBehavior : MonoBehaviour {


    [SerializeField] private float speedIncrement;
    [SerializeField] private GameManager gameManager;

    private Image imageGauge;

    public bool IsIncrementing { get; private set; }
    public bool IsDecrementing { get; private set; }


    void Awake() {

        imageGauge = GetComponent<Image>();
    }

    void OnEnable() {

        IsIncrementing = false;
        IsDecrementing = false;

        imageGauge.fillAmount = 0;
    }

    void Update() {

        if (IsIncrementing) {
            AddGaugeAmount(speedIncrement * Time.deltaTime);
        } else if (IsDecrementing) {
            AddGaugeAmount(-speedIncrement * Time.deltaTime);
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

        var amount = imageGauge.fillAmount;

        gameManager.OnBoostGaugeValueChange(amount);
    }

}
