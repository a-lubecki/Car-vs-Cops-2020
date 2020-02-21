using UnityEngine;


public class GameManager : MonoBehaviour, IItemDestructorBehaviorListener, IScoreTimerManagerListener {


    [SerializeField] private GameObject goMainCar = null;
    [SerializeField] private ItemGeneratorBehavior itemGeneratorBehavior = null;
    [SerializeField] private ScoreManager scoreManager = null;
    [SerializeField] private TimerBehavior scoreTimerBehavior = null;
    [SerializeField] private ComboManager boostManager = null;
    [SerializeField] private UIManager uiManager = null;

    private CarControlsManager carControlsManager;
    private MainCarBehavior mainCarBehavior;
    private LifeBehavior mainCarLifeBehavior;

    private bool isPlaying;
    private bool isGameOver;


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

        uiManager.ShowUIOnboarding(true);

        itemGeneratorBehavior.DespawnAll();

        //deactivate and reactivate to fully init the main car
        //the game object couldn't be deactivated during the car explosion because
        //some elements must be kept in the scene : camera, chasing target, etc
        mainCarBehavior.transform.rotation = Quaternion.identity;
        mainCarBehavior.transform.position = Vector3.zero;
        goMainCar.SetActive(true);

        SetScore(0);
    }

    public void StartPlaying() {

        if (isPlaying) {
            return;
        }

        isPlaying = true;
        isGameOver = false;

        carControlsManager.SetControlsEnabled(true);
        mainCarBehavior.InitLife();

        uiManager.ShowUIHUD(true);

        SpawnNewEnemies(4);
        SpawnNewObstacles(50);
        SpawnNewHeart(1);

        scoreTimerBehavior.StartTimer();
    }

    public void StopPlaying() {

        if (!isPlaying) {
            return;
        }

        isPlaying = false;
        isGameOver = true;

        carControlsManager.SetControlsEnabled(false);

        uiManager.ShowUIGameOver(true);

        scoreTimerBehavior.StopTimer();
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
            OnEnemyDestroyed(enemyBehavior, enemyBehavior.HasExploded);
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

    private void OnEnemyDestroyed(EnemyBehavior enemyBehavior, bool hasExploded) {

        if (!isPlaying && !isGameOver) {
            return;
        }

        //handle score only when playing and no gameover
        if (isPlaying && hasExploded) {

            //increment multiplier before adding score
            if (boostManager.IsComboEnabled()) {
                boostManager.ComboMultiplier++;
            }

            //calculate new score including multiplier
            var newScore = 6;
            if (boostManager.IsComboEnabled()) {
                newScore = 4 * boostManager.ComboMultiplier;
            }

            AddValueToScore(newScore, true);
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
            if (!boostManager.IsComboEnabled()) {
                boostManager.ComboMultiplier = 1;
            }
        } else {
            boostManager.ComboMultiplier = 0;
        }
    }

    public void OnTimerTick() {

        if (!isPlaying) {
            return;
        }

        AddValueToScore(1, false);
    }

    private void SetScore(int score) {

        scoreManager.Score = score;

        uiManager.UpdateScore(scoreManager.Score, 0, true);
    }

    private void AddValueToScore(int addedValue, bool displayAddedValue) {

        scoreManager.Score += addedValue;

        uiManager.UpdateScore(
            scoreManager.Score,
            displayAddedValue ? addedValue : 0,
            true
        );
    }

}
