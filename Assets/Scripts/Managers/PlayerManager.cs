using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Info")]
    private PlayerInputManager playerManager;
    public List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();
    [SerializeField] private GameObject camTarget;

    [Header("Variables")]
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField]
    private int maxPlayers = 4;



    private void OnEnable()
    {
        playerManager = GetComponent<PlayerInputManager>();
        playerManager.EnableJoining();
    }



    private void Update()
    {
        if(playerConfigs.Count != 0)
            camTarget.transform.position = SetAvgPosition();
        
    }

    public void HandlePlayerJoin(PlayerInput player)
    {
        EventBus.Publish(EventType.PLAYER_JOINED);
        Debug.Log("Player Joined" + player.playerIndex);
        if(!playerConfigs.Any(p => p.PlayerIndex == player.playerIndex))
        {
            player.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(player));
        }

        InventoryManager.Instance.LinkInventory(player.gameObject.GetComponent<PlayerController>());
        playerConfigs[player.playerIndex].PlayerParent.GetComponent<CharacterController>().enabled = false;
        playerConfigs[player.playerIndex].PlayerParent.position = FindSpawnPos();
        playerConfigs[player.playerIndex].PlayerParent.GetComponent<CharacterController>().enabled = transform;

    }
    public void HandlePlayerLeft(PlayerInput player)
    {
        EventBus.Publish(EventType.PLAYER_LEFT);
    }


    private Vector3 FindSpawnPos()
    {
        Vector2 newPos = Random.insideUnitCircle * spawnRadius;
        int maxTrys = 10;
        int curTry = 0;
        while (curTry < maxTrys)
        {
            curTry++;
            Debug.Log("Finding available spawn location");
            newPos = Random.insideUnitCircle * spawnRadius;

            if (!Physics.CheckSphere(newPos, spawnRadius, ~LayerMask.GetMask("Floor")))
            {
                Debug.Log("Spawn location found at " + newPos);
                break;
            }
        }
        if(curTry >= maxTrys)
            Debug.LogError("Couldn't find a good spawn location \nGoing to default spawn location");
        
        Vector3 spawnPos = new Vector3(newPos.x, 0, newPos.y);
        return spawnPos;
    }

    private Vector3 SetAvgPosition()
    {
        float totalX = 0f;
        float totalZ = 0f;
        foreach (PlayerConfiguration player in playerConfigs) 
        {
            totalX += player.Input.transform.position.x;
            totalZ += player.Input.transform.position.z;
        }
        float avgX = totalX / playerConfigs.Count;
        float avgZ = totalZ / playerConfigs.Count;

        return new Vector3(avgX, 0, avgZ);

    }
}
[System.Serializable]
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput player)
    {
        PlayerParent = player.transform;
        PlayerIndex = player.playerIndex;
        Input = player;
    }
    [SerializeField]
    public Transform PlayerParent { get; set; }
    [SerializeField]
    public PlayerInput Input { get; set; }
    [SerializeField]
    public int PlayerIndex { get; set; }
    [SerializeField]
    public bool IsReady { get; set; }
}