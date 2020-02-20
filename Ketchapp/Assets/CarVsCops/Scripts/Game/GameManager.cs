using UnityEngine;


public class GameManager : MonoBehaviour, IItemDestructorBehaviorListener {


    [SerializeField] private GameObject goMainCar;
    [SerializeField] private ItemGeneratorBehavior itemGeneratorBehavior;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private BoostManager boostManager;

    private CarControlsManager carControlsManager;
    private MainCarBehavior mainCarBehavior;
    private LifeBehavior mainCarLifeBehavior;

    private bool isPlaying;
    private bool isGameOver;

    public bool IsBoostEnabled {
        get {
            return boostManager.IsBoostEnabled();
        }
    }


    void Awake() {

        carControlsManager = goMainCar.GetComponent<CarControlsManager>();
        mainCarBehavior = goMainCar.GetComponent<MainCarBehavior>();
        mainCarLifeBehavior = goMainCar.GetComponent<LifeBehavior>();
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

        scoreManager.Score = 0;
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
        SpawnNewHeart(1);
    }

    public void StopPlaying() {

        if (!isPlaying) {
            return;
        }

        isPlaying = false;
        isGameOver = true;

        carControlsManager.SetControlsEnabled(false);
    }

    private void SpawnNewEnemies(int count) {

        itemGeneratorBehavior.SpawnPoliceCars(count, this, goMainCar);
    }

    private void SpawnNewObstacles(int count) {

        itemGeneratorBehavior.SpawnObstacles(count, this);
    }

    private void SpawnNewHeart(int count) {

        itemGeneratorBehavior.SpawnNewHearts(count, this, mainCarLifeBehavior);
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

        if (!isPlaying && !isGameOver) {
            return;
        }

        //handle score only when playing and no gameover
        if (isPlaying) {

            //increment multiplier before adding score
            if (boostManager.IsBoostEnabled()) {
                boostManager.BoostMultiplier++;
            }

            //calculate new score including multiplier
            var newScore = 6;
            if (boostManager.IsBoostEnabled()) {
                newScore = 4 * boostManager.BoostMultiplier;
            }
            scoreManager.Score += newScore;
        }

        SpawnNewEnemies(1);
    }

    private void OnObstacleDestroyed(ObstacleBehavior obstacleBehavior) {

        if (!isPlaying && !isGameOver) {
            return;
        }

        SpawnNewObstacles(1);
    }

    private void OnHeartDestroyed(HeartBehavior heartBehavior) {

        if (!isPlaying && !isGameOver) {
            return;
        }

        SpawnNewHeart(1);
    }

    public void OnBoostGaugeValueChange(float amount) {

        if (!isPlaying) {
            return;
        }

        if (amount >= 1) {
            SetBoostEnabled(true);
        }
    }

    private void SetBoostEnabled(bool enabled) {

        if (enabled) {
            if (!boostManager.IsBoostEnabled()) {
                boostManager.BoostMultiplier = 1;
            }
        } else {
            boostManager.BoostMultiplier = 0;
        }
    }

}
