using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    
}


//Functions for if transform is outside of screen and
//Clamping a Vector3 to stay inside the screen
public class ScreenBorder
{
    private static Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    //Actual width and height of the screen in world units
    public Vector2 size = new Vector2(screenBounds.x, Camera.main.orthographicSize);

    
    //Returns true if self + object radius is outside of screen border
    public bool IsOutside(Transform self)
    {
        Renderer renderer = null;

        //for (int i = 0; i < self.childCount; i++)
        //{
        //    if(self)
        //}
        if(self.GetComponent<Renderer>().isVisible)
            return false;

        return true;
        //if ((((self.position.x < (size.x * -1) + width) || (self.position.x > (size.x) - width) ||
        //        (self.position.z < (size.y * -1) + height) || (self.position.z > (size.y) - height))))
        //    return true;

        //return false;
    }

    //Returns a Vector3 that has X and Z clamped to the inside of the screen
    public Vector3 ClampToInside(Transform self, float height, float width)
    {
        Vector3 returnVec = self.position;

        returnVec.x = Mathf.Clamp(returnVec.x, size.x * -1 + width, size.x - width);
        returnVec.z = Mathf.Clamp(returnVec.z, size.y * -1 + height, size.y - height);

       

        return returnVec;
    }

}
