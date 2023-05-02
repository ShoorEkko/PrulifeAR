using System;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown stickerDropdown; //Sticker choices
    [SerializeField] private RawImage stickerImage; //Image that holds the current sticker
    [SerializeField] private GameObject[] arStickers; //Image that holds the current sticker
    [SerializeField] private Texture2D[] stickerTextures;
    [SerializeField] private Camera[] cameraToFreeze;
    [SerializeField] private DOTweenAnimation[] movingFrames;
    [SerializeField] private GameObject[] movingObjects;
    [SerializeField] private List<AnimationObjectClass> animObjectList;
    private ButtonManager buttonManager;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] GameObject buttonHandler;
    [SerializeField] GameObject downloadButton;
    public TMP_Text downloadLinkText;
    public bool takingScreenshot = false;
    
    public string downloadButtonLabel = "Download Screenshot";
    public string screenshotName = "Screenshot"; //remove later
    private string screenshotPath;
    private string downloadURL;

    private Texture2D texture;

    void Start()
    {
        ChangeSticker(0);
        // Set the initial texture based on the first option
        stickerImage.enabled = false;

        // Add a listener to the dropdown to handle option changes
        stickerDropdown.onValueChanged.AddListener(ChangeSticker);

        
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

    private void ChangeSticker(int optionIndex)
    {
        for (int i = 0; i < arStickers.Length; i++) 
            arStickers[i].gameObject.SetActive(i == optionIndex);

        // // Check if the option index is within the range of the textures array
        // if (optionIndex > 0 && optionIndex < stickerTextures.Length)
        // {
        //     stickerImage.enabled = true;
        //     // Set the sticker image's texture to the selected option's texture
        //     stickerImage.texture = stickerTextures[optionIndex];
        // }
        // else if(optionIndex == 0)
        // {
        //     stickerImage.enabled = false;
        // }

        // // Get the screen width and height
        // float screenWidth = Screen.width;
        // float screenHeight = Screen.height;

        // // Set the padding value
        // float padding = 100f;

        // // Get the stickerImage's width and height
        // float stickerImageWidth = stickerImage.rectTransform.rect.width;
        // float stickerImageHeight = stickerImage.rectTransform.rect.height;

        // // Calculate the maximum x and y positions within the screen boundaries
        // float maxX = screenWidth - stickerImageWidth - padding * 2;
        // float maxY = screenHeight - stickerImageHeight - padding * 2;

        // // Calculate the random position within the adjusted screen boundaries
        // float randomX = UnityEngine.Random.Range(padding, maxX);
        // float randomY = UnityEngine.Random.Range(padding, maxY);

        // // Set the stickerImage's position to the random position
        // stickerImage.rectTransform.anchoredPosition = new Vector2(randomX, randomY);
    }

    public void MoveFrames()
    {
        Debug.Log("test");
        for (int x = 0; x < animObjectList.Count; x++)
        {
            if (animObjectList[x].temporaryParent.parent.gameObject.activeSelf)
            {
                animObjectList[x].moveDownParent.gameObject.SetActive(true);
                for (int i = 0; i < animObjectList[x].childObjects.Length; i++)
                {
                    animObjectList[x].childObjects[i].transform.parent = animObjectList[x].moveDownParent;
                }
            }
            animObjectList[x].moveUpParent.gameObject.SetActive(false);
        }

    }

    public void ResetFrames()
    {
        for (int x = 0; x < animObjectList.Count; x++)
        {
            if (animObjectList[x].temporaryParent.parent.gameObject.activeSelf)
            {
                animObjectList[x].moveUpParent.gameObject.SetActive(true);
                for (int i = 0; i < animObjectList[x].childObjects.Length; i++)
                {
                    animObjectList[x].childObjects[i].transform.parent = animObjectList[x].moveUpParent;
                }
            }
            animObjectList[x].moveDownParent.gameObject.SetActive(false);
        }
        
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

}

[Serializable]
public class AnimationObjectClass
{
    public Transform temporaryParent;
    public Transform moveDownParent;
    public Transform moveUpParent;  
    public GameObject[] childObjects;
}