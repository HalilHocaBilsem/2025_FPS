using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    public WheelCollider SolOnCollider;
    public WheelCollider SagOnCollider;
    public WheelCollider SagArkaCollider;
    public WheelCollider SolArkaCollider;


    public Transform SolOnTeker;
    public Transform SagOnTeker;
    public Transform SolArkaTeker;
    public Transform SagArkaTeker;


    public float MotorGuc;
    public float FrenGuc;

    public Transform OyuncuYeri;

    Player Oyuncu;
    CharacterController oyuncuCh;

    public float maxDonus = 45f;   // Maksimum direksiyon açısı


    public AudioClip MotorSesi;
    public AudioClip FrenSesi;

    AudioSource sesCalar;
    public float minPitch = 0.8f;
    public float maxPitch = 4.0f;
    float maxSpeed = 90;
    public Light solFar;
    public Light sagFar;
    Rigidbody carRigidbody;

    void Start()
    {
        Oyuncu = GameObject.FindAnyObjectByType<Player>();
        oyuncuCh = Oyuncu.gameObject.GetComponent<CharacterController>();
        sesCalar = GetComponent<AudioSource>();
        carRigidbody = GetComponent<Rigidbody>();

   
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Oyuncu.ArabadaMi)
            {
                Oyuncu.gameObject.transform.parent = null;
                Oyuncu.ArabadaMi = false;
                oyuncuCh.enabled = true;
                sesCalar.Stop();
                solFar.enabled = sagFar.enabled = false;
                SolArkaCollider.brakeTorque = SagArkaCollider.brakeTorque = SagOnCollider.brakeTorque = SolOnCollider.brakeTorque = FrenGuc;

            }
            else
            {
                if (Vector3.Distance(this.transform.position, Oyuncu.transform.position) < 5)
                {
                    oyuncuCh.enabled = false;
                    Oyuncu.gameObject.transform.parent = OyuncuYeri;
                    Oyuncu.ArabadaMi = true;
                    Oyuncu.gameObject.transform.position = OyuncuYeri.position;
                    sesCalar.clip = MotorSesi;
                    sesCalar.loop = true;
                    sesCalar.pitch = minPitch;
                    sesCalar.Play();
                    solFar.enabled = sagFar.enabled = true;
                }


            }
        }



        if (Oyuncu.ArabadaMi)
        {
            if (Input.GetKey(KeyCode.W))
            {
                SolArkaCollider.brakeTorque = SagArkaCollider.brakeTorque = SagOnCollider.brakeTorque = SolOnCollider.brakeTorque = 0;
                SagOnCollider.motorTorque = SolOnCollider.motorTorque = MotorGuc;

            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SagOnCollider.motorTorque = SolOnCollider.motorTorque = 0;
                SolArkaCollider.brakeTorque = SagArkaCollider.brakeTorque=    SagOnCollider.brakeTorque = SolOnCollider.brakeTorque = FrenGuc;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                SagOnCollider.motorTorque = SolOnCollider.motorTorque = MotorGuc * -1;
                SolArkaCollider.brakeTorque = SagArkaCollider.brakeTorque = SagOnCollider.brakeTorque = SolOnCollider.brakeTorque = 0;
            }


            if (Input.GetKeyUp(KeyCode.S)|| Input.GetKeyUp(KeyCode.W))
            {
                SagOnCollider.motorTorque = SolOnCollider.motorTorque = 0;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                SolArkaCollider.brakeTorque = SagArkaCollider.brakeTorque = SagOnCollider.brakeTorque = SolOnCollider.brakeTorque = 0;
                SagOnCollider.motorTorque = SolOnCollider.motorTorque = 1;
            }

            var steerInput = Input.GetAxis("Horizontal"); // A (-1) ve D (+1)
            float steerAngle = steerInput * maxDonus;

            SolOnCollider.steerAngle = SagOnCollider.steerAngle = steerAngle;

            float speed = carRigidbody.velocity.magnitude; // m/s
            float normalizedSpeed = Mathf.InverseLerp(0, maxSpeed, speed);
            sesCalar.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);

        }




        TekerlekleriDondur(SolOnCollider, SolOnTeker);
        TekerlekleriDondur(SagOnCollider, SagOnTeker);
        TekerlekleriDondur(SolArkaCollider, SolArkaTeker);
        TekerlekleriDondur(SagArkaCollider, SagArkaTeker);
    }


    void TekerlekleriDondur(WheelCollider col, Transform tekerlek)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        tekerlek.position = pos;
        tekerlek.rotation = rot;
    }
}
