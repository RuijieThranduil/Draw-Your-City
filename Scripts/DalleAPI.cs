using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class DataItem
{
    public string url;
}

[System.Serializable]
class ResponseData
{
    private long created;
    public DataItem[] data;
}
public class DalleAPI : MonoBehaviour
{
    [SerializeField] private string apiKey = ""; // Inspector  API
    
    
    // text to image 
    public IEnumerator GenerateImage(string prompt, System.Action<Texture2D> onComplete)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            Debug.LogError("Prompt cannot be empty.");
            yield break;
        }

        // JSON 
        string json = $"{{\"prompt\":\"{prompt}\",\"n\":1,\"size\":\"1024x1024\"}}";
        Debug.Log($"Sending request: {json}");

        // HTTP
        string apiUrl = "https://api.openai.com/v1/images/generations";
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Response: {request.downloadHandler.text}");

            string responseText = request.downloadHandler.text;
            string imageUrl = ParseImageUrl(responseText);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                // ����ͼƬ
                StartCoroutine(DownloadImage(imageUrl, onComplete));
            }
            else
            {
                Debug.LogError("Failed to extract image URL from response.");
            }
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
        }
    }
    
    // Text + Mask ( 0&1 )+ scaching =====>> Image
    public IEnumerator GenerateImageWithPromptAndImage(string prompt, System.Action<Texture2D> onComplete)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            Debug.LogError("Prompt cannot be empty.");
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("prompt", prompt);
        form.AddField("n", "1");
        form.AddField("size", "1024x1024");

        
        Texture2D texture = new Texture2D(512,512); // Create a new 2D Texture object 
        texture.LoadImage(LoadImageBytes("/Resources/src.png")); // Load image data from a byte array
        Texture2D stretchTexture = StretchTexture(texture,512,512);
        
        byte[] imgBytes = stretchTexture.EncodeToPNG();
        
        form.AddBinaryData("image", imgBytes);
        
        Debug.Log($"src 大小占用：{stretchTexture.GetRawTextureData().Length/1024/1024} mb"); 
        
        Texture2D texture2 = new Texture2D(512,512); // Create a new 2D texture object
        texture2.LoadImage(LoadImageBytes("/Resources/mask.png")); // Load image data from a byte array
        Texture2D stretchTexture2 = StretchTexture(texture2,512,512);
        byte[] maskBytes = stretchTexture2.EncodeToPNG();
        
        form.AddBinaryData("mask", maskBytes);
        
        Debug.Log($"mask 大小占用：{stretchTexture2.GetRawTextureData().Length/1024/1024} mb"); 
        
        
        UnityWebRequest request = UnityWebRequest.Post("https://api.openai.com/v1/images/edits", form);
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Response: {request.downloadHandler.text}");

            string responseText = request.downloadHandler.text;
            string imageUrl = ParseImageUrl(responseText);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                // ����ͼƬ
                StartCoroutine(DownloadImage(imageUrl, onComplete));
            }
            else
            {
                Debug.LogError("Failed to extract image URL from response.");
            }
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
        }
    }
    
        /**
     * prompt + mask + src => image *
     */
    public IEnumerator GenerateImageWithMask(string prompt, Texture2D texInputImage, Texture2D texInputMask, System.Action<Texture2D> onComplete)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            Debug.LogError("Prompt cannot be empty.");
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("prompt", prompt);
        form.AddField("n", "1");
        form.AddField("size", "1024x1024");
        
        Texture2D texture = new Texture2D(512,512); // Create a new Texture2D Object
        texture.LoadImage(texInputImage.EncodeToPNG()); // Load image data from a byte array
        Texture2D stretchTexture = StretchTexture(texture,512,512);
        
        byte[] imgBytes = stretchTexture.EncodeToPNG();
        
        form.AddBinaryData("image", imgBytes);
        
        Debug.Log($"src 大小占用：{stretchTexture.GetRawTextureData().Length/1024/1024} mb"); 
        
        Texture2D texture2 = new Texture2D(512,512); // Create a new Texture2D Object 
        texture2.LoadImage(texInputMask.EncodeToPNG()); // Load image data from a byte array
        Texture2D stretchTexture2 = StretchTexture(texture2,512,512);
        byte[] maskBytes = stretchTexture2.EncodeToPNG();
        
        form.AddBinaryData("mask", maskBytes);
        
        Debug.Log($"mask 大小占用：{stretchTexture2.GetRawTextureData().Length/1024/1024} mb"); 
        
        
        UnityWebRequest request = UnityWebRequest.Post("https://api.openai.com/v1/images/edits", form);
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Response: {request.downloadHandler.text}");

            string responseText = request.downloadHandler.text;
            string imageUrl = ParseImageUrl(responseText);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                // ����ͼƬ
                StartCoroutine(DownloadImage(imageUrl, onComplete));
            }
            else
            {
                Debug.LogError("Failed to extract image URL from response.");
                onComplete?.Invoke(default);
            }
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
            onComplete?.Invoke(default);
        }
    }

    Texture2D StretchTexture(Texture2D originalTexture, int newWidth, int newHeight)
    {
        Texture2D stretchedTexture = new Texture2D(newWidth, newHeight);

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                float u = (float)x / newWidth;
                float v = (float)y / newHeight;

                int originalX = Mathf.RoundToInt(u * originalTexture.width);
                int originalY = Mathf.RoundToInt(v * originalTexture.height);

                Color pixel = originalTexture.GetPixel(originalX, originalY);
                stretchedTexture.SetPixel(x, y, pixel);
            }
        }

        stretchedTexture.Apply();

        return stretchedTexture;
    }
    
    byte[] LoadImageBytes(string imagePath)
    {
        string filePath = Application.dataPath + imagePath;
        return System.IO.File.ReadAllBytes(filePath);
    }
    
    private string ParseImageUrl(string jsonResponse)
    {
        //  JSON  URL
        var playerData = JsonUtility.FromJson<ResponseData>(jsonResponse);
        return playerData.data[0].url;
    }

    private IEnumerator DownloadImage(string url, System.Action<Texture2D> onComplete)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            onComplete?.Invoke(texture);
        }
        else
        {
            Debug.LogError($"Failed to download image: {request.error}");
            onComplete?.Invoke(default);
        }
    }
}
