using System.Collections;
using System.Collections.Generic;
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

        
        //player.SwitchCurrentControlScheme("Controller", Keyboard.current, Mouse.current);
        //Transform playerParent = player.transform.parent;
        ////Debug.Log(playerParent.name);
        InventoryManager.Instance.LinkInventory(player.gameObject.GetComponent<PlayerController>());
        ////playerParent.position = FindSpawnPos();

    }
    public void HandlePlayerLeft(PlayerInput player)
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
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput player)
    {
        PlayerIndex = player.playerIndex;
        Input = player;
    }

    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
}