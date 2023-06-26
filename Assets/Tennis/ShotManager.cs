using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShotManager : MonoBehaviour
{
    public Shot topSpin;
    public Shot flat;
}


[System.Serializable]
public class Shot
{
    public float upForce;
    public float hitForce;
}


[Serializable]
public struct ScoreManagement
{
    public int score;
    public int round;
    public int totalWin;
    public int aces;
    public int breakPointsWon;
    public int receivingPointsWon;
    public int winners;
    public int unforcesErrors;
    public int totalPointsWon;
    public int fastestServe;
    public int advantageCount;
}


public enum WhoHit
{
    AI,
    Player
}