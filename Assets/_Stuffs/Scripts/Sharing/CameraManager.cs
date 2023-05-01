using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;


public class CameraManager : MonoBehaviour
{
    [SerializeField]private Camera[] cameraToFreeze;
    private ButtonManager buttonManager;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] GameObject buttonHandler;
    [SerializeField] GameObject downloadButton;

    public TMP_Text downloadLinkText;
    public bool takingScreenshot = false;
    
    //public string screenshotName = "screenshot.png";
    public string downloadButtonLabel = "Download Screenshot";

    public string screenshotName = "Screenshot"; //remove later

    private string screenshotPath;
    private string downloadURL;

    private Texture2D texture;
    [SerializeField]private RawImage frozenImage;

    void Start()
    {

    }

    public void StartRecording()
    {
        
    }

    public IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();
        // Create a new texture with the screen dimensions
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        // Read the pixels from the screen and apply them to the texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Encode the texture to a PNG file
        byte[] bytes = texture.EncodeToPNG();
        Destroy(texture);

        
        // Save the screenshot to the device's persistent data path
        screenshotPath = Path.Combine(Application.persistentDataPath, screenshotName);
        File.WriteAllBytes(screenshotPath, bytes);

        // Generate the download URL
        downloadURL = "file://" + screenshotPath;

        // Enable the download button and display the download link
        downloadButton.gameObject.SetActive(true);
        downloadLinkText.text = "Download Screenshot";
    }

    public void DownloadScreenshot()
    {
        StartCoroutine(DownloadScreenshotFile());
    }

    public void TwitterShare()
    {
        StartCoroutine(PostTweet());
    }

    private IEnumerator DownloadScreenshotFile() //Needs permission to download or a server to handle
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(downloadURL);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // Save the downloaded file to the user's device
            string savePath = Path.Combine(Application.persistentDataPath, screenshotName);
            File.WriteAllBytes(savePath, webRequest.downloadHandler.data);

            // Show a confirmation message or perform further actions
            Debug.Log("Screenshot downloaded successfully!");
        }
        else
        {
            // Show an error message or handle the failure case
            Debug.Log("Screenshot download failed: " + webRequest.error);
        }
    }

    private IEnumerator PostTweet()
    {
        yield return new WaitForEndOfFrame(); //Needs permission to share
    }

    private IEnumerator PostFacebook()
    {
        yield return new WaitForEndOfFrame(); //Needs permission to share
    }

    private IEnumerator PostInstagram()
    {
        yield return new WaitForEndOfFrame(); //Needs permission to share
    }

}
