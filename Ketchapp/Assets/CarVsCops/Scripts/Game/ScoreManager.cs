using System;
using UnityEngine;


public class ScoreManager : MonoBehaviour {


    private int score;
    public int Score {
        get {
            return score;
        }
        set {
            if (value < 0) {
                throw new ArgumentException();
            }

            score = value;

            UpdateMaxScore(score);
        }
    }

    public int MaxScore { get; private set; }


    public void InitMaxScore(int maxScore) {
        MaxScore = maxScore;
    }

    private void UpdateMaxScore(int newScore) {

        if (newScore > MaxScore) {
            MaxScore = newScore;
        }
    }

}
