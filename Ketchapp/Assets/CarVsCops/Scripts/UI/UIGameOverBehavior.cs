using UnityEngine;
using TMPro;


public class UIGameOverBehavior : BaseUIBehavior {


    [SerializeField] private TextMeshProUGUI textBestScore;
    [SerializeField] private ScoreManager scoreManager;


    protected override void UpdateUI(bool animated) {
        base.UpdateUI(animated);

        textBestScore.text = scoreManager.MaxScore.ToString();
     }

}
