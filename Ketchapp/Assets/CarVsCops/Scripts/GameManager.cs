using UnityEngine;


public class GameManager : MonoBehaviour, IItemDestructorBehaviorListener {


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

    private int score;///TODO ScoreManager


    void Start() {

        ShowOnboarding();
    }

    void Update() {

        if (Input.GetMouseButtonDown(0)) {

            if (isGameOver) {
                ShowOnboarding();
            } else if (!isPlaying) {
                StartPlaying();
            }
        }
    }

    public void ShowOnboarding() {

        StopPlaying();

        isPlaying = false;
        isGameOver = false;

        carControlsManager.SetControlsEnabled(false);

        itemGeneratorBehavior.DespawnAll();

        //deactivate and reactivate to fully init the main car
        //the game object couldn't be deactivated during the car explosion because
        //some elements must be kept in the scene : camera, chasing target, etc
        goMainCar.SetActive(false);
        mainCarVehicleBehavior.transform.rotation = Quaternion.identity;
        mainCarVehicleBehavior.transform.position = Vector3.zero;
        goMainCar.SetActive(true);

        mainCarVehicleBehavior.Show();

        score = 0;
    }

    public void StartPlaying() {

        if (isPlaying) {
            return;
        }

        isPlaying = true;
        isGameOver = false;

        carControlsManager.SetControlsEnabled(true);

        SpawnNewEnemies(4);
        SpawnNewObstacles(30);
    }

    public void StopPlaying() {

        if (!isPlaying) {
            return;
        }

        isPlaying = false;
        isGameOver = true;

        carControlsManager.SetControlsEnabled(false);
    }

    public void SpawnNewEnemies(int count) {

        itemGeneratorBehavior.SpawnPoliceCars(count, this, goMainCar);
    }

    public void SpawnNewObstacles(int count) {

        itemGeneratorBehavior.SpawnObstacles(count, this);
    }

    public void OnItemDestroyed(GameObject goItem) {

        var enemyBehavior = goItem.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null) {
            OnEnemyDestroyed(enemyBehavior);
        }

        var obstacleBehavior = goItem.GetComponent<ObstacleBehavior>();
        if (obstacleBehavior != null) {
            OnObstacleDestroyed(obstacleBehavior);
        }
    }

    private void OnEnemyDestroyed(EnemyBehavior enemyBehavior) {

        if (!isPlaying) {
            return;
        }

        score++;

        SpawnNewEnemies(1);
    }

    private void OnObstacleDestroyed(ObstacleBehavior obstacleBehavior) {

        if (!isPlaying) {
            return;
        }

        SpawnNewObstacles(1);
    }

}
