using UnityEngine;


public class UIManager : MonoBehaviour, IQuitGameBehaviorListener {


    [SerializeField] private UIOnboardingBehavior uiOnboardingBehavior = null;
    [SerializeField] private UIGameOverBehavior uiGameOverBehavior = null;
    [SerializeField] private UIHUDBehavior uiHudBehavior = null;
    [SerializeField] private UIComboBehavior uiComboBehavior = null;
    [SerializeField] private BaseUIBehavior uiQuitBehavior = null;


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

    public void UpdateLife(int life, int previousLife, bool animated) {

        uiHudBehavior.UpdateLife(life, previousLife, animated);
    }

    public void UpdateBoost(bool displayed, float percentage) {

        uiHudBehavior.UpdateTextBoostAlpha(displayed, percentage);
    }

    public void UpdateComboMultiplier(int multiplier, bool animated) {

        uiComboBehavior.UpdateMultiplier(multiplier, animated);
    }

    public void UpdateScore(int score, int addedValue, bool animated) {

        uiHudBehavior.UpdateTextScore(score, addedValue, animated);
    }

    public void OnQuitModeEnabled() {

        uiQuitBehavior.Show(true);
    }

    public void OnQuitModeDisabled() {

        uiQuitBehavior.Hide(true);
    }
    
}
