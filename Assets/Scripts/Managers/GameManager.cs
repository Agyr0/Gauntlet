using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public PlayerManager playerManager;
    [HideInInspector]
    public InventoryManager inventoryManager;
    public Transform screenCenter;
    public ScreenBorder screenBorder;
    private string potionEffect = "PotionEffect";
    private int _level = 0;
    public int Level { get { return _level; } }

    private void OnEnable()
    {
        _level = 1;
        EventBus.Subscribe(EventType.NEXT_ROUND, IncreaseRound);
        EventBus.Subscribe(EventType.PLAYER_LEFT, CheckGameOver);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.NEXT_ROUND, IncreaseRound);
        EventBus.Unsubscribe(EventType.PLAYER_LEFT, CheckGameOver);
    }

    private void Start()
    {
        screenBorder = new ScreenBorder();
        playerManager = PlayerManager.Instance;
        inventoryManager = InventoryManager.Instance;
    }

    /// <summary>
    /// Inceases level by 1
    /// </summary>
    public void IncreaseRound()
    {
        _level++;
        EventBus.Publish(EventType.LEVEL_CHANGED);
        Debug.Log("Increased level");
    }
    /// <summary>
    /// Increases level by <paramref name="num"/>
    /// </summary>
    /// <param name="num"></param>
    public void IncreaseRound(int num)
    {
        _level += num;
        EventBus.Publish(EventType.LEVEL_CHANGED);
    }
    /// <summary>
    /// For when the potion is being used
    /// </summary>
    /// <param name="player"></param>
    public void UsePotion(PlayerController player)
    {
        RaycastHit[] hits = Physics.BoxCastAll(Camera.main.transform.position, new Vector3(screenBorder.size.x, 20, screenBorder.size.y), Camera.main.transform.forward, Quaternion.identity, 30f, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < hits.Length; i++)
        {
            //Damage all enemies on screen
            //If potion was used do damage based on class magic value
            if (hits[i].transform.GetComponent<Enemy>() != null)
                hits[i].transform.GetComponent<Enemy>().TakeDamage((int)player.classData.Magic * 100);
        }
        GameObject potion = ObjectPooler.Instance.GetPooledObject(potionEffect);
        if (potion != null)
        {
            potion.transform.position = player.transform.position;
            potion.transform.rotation = Quaternion.identity;
            potion.SetActive(true);
        }
    }
    /// <summary>
    /// For when the potion is being shot
    /// </summary>
    /// <param name="player"></param>
    /// <param name="_potionObj"></param>
    public void UsePotion(PlayerController player, GameObject _potionObj)
    {
        RaycastHit[] hits = Physics.BoxCastAll(Camera.main.transform.position, new Vector3(screenBorder.size.x, 20, screenBorder.size.y), Camera.main.transform.forward, Quaternion.identity, 30f, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < hits.Length; i++)
        {
            //Damage all enemies on screen
            //If potion was shot do less damage 
            if (hits[i].transform.GetComponent<Enemy>() != null)
                hits[i].transform.GetComponent<Enemy>().TakeDamage(((int)player.classData.Magic / 2) * 100);

        }
        GameObject potion = ObjectPooler.Instance.GetPooledObject(potionEffect);
        if (potion != null)
        {
            potion.transform.position = _potionObj.transform.position;
            potion.transform.rotation = Quaternion.identity;
            potion.SetActive(true);
        }
    }

    /// <summary>
    /// Destroys all players in <see cref="PlayerManager.playerConfigs"/> and sets class data to default then clears 
    /// <see cref="PlayerManager.playerConfigs"/> and <see cref="PlayerManager.usedClasses"/>.
    /// <br> Finally, stops naration, resets level to "1" and publishes <see cref="EventType.PLAYER_LEFT"/> and <see cref="EventType.DISABLE_JOINING"/>. </br>
    /// </summary>
    public void ResetGame()
    {
        //Debug.Log("Resetting game");
        //Resets all players stats to base and then destorys players
        for (int i = 0; i < playerManager.playerConfigs.Count; i++)
        {
            playerManager.playerConfigs[i].PlayerParent.GetComponent<PlayerController>().classData.ResetValuesToDefault();
            Destroy(playerManager.playerConfigs[i].PlayerParent.gameObject);
        }
        //Clears player lists
        playerManager.playerConfigs.Clear();
        playerManager.usedClasses.Clear();
        //Stop naration
        NaratorManager.Instance.audioSource.Stop();
        //Set level back to 1
        _level = 1;
        //Published player left and disables joining
        EventBus.Publish(EventType.PLAYER_LEFT);
        EventBus.Publish(EventType.DISABLE_JOINING);
    }

    private IEnumerator GameOverEvent()
    {
        EventBus.Publish(EventType.DISABLE_JOINING);
        UIManager.Instance.SetTimeScale(false);
        NaratorManager.Instance.canPlayRandomClip = false;
        NaratorManager.Instance.audioSource.Stop();
        yield return new WaitForSecondsRealtime(3f);

        UIManager.Instance.state = CanvasState.GameOver;
        EventBus.Publish(EventType.GAME_OVER);
        EventBus.Publish(EventType.UI_CHANGED);

        yield return null;
    }

    private void CheckGameOver()
    {
        if (playerManager.playerConfigs.Count > 0)
            return;
        if (UIManager.Instance.state == CanvasState.Level || UIManager.Instance.state == CanvasState.Pause)
            StartCoroutine(GameOverEvent());
    }
}


//Functions for if transform is outside of screen and
//Clamping a Vector3 to stay inside the screen
public class ScreenBorder
{
    private static Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    //Actual width and height of the screen in world units
    public Vector2 size = new Vector2(screenBounds.x, Camera.main.orthographicSize);

    /// <summary>
    /// Returns true if object is outside of screen border.
    /// <br> <paramref name="self"/> is the object you are checking. </br>
    /// <br> With a buffer of <paramref name="height"/> and <paramref name="width"/>.</br>
    /// </summary>
    /// <param name="self"></param>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    //Returns true if self + object radius is outside of screen border
    public bool IsOutside(Transform self, float height, float width)
    {
        if ((((self.position.x < (size.x / 1.4f) * -1 + width + Camera.main.transform.position.x) || (self.position.x > Camera.main.transform.position.x + 1.5f) ||
                (self.position.z < (size.y * -1) + height + Camera.main.transform.position.z) || (self.position.z > (size.y) - height + Camera.main.transform.position.z))))
            return true;

        return false;
    }

    /// <summary>
    /// Returns a Vector3 to clamp an object inside screen.
    /// <br> Clamps <paramref name="self"/> to the viewable screen size. </br>
    /// <br> With a buffer of <paramref name="height"/> and <paramref name="width"/>. </br>
    /// </summary>
    /// <param name="self"></param>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    //Returns a Vector3 that has X and Z clamped to the inside of the screen
    public Vector3 ClampToInside(Transform self, float height, float width)
    {
        Vector3 returnVec = self.position;

        returnVec.x = Mathf.Clamp(returnVec.x, (size.x / 1.4f) * -1 + width + Camera.main.transform.position.x,Camera.main.transform.position.x + 1.5f);
        returnVec.z = Mathf.Clamp(returnVec.z, size.y * -1 + height + Camera.main.transform.position.z, size.y - height + Camera.main.transform.position.z);

       

        return returnVec;
    }

}
