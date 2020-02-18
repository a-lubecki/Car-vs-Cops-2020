

public class MainCarTurnBehavior : BaseTurnBehavior, ICarControlsManagerListener {


    public void onLeftPressed() {

        //udate bool then wait for next update
        mustRotateLeft = true;
        mustRotateRight = false;
    }

    public void onRightPressed() {

        //udate bool then wait for next update
        mustRotateRight = true;
        mustRotateLeft = false;
    }

}
