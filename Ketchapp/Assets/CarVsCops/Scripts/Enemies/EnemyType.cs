using System;


public enum EnemyType {

    POLICE_CAR,
    POLICE_VAN,
    HELICOPTER,
    TANK,
    MISSILE,

}

public static class EnemyTypeFunctions {

    public static EnemyType[] Values() {
        return Enum.GetValues(typeof(EnemyType)) as EnemyType[];
    }

    ///an array containing all the enemies to generate by score threshold
    private static EnemyThreshold[] thresholds = new EnemyThreshold[] {

        new EnemyThreshold {
            score = 0,
            enemyTypes = new EnemyType[] {
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
            },
        },

        new EnemyThreshold {
            score = 200,
            enemyTypes = new EnemyType[] {
                EnemyType.POLICE_VAN,
            },
        },

        new EnemyThreshold {
            score = 500,
            enemyTypes = new EnemyType[] {
                EnemyType.POLICE_VAN,
            },
        },

        new EnemyThreshold {
            score = 800,
            enemyTypes = new EnemyType[] {
                EnemyType.HELICOPTER,
            },
        },

        new EnemyThreshold {
            score = 1200,
            enemyTypes = new EnemyType[] {
                EnemyType.TANK,
                EnemyType.MISSILE,
            },
        },

    };

    public static EnemyType[] GetNewEnemiesToGenerate(int score, int previousScore) {

        foreach (var threshold in thresholds) {

            if (previousScore < threshold.score && threshold.score <= score) {
                //the score has reached the threshold
                return threshold.enemyTypes;
            }
        }

        //not found
        return null;
    }

}


///a struct containg the enemy types to generate at a given score threshold
public struct EnemyThreshold {

    public int score;
    public EnemyType[] enemyTypes;

}