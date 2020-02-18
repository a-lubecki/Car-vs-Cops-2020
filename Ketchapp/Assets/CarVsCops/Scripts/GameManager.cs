using System;
using UnityEngine;


public class GameManager : MonoBehaviour {


    [SerializeField] private GameObject goMainCar;
    [SerializeField] private CarControlsManager carControlsManager;
    [SerializeField] private ItemGeneratorBehavior itemGeneratorBehavior;

    private VehicleBehavior mainCarVehicleBehavior {
        get {
            return goMainCar.GetComponent<VehicleBehavior>();
        }
    }

    private bool isPlaying;
    private bool isGameOver;


    void Start() {

        ShowOnboarding();
    }

    void Update() {

        if (Input.GetMouseButtonDown(0)) {

            if (!isPlaying) {
                StartPlaying();
            } else if (isGameOver) {
                ShowOnboarding();
            }
        }
    }

    public void ShowOnboarding() {

        isGameOver = false;

        mainCarVehicleBehavior.Show();

        carControlsManager.SetControlsEnabled(false);
    }

    public void StartPlaying() {

        if (isPlaying) {
            throw new ArgumentException();
        }

        isPlaying = true;

        carControlsManager.SetControlsEnabled(true);

        itemGeneratorBehavior.GeneratePoliceCars(4, goMainCar);
    }

    public void StopPlaying() {

        if (!isPlaying) {
            throw new ArgumentException();
        }

        isPlaying = false;
        isGameOver = true;

        carControlsManager.SetControlsEnabled(false);
    }

}
