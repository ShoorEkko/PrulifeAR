using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitcher : MonoBehaviour
{   
    private int _currentIndex;
    [Header("Reference")]
    [SerializeField]private Image m_baseImage;
    [SerializeField]private Sprite[] m_toggleSprites;

    [Header("Events")]
    public UnityEvent<int> OnSwitched = new UnityEvent<int>();


    void Start()
    {
     //   _currentIndex= 1;
        OnSwitched.Invoke(1);
    }
    public void ToggleSwitch(){
        if(_currentIndex.Equals(0)){
            OnSwitched.Invoke(0);
            m_baseImage.sprite =m_toggleSprites[1];
            _currentIndex= 1;
        }else{
            OnSwitched.Invoke(1);
            m_baseImage.sprite =m_toggleSprites[0];
            _currentIndex= 0;
        }
    }
}
