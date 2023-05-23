using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOutTransicion : MonoBehaviour
{
    // Start is called before the first frame update
    public float tiempoEspera = 1f;

    void Start()
    {
        StartCoroutine(saltarAnimacion());
    }

    private IEnumerator saltarAnimacion()
    {
        yield return new WaitForSeconds(tiempoEspera);
        SceneManager.LoadScene("Juego");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Juego");
        } 
    }
}
