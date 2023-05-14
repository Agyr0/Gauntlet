using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public PlayerManager playerManager;
    [HideInInspector]
    public InventoryManager inventoryManager;
    public Transform screenCenter;
    public ScreenBorder screenBorder;
    private int _level = 0;

    public int Level { get { return _level; } }

    private void OnEnable()
    {
        _level = 0;
        EventBus.Subscribe(EventType.NEXT_ROUND, IncreaseRound);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.NEXT_ROUND, IncreaseRound);
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

    public void UsePotion(bool isShot)
    {
        Debug.Log("Used potion and wasShot: " + isShot);
        RaycastHit[] hits = Physics.BoxCastAll(Camera.main.transform.position, new Vector3(screenBorder.size.x, 20, screenBorder.size.y), Camera.main.transform.forward, Quaternion.identity, 30f, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < hits.Length; i++)
        {
            //Damage all enemies on screen
            //If potion was used do damage based on class magic value
            //if (!isShot)
            //    hits[i].transform.GetComponent<Enemy>().TakeDamage(classData.Magic);
            ////If potion was shot do less damage 
            //else if (isShot)
            //    hits[i].transform.GetComponent<Enemy>().TakeDamage(classData.Magic / 2);

        }
    }
}


//Functions for if transform is outside of screen and
//Clamping a Vector3 to stay inside the screen
public class ScreenBorder
{
    private static Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    //Actual width and height of the screen in world units
    public Vector2 size = new Vector2(screenBounds.x, Camera.main.orthographicSize);
    
    //Returns true if self + object radius is outside of screen border
    public bool IsOutside(Transform self, float height, float width)
    {
        if ((((self.position.x < (size.x / 1.4f) * -1 + width + Camera.main.transform.position.x) || (self.position.x > Camera.main.transform.position.x + 1.5f) ||
                (self.position.z < (size.y * -1) + height + Camera.main.transform.position.z) || (self.position.z > (size.y) - height + Camera.main.transform.position.z))))
            return true;

        return false;
    }

    //Returns a Vector3 that has X and Z clamped to the inside of the screen
    public Vector3 ClampToInside(Transform self, float height, float width)
    {
        Vector3 returnVec = self.position;

        returnVec.x = Mathf.Clamp(returnVec.x, (size.x / 1.4f) * -1 + width + Camera.main.transform.position.x,Camera.main.transform.position.x + 1.5f);
        returnVec.z = Mathf.Clamp(returnVec.z, size.y * -1 + height + Camera.main.transform.position.z, size.y - height + Camera.main.transform.position.z);

       

        return returnVec;
    }

}
