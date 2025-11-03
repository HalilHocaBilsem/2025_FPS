using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool ArabadaMi = false;


    //Player isimli kapsüle eklediğimiz CharacterController'a ulaşmak için bir değişken oluştur
    CharacterController karakter = null;
    //Yürüme hızı
    [SerializeField]
    float yurumeHizi = 6f;
    //mouse (fare) hassasiyeti. Bunu artırırsak fare hızı artar.
    [SerializeField]
    float fareHassasiyet = 3f;
    //yer çekimi miktarı
    [SerializeField]
    float yerCekimi = -8f;

    //karakterin Y yönündeki hareket miktarını belirten değişken
    float dikeyHareketMiktar = 0;
    //ziplama gücü [SerializeField] bunun editör ekranından düzenlenmesini sağlar.
    [SerializeField]
    float ziplamaGucu = 2;
    float xRotation;
    public float maxSlopeAngle = 45f; // degrees

    //Bu metot oyun başladığında 1 kez çalışır.
    void Start()
    {
        //CharacterController komponentini al.
        karakter = GetComponent<CharacterController>();
        dikeyHareketMiktar = yerCekimi;

        Cursor.lockState = CursorLockMode.Locked;
    }



    //Bu metot ekran her çizildiğinde çalıştırılır.
    //Youn 100 FPS ile çalışıyor ise bu metot saniyede 100 kez çalışır.
    void Update()
    {
        //=== CAMERA LOOK ===//
        float mouseX = Input.GetAxis("Mouse X") * fareHassasiyet;
        float mouseY = Input.GetAxis("Mouse Y") * fareHassasiyet * -1;

        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -60, 60);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);




        transform.Rotate(0, mouseX, 0); // Horizontal rotation (player)
                                        // Vertical rotation (camera)



        if (ArabadaMi)
        {
            return;
        }

        //=== MOVEMENT INPUT ===//
        Vector3 hareketYonu = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            hareketYonu += transform.forward;
        if (Input.GetKey(KeyCode.S))
            hareketYonu -= transform.forward;
        if (Input.GetKey(KeyCode.D))
            hareketYonu += transform.right;
        if (Input.GetKey(KeyCode.A))
            hareketYonu -= transform.right;

        int hizCarpani = Input.GetKey(KeyCode.LeftShift) ? 4 : 1;
        Vector3 yatayHareket = hareketYonu.normalized * yurumeHizi * hizCarpani;

        //=== GRAVITY & JUMP ===//
        if (karakter.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && CanMoveOnSlope())
            {
                dikeyHareketMiktar = ziplamaGucu;
            }
            else
            {
                dikeyHareketMiktar = -1f; // small downward force to keep grounded
            }
        }
        else
        {
            dikeyHareketMiktar += yerCekimi * Time.deltaTime;
        }

        Vector3 dikeyHareket = Vector3.up * dikeyHareketMiktar;

        //=== FINAL MOVE ===//
        Vector3 toplamHareket = (yatayHareket + dikeyHareket) * Time.deltaTime;
        karakter.Move(toplamHareket);
    }


    bool CanMoveOnSlope()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float rayDistance = 1.5f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            return angle <= maxSlopeAngle;
        }

        // If nothing is hit, allow movement (e.g., falling in air)
        return false;
    }

}
