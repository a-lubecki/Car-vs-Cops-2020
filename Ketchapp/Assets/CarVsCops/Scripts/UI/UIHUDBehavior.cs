using UnityEngine;
using TMPro;
using DG.Tweening;


public class UIHUDBehavior : BaseUIBehavior {


    [SerializeField] private TextMeshProUGUI textScore = null;
    [SerializeField] private TextMeshProUGUI textAddedScoreValue = null;
    [SerializeField] private TextMeshProUGUI textBoost = null;


    protected void Start() {

        textAddedScoreValue.alpha = 0;
        UpdateTextBoostAlpha(false, 0);
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

    public void UpdateTextBoostAlpha(bool displayed, float percentage) {

        var c = textBoost.color;
        c.a = displayed ? percentage : 0;
        textBoost.color = c;
    }

}
