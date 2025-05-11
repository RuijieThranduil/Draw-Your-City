using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ImageGenerator : MonoBehaviour
{
    public DalleAPI dalleAPI;
    public TMP_InputField promptInput;
    public RawImage generatedImage;

    public TMP_Text statusText;
    public TMP_Text downloadProgressText; // 新增用于显示下载进度的文本

    private Texture2D currentTexture; // 当前展示的贴图
    
    #region 创建mask的参数
    
    public float maskRadius = 100f;
    public RawImage maskImage;
    private bool maskNeedUpdate = true;
    
    #endregion
    
    public void OnGenerateButtonClick()
    {
        string prompt = promptInput.text;

        if (string.IsNullOrEmpty(prompt))
        {
            statusText.text = "Please enter a valid prompt.";
            return;
        }

        statusText.text = "Generating image...";
        StartCoroutine(dalleAPI.GenerateImage(prompt, OnImageGenerated));
    }
    
    public void OnGenerateButtonWithMaskClick()
    {
        string prompt = promptInput.text;

        if (string.IsNullOrEmpty(prompt))
        {
            statusText.text = "Please enter a valid prompt.";
            return;
        }

        statusText.text = "Generating image...";
        StartCoroutine(dalleAPI.GenerateImageWithPromptAndImage(prompt, OnImageGenerated));
    }

    /**
     * 保存贴图到本地 *
     */
    public void SaveSrcToFile()
    {
        if (currentTexture == null)
        {
            statusText.text = "please create texture first!";
            return;
        }
        
        SaveTextureToFile(currentTexture, Application.dataPath + "/Resources/src.png");
    }
    
    /**
     * 生成mask图*
     */
    public void CreateMask()
    {
        var tex = LoadTextureFromFile(Application.dataPath + "/Resources/src.png");; // src.png 必须在Resources路径下
        if (tex == null)
        {
            statusText.text = "please create \"Asset/Resources/src.png\" first! ";
            return;
        }
        // // 调整图片为正方形
        // Texture2D squareTexture = MakeTextureSquare(tex);
        // EraseTexture(squareTexture);
        
        Texture2D stretchTexture = StretchTexture(tex,512,512);
        EraseTexture(stretchTexture);
    }
    
    Texture2D MakeTextureSquare(Texture2D texture)
    {
        int size = Mathf.Min(texture.width, texture.height); // 获取宽度和高度的最小值
        Texture2D squareTexture = new Texture2D(size, size); // 创建一个新的正方形 Texture2D 对象

        // 将原始图片的像素复制到正方形图片中心
        int offsetX = (texture.width - size) / 2;
        int offsetY = (texture.height - size) / 2;
        squareTexture.SetPixels(0, 0, size, size, texture.GetPixels(offsetX, offsetY, size, size));
        squareTexture.Apply();

        return squareTexture;
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
    
    /**
     * 抠图 *
     */
    private void EraseTexture(Texture2D texture)
    {
        // 创建一个新的贴图副本，以便在其上进行修改
        Texture2D modifiedTexture = Instantiate(texture);
        
        // 遍历贴图的所有像素，将擦除区域的像素 alpha 通道设置为 0
        for (int x = 0; x < modifiedTexture.width; x++)
        {
            for (int y = 0; y < modifiedTexture.height; y++)
            {
                if (Vector2.Distance(new Vector2(x, y), new Vector2(modifiedTexture.width/2,modifiedTexture.height/2)) <= maskRadius)
                {
                    Color pixelColor = modifiedTexture.GetPixel(x, y);
                    pixelColor.a = 0f;
                    modifiedTexture.SetPixel(x, y, pixelColor);
                }
            }
        }
        

        // 应用修改后的贴图
        modifiedTexture.Apply();
        
        // 保存贴图到本地
        SaveTextureToFile(modifiedTexture, Application.dataPath + "/Resources/mask.png");

        maskNeedUpdate = true;
    }
    
    /**
     * 保存贴图到本地 *
     */
    void SaveTextureToFile(Texture2D texture, string filePath)
    {
        byte[] bytes = texture.EncodeToPNG(); // 或者使用 EncodeToJPG 方法保存为 JPG 格式
        System.IO.File.WriteAllBytes(filePath, bytes);
        
        statusText.text = $"create {filePath.Split("Assets")[1]} successfully! ";
    }
    
    private void OnImageGenerated(Texture2D texture)
    {
        if (texture != null)
        {
            statusText.text = "Image generated successfully!";
            generatedImage.texture = texture;

            currentTexture = texture;
            
            // generatedImage.SetNativeSize();
        }
        else
        {
            statusText.text = "Failed to generate image.";
        }
    }

    private void Update()
    {
        if (maskNeedUpdate)
        {
            // 从本地加载保存的图片
            Texture2D texture = LoadTextureFromFile(Application.dataPath + "/Resources/mask.png");

            // 将加载的图片赋值给 Image 组件的 texture 属性
            maskImage.texture = texture;
            
            maskNeedUpdate = false;
        }
    }
    
    Texture2D LoadTextureFromFile(string filePath)
    {
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(512,512); // 创建一个新的 Texture2D 对象
        texture.LoadImage(bytes); // 从字节数组加载图片数据
        return texture;
    }
}
