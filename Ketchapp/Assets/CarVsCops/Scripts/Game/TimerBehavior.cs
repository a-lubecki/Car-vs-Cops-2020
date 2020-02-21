using System.Collections;
using UnityEngine;


public class TimerBehavior : MonoBehaviour {


    public GameObject goListener;
    private IScoreTimerManagerListener Listener {
        get {
            if (goListener == null) {
                return null;
            }
            return goListener?.GetComponent<IScoreTimerManagerListener>();
        }
    }

    private bool isRunning;


    void OnDisable() {

        isRunning = false;

        StopAllCoroutines();
    }

    public void StartTimer() {

        if (isRunning) {
            //already running
            return;
        }

        isRunning = true;

        StartCoroutine(Run());
    }

    private IEnumerator Run() {

        while (isRunning) {

            yield return new WaitForSeconds(1);

            Listener?.OnTimerTick();
        }
    }

    public void StopTimer() {

        if (!isRunning) {
            //not running yet
            return;
        }

        isRunning = false;
    }

}

public interface IScoreTimerManagerListener {

    void OnTimerTick();

}