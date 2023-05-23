using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementScript : MonoBehaviour
{
    public AudioSource audioJump;
    public AudioSource audioLand;
    public AudioSource audioBump;
    public AudioSource audioSplat;
    public float jumpHorSpeed = 165;
    public float walkingSpeed = 1.5f;
    public float fallHeight = 193;
    [SerializeField]
    private Transform cameraTransform;
    public bool isGrounded;
    private Rigidbody rb;
    private float jumpValue = 0.0f;
    private float lastYValue;
    private Vector3 lastVelocity;
    private Animator animator;
    private bool lastGrounded = false;
    public int numSaltos = 0;
    public int numCaidas = 0;
    private bool pulsaE = false;
    private bool pausa = false;
    private bool pulsaEsc = false;
    private bool cameraLocked = true;
    private float lastCameraPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
        lastYValue = transform.position.y;
        animator = GetComponent<Animator>();
        lastCameraPosition = cameraTransform.rotation.eulerAngles.y;
        rb.freezeRotation = true;
    }
    
    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && !pulsaEsc){
            pulsaEsc = true;
            pausa = !pausa;
        }
        if(Input.GetKeyUp(KeyCode.Escape)){
            pulsaEsc = false;
        }

        Vector3 movementDirection;

        //Si estamos en pausa no hacemos nada mas
        if(pausa){
            transform.rotation = Quaternion.Euler(rb.rotation.x, lastCameraPosition, rb.rotation.z);
            movementDirection = Quaternion.AngleAxis(lastCameraPosition, Vector3.up) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            return;
        }

        
        if(Input.GetKeyDown(KeyCode.E) && !pulsaE){
            if(cameraLocked)
                lastCameraPosition = cameraTransform.rotation.eulerAngles.y;
            cameraLocked = !cameraLocked;
        }
        if(Input.GetKeyUp(KeyCode.E)){
            pulsaE = false;
        }


        if(cameraLocked){
            transform.rotation = Quaternion.Euler(rb.rotation.x, cameraTransform.rotation.eulerAngles.y, rb.rotation.z);
            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
        else{
            transform.rotation = Quaternion.Euler(rb.rotation.x, lastCameraPosition, rb.rotation.z);
            movementDirection = Quaternion.AngleAxis(lastCameraPosition, Vector3.up) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
            
        
        //Movimiento en el suelo
        if((Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && isGrounded && jumpValue == 0.0f){
            animator.SetBool("Caminando", true);
            //reducir velocidad al andar en diagonal
            if((Input.GetKey("w") && Input.GetKey("a")) || (Input.GetKey("w") && Input.GetKey("d")) || (Input.GetKey("s") && Input.GetKey("a")) || (Input.GetKey("s") && Input.GetKey("d")))
                transform.position += movementDirection * walkingSpeed * Mathf.Sqrt(0.5f); //velocidad suelo reducida
            else
                transform.position += movementDirection * walkingSpeed; //velocidad suelo normal
                
        }
        //Desactivar animacion de caminar
        else{
            animator.SetBool("Caminando", false);
        }

        //Al presionar espacio en el suelo para saltar
        if (Input.GetKey("space") && isGrounded){
            animator.SetBool("cargandoSalto", true);
            jumpValue += 10.41f; //velocidad de barra de salto
        }
        
        //Cuando pasa de estar en el suelo a no estarlo
        if(!isGrounded && lastGrounded){
            //preservar velocidad de caminar al dejarse caer de un borde
            if(jumpValue==0){
                if((Input.GetKey("w") && Input.GetKey("a")) || (Input.GetKey("w") && Input.GetKey("d")) || (Input.GetKey("s") && Input.GetKey("a")) || (Input.GetKey("s") && Input.GetKey("d")))
               movementDirection *= Mathf.Sqrt(0.5f);   //reducir velocidad en diagonal

                Vector3 velocity = movementDirection * jumpHorSpeed;
                rb.velocity = new Vector3(velocity.x, jumpValue, velocity.z);
            }
            lastYValue = rb.position.y;
        }
        //Cuando pasa de no estar en el suelo a estarlo
        if(isGrounded && !lastGrounded){
            audioLand.Play();
        
        }
            

        
        if (jumpValue >= 500f && isGrounded){ //tope de la barra de salto
            numSaltos++;
            audioJump.Play();
            Vector3 velocity = new Vector3(0, 0, 0);
            if (Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("d"))  //Si solo se esta presionando el boton de caminar hacia delante, se preserva la velocidad en los ejes 'x' y 'z'
            {
                velocity = movementDirection * jumpHorSpeed;
            }
            rb.velocity = new Vector3(velocity.x, 500f, velocity.z);
            Invoke("ResetJump", 0.2f);
        }


        if (Input.GetKeyUp("space")){    //Al soltar espacio sin haber cargado la barra al maximo
            if (isGrounded)     //Si esta en el suelo podra saltar
            {
                numSaltos++;
                audioJump.Play();
                Vector3 velocity = new Vector3(0, 0, 0);
                if (Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("d"))  //Si solo se esta presionando el boton de caminar hacia delante, se preserva la velocidad en los ejes 'x' y 'z'
                {
                    velocity = movementDirection * jumpHorSpeed;
                }
                rb.velocity = new Vector3(velocity.x, jumpValue, velocity.z);
                Invoke("ResetJump2", 0.2f);
            }
        }
        lastGrounded = isGrounded;
    }
    
    private void FixedUpdate(){
        lastVelocity = rb.velocity;
    }
    
    void ResetJump(){
        jumpValue = 0.0f;
    }

    void ResetJump2(){
        jumpValue = 0.0f;
    }

    //Devuelve True si la colision es con suelo y false si es con pared
    bool DetectaSuelo_Pared(Collision collision){
        var renderer = GetComponent<Renderer>();

        var bottom = renderer.bounds.center;
        bottom.y -= renderer.bounds.extents.y;
        float minDist = float.PositiveInfinity;
        float angle = 180f;
        // Find closest point to bottom.
        for (int i = 0; i < collision.contactCount; i++)
        {
            var contact = collision.GetContact(i);
            var tempDist = Vector3.Distance(contact.point, bottom);
            if (tempDist < minDist)
            {
                minDist = tempDist;
                // Check how close the contact normal is to our up vector.
                angle = Vector3.Angle(transform.up, contact.normal);
            }
        }
        // Check if the angle is too steep.
        if (angle <= 45f)
            return true;
        else 
            return false;
    }

    //Comprobación de si está en el suelo
    void OnCollisionStay(Collision collision)
    {
        if(DetectaSuelo_Pared(collision))
        {
            if (!isGrounded)
            {
                animator.SetBool("cargandoSalto", false);
            }
            isGrounded = true;
        }
        else{
            if(!isGrounded)
                audioBump.Play();
            isGrounded = false;
        }
    }


    void OnCollisionExit(Collision collision){
        isGrounded = false;
    }
    
    //Al colisionar
    private void OnCollisionEnter(Collision other){
        //Rebote con paredes
        if(!DetectaSuelo_Pared(other)){
            //It will only rebound if the player is not grounded
            var normalVector = other.contacts[0].normal;
            var speed = lastVelocity.magnitude;
            if (!(Mathf.Abs(normalVector.x) <= 0.01f && Mathf.Abs(normalVector.z) <= 0.01f)){
                var direction = Vector3.Reflect(lastVelocity.normalized, normalVector);
                rb.velocity = new Vector3(direction.x*speed/2, direction.y*speed, direction.z*speed/2);
            }
        }
        //Colision con el suelo
        else{
            //caida
            if(lastYValue-rb.position.y >= fallHeight){
                numCaidas++;
                audioSplat.Play();
                lastYValue = rb.position.y;
            }     
        }
    }
    
    //Al minimizar el juego
    private void OnApplicationFocus(bool focusStatus){
        if (focusStatus)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}