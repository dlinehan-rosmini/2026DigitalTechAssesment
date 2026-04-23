using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IconBakerScript : MonoBehaviour
{
    [Header("Setup")]
    public Camera iconCamera;
    public GameObject targetPrefab;

    [Header("Icon Settings")]
    public int resolution = 512;
    public Vector3 rotationOffset = new Vector3(0, 90, 0); // 90 on Y gives a nice side-profile

    [ContextMenu("Bake Icon")]
    public void Bake()
    {
        if (iconCamera == null || targetPrefab == null)
        {
            Debug.LogError("Please assign the Camera and Target Prefab.");
            return;
        }

        // 1. Create a transparent RenderTexture with a 24-bit depth buffer
        RenderTexture rt = new RenderTexture(resolution, resolution, 24, RenderTextureFormat.ARGB32);
        iconCamera.targetTexture = rt;

        // 2. Spawn the gun at the exact center, applying the rotation offset
        GameObject tempObj = Instantiate(targetPrefab, Vector3.zero, Quaternion.Euler(rotationOffset));

        // 3. Force the gun and all its parts onto the Icon_Studio layer
        int studioLayer = LayerMask.NameToLayer("Icon_Studio");
        tempObj.layer = studioLayer;
        foreach (Transform child in tempObj.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = studioLayer;
        }

        // 4. Command URP to render this single camera frame safely
        iconCamera.Render();

        // 5. Extract the pixels from the GPU to a Texture2D
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        screenShot.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        screenShot.Apply();

        // 6. Clean up memory and destroy the temporary gun clone
        RenderTexture.active = null;
        iconCamera.targetTexture = null;
        DestroyImmediate(tempObj);
        DestroyImmediate(rt);

        // 7. Save to a PNG file
        byte[] bytes = screenShot.EncodeToPNG();
        string directoryPath = Application.dataPath + "/GeneratedIcons";

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = directoryPath + "/" + targetPrefab.name + "_Icon.png";
        File.WriteAllBytes(filePath, bytes);

        Debug.Log("Icon successfully baked and saved to: " + filePath);

        // 8. Tell Unity to refresh the project window so the file appears immediately
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}