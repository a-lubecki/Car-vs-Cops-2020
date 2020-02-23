using System.Collections;
using UnityEngine;
using DG.Tweening;


public class GameManager : MonoBehaviour, IItemDestructorBehaviorListener, ILifeBehaviorListener,
    IBoostGaugeBehaviorListener, IComboBehaviorListener, IScoreTimerManagerListener{


    [SerializeField] private GameObject goMainCar = null;
    [SerializeField] private ItemGeneratorBehavior itemGeneratorBehavior = null;
    [SerializeField] private ScoreManager scoreManager = null;
    [SerializeField] private TimerBehavior scoreTimerBehavior = null;
    [SerializeField] private ComboBehavior comboBehavior = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private AudioBehavior audioBehavior = null;
    [SerializeField] private RandomSoundsBehavior randomSoundsBehavior = null;
    [SerializeField] private GameSaveBehavior gameSaveBehavior = null;

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

        //init the ground lines color
        Camera.main.backgroundColor = Color.black;
    }

    public void StartPlaying() {

        if (isPlaying) {
            return;
        }

        isPlaying = true;
        isGameOver = false;

        carControlsManager.SetControlsEnabled(true);
        mainCarBehavior.InitVehicle();

        comboBehavior.SetComboEnabled(false);

        itemGeneratorBehavior.SpawnObstacles(50, this);
        itemGeneratorBehavior.SpawnNewHearts(1, this, mainCarLifeBehavior);
        SpawnNewEnemiesDependingOnScore(0, -1);

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

        gameSaveBehavior.SaveGame();
    }

    private IEnumerator PlayGameOverSoundAfterDelay() {

        yield return new WaitForSeconds(1);

        audioBehavior.PlaySound("GameOver");
    }

    private void SpawnNewEnemiesDependingOnScore(int score, int previousScore) {

        var enemyTypes = EnemyTypeFunctions.GetNewEnemiesToGenerate(score, previousScore);
        if (enemyTypes == null) {
            //no new enemies to generate
            return;
        }

        foreach (var enemyType in enemyTypes) {
            itemGeneratorBehavior.SpawnEnemy(enemyType, 1, this, goMainCar);
        }
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

        //respawn it
        itemGeneratorBehavior.SpawnEnemy(enemyBehavior.EnemyType, 1, this, goMainCar);
    }

    private void OnObstacleDestroyed(ObstacleBehavior obstacleBehavior) {

        if (!isPlaying && !isGameOver) {
            return;
        }

        //respawn it
        itemGeneratorBehavior.SpawnObstacles(1, this);
    }

    private void OnHeartDestroyed(HeartBehavior heartBehavior) {

        if (!isPlaying && !isGameOver) {
            return;
        }

        //respawn it
        itemGeneratorBehavior.SpawnNewHearts(1, this, mainCarLifeBehavior);
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

        //shake camera
        var camera = Camera.main;
        var initialCamPos = camera.transform.localPosition;
        var tween = camera.DOShakePosition(0.5f, 2, 20);
        tween.OnKill(() => camera.transform.localPosition = initialCamPos);

        //change the ground lines color
        camera.DOColor(Color.white, 0.5f);
    }

    public void OnComboDisabled() {

        uiManager.HideUICombo(true);

        audioBehavior.PlaySound("ComboStop");

        //change the ground lines color
        Camera.main.DOColor(Color.black, 0.5f);
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

        var previousScore = scoreManager.Score;

        scoreManager.Score += addedValue;

        uiManager.UpdateScore(
            scoreManager.Score,
            displayAddedValue ? addedValue : 0,
            true
        );

        SpawnNewEnemiesDependingOnScore(scoreManager.Score, previousScore);
    }

}
