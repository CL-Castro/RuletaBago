using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class GameController : MonoBehaviour
{
    [Header("Configuracion Ruleta")] public Image ruleta;
    public float RotationSpeed;
    private float _rotationSpeed;
    public bool Giro;
    public Text[] textos;
    public GameObject puntero;
    
    // Start is called before the first frame update
    void Start()
    {
        _rotationSpeed = RotationSpeed;
        Giro = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Giro)
        {
            ruleta.transform.Rotate(Vector3.forward * ( RotationSpeed * Time.deltaTime ));
            StartCoroutine(girando());
            if (RotationSpeed <= 0)
            {
                Giro = false;
                print("Se detuvo");
                float dist = Int32.MaxValue;
                string select = ""; 
                foreach (Text txt in textos)
                {
                    if (Vector3.Distance(txt.transform.position, puntero.transform.position) < dist)
                    {
                        dist = Vector3.Distance(txt.transform.position, puntero.transform.position);
                        select = txt.text;
                    }
                }
                print("Seleccion -> "+select);
                RotationSpeed = _rotationSpeed;
            }
        }
        else
        {
            RotationSpeed = _rotationSpeed;
        }
    }

    public void Girar()
    {
        if (!Giro)
            Giro = true;
    }

    IEnumerator girando()
    {
        yield return new WaitForSeconds(1f);
        RotationSpeed -= 0.5f;
    }
    
    
}
