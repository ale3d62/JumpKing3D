using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class teclaSpace : MonoBehaviour
{
    public GameObject[] objetos; // Array que contiene los objetos a activar/inactivar
    public AudioSource start; // Audio de start
    public Text texto; // Array que contiene los objetos a activar/inactivar
    private bool objetosActivados = false; // Variable booleana para controlar el estado de los objetos
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivarDesactivarGameObject());
    }

    private IEnumerator ActivarDesactivarGameObject()
    {
        while (true)
        {
            //Debug.Log(objetosActivados);
            texto.enabled = !texto.enabled;
            yield return new WaitForSeconds(0.5f);

            if(objetosActivados){
                texto.enabled = false;
                break;
            }        
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!objetosActivados)
            {
                // Activar los objetos
                foreach (GameObject objeto in objetos)
                {
                    objeto.SetActive(true);
                }
                objetosActivados = true;
                start.Play();
            }
        } 
    }
}
