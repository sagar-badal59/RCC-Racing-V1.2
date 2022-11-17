using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static bool CompleteGameStartCountdownTimer;
    public static int NoOfPayers = 7;
    public static HashSet<string> winnerplayerPostions = new HashSet<string>();
    public static int MINIMUM_PLAYER = 2;
}
