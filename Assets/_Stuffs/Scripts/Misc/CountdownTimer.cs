using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField]private Image m_baseImage;
    [SerializeField]private Sprite[] m_countdownSprites;
    public UnityEvent OnCountdownFinished = new UnityEvent();
    void OnEnable()
    {
        StartCoroutine(TimerRoutine());
    }
    
    
    public IEnumerator TimerRoutine(){
        m_baseImage.sprite = m_countdownSprites[0];
        while (true)
        {
           for (int i = 0; i < 3; i++)
           {
             m_baseImage.sprite = m_countdownSprites[i];
             yield return new WaitForSeconds(1);
             if(i.Equals(2)){
                yield return new WaitForSeconds(0.5f);
                OnCountdownFinished.Invoke();
                gameObject.SetActive(false);
                yield break;
             }
           }
        }
    }
}
