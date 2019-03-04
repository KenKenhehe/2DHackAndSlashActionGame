using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEventHandler : MonoBehaviour {
    public GameObject enemyPref;
    public float yPosition;
    public float timeLimit = 10;
    public bool gameOver = false;

    EnemyShooterController[] enemies;
    GameStateSwitch gameSwitch;
	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnEnemyAtRandomX());
        gameSwitch = FindObjectOfType<GameStateSwitch>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            Application.Quit();
            print("quit");
        }
        ResetTime();
        CountDown();
        UpdateEnemyCount();
        FinishAndShowMessage();
	}

   void ResetTime()
    {
        if (Time.timeScale < 1)
        {
            Time.timeScale += 15 * Time.deltaTime;
        }
        else if (Time.timeScale > 1)
        {
            Time.timeScale = 1;
        }
    }

    void UpdateEnemyCount()
    {
        enemies = FindObjectsOfType<EnemyShooterController>();
    }

    void CountDown()
    {
        timeLimit -= Time.deltaTime;
    }

    IEnumerator SpawnEnemyAtRandomX()
    {
        while (timeLimit >= 0)
        {
            GameObject enemyObject = Instantiate(enemyPref, new Vector3(Random.Range(-10, 10), yPosition, -1), Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }

    void FinishAndShowMessage()
    {
        if(timeLimit <= 0 && enemies.Length <= 0)
        {
            gameOver = true;
            print("done");
            gameSwitch.ShowGameOverState(true);
        }
    }
}
