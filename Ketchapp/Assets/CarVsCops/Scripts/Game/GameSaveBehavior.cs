using UnityEngine;
using TigerForge;


public class GameSaveBehavior : MonoBehaviour {


    private static readonly string KEY_MAX_SCORE = "maxScore";


    [SerializeField] private ScoreManager scoreManager = null;


    private EasyFileSave saveFile;

    protected void Awake() {

        LoadGame();
    }

    private void InitSave() {

        if (saveFile == null) {
            saveFile = new EasyFileSave();
        }
    }

    public void LoadGame() {

        InitSave();

        if (!saveFile.Load()) {
            //no save file, fresh game launch
            return;
        }

        scoreManager.InitMaxScore(saveFile.GetInt(KEY_MAX_SCORE));
        //TODO init other game data

        saveFile.Dispose();
    }

    public void SaveGame() {

        InitSave();

        saveFile.Add(KEY_MAX_SCORE, scoreManager.MaxScore);
        //TODO save other game data

        saveFile.Save();
    }

}
