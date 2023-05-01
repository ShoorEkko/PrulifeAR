using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameHandler : MonoBehaviour
{
    [SerializeField]private GameObject[] m_frames;
    
    public void SwitchFrame(int index)
    {
        for (int i = 0; i < m_frames.Length; i++) m_frames[i].gameObject.SetActive(i == index);
    }
}
