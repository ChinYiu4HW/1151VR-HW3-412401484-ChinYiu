using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Roulettecontroller : MonoBehaviour
{
    private float rotSpeed = 0f;
    public float deceleration = 0.985f; // 減速

    [Header("音效 + VFX")]
    public AudioSource audioSource;
    public AudioSource congratsAudio;
    public ParticleSystem stopEffect1;
    public ParticleSystem stopEffect2;
    public ParticleSystem stopEffect3;

    [Header("UI 文字")]
    public TextMeshProUGUI resultText; 
    private string[] prizes = { "滷肉飯", "珍珠奶茶", "小籠包", "臭豆腐", "鹽酥雞", "車輪餅", "芒果冰", "大雞排" }; 

    private bool isSpinning = false;
    private float initialMaxSpeed = 10f;

    void Start()
    {
        Application.targetFrameRate = 60;
        if (resultText != null)
        {
            resultText.text = "點擊開始轉動 ! ";
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isSpinning)
            {
                
                isSpinning = true;
                rotSpeed = initialMaxSpeed;

                if (audioSource != null)
                {
                    audioSource.volume = 1.0f;
                    audioSource.Play();
                }

                if (stopEffect1 != null) stopEffect1.Stop();
                if (stopEffect2 != null) stopEffect2.Stop();
                if (stopEffect3 != null) stopEffect3.Stop();
            }
            else
            {
                isSpinning = false;
            }
        }

        
        if (!isSpinning && rotSpeed > 0)
        {
            rotSpeed *= deceleration;

            //音效音量隨著速度變慢而跟著變小聲
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.volume = Mathf.Clamp01(rotSpeed / initialMaxSpeed);
            }

            //轉盤停下來時播放恭喜音效
            if (congratsAudio != null)
            {
                congratsAudio.Play();
            }


            if (rotSpeed < 0.01f)
            {
                rotSpeed = 0f;
                if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
                if (stopEffect1 != null) stopEffect1.Play();
                if (stopEffect2 != null) stopEffect2.Play();
                if (stopEffect3 != null) stopEffect3.Play();

                ShowResult();
            }
        }

        transform.Rotate(0, 0, this.rotSpeed);
    }

    void ShowResult()
    {
        if (resultText == null) return;

        float currentAngle = transform.eulerAngles.z;
        float offsetAngle = 5f; 
        float adjustedAngle = (currentAngle - offsetAngle) % 360;
        if (adjustedAngle < 0) adjustedAngle += 360;

        int prizeIndex = Mathf.FloorToInt(adjustedAngle / (360f / prizes.Length));
        resultText.text = "今晚吃" + prizes[prizeIndex];
    }




}