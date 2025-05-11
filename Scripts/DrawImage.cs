using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class DrawImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Shader drawShader;
    public float brushSize = 0.02f;
    public Color brushColor = Color.black; // use drawing function , set brush size

    private RawImage _rawImage;
    public RawImage rawImage
    {
        get
        {
            if (_rawImage == null)
                _rawImage = GetComponent<RawImage>();

            return _rawImage;
        }
        set { _rawImage = value; }
    }

    private Material drawMaterial;      
    private RenderTexture renderTexture;
    
    private int texWidth = 0;
    private int texHeight = 0;
    
    private Vector2 PrePos;

    [Range(1, 10)]
    public float drawLineInterval = 2.0f;

    void Start()
    {
        texWidth = (int)rawImage.rectTransform.rect.width / 4;
        texHeight = (int)rawImage.rectTransform.rect.height / 4;
        renderTexture = new RenderTexture(texWidth, texHeight, 0, RenderTextureFormat.ARGB32, 0);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        drawMaterial = new Material(drawShader);
        drawMaterial.SetTexture("_MainTex", renderTexture);

        rawImage.texture = renderTexture;
    }

    public void SetCanDraw(bool canDraw)
    {
        rawImage.raycastTarget = canDraw;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {        
        PrePos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 screenPos = eventData.position;
        if (Vector2.Distance(screenPos, PrePos) < 0.02f)
        {
            return;
        }

        Vector2 dir = screenPos - PrePos;

        var rtTmp = RenderTexture.GetTemporary(texWidth, texHeight, 0, RenderTextureFormat.ARGB32, 0);

        int count = Mathf.CeilToInt(dir.magnitude / drawLineInterval);
        
        //Debug.Log(count);
        for (int i = 0; i < count; i++)
        {
            Vector2 drawPos = PrePos + dir.normalized * (i * drawLineInterval); 
            
            drawPos = CalculateUV(drawPos);

            drawMaterial.SetFloat("_BrushSize", brushSize);
            drawMaterial.SetColor("_BrushColor", brushColor);
            //unit the drawings to an Image
            drawPos.x *= (texWidth / (float)texHeight);
            drawMaterial.SetVector("_DrawPos", drawPos);
            drawMaterial.SetFloat("_TexWidth", texWidth);
            drawMaterial.SetFloat("_TexHeight", texHeight);
            
            Graphics.Blit(renderTexture, rtTmp, drawMaterial, 0);
            Graphics.Blit(rtTmp, renderTexture);
        }
        
        RenderTexture.ReleaseTemporary(rtTmp);
        
        PrePos = screenPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    Vector2 CalculateUV(Vector2 screenPos)
    {
        var rt = rawImage.rectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            screenPos,
            GameMgr.Ins.uiCamera,
            out var localPos
        );

        Vector2 uv = new Vector2(
            (localPos.x - rt.rect.xMin) / rt.rect.width,
            (localPos.y - rt.rect.yMin) / rt.rect.height
        );

        return uv;
    }

    public void Clear()
    {
        if (renderTexture == null)
        {
            return;
        }
        RenderTexture tempRT = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height,  0, RenderTextureFormat.ARGB32);
        //  Graphics.Blit(null, tempRT);
        Graphics.Blit(tempRT, renderTexture, drawMaterial, 1);
        RenderTexture.ReleaseTemporary(tempRT);
    }
    // return the united image to "RawImage"
    public Texture2D CropCurrentTexture()
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
    
    public Texture2D CropCurrentTexture_InvertMask()
    {
        if (renderTexture!= null)
        {
            var rtTmp = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 0, RenderTextureFormat.ARGB32, 0);
            Graphics.Blit(renderTexture, rtTmp, drawMaterial, 2);
            
            Texture2D texture2D = new Texture2D(rtTmp.width, rtTmp.height, TextureFormat.RGBA32, false);
            RenderTexture.active = rtTmp;
            texture2D.ReadPixels(new Rect(0, 0, rtTmp.width, rtTmp.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = null;
            
            RenderTexture.ReleaseTemporary(rtTmp);
            
            return texture2D;
        }
        // return the united image to "RawImage"
        return default;
    }
}