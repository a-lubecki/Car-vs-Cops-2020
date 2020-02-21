using UnityEngine;
using DG.Tweening;


public abstract class BaseUIBehavior : MonoBehaviour {


    private CanvasGroup canvasGroup;


    void Awake() {

        canvasGroup = GetComponent<CanvasGroup>();

        Hide(false);
    }

    public void Show(bool animated) {

        canvasGroup.DOFade(1, animated ? 0.5f : 0);

        UpdateUI(animated);
    }

    public void Hide(bool animated) {

        canvasGroup.DOFade(0, animated ? 0.5f : 0);

        UpdateUI(animated);
    }

    protected virtual void UpdateUI(bool animated) {
        //override this
    }

}
