using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour, IItemDestructorBehaviorListener, ILifeBehaviorListener,
    IBoostGaugeBehaviorListener, IComboBehaviorListener, IScoreTimerManagerListener {


    [SerializeField] private GameObject goMainCar = null;
    [SerializeField] private ItemGeneratorBehavior itemGeneratorBehavior = null;
    [SerializeField] private ScoreManager scoreManager = null;
    [SerializeField] private TimerBehavior scoreTimerBehavior = null;
    [SerializeField] private ComboBehavior comboBehavior = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private AudioBehavior audioBehavior = null;
    [SerializeField] private RandomSoundsBehavior randomSoundsBehavior = null;

    private CarControlsManager carControlsManager;
    private MainCarBehavior mainCarBehavior;
    private LifeBehavior mainCarLifeBehavior;

    private bool isPlaying;
    private bool isGameOver;


    protected void Awake() {

        carControlsManager = goMainCar.GetComponent<CarControlsManager>();
        mainCarBehavior = goMainCar.GetComponent<MainCarBehavior>();
        mainCarLifeBehavior = goMainCar.GetComponent<LifeBehavior>();
    }

    protected void Start() {

        ShowOnboarding(true);
    }

    protected void Update() {

        if (Input.GetMouseButtonDown(0)) {

            if (isGameOver) {
                ShowOnboarding(false);
            } else if (!isPlaying) {
                StartPlaying();
            }
        }
    }

    public void ShowOnboarding(bool isFreshStart) {

        StopPlaying();

        isPlaying = false;
        isGameOver = false;

        carControlsManager.SetControlsEnabled(false);

        itemGeneratorBehavior.DespawnAll();

        //reinit the main car
        mainCarBehavior.transform.rotation = Quaternion.identity;
        mainCarBehavior.transform.position = Vector3.zero;
        goMainCar.SetActive(true);

        SetScore(0);

        uiManager.ShowUIOnboarding(true);

        audioBehavior.PlayMusicMenu();
        if (!isFreshStart) {
            audioBehavior.PlaySound("Restart");
        }
    }

    public void StartPlaying() {

        if (isPlaying) {
            return;
        }

        isPlaying = true;
        isGameOver = false;

        carControlsManager.SetControlsEnabled(true);
        mainCarBehavior.InitLife();

        comboBehavior.SetComboEnabled(false);

        SpawnNewEnemies(4);
        SpawnNewObstacles(50);
        SpawnNewHeart(1);

        scoreTimerBehavior.StartTimer();

        uiManager.ShowUIHUD(true);

        audioBehavior.PlayMusicInGame();
        audioBehavior.PlaySound("Play");

        randomSoundsBehavior.StartRandomSoundsPlaying();
    }

    public void StopPlaying() {

        if (!isPlaying) {
            return;
        }

        isPlaying = false;
        isGameOver = true;

        carControlsManager.SetControlsEnabled(false);

        scoreTimerBehavior.StopTimer();

        uiManager.ShowUIGameOver(true);

        audioBehavior.PlayMusicMenu();
        StartCoroutine(PlayGameOverSoundAfterDelay());

        randomSoundsBehavior.StopRandomSoundsPlaying();
    }

    private IEnumerator PlayGameOverSoundAfterDelay() {

        yield return new WaitForSeconds(1);

        audioBehavior.PlaySound("GameOver");
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

    public void OnItemDestroyed(GameObject goItem, bool hasExploded) {

        var enemyBehavior = goItem.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null) {
            OnEnemyDestroyed(enemyBehavior, hasExploded);
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
            if (comboBehavior.IsComboEnabled) {
                comboBehavior.SetComboMultiplier(comboBehavior.ComboMultiplier + 1);
            }

            //calculate new score including multiplier
            var newScore = 6;
            if (comboBehavior.IsComboEnabled) {
                newScore = 4 * comboBehavior.ComboMultiplier;
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

    public void OnLifeChange(int life, int previousLife) {

        uiManager.UpdateLife(life, previousLife, true);

        if (previousLife > 0 && life > previousLife) {
            audioBehavior.PlaySound("LifeGain");
        } else if (life < previousLife) {
            audioBehavior.PlaySound("LifeLose");
        }
    }

    public void OnBoostGaugeValueUpdate(float percentage) {

        if (!isPlaying) {
            return;
        }

        if (percentage >= 1) {
            comboBehavior.SetComboEnabled(true);
        }

        uiManager.UpdateBoost(!comboBehavior.IsComboEnabled, percentage);
    }

    public void OnComboEnabled() {

        uiManager.ShowUICombo(true);
        uiManager.UpdateComboMultiplier(comboBehavior.ComboMultiplier, false);

        audioBehavior.PlaySound("ComboStart");
    }

    public void OnComboDisabled() {

        uiManager.HideUICombo(true);

        audioBehavior.PlaySound("ComboStop");
    }

    public void OnComboMultiplierChange() {

        uiManager.UpdateComboMultiplier(comboBehavior.ComboMultiplier, true);
    }

    public void OnTimerTick() {

        if (!isPlaying) {
            return;
        }

        //add 1 point to score every second
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
