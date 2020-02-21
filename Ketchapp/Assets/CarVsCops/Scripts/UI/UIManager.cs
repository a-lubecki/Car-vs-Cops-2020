using UnityEngine;


public class UIManager : MonoBehaviour {


    [SerializeField] private UIOnboardingBehavior uiOnboardingBehavior;
    [SerializeField] private UIGameOverBehavior uiGameOverBehavior;
    [SerializeField] private UIHUDBehavior uiHudBehavior;
    [SerializeField] private UIComboBehavior uiComboBehavior;


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

}
