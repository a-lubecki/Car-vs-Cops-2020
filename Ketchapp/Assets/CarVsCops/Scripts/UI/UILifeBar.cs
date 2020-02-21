using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UILifeBar : MonoBehaviour, ILifeBehaviorListener {


    [SerializeField] private RawImage[] hearts = null;


    public void OnLifeChange(int life, int previousLife) {

        UpdateLife(life, previousLife, true);
    }

    public void UpdateLife(int life, int previousLife, bool animated) {

        int lifePos = life - 1;

        for (int i = 0 ; i < hearts.Length ; i++) {

            if (i <= lifePos) {
                //heart is shown
                ShowHeart(i, animated);
            } else {
                //heart is hidden
                HideHeart(i, animated);
            }
        }
    }

    private RawImage getHeart(int pos) {

        if (pos < 0) {
            return null;
        }
        if (pos >= hearts.Length) {
            return null;
        }

        return hearts[pos];
    }

    private void ShowHeart(int pos, bool animated) {

        var h = getHeart(pos);
        if (h == null) {
            return;
        }

        h.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic);
    }

    private void HideHeart(int pos, bool animated) {

        var h = getHeart(pos);
        if (h == null) {
            return;
        }

        h.transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
    }

}
