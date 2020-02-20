

public class MainCarTurnBehavior : BaseTurnBehavior, ICarControlsManagerListener {


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
