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
    public string[] textos;
    private GameObject[] textosObj;
    public GameObject puntero;
    public List<string> Seleccionadas;

    [Header("Juego")] public GameObject Respuesta;

    [Header("Abcedario")] public GameObject Content;
    public List<GameObject> Letras = new List<GameObject>();
    public GameObject LetraPrefab;
    public bool visible;
    public GameObject Panel;
    
    // Start is called before the first frame update
    void Start()
    {
        _rotationSpeed = RotationSpeed;
        Giro = false;
        visible = true;
        textosObj = GameObject.FindGameObjectsWithTag("ElementoRuleta");
        for (int i = 0; i < textosObj.Length; i++)
        {
            textosObj[i].GetComponent<Text>().text = textos[i];
        }
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
                foreach (GameObject txt in textosObj)
                {
                    if (Vector3.Distance(txt.transform.position, puntero.transform.position) < dist)
                    {
                        dist = Vector3.Distance(txt.transform.position, puntero.transform.position);
                        select = txt.GetComponent<Text>().text;
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

    public void VerificarLetra()
    {
        if (!string.IsNullOrEmpty(Respuesta.GetComponent<Text>().text))
        {
            if (!Seleccionadas.Contains(Respuesta.GetComponent<Text>().text))
            {
                Seleccionadas.Add(Respuesta.GetComponent<Text>().text);
                print("Se verifica la letra: "+Respuesta.GetComponent<Text>().text);
            }
            else
            {
                print("Letra ya seleccionada");
            }
        }
    }

    public void VerLetras()
    {
        visible = !visible;
        if (!visible)
        {
            Panel.SetActive(true);
            foreach (string letra in Seleccionadas)
            {
                var tempLetra = Instantiate(LetraPrefab);
                //Parent to the panel
                tempLetra.transform.SetParent(Content.transform);
                tempLetra.GetComponent<Text>().text = letra.ToUpper();
                
                Letras.Add(tempLetra);
            }

            visible = false;
        }
        else
        {
            Panel.SetActive(false);
            if (Letras != null)
            {
                foreach (var score in Letras)
                {
                    Destroy(score);
                }
            }
            visible = true;
        }
    }
    
}
