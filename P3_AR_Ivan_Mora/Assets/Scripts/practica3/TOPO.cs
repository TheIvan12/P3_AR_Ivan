using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum EstadoTopo {INACTIVO , APARECE, ESPERA, DESAPARECE};

public class TOPO : MonoBehaviour
{
    public EstadoTopo estado =  EstadoTopo.INACTIVO;

    public float tAparece;
    public float tEspera;
    public float tDesaparece;

    public Vector3 escalaMin = Vector3.zero;
    public Vector3 escalaMax = new Vector3(1f, 1f, 1f);

    private float timer = 0f;

    public float zDescubierto;
    public float zEnterrado;

    private Gestortopos gt;

    public AudioClip sfxAcierto;
    public AudioClip sfxFallo;


    public GameObject spriteAcierto;
    public GameObject spritefallo;

    public ParticleSystem TopoParticula;

    // Start is called before the first frame update
    void Start()
    {
        // guardamos referencia al gestor para abreviar el codigo 
        // La guardamos en una variable de la clase, desde el intance singleton
        gt = Gestortopos.instance;
        spriteAcierto.SetActive(false);
        spritefallo.SetActive(false);   
       
    }

    private void OnEnable() // se llama cada vez que se activa este componente
    {
        ResetTopo();
    }

    // Update is called once per frame
    void Update()
    {
      

      switch (estado)
        {
            //no hace nada 
            case EstadoTopo.INACTIVO:
                break;
            // Animacion de aparecer, y pasa a esperar
            case EstadoTopo.APARECE:
                Apareciendo();
                break;
            // Espera un tiempo, o hasta que le golpeen, y desaparece 
            case EstadoTopo.ESPERA:
                Esperando();
                break;
            // Animacion de desaparecer, y pasa a inactivo
            case EstadoTopo.DESAPARECE:
                Desapareciendo();
                break;
        }
        
    }

    void Apareciendo ()
    {
        // animacion de aparecer
        timer += Time.deltaTime;

        // Interpolamos la escala entre Min y Max, durante tAparece tiempo.
        transform.localScale = Vector3.Lerp(escalaMin, escalaMax, timer / tAparece);

        Vector3 auxPos = transform.localPosition; // me guardo la posicion actual

        auxPos.z = Mathf.Lerp(zEnterrado, zDescubierto, timer / tAparece);

        transform.localPosition = auxPos;


        //compruebo que el timer es mayor o igual que el tiempo que ha pasado en el contador de aparecer
        if (timer >= tAparece)
        {
            // Pasar a Esperar
            estado = EstadoTopo.ESPERA;

            timer = 0f;
        }

            
        //pasar a esperar
    }

    void Esperando()
    {
        timer += Time.deltaTime;

        // Mientras espera no hace nada 

        //Comprobamos que ha pasado tEspera
         if (timer >= tEspera)
        {
            // Pasa a desaparecer
            estado = EstadoTopo.DESAPARECE;

            // Reinicio el timer
            timer = 0f;

            SoundManager.instance.playSFX(sfxFallo);
            Gestortopos.instance.RestarPuntos();  // Restamos puntos si el topo desaparece 
            spritefallo.SetActive(true);
        }


        

        //Reinicio el timer
    }

    void Desapareciendo()
    {
        timer += Time.deltaTime;

        // Animacion de desaparecer

        // Interpolamos la escala entre Max y Min, durante tDesaparece tiempo.
        transform.localScale = Vector3.Lerp(escalaMax, escalaMin, timer / tDesaparece);

        Vector3 auxPos = transform.localPosition; // me guardo la posicion actual

        auxPos.z = Mathf.Lerp(zDescubierto,zEnterrado,timer / tDesaparece);
        
        transform.localPosition = auxPos; // asigno la posicion al objeto


        // Comprobamos que ha pasado tDesaparecer
        if (timer >= tDesaparece)
        {
            // Pasa a inactivo
            estado = EstadoTopo.INACTIVO;

            // Reinicio el timer
            timer = 0f;

            spritefallo.SetActive(false);
            spriteAcierto.SetActive(false);

            
        }

        
    }

    public void ResetTopo()
    {
        transform.localScale = escalaMin;
        estado = EstadoTopo.INACTIVO;
        Vector3 aux = transform.localPosition; aux.z = zEnterrado;
        transform.localPosition = aux;
        timer = 0f;

        spriteAcierto.SetActive(false);
        spritefallo.SetActive(false);
        TopoParticula.Clear();


    }

    public void ActivarTopo()
    {
        transform.localScale = escalaMin;
        Vector3 aux = transform.localPosition;
        aux.z = zEnterrado;
        transform.localPosition = aux;
        estado = EstadoTopo.APARECE;
        timer = 0f;
    }

    public void TopoGolpeado()
    {

        estado = EstadoTopo.DESAPARECE;
        Vector3 aux = transform.localPosition;
        aux.z = zDescubierto;
        transform.localPosition = aux;

        transform.localScale = escalaMax;
        SoundManager.instance.playSFX(sfxAcierto);

        timer = 0f;


        //gt.SumaPuntos();

     
        Gestortopos.instance.SumaPuntos(); // Sumamos puntos si el topo aparece
        spriteAcierto.SetActive(true);

        TopoParticula.Play();
        //TopoParticula.Emit(10);
        
      
    }
}
