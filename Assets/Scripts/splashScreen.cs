using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashScreen : MonoBehaviour
{
    public float tiempoEspera = 1f;

    void Start()
    {
        StartCoroutine(saltarAnimacion());
    }

    private IEnumerator saltarAnimacion()
    {
        yield return new WaitForSeconds(tiempoEspera);
        SceneManager.LoadScene("MenuPpal");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MenuPpal");
        } 
    }
}
