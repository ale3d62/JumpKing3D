using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class comportamientoMenuJuego : MonoBehaviour
{
    public MovementScript scriptMovimiento;
    public GameObject menu;
    public GameObject botones;
    public Text tiempo;
    public Text saltos;
    public Text caidas;
    private int index = 0;
    private int numSaltos = 0;
    private int numCaidas = 0;
    private float tiempoJugado;
    private bool menuActivado = false;
    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

    }

    // Update is called once per frame
    void Update()
    {
        tiempoJugado += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape)){           
            menuActivado = !menuActivado; // Desactivo menu
            
            if(menuActivado){
                index = 0; // Activo solo botón de reanudar
                botones.transform.GetChild(0).gameObject.SetActive(true);
                botones.transform.GetChild(1).gameObject.SetActive(false);
                botones.transform.GetChild(2).gameObject.SetActive(false);
                // Comportamiento de menu tiempo etc

                ActualizarTextoTiempo();
                caidas.text = "CAIDAS: "+scriptMovimiento.numCaidas;
                saltos.text = "SALTOS: "+scriptMovimiento.numSaltos;
            }

            menu.transform.GetChild(0).gameObject.SetActive(menuActivado);
            menu.transform.GetChild(1).gameObject.SetActive(menuActivado);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && menuActivado)
        {
            if(index == 2){
                botones.transform.GetChild(index).gameObject.SetActive(false);
                index = 0;
                botones.transform.GetChild(index).gameObject.SetActive(true);
            }else{
                botones.transform.GetChild(index).gameObject.SetActive(false);
                index += 1;
                botones.transform.GetChild(index).gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && menuActivado)
        {
            if(index == 0){
                botones.transform.GetChild(index).gameObject.SetActive(false);
                index = 2;
                botones.transform.GetChild(index).gameObject.SetActive(true);
            }else{
                botones.transform.GetChild(index).gameObject.SetActive(false);
                index -= 1;
                botones.transform.GetChild(index).gameObject.SetActive(true);
            }
        }

        if(Input.GetKeyDown(KeyCode.Return) && menuActivado){
            switch (index)
            {
                case 0:
                    menuActivado = !menuActivado;
                    menu.transform.GetChild(0).gameObject.SetActive(menuActivado);
                    menu.transform.GetChild(1).gameObject.SetActive(menuActivado);
                    break;
                case 1:                    
                    break;
                case 2:
                    SceneManager.LoadScene("MenuPpal");
                    break;
                
                default:
                    break;
            }
        }  
    }

    private void ActualizarTextoTiempo()
    {
        int horas = (int)tiempoJugado / 3600;
        int minutos = ((int)tiempoJugado % 3600) / 60;
        int segundos = ((int)tiempoJugado % 3600) % 60;

        tiempo.text = string.Format("Tiempo: {0}H {1}M {2}S", horas, minutos, segundos);
    }

    public void CambiarFullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void CambiarBorderless()
    {
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }

    public void Salir()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
