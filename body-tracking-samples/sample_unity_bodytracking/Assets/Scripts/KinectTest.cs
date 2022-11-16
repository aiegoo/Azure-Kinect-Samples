using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.rfilkov.kinect;

public class KinectTest : MonoBehaviour
{
    public Text jointName;
    public Text statusText;


    public Text statusText_X;
    public Text statusText_Y;
    public Text statusText_Z;

    public KinectInterop.JointType jointNum;

    public Vector3 tempKinectVector;

    public KinectManager kinectManager;

    //특정 손의 좌표는 계속 받게 만든다.
    public Vector3 nowHandVecotr3 = new Vector3(0,0,0);
    //public bool isHandRight = true;

    // Start is called before the first frame update
    void Start()
    {
        //jointNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        jointName.text = jointNum.ToString();
        statusText.text = kinectManager.IsJointKinectTracked(KinectManager.Instance.GetPrimaryUserID(), (int)jointNum).ToString();


        tempKinectVector = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)jointNum, true);
        statusText_X.text = tempKinectVector.x.ToString();
        statusText_Y.text = tempKinectVector.y.ToString();
        statusText_Z.text = tempKinectVector.z.ToString();


        /*
        if (isHandRight)
        {
            nowHandVecotr3 = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), KinectInterop.JointType.HandRight, true);
        }
        else
        {
            nowHandVecotr3 = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), KinectInterop.JointType.HandLeft, true);
        }
        */
    }
}
