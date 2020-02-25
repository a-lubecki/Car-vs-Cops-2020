using UnityEngine;
using TMPro;


public class UIGameOverBehavior : BaseUIBehavior {


    [SerializeField] private TextMeshProUGUI textBestScore = null;
    [SerializeField] private ScoreManager scoreManager = null;


    protected override void UpdateUI(bool animated) {
        base.UpdateUI(animated);

        textBestScore.text = scoreManager.MaxScore.ToString();
    }

}
