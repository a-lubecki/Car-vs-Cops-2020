using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class UIComboBehavior : BaseUIBehavior {


    [SerializeField] private RawImage imageFX = null;
    [SerializeField] private TextMeshProUGUI textMultiplier = null;

    [SerializeField] private Color colorFX0 = Color.white;
    [SerializeField] private Color colorFX1 = Color.white;

    [SerializeField] private float multiplierAnimationScale = 1;
    [SerializeField] private Color multiplierAnimationColor = Color.white;

    private Color currentColor;
    private Color originalTextMultiplierColor;


    protected override void Awake() {
        base.Awake();

        originalTextMultiplierColor = textMultiplier.color;
    }

    protected override void UpdateUI(bool animated) {
        base.UpdateUI(animated);

        imageFX.enabled = IsShown;

        if (IsShown) {
            PlayFXColorChange();
        }
    }

    private void PlayFXColorChange() {

        imageFX.DOKill(false);

        imageFX.color = colorFX0;
        imageFX.DOColor(colorFX1, 0.6f).SetLoops(-1, LoopType.Yoyo);
    }

    public void UpdateMultiplier(int multiplier, bool animated) {

        textMultiplier.text = "x" + multiplier;

        if (animated && multiplier >= 1) {

            //change scale
            textMultiplier.transform.localScale = Vector3.one * multiplierAnimationScale;
            textMultiplier.transform.DOScale(Vector3.one, 0.5f);

            //change color
            if (multiplier >= 2) {
                textMultiplier.color = multiplierAnimationColor;
                var tColor = textMultiplier.DOColor(originalTextMultiplierColor, 0.5f);
            }
        }
    }

}
