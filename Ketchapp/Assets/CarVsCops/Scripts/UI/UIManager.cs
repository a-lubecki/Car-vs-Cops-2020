using UnityEngine;


public class UIManager : MonoBehaviour {


    [SerializeField] private UIOnboardingBehavior uiOnboardingBehavior = null;
    [SerializeField] private UIGameOverBehavior uiGameOverBehavior = null;
    [SerializeField] private UIHUDBehavior uiHudBehavior = null;
    [SerializeField] private UIComboBehavior uiComboBehavior = null;


    public void ShowUIOnboarding(bool animated) {

        uiOnboardingBehavior.Show(animated);
        uiGameOverBehavior.Hide(animated);
        uiHudBehavior.Hide(animated);
        HideUICombo(animated);
    }

    public void ShowUIGameOver(bool animated) {

        uiGameOverBehavior.Show(animated);
        uiOnboardingBehavior.Hide(animated);
        HideUICombo(animated);
        UpdateBoost(false, 0);
    }

    public void ShowUIHUD(bool animated) {

        uiHudBehavior.Show(animated);
        uiOnboardingBehavior.Hide(animated);
        uiGameOverBehavior.Hide(animated);
    }

    public void ShowUICombo(bool animated) {

        uiComboBehavior.Show(animated);
    }

    public void HideUICombo(bool animated) {

        uiComboBehavior.Hide(animated);
    }

    public void UpdateScore(int score, int addedValue, bool animated) {

        uiHudBehavior.UpdateTextScore(score, addedValue, animated);
    }

    public void UpdateBoost(bool displayed, float percentage) {

        uiHudBehavior.UpdateTextBoostAlpha(displayed, percentage);
    }
}
