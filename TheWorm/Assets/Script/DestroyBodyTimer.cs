using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBodyTimer : MonoBehaviour
{
    float timer = 0;
    float maxTimer;
    GameObject player;
    PlayerMovement playerMovement;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();    
    }
    void Update()
    { 
        maxTimer = playerMovement.maxMoveTimer;

        timer += Time.deltaTime;
        if(timer >= maxTimer)
        {
            Destroy(gameObject);
            timer -= playerMovement.maxMoveTimer;
        }
    }
}
