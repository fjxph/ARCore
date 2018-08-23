using System.Collections.Generic;
using System.IO;
using Baidu.Aip.Face;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class BaiduAITest : MonoBehaviour
{
    private string APP_ID = "11651867";
    private string API_KEY = "PnIgyVIuSBfRn6k5VYLDbbFl";
    private string SECRET_KEY = "DRZrvVXj7Www0XOt88PuASCHHANs2gql";

    private string ImageURL = "/1.jpg";

    private Face client;
    private string image;
    private string imageType = "BASE64";
    private Dictionary<string, object> options;
    public Text text;
    private Texture2D texture;
    private RenderTexture rt;
    int num = 0;  //截图计数

    private float time;
    private void Awake()
    {
        ////rt = new RenderTexture(Screen.width, Screen.height, 24);
        //rt = Camera.main.targetTexture;
        //texture = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        //Camera.main.targetTexture = rt;
        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
               delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                           System.Security.Cryptography.X509Certificates.X509Chain chain,
                           System.Net.Security.SslPolicyErrors sslPolicyErrors)
               {
                   return true; // **** Always accept
               };

        client = new Face(API_KEY, SECRET_KEY);
        Debug.Log("new Face");
        options = new Dictionary<string, object>()
        {
            {"face_field", "age"},
            {"max_face_num", 2},
            {"face_type", "LIVE"}
        };
    }
    void Start()
    { }
    public void ImgToBase64String(string path)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            string base64String = Convert.ToBase64String(buffer);
            Debug.Log("获取当前图片base64为---" + base64String);
            text.text = "获取当前图片base64成功\r";
            image = base64String;
        }
        catch (Exception e)
        {
            Debug.Log("ImgToBase64String 转换失败:" + e.Message);
        }
    }
    IEnumerator AndroidImgToBase64String(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.error == null)
        {
            string base64String = Convert.ToBase64String(www.texture.EncodeToPNG());
            Debug.Log("获取当前图片base64为---" + base64String);
            text.text = "获取当前图片base64成功\r";
            image = base64String;
        }
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time < 5)
            return;
        time = 0;
        //将图片保存起来
        image = Convert.ToBase64String(CaptureCamera(Camera.main,new Rect(0,0,256,256)));
        try
        {
            var result = client.Detect(image, imageType, options);
            text.text = result.ToString();
        }
        catch (Exception e)
        {
            text.text += e;
            throw;
        }
    }
    byte[] CaptureCamera(Camera camera, Rect rect)
    {
        // 创建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.targetTexture = rt;
        camera.Render();
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
        //ps: camera2.targetTexture = rt;  
        //ps: camera2.Render();  
        //ps: -------------------------------------------------------------------  

        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        GameObject.Destroy(rt);
        // 最后将这些纹理数据，成一个png图片文件  
        byte[] bytes = screenShot.EncodeToPNG();
        Debug.Log(string.Format("截屏了一张照片"));

        return bytes;
    }
}
