using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;
using UnityEngine.UI;

public class NewGestureManager : MonoBehaviour
{
    //�⺻ �� ������
    private bool leftHandChecked;
    private Vector3 leftHandPosition;

    private bool shoulderLeftChecked;
    private Vector3 shoulderLeftPosition;

    private bool rightHandChecked;
    private Vector3 rightHandPosition;

    private bool shoulderRightChecked;
    private Vector3 shoulderRightPosition;

    private bool neckChecked;
    private Vector3 neckPosition;

    private bool spineChestChecked;
    private Vector3 spineChestPosition;

    //��� ��ġ
    private bool pelvisChecked;
    private Vector3 pelvisPosition;

    //-----------------------------------------------------
    //�������� ���� ������
    //public Vector3 skatingStartPosition;

    //������ ��� checkbox
    public bool baseGestureActive = false;

    //kinect manager ��������
    public KinectManager kinectManager;

    public enum GestureTypeNormal
    {
        //����
        none,
        leftHandUp,
        rightHandUp,
        bothHandUp,
    }

    public enum GestureTypeSkating
    {
        //���ǵ� ��������
        none,
        basePose,
        leftPose,
        rightPose,
    }
    public enum GestureTypeBoxing
    {
        //����
        none,

        guard,

        ready,

        leftShortPunch,
        leftLongPunch,

        rightShortPunch,
        rightLongPunch,

        ultimateAttack,
    }
    public enum GestureTypePong
    {
        //Ź��
        none,

        //�޼� �غ�
        leftHandReadyLeft,
        leftHandReadyRight,
        //������ �غ�
        rightHandReadyLeft,
        rightHandReadyRight,

        //�޼� �߻�
        leftHandLeftShoot,
        leftHandMiddleShoot,
        leftHandRightShoot,
        //������ �߻�
        rightHandLeftShoot,
        rightHandMiddleShoot,
        rightHandRightShoot,

    }

    //��ü on off
    public bool gestureActivate = true;

    [Space(10)]
    //���� ���� ����
    public bool gestureTypeNormalActive = false;
    public GestureTypeNormal nowGestureTypeNormal = GestureTypeNormal.none;

    [Space(10)]
    public bool gestureTypeSpeedActive = false;
    public GestureTypeSkating nowGestureTypeSpeed = GestureTypeSkating.none;

    [Space(10)]
    public bool gestureTypeBoxingActive = false;
    public GestureTypeBoxing nowGestureTypeBoxing = GestureTypeBoxing.none;
    public float punchFront = 0.4f;
    public float punchSide = 0.05f;

    //public float ready
    public float readySideDeviation = 0.05f;
    public float readyFrontDeviation = 0.15f;

    //���� �߰� ����
    public float gaurdUpperDeviation = 0.05f;

    [Space(10)]
    public bool gestureTypePongActive = false;
    public GestureTypePong nowGestureTpyePong = GestureTypePong.none;

    //UI-----------------------------------------------------------------------
    [Space(10)]
    public Text normalGestureText;
    public Text speedGestureText;
    public Text boxingGestureText;
    public Text pongGestureText;


    // Start is called before the first frame update
    void Start()
    {
        //�⺻ ���� ����
        GestureSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (gestureActivate)
        {
            CheckGestureStatus();
        }
    }

    //���� ����
    void CheckGestureStatus()
    {
        //�⺻ ���� �ʱ�ȭ
        CheckingSetting();

        if (gestureTypeNormalActive)
        {
            nowGestureTypeNormal = GestureTypeNormal.none;

            CheckLeftHandUp();

            CheckRightHandUp();

            normalGestureText.text = nowGestureTypeNormal.ToString();
            //CheckBothHandUp();
        }

        if (gestureTypeSpeedActive)
        {
           
        }

        if (gestureTypeBoxingActive)
        {
            
            CheckReady();

            CheckGuard();

            CheckLeftPunch();
            CheckRightPunch();

            boxingGestureText.text = nowGestureTypeBoxing.ToString();
        }

        if (gestureTypePongActive)
        {
            
        }
    }

    void GestureSetting()
    {
        if (gestureTypeNormalActive)
        {
            nowGestureTypeNormal = GestureTypeNormal.none;
        }

        if (gestureTypeSpeedActive)
        {
            nowGestureTypeSpeed = GestureTypeSkating.none;
        }

        if (gestureTypeBoxingActive)
        {
            nowGestureTypeBoxing = GestureTypeBoxing.ready;
        }

        if (gestureTypePongActive)
        {
            nowGestureTpyePong = GestureTypePong.none;
        }
    }

#region normalGesture
    void CheckingSetting()
    {
        leftHandChecked = false;
        shoulderLeftChecked = false;

        rightHandChecked = false;
        shoulderRightChecked = false;

        neckChecked = false;
        spineChestChecked = false;
        pelvisChecked = false;


    }

    void CheckLeftHandUp()
    {
        if (!leftHandChecked)
        {
            leftHandPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.HandLeft, true);
            leftHandChecked = true;
        }

        if (!neckChecked)
        {
            neckPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.Neck, true);
            neckChecked = true;
        }

        if(leftHandPosition.y > neckPosition.y)
        {
            if(nowGestureTypeNormal == GestureTypeNormal.rightHandUp)
            {
                nowGestureTypeNormal = GestureTypeNormal.bothHandUp;
            }
            else
            {
                nowGestureTypeNormal = GestureTypeNormal.leftHandUp;
            }
            
        }
    }

    void CheckRightHandUp()
    {
        if (!rightHandChecked)
        {
            rightHandPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.HandRight, true);
            rightHandChecked = true;
        }

        if (!neckChecked)
        {
            neckPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.Neck, true);
            neckChecked = true;
        }

        if (rightHandPosition.y > neckPosition.y)
        {
            if (nowGestureTypeNormal == GestureTypeNormal.leftHandUp)
            {
                nowGestureTypeNormal = GestureTypeNormal.bothHandUp;
            }
            else
            {
                nowGestureTypeNormal = GestureTypeNormal.rightHandUp;
            }
        }

        
    }

    #endregion

#region skatingGesture
    void CheckLeftHandPose()
    {

    }

#endregion

#region boxingGesture
    void CheckReady()
    {

        if(nowGestureTypeBoxing != GestureTypeBoxing.ready)
        {
            if (!leftHandChecked)
            {
                leftHandPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.HandLeft, true);
                leftHandChecked = true;
            }

            if (!shoulderLeftChecked)
            {
                shoulderLeftPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.ShoulderLeft, true);
                shoulderLeftChecked = true;
            }

            if (!rightHandChecked)
            {
                rightHandPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.HandRight, true);
                rightHandChecked = true;
            }

            if (!shoulderRightChecked)
            {
                shoulderRightPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.ShoulderRight, true);
                shoulderRightChecked = true;
            }


            if (!spineChestChecked)
            {
                spineChestPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.SpineChest, true);
                spineChestChecked = true;
            }

            //readyFrontDeviation
            //if()

            if ((leftHandPosition.x > shoulderLeftPosition.x - readySideDeviation) && ((spineChestPosition.z - leftHandPosition.z) < readyFrontDeviation))
            {
                if ((rightHandPosition.x < shoulderRightPosition.x + readySideDeviation) && ((spineChestPosition.z - rightHandPosition.z) < readyFrontDeviation))
                {
                    nowGestureTypeBoxing = GestureTypeBoxing.ready;
                }
            }
        }
    }

    void CheckLeftPunch()
    {
        if (nowGestureTypeBoxing == GestureTypeBoxing.ready)
        {
            //��
            if (!leftHandChecked)
            {
                leftHandPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.HandLeft, true);
                leftHandChecked = true;
            }

            //����
            if (!spineChestChecked)
            {
                spineChestPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.SpineChest, true);
                spineChestChecked = true;
            }

            //���
            if (!shoulderLeftChecked)
            {
                shoulderLeftPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.ShoulderLeft, true);
                shoulderLeftChecked = true;
            }

            //����
            if ((spineChestPosition.z - leftHandPosition.z) > punchFront)
            {
                if ((shoulderLeftPosition.x - leftHandPosition.x) > punchSide)
                {
                    nowGestureTypeBoxing = GestureTypeBoxing.leftLongPunch;
                }
                else
                {
                    nowGestureTypeBoxing = GestureTypeBoxing.leftShortPunch;
                }
            }

        }
    }

    void CheckRightPunch()
    {
        if (nowGestureTypeBoxing == GestureTypeBoxing.ready)
        {
            if (!rightHandChecked)
            {
                rightHandPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.HandRight, true);
                rightHandChecked = true;
            }

            if (!spineChestChecked)
            {
                spineChestPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.SpineChest, true);
                spineChestChecked = true;
            }

            //���
            if (!shoulderRightChecked)
            {
                shoulderRightPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.ShoulderRight, true);
                shoulderRightChecked = true;
            }

            if (nowGestureTypeBoxing == GestureTypeBoxing.ready)
            {
                //����
                if ((spineChestPosition.z - rightHandPosition.z) > punchFront)
                {
                    if ((rightHandPosition.x - shoulderRightPosition.x) > punchSide)
                    {
                        nowGestureTypeBoxing = GestureTypeBoxing.rightLongPunch;
                    }
                    else
                    {
                        nowGestureTypeBoxing = GestureTypeBoxing.rightShortPunch;
                    }
                }
            }
        }

    }

    void CheckGuard()
    {
        if (nowGestureTypeBoxing == GestureTypeBoxing.ready || nowGestureTypeBoxing == GestureTypeBoxing.guard)
        {
            if (!leftHandChecked)
            {
                leftHandPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.HandLeft, true);
                leftHandChecked = true;
            }

            if (!shoulderLeftChecked)
            {
                shoulderLeftPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.ShoulderLeft, true);
                shoulderLeftChecked = true;
            }

            if (!rightHandChecked)
            {
                rightHandPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.HandRight, true);
                rightHandChecked = true;
            }

            if (!shoulderRightChecked)
            {
                shoulderRightPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.ShoulderRight, true);
                shoulderRightChecked = true;
            }

            if (!neckChecked)
            {
                neckPosition = kinectManager.GetJointKinectPosition(KinectManager.Instance.GetPrimaryUserID(), (int)KinectInterop.JointType.Neck, true);
                neckChecked = true;
            }

            //���� ����
            
            if ((leftHandPosition.x > shoulderLeftPosition.x - readySideDeviation) && (leftHandPosition.y > neckPosition.y + gaurdUpperDeviation))
            {
                if ((rightHandPosition.x < shoulderRightPosition.x + readySideDeviation) && (rightHandPosition.y > neckPosition.y + gaurdUpperDeviation))
                {
                    nowGestureTypeBoxing = GestureTypeBoxing.guard;
                }
                else
                {
                    nowGestureTypeBoxing = GestureTypeBoxing.ready;
                }
            }
            else
            {
                nowGestureTypeBoxing = GestureTypeBoxing.ready;
            }

        }
    }

#endregion

#region pongGestrue
    void handReady()
    {

    }
#endregion

}
