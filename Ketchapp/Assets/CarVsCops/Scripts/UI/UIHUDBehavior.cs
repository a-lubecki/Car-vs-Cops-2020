using UnityEngine;
using TMPro;
using DG.Tweening;


public class UIHUDBehavior : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI textBoost;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private BoostGaugeBehavior boostGaugeBehavior;


    //optim: this boolean is used to avoid calling the DOTween fade method every Update call
    private bool isDisplayingBoost;


    void OnEnable() {

        //first init
        UpdateTextBoost(false, true);
    }

    void Update() {

        //the text is reactive
        UpdateTextBoost(true, false);
    }

    public void UpdateUI(bool animated) {


        UpdateTextBoost(animated, true);
     }

    public void UpdateTextBoost(bool animated, bool forceUpdate) {

        var isTextBoostActive = boostGaugeBehavior.IsIncrementing && !gameManager.IsBoostEnabled;
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

}
