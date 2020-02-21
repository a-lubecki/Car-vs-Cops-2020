using UnityEngine;

public class LifeBehavior : MonoBehaviour {


    [SerializeField] private int maxLife = 3;
    private int life;
    public bool isInvincible;


    public int Life {
        get {
            return life;
        }
        set {
            if (life < 0) {
                life = 0;
            } else if (life > maxLife) {
                life = maxLife;
            } else {
                life = value;
            }
        }
    }


    void OnEnable() {
        life = maxLife;
    }

    public bool IsDead() {
        return life <= 0;
    }

    public void IncrementLife(int value = 1) {
        Life += value;
    }

    public void DecrementLife(int value = 1) {

        if (isInvincible) {
            //can't lose life
            return;
        }

        Life -= value;
    }

}
