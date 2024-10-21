using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScores", menuName ="ScoreManager")]
public class ScriptableObjects : ScriptableObject
{
    public float currentScore;
    public float lastScore;
    public float maxScore;
}
