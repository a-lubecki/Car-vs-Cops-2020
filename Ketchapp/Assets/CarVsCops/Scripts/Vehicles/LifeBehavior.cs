using UnityEngine;

public class LifeBehavior : MonoBehaviour {


    public GameObject goListener = null;

    private ILifeBehaviorListener Listener {
        get {
            if (goListener == null) {
                return null;
            }
            return goListener?.GetComponent<ILifeBehaviorListener>();
        }
    }

    [SerializeField] private int maxLife = 3;

    public int MaxLife {
        get {
            return maxLife;
        }
    }

    public bool isInvincible;
    private int life;


    public int Life {
        get {
            return life;
        }
        set {

            var previousLife = life;

            if (life < 0) {
                life = 0;
            } else if (life > maxLife) {
                life = maxLife;
            } else {
                life = value;
            }

            if (life != previousLife) {
                Listener?.OnLifeChange(life, previousLife);
            }
        }
    }

    public bool IsDead() {
        return life <= 0;
    }

    public void TryIncrementLife(int value = 1) {

        Life += value;
    }

    public void TryDecrementLife(int value = 1) {

        if (isInvincible) {
            //can't lose life
            return;
        }

        var previousLife = life;

        Life -= value;

        if (previousLife != life) {
            Listener?.OnLifeChange(life, previousLife);
        }
    }

}


public interface ILifeBehaviorListener {

    void OnLifeChange(int life, int previousLife);

}
