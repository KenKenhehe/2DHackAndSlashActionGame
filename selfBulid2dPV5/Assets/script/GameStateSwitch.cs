using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateSwitch : MonoBehaviour {
    SceneEventHandler sceneEventHandler;
    public Text gameOverText;
	// Use this for initialization
	void Start () {
        gameOverText.enabled = false;
        sceneEventHandler = FindObjectOfType<SceneEventHandler>();
	}
	
	// Update is called once per frame
	void Update () {
        OnGameOver();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            sceneEventHandler.gameOver = true;
            ShowGameOverState(false);
        }
    }

    void OnGameOver()
    {
        if(sceneEventHandler.gameOver == true && Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ShowGameOverState(bool win)
    {
        if(win == false)
        {
            gameOverText.enabled = true;
            gameOverText.text = "Game Over press R restart";
        }

        else if(win == true)
        {
            gameOverText.enabled = true;
            gameOverText.text = "We made it...that feels good... \n ' R ' to restart?";
        }
    }
}
