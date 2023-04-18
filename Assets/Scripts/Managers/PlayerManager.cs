using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Info")]
    private PlayerInputManager inputManager;
    public List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private GameObject camTarget;

    [Header("Variables")]
    [SerializeField] private float spawnRadius = 3f;


    private void OnEnable()
    {
        inputManager = GetComponent<PlayerInputManager>();
        inputManager.onPlayerJoined += PlayerJoined;
    }
    private void OnDisable()
    {
        inputManager.onPlayerJoined -= PlayerJoined;
    }


    private void Update()
    {
        if(players.Count != 0)
            camTarget.transform.position = SetAvgPosition();
        
    }

    public void PlayerJoined(PlayerInput player)
    {
        EventBus.Publish(EventType.PLAYER_JOINED);
        players.Add(player);

        Transform playerParent = player.transform.parent;
        //Debug.Log(playerParent.name);
        InventoryManager.Instance.LinkInventory(player.gameObject.GetComponent<PlayerController>());
        //playerParent.position = FindSpawnPos();

    }
    public void PlayerLeft(PlayerInput player)
    {
        EventBus.Publish(EventType.PLAYER_LEFT);
    }


    private Vector3 FindSpawnPos()
    {
        bool foundPos = false;
        Vector2 spawnPos = Vector2.zero;
        while (!foundPos)
        {
            spawnPos = Random.insideUnitCircle * spawnRadius;

            if (!Physics.CheckSphere(spawnPos, spawnRadius))
                foundPos = true;
        }
        return spawnPos;
    }

    private Vector3 SetAvgPosition()
    {
        float totalX = 0f;
        float totalZ = 0f;
        foreach (PlayerInput player in players) 
        {
            totalX += player.transform.position.x;
            totalZ += player.transform.position.z;
        }
        float avgX = totalX / players.Count;
        float avgZ = totalZ / players.Count;

        return new Vector3(avgX, 0, avgZ);

    }
}
