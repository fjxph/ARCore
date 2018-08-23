using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using GoogleARCoreInternal;
public class Test : MonoBehaviour
{
    private Vector2 ScreenCenter;
    private TrackableHit FirstHit;
    private bool m_IsSetFirst = false;
    private TrackableHit SecondHit;
    private bool m_IsSetSecond = false;
    private TrackableHit CurHit;

    public Text LogText;
    public Button Set;
    public LineRenderer Line;
    public Text DistenceText;
    // Use this for initialization
    void Start()
    {
        ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Set.onClick.AddListener(SetPoint);
    }
    void OnDestroy()
    {
        Set.onClick.RemoveListener(SetPoint);
    }
    // Update is called once per frame
    void Update()
    {
        LogText.text = "色彩校正：" + Frame.LightEstimate.ColorCorrection;
        LogText.text += "\n像素强度：" + Frame.LightEstimate.PixelIntensity;
        LogText.text += "\n光估计值：" + Frame.LightEstimate.State.ToString();
        //Debug.Log("色彩校正：" + Frame.LightEstimate.ColorCorrection);
        //Debug.Log("像素强度：" + Frame.LightEstimate.PixelIntensity);
        //Debug.Log("光估计值：" + Frame.LightEstimate.State.ToString());
        if (m_IsSetFirst)
        {
            if (Frame.Raycast(ScreenCenter.x, ScreenCenter.y, TrackableHitFlags.PlaneWithinPolygon, out CurHit))
            {
                Line.SetPosition(1, CurHit.Pose.position);
                DistenceText.text = Vector3.Distance(FirstHit.Pose.position, CurHit.Pose.position).ToString();
            }
        }
        DistenceText.transform.position = (Camera.main.WorldToScreenPoint(Line.GetPosition(0) + Line.GetPosition(1))) / 2 + new Vector3(0, 0.02f, 0);
    }

    void SetPoint()
    {
        if (Frame.Raycast(ScreenCenter.x, ScreenCenter.y, TrackableHitFlags.PlaneWithinPolygon, out CurHit))
        {
            if (!m_IsSetFirst)
            {
                FirstHit = CurHit;
                m_IsSetFirst = true;
                Line.SetPosition(0, FirstHit.Pose.position);
                LogText.text = "添加第1个点,这个点的位置信息为" + FirstHit.Pose.position + "\n";
            }
            else
            {
                SecondHit = CurHit;
                Line.SetPosition(1, SecondHit.Pose.position);
                m_IsSetFirst = false;
                LogText.text += "添加第2个点,这个点的位置信息为" + SecondHit.Pose.position + "\n";
                LogText.text += "两个点的距离为" + Vector3.Distance(FirstHit.Pose.position, SecondHit.Pose.position);
            }
        }
    }
}
