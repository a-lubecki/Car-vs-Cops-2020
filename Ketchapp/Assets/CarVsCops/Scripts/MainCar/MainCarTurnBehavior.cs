

public class MainCarTurnBehavior : BaseTurnBehavior, ICarControlsManagerListener {


    protected void OnEnable() {

        MustRotateLeft = false;
        MustRotateRight = false;
    }

    public void onLeftPressed() {

        //udate bool then wait for next update
        MustRotateLeft = true;
        MustRotateRight = false;
    }

    public void onRightPressed() {

        //udate bool then wait for next update
        MustRotateRight = true;
        MustRotateLeft = false;
    }

}
