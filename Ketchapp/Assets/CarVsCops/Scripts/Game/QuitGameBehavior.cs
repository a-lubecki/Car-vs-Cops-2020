using System.Collections;
using UnityEngine;


public class QuitGameBehavior : MonoBehaviour {


    [SerializeField] private GameObject goListener = null;
    private IQuitGameBehaviorListener Listener {
        get {
            if (goListener == null) {
                return null;
            }
            return goListener?.GetComponent<IQuitGameBehaviorListener>();
        }
    }

    private bool isAboutToQuit;


    protected void Update() {

        //manage physical back button on android
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (isAboutToQuit) {
                Application.Quit();
            } else {
                PrepareToQuit();
            }
        }
    }

    private void PrepareToQuit() {

        if (isAboutToQuit) {
            //already quitting
            return;
        }

        isAboutToQuit = true;

        Listener?.OnQuitModeEnabled();

        StartCoroutine(StopQuittingAfterDelay());
    }

    private IEnumerator StopQuittingAfterDelay() {

        yield return new WaitForSeconds(3);

        isAboutToQuit = false;

        Listener?.OnQuitModeDisabled();
    }

}


public interface IQuitGameBehaviorListener {

    void OnQuitModeEnabled();
    void OnQuitModeDisabled();

}