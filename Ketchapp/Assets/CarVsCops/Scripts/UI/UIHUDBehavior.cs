using UnityEngine;
using TMPro;
using DG.Tweening;


public class UIHUDBehavior : BaseUIBehavior {


    [SerializeField] private TextMeshProUGUI textScore = null;
    [SerializeField] private TextMeshProUGUI textAddedScoreValue = null;
    [SerializeField] private TextMeshProUGUI textBoost = null;

    [SerializeField] private ComboManager boostManager = null;
    [SerializeField] private BoostGaugeBehavior boostGaugeBehavior = null;


    //optim: this boolean is used to avoid calling the DOTween fade method every Update call
    private bool isDisplayingBoost;


    void Start() {

        textAddedScoreValue.alpha = 0;
    }

    void OnEnable() {

        //first init
        UpdateTextBoost(false, true);
    }

    void Update() {

        //the text is reactive
        UpdateTextBoost(true, false);
    }

    protected override void UpdateUI(bool animated) {
        base.UpdateUI(animated);

        UpdateTextBoost(animated, true);
     }

    public void UpdateTextBoost(bool animated, bool forceUpdate) {

        var isTextBoostActive = boostGaugeBehavior.IsIncrementing && !boostManager.IsComboEnabled();
        if (!forceUpdate && isTextBoostActive == isDisplayingBoost) {
            //previously updated
            return;
        }

        isDisplayingBoost = isTextBoostActive;

        textBoost.DOFade(
            isTextBoostActive ? 1 : 0,
            animated ? 1 : 0
        );
    }

    public void UpdateTextScore(int score, int addedValue, bool animated) {

        textScore.text = score.ToString();

        //animated aded value
        if (addedValue > 0) {
            textAddedScoreValue.text = "+" + addedValue.ToString();
            textAddedScoreValue.alpha = 1;
            textAddedScoreValue.DOFade(0, 1f);
        }
    }

}
