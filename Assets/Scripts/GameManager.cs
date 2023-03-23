using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;

    public void CheckWinState(){
        int alivecount = 0;

        foreach (GameObject player in players) {
            if(player.activeSelf) {
                alivecount++;
            }
        }

        if(alivecount <= 1){
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
