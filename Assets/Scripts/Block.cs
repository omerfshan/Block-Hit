using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    private int count; // Blok sayısı
    private Text countText; // Blok için bağlı Text bileşeni
    private AudioSource bounceSound;

    private void Awake() {
        bounceSound=GameObject.Find("BounceSound").GetComponent<AudioSource>();
    }
    void Start()
    {
        // Blok içindeki Canvas'ın altındaki Text bileşenini bul
        countText = GetComponentInChildren<Text>();
        countText.text=count.ToString();
        if (countText == null)
        {
            Debug.LogError("Text bileşeni bulunamadı! Blok: " + gameObject.name);
        }
    }

    void Update()
    {
        // Blok aşağı düştüyse yok et
        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }

    public void SetStartingCount(int count)
    {
        this.count = count;
        if (countText != null)
        {
            countText.text = count.ToString();
            Debug.Log($"Text güncellendi: {countText.text}"); // Kontrol için
        }
        else
        {
            // Debug.LogError("countText atanmadı!");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name == "Ball" && count > 0)
        {
            count--;
            if (countText != null)
            {
                countText.text = count.ToString();
                bounceSound.Play();
                // Debug.Log($"Çarpışma sonrası Text: {countText.text}");
            }

            // Kamera titretme işlemi
            Camera.main.GetComponent<CameraTransitions>().Shake();

            if (count == 0)
            {
                Destroy(gameObject);
                Camera.main.GetComponent<CameraTransitions>().MediumShake();
                GameObject.Find("ExtraBallProgress").GetComponent<Progress>().IncreaseCurrentWidth();
            }
        }
    }
}
