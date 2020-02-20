using UnityEngine;


public class GameManager : MonoBehaviour, IItemDestructorBehaviorListener {


    [SerializeField] private GameObject goMainCar;
    [SerializeField] private ItemGeneratorBehavior itemGeneratorBehavior;

    private CarControlsManager carControlsManager;
    private MainCarBehavior mainCarBehavior;
    private LifeBehavior lifeBehavior;

    private bool isPlaying;
    private bool isGameOver;

    private int score;///TODO ScoreManager


    void Awake() {

        carControlsManager = goMainCar.GetComponent<CarControlsManager>();
        mainCarBehavior = goMainCar.GetComponent<MainCarBehavior>();
        lifeBehavior = goMainCar.GetComponent<LifeBehavior>();
    }

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
        mainCarBehavior.transform.rotation = Quaternion.identity;
        mainCarBehavior.transform.position = Vector3.zero;
        goMainCar.SetActive(true);

        mainCarBehavior.Init();

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
        SpawnNewObstacles(50);
        SpawnNewHeart(3);
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

    public void SpawnNewHeart(int count) {

        itemGeneratorBehavior.SpawnNewHearts(count, this, lifeBehavior);
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

        var heartBehavior = goItem.GetComponent<HeartBehavior>();
        if (heartBehavior != null) {
            OnHeartDestroyed(heartBehavior);
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

    private void OnHeartDestroyed(HeartBehavior heartBehavior) {

        if (!isPlaying) {
            return;
        }

        SpawnNewHeart(1);
    }

}
