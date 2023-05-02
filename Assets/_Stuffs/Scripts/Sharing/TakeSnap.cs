using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zappar.Additional.SNS;

public class TakeSnap : MonoBehaviour
{
    [Range(0f,1f)]
    public float encodeQuality = 0.75f;

    [Header("EVENTS")]
    public UnityEvent OnSnapSaved = new UnityEvent();
    public UnityEvent OnSnapShared = new UnityEvent();
    public UnityEvent OnSharePanelClosed = new UnityEvent();

    [SerializeField] CameraManager cameraManager;


    private void Start()
    {
        ZSaveNShare.Initialize();
    }

    public void TakeSnapshot()
    {
        //cameraManager.MoveFrames();
        ZSaveNShare.RegisterSNSCallbacks(OnSaved, OnShared, OnPromptClosed);
        StartCoroutine(ZSaveNShare.TakeSnapshot(OnSnapshotCaptured, encodeQuality));
    }

    public void OnSnapshotCaptured()
    {
        Debug.Log("Open prompt");
        ZSaveNShare.OpenSNSSnapPrompt();
    }

    public void OnSaved()
    {
        Debug.Log("Prompt saved");
        OnSnapSaved.Invoke();
    }

    public void OnShared()
    {
        Debug.Log("Prompt shared");
        OnSnapShared.Invoke();
    }

    public void OnPromptClosed()
    {
        cameraManager.ResetFrames();
        Debug.Log("Save and share prompt closed");
        OnSharePanelClosed.Invoke();
        ZSaveNShare.DeregisterSNSCallbacks(OnSaved, OnShared, OnPromptClosed);
    }
}
