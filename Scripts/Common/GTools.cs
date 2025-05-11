using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GTools
{
    public static Texture2D GetTexture2DFromRenderTexture(RenderTexture renderTexture)
    {
        if (renderTexture!= null)
        {
            Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = null;
            return texture2D;
        }

        return default;
    }
    
    public static void SaveTextureToFile(Texture2D texture, string filePath)
    {
        string dirPath = Path.GetDirectoryName(filePath);
        if (dirPath != null && !Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);
    }
}
