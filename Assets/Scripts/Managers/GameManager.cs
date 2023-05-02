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

    private void Start()
    {
        screenBorder = new ScreenBorder();
        EventBus.Publish(EventType.ENABLE_JOINING);
        playerManager = PlayerManager.Instance;
        inventoryManager = InventoryManager.Instance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(screenCenter.position.x - (screenBorder.size.x / 2), screenCenter.position.y, screenCenter.position.z), new Vector3(screenBorder.size.x, 0, screenBorder.size.y*2));
        
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
        if ((((self.position.x < (size.x * -1) + width + Camera.main.transform.position.x) || (self.position.x > (size.x) - width + Camera.main.transform.position.x) ||
                (self.position.z < (size.y * -1) + height + Camera.main.transform.position.z) || (self.position.z > (size.y) - height + Camera.main.transform.position.z))))
            return true;

        return false;
    }

    //Returns a Vector3 that has X and Z clamped to the inside of the screen
    public Vector3 ClampToInside(Transform self, float height, float width)
    {
        Vector3 returnVec = self.position;

        returnVec.x = Mathf.Clamp(returnVec.x, size.x * -1 + width + Camera.main.transform.position.x,Camera.main.transform.position.x + 1.5f);
        returnVec.z = Mathf.Clamp(returnVec.z, size.y * -1 + height + Camera.main.transform.position.z, size.y - height + Camera.main.transform.position.z);

       

        return returnVec;
    }

}
