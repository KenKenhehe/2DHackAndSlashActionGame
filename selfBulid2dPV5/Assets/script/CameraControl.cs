using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public GameObject player;
    public float cameraSpeed = 5;
    public float offsetY = 0.2f;
    public float offsetX = 0.6f;
    public float maxX;
    public float minX;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        AdjustCameraPosition();
        OnlyMoveBetween(minX, maxX);
	}

    void AdjustCameraPosition()
    {
        if (player != null)
        {
            transform.position = new Vector3(
               Mathf.Lerp(transform.position.x + offsetX, player.transform.position.x + offsetX, Time.deltaTime * cameraSpeed),
               Mathf.Lerp(transform.position.y + offsetY, player.transform.position.y + offsetY, Time.deltaTime * cameraSpeed),
               player.transform.position.z
                );
        }
        else
        {
            transform.position = transform.position;
        }
    }

    void OnlyMoveBetween(float minX, float maxX)
    {
        if (transform.position.x >= maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
    }
}
