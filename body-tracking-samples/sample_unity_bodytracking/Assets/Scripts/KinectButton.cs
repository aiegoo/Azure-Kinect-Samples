using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KinectButton : MonoBehaviour
{
    public Text targetName;

    public float targetTime = 2f;
    public Slider slider;

    private bool isTriggered = false;

    private void Start()
    {
        slider.maxValue = targetTime;
    }
    void LateUpdate()
    {
        
        if(slider.value >= targetTime)
        {
            ChangeTarget("완료");
        }
        else if(slider.value > 0 && isTriggered == false)
        {
            slider.value -= Time.deltaTime;
        }

        isTriggered = false;
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (slider.value < targetTime)
        {
            if (collision.gameObject.CompareTag("Cursor"))
            {
                ChangeTarget("지정 중");

                slider.value += Time.deltaTime;

                isTriggered = true;
            }
        }
    }
    

    
 
    

    void ChangeTarget(string target)
    {
        targetName.text = target;

    }
}
