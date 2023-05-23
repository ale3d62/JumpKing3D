using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class comportamientoMenu : MonoBehaviour
{
    public AudioSource confirm;
    public GameObject botones;
    public GameObject subMenu;
    public GameObject subBotones;
    public GameObject subMenuOp;
    public GameObject subBotonesOpciones;
    private int index = 0;
    private int index2 = 0;
    private int index3 = 0;
    private bool subMenuAbierto = false;
    private bool subMenuOpciones = false;
    private bool enterPulsado = false;

    // Start is called before the first frame update
    void Start()
    {
        //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    // Update is called once per frame
    void Update()
    {
        if (!subMenuAbierto && !subMenuOpciones)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(index == 3){
                botones.transform.GetChild(index).gameObject.SetActive(false);
                index = 0;
                botones.transform.GetChild(index).gameObject.SetActive(true);
            }else{
                botones.transform.GetChild(index).gameObject.SetActive(false);
                index += 1;
                botones.transform.GetChild(index).gameObject.SetActive(true);
            }
        }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            { 
                if(index == 0){
                    botones.transform.GetChild(index).gameObject.SetActive(false);
                    index = 3;
                    botones.transform.GetChild(index).gameObject.SetActive(true);
                }else{
                    botones.transform.GetChild(index).gameObject.SetActive(false);
                    index -= 1;
                    botones.transform.GetChild(index).gameObject.SetActive(true);
                }
            }

            if(Input.GetKeyDown(KeyCode.Return)){
                confirm.Play();
                switch (index)
                {
                    case 0:
                        SceneManager.LoadScene("EscenaTransicion");
                        break;
                    case 1:
                        index3 = 0;
                        subBotonesOpciones.transform.GetChild(0).gameObject.SetActive(true);
                        subBotonesOpciones.transform.GetChild(1).gameObject.SetActive(false);
                        subBotonesOpciones.transform.GetChild(2).gameObject.SetActive(false);
                        subMenuOp.SetActive(true);
                        subMenuOpciones = true;
                        enterPulsado = true;
                        break;

                    case 2:                    
                        index2 = 0;
                        subBotones.transform.GetChild(0).gameObject.SetActive(true);
                        subBotones.transform.GetChild(1).gameObject.SetActive(false);
                        subBotones.transform.GetChild(2).gameObject.SetActive(false);
                        subMenu.SetActive(true);
                        subMenuAbierto = true;
                        enterPulsado = true;
                        break;
                    case 3:
                        Salir();
                        break;
                    
                    default:
                        break;
                }
            }

        }
       
        // Submenu Opciones
        if (subMenuOpciones)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(index3 == 2){
                    subBotonesOpciones.transform.GetChild(index3).gameObject.SetActive(false);
                    index3 = 0;
                    subBotonesOpciones.transform.GetChild(index3).gameObject.SetActive(true);
                }else{
                    subBotonesOpciones.transform.GetChild(index3).gameObject.SetActive(false);
                    index3 += 1;
                    subBotonesOpciones.transform.GetChild(index3).gameObject.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(index3 == 0){
                    subBotonesOpciones.transform.GetChild(index3).gameObject.SetActive(false);
                    index3 = 2;
                    subBotonesOpciones.transform.GetChild(index3).gameObject.SetActive(true);
                }else{
                    subBotonesOpciones.transform.GetChild(index3).gameObject.SetActive(false);
                    index3 -= 1;
                    subBotonesOpciones.transform.GetChild(index3).gameObject.SetActive(true);
                }
            }

            if(Input.GetKeyDown(KeyCode.Return) && !enterPulsado){
                confirm.Play();
                switch (index3)
                {
                    case 0:
                        CambiarFullScreen();
                        break;
                    case 1:
                        CambiarBorderless();
                        break;
                    case 2:
                        CambiarWindowed();
                        break;
                    
                    default:
                        break;
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape)){
                subMenuOpciones = false;
                subMenuOp.SetActive(false);
            }
        }

        // Submenu
        if (subMenuAbierto)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(index2 == 2){
                    subBotones.transform.GetChild(index2).gameObject.SetActive(false);
                    index2 = 0;
                    subBotones.transform.GetChild(index2).gameObject.SetActive(true);
                }else{
                    subBotones.transform.GetChild(index2).gameObject.SetActive(false);
                    index2 += 1;
                    subBotones.transform.GetChild(index2).gameObject.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(index2 == 0){
                    subBotones.transform.GetChild(index2).gameObject.SetActive(false);
                    index2 = 2;
                    subBotones.transform.GetChild(index2).gameObject.SetActive(true);
                }else{
                    subBotones.transform.GetChild(index2).gameObject.SetActive(false);
                    index2 -= 1;
                    subBotones.transform.GetChild(index2).gameObject.SetActive(true);
                }
            }

            if(Input.GetKeyDown(KeyCode.Return) && !enterPulsado){
                confirm.Play();
                switch (index2)
                {
                    case 0:
                        SceneManager.LoadScene("EscenaBicho");
                        break;
                    case 1:
                        SceneManager.LoadScene("CancionBicho");
                        break;
                    case 2:
                        SceneManager.LoadScene("Creditos");
                        break;
                    
                    default:
                        break;
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape)){
                subMenuAbierto = false;
                subMenu.SetActive(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            enterPulsado = false;
        }
    }

    public void CambiarFullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        //Debug.Log("Hijoputa");
    }

    public void CambiarBorderless()
    {
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void CambiarWindowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
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
