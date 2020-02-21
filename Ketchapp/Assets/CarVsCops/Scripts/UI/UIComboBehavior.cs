using UnityEngine;
using TMPro;
using DG.Tweening;


public class UIComboBehavior : BaseUIBehavior {


    [SerializeField] private TextMeshProUGUI textMultiplier = null;
    [SerializeField] private float multiplierAnimationScale = 1;
    [SerializeField] private Color multiplierAnimationColor = Color.white;

    private Color originalTextMultiplierColor;


    protected override void Awake() {
        base.Awake();

        originalTextMultiplierColor = textMultiplier.color;
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
