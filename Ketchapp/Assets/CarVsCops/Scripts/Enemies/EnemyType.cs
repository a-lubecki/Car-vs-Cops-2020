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

    private static EnemyThreshold[] thresholds = new EnemyThreshold[] {

        new EnemyThreshold {///TODO TEST
            score = 0,
            enemyTypes = new EnemyType[] {
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
            },
        },
        /*
        new EnemyThreshold {
            score = 0,
            enemyTypes = new EnemyType[] {
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
                EnemyType.POLICE_CAR,
            },
        },*/

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
            //200 => 201
            if (previousScore < threshold.score && threshold.score <= score) {
                //found
                return threshold.enemyTypes;
            }
        }

        //not found
        return null;
    }

}


public struct EnemyThreshold {

    public int score;
    public EnemyType[] enemyTypes;

}