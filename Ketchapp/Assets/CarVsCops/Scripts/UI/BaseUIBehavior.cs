using UnityEngine;
using DG.Tweening;


public abstract class BaseUIBehavior : MonoBehaviour {


    private CanvasGroup canvasGroup;

    public bool IsShown { get; private set; }


    protected virtual void Awake() {

        canvasGroup = GetComponent<CanvasGroup>();

        IsShown = true;
        Hide(false);
    }

    public void Show(bool animated) {

        if (IsShown) {
            //already shown
            return;
        }

        IsShown = true;

        canvasGroup.DOFade(1, animated ? 0.5f : 0);

        UpdateUI(animated);
    }

    public void Hide(bool animated) {

        if (!IsShown) {
            //already hidden
            return;
        }

        IsShown = false;

        canvasGroup.DOFade(0, animated ? 0.5f : 0);

        UpdateUI(animated);
    }

    protected virtual void UpdateUI(bool animated) {
        //override this
    }

}
