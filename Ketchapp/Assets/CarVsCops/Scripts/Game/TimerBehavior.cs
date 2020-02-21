using System.Collections;
using UnityEngine;


public class TimerBehavior : MonoBehaviour {


    [SerializeField] private GameObject goListener;

    private IScoreTimerManagerListener listener {
        get {
            return goListener.GetComponent<IScoreTimerManagerListener>();
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

        yield return new WaitForSeconds(1);

        listener.OnTimerTick();
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