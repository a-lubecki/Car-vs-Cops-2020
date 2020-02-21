using UnityEngine;


public class CarControlsManager : MonoBehaviour {


    private bool controlsEnabled = true;

    public GameObject goListener;
    private ICarControlsManagerListener Listener {
        get {
            if (goListener == null) {
                return null;
            }
            return goListener?.GetComponent<ICarControlsManagerListener>();
        }
    }


    public void SetControlsEnabled(bool controlsEnabled) {
        this.controlsEnabled = controlsEnabled;
    }

    protected void Update() {

        if (!controlsEnabled) {
            return;
        }

        //handle long touch on the right or left side of the screen
        if (Input.GetMouseButton(0)) {

            if (Input.mousePosition.x < 0.5f * Screen.width) {
                Listener?.onLeftPressed();
            } else {
                Listener?.onRightPressed();
            }
        }

        //handle long press arrows for debugging
        #if UNITY_EDITOR

        if (Input.GetKey(KeyCode.LeftArrow)) {
            Listener?.onLeftPressed();
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            Listener?.onRightPressed();
        }

        #endif
    }

}


public interface ICarControlsManagerListener {

    void onLeftPressed();

    void onRightPressed();

}
