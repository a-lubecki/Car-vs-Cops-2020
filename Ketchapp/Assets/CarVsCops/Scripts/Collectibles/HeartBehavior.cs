using System;


public class HeartBehavior : CollectibleBehavior {


    private LifeBehavior lifeBehavior;


    public void Init(LifeBehavior lifeBehavior) {

        this.lifeBehavior = lifeBehavior ?? throw new ArgumentException();
    }

    protected override void OnCollected() {

        lifeBehavior.IncrementLife();
    }

}
