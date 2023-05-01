using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] CameraManager cameraManager;
    [SerializeField] private Button twitterButton;
    [SerializeField] private Button instagramButton;
    [SerializeField] private Button facebookButton;
    [SerializeField] private Button recordButton;
    [SerializeField] GameObject[] buttonsToSwap;
    public void TakeScreenshot()
    {
        StartCoroutine(cameraManager.CaptureScreenshot());
    }

    public void DownloadImage()
    {
        cameraManager.DownloadScreenshot();
    }

    public void TakeRecord()
    {
        cameraManager.StartRecording();
    }

    public void ShareToTwitter()
    {
        cameraManager.TwitterShare();
    }

    public void ShareToInstagram()
    {
        //StartCoroutine(cameraManager.TakeScreenshotAndShare("com.instagram.android", "com.burbn.instagram"));
    }

    public void ShareToFacebook()
    {
        //StartCoroutine(cameraManager.TakeScreenshotAndShare("com.facebook.katana", "com.facebook.katana"));
    }

    public void SwapTrigger()
    {
        bool button1Active = buttonsToSwap[0].gameObject.activeSelf;
        bool button2Active = buttonsToSwap[1].gameObject.activeSelf;

        buttonsToSwap[0].gameObject.SetActive(button2Active);
        buttonsToSwap[1].gameObject.SetActive(button1Active);
    }
    
}