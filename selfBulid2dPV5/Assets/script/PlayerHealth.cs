using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    int healthPoints;
    public int totalHealth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetHealthPoints()
    {
        return healthPoints;
    }

    public void SetHealthPoint(int healthPoints)
    {
        this.healthPoints = healthPoints;
    }
}
