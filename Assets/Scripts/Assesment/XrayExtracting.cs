using System.IO;
using UnityEngine;
using System.Collections;

public class XRayExtraction : MonoBehaviour
{

    public string folderName = "SavedImages"; // Folder to save images in

    private int imageIndex = 0;
    public RenderTexture cameraRenderView1; // Assign in Inspector
    public RenderTexture cameraRenderView2; // Assign in Inspector
    public GameObject XrayDistalScreen; // this one is in the ui to be captured if in the distal part


    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveXrayImage("xrayFileName", "null");

        }
    }

    public void SaveXrayImage(string xrayFileName, string secondXrayFileName = null)
    {
        SaveRenderTextureToImage(xrayFileName, cameraRenderView1);
        if (XrayDistalScreen != null && XrayDistalScreen.activeInHierarchy)
        {
            SaveRenderTextureToImage(secondXrayFileName, cameraRenderView2);
        }
    }


    public void SaveRenderTextureToImage(string baseFileName, RenderTexture screen)
    {
        // Ensure save folder exists
        string folderPath = Path.Combine(Application.dataPath, folderName);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // Start with default name
        string filename = $"{baseFileName}.png";
        string fullPath = Path.Combine(folderPath, filename);
        int duplicateIndex = 1;

        // Loop until we find a filename that doesn't exist
        while (File.Exists(fullPath))
        {
            filename = $"{baseFileName} ({duplicateIndex}).png";
            fullPath = Path.Combine(folderPath, filename);
            duplicateIndex++;
        }

        // Set active RenderTexture
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = screen;

        // Copy pixels into Texture2D
        Texture2D tex = new Texture2D(screen.width, screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, screen.width, screen.height), 0, 0);
        tex.Apply();

        // Save to file
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(fullPath, bytes);

        Debug.Log("Saved image to: " + fullPath);

        // Restore
        RenderTexture.active = currentRT;
        Destroy(tex);
    }


}
