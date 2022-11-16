using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KinectMouse : MonoBehaviour
{

    public KinectTest kinectTest;

    public int ScreenX = 1920;
    public int ScreenY = 1000;

    public List<Sprite> cursorImageList;
    public Image cursorImage;
    public int cursorImageNumber = 0;

    [Space (10)]

    public float MouseAccel = 3f;
    public float advanceXposition = 0;
    public float advanceYposition = -300;

    public float appliedMouseX = 0;
    public float appliedMouseY = 0;

    //private Collider2D collider2D;


    private void Start()
    {
        if(cursorImageList.Count > 0)
        {
            if(cursorImageList.Count > cursorImageNumber)
            {
                this.GetComponent<Image>().sprite = cursorImageList[cursorImageNumber];
            }
        }

        //collider2D = this.GetComponent<Collider2D>();
    }

    void Update()
    {

        appliedMouseX = ScreenX * (0.5f) * kinectTest.tempKinectVector.x * MouseAccel + advanceXposition;
        appliedMouseY = ScreenY * (0.5f) * kinectTest.tempKinectVector.y * MouseAccel + advanceYposition;

        if(appliedMouseX > ScreenX* (0.5f))
        {
            appliedMouseX = ScreenX * (0.5f);
        }

        if (appliedMouseX < -ScreenX * (0.5f))
        {
            appliedMouseX = -ScreenX * (0.5f);
        }

        if (appliedMouseY > ScreenY * (0.5f))
        {
            appliedMouseY = ScreenY * (0.5f);
        }

        if (appliedMouseY < -ScreenY * (0.5f))
        {
            appliedMouseY = -ScreenY * (0.5f);
        }

        this.transform.localPosition = new Vector3(appliedMouseX, appliedMouseY, 0);
        
    }
}
