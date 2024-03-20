using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gestortopos : MonoBehaviour
{

    public int puntuacion = 0;
    public int PuntosFallo = 10;
    public int PuntosAcierto = 10;
    public TOPO[] listaTopos;

    private float timer = 0;
    public float tiempoEntreTopos = 1f;

    // Referencia statica para el singleton
    static public Gestortopos instance = null;

  
    
    public AudioClip musica;
  

    public TextMeshPro TextoPuntos;

    // Si la partida ha acabado es true 
    public bool gameOver = false;

    // Topos que apaecen en cada ronda
    public int toposPorRonda = 10;
    // Topos que han aparecido en esta ronda
    private int toposSpawneados = 0;




    // Awake se llama antes que el start
    public void Awake()
    {
        // Asigno la insntacia de la clase para el singleton
        // Si no hay instacia asignada, asigno este
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Si hay instancia asignada, destruyo este objeto.
        // porque solo debe haber un singleton en escena
        else Destroy(this.gameObject);

    }
       


    // Start is called before the first frame update
    void Start()
    {
       

        instance = this;

        listaTopos = GetComponentsInChildren<TOPO>(); // Busca entre los hijos de los topos

        SoundManager.instance.playMusic(musica);

        

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            // Activa un topo cada tiempo entre topos segundos
            timer = timer + Time.deltaTime;

            if (timer >= tiempoEntreTopos)
            {
                ActivaTopoAleatorio();
                toposSpawneados++;
                timer = 0;
                CompruebaFinDeRonda();
            }

            CompruebaFinDeRonda();
        }



        if (Input.GetMouseButtonDown(0))
        {
            PulsacionJugador();
        }
    }

    public void ActivaTopoAleatorio()
    {
        bool TopoEncontrado = false;

        while (TopoEncontrado == false) // Mienras topoEncontrado es false ( no se ha encontrado)
        {

            // Obtengo una posicion aleatoria de la lista de topos
            // Entre 0 y el tamaño de la lista

            int random = Random.Range(0, listaTopos.Length);


            // Compruebo que el topo este inactivo antes de activarlo
            if (listaTopos[random].estado == EstadoTopo.INACTIVO)
            {
                listaTopos[random].ActivarTopo(); //Activo topo en la posicion random

                TopoEncontrado = true;// He encontrado el topo
                
            }

        }
    }

    // Reiniciar valores para una partida nueva
    private void  ResetPartida()
    {
        puntuacion = 0;
        ActualizaMarcador();
        timer = 0;
        gameOver = false;
        toposSpawneados = 0;
        

        // Reiniciar los topos 
        // for normal 
        /*
        for(int i = 0; i < listaTopos.Length; i++) 
        {
            listaTopos[i].ResetTopo();
        }*/

        // for each - siempre recorre la lista completa
        foreach (TOPO topo in listaTopos)
        {
            topo.ResetTopo();
        }
    }

    private void CompruebaFinDeRonda()
    {
        /*
        if (puntuacion > 100)
        {
            gameOver = true;
        }*/


        // Rondas hasta aparecer x ronda 

        
        if (toposSpawneados >= toposPorRonda)
        {
            gameOver = true;
        }

         
    }

    public void PulsacionJugador()
    {
       
      
        // Me guardo un ray entre la cámara y la posicion del ratón
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction, Color.red, 4);

        RaycastHit infoRayo;
   
        if (Physics.Raycast(ray, out infoRayo)) // Hago el raycast y guardo el raycasthit(informacion) en inforayo
        {
            if (infoRayo.transform.tag == "Reset") // si el rayo choca con un objeto con el tag Reset
            {
                // He chocado con el boton reset
                ResetPartida();
            }

            else // Si no es un botón, es un topo 
            {
                // Debug.Log("El rayo ha chocado con" + infoRayo.transform.gameObject.name);
                // Debug.Log("El rayo estaba a una distancia de" + infoRayo.distance);
                // Debug.Log("El rayo ha chocado con" + infoRayo.point);
                //Destroy(infoRayo.transform.gameObject);

                // Compruebo si el objeto con el que ha chocado el rayo
                // Tiene el componente TOPO y lo guardo en una variable
                TOPO auxCompTopo = infoRayo.transform.gameObject.GetComponentInParent<TOPO>();

                TOPO auxHitTopo2 = infoRayo.collider.GetComponentInParent<TOPO>();


                // Si auxCompTopo existe (si he chocado con un topo)
                if (auxCompTopo != null)
                {
                    if (auxCompTopo.estado == EstadoTopo.APARECE || auxCompTopo.estado == EstadoTopo.ESPERA)
                    {
                        auxCompTopo.TopoGolpeado();

                    }
                }

            }

        }
    }

    public void SumaPuntos()
    {
        puntuacion += PuntosFallo;
        ActualizaMarcador();
        // Debug.Log("Puntos Sumados");
    }


    public void RestarPuntos()
    {
        

        if (puntuacion > 0)
        {

            puntuacion -= PuntosFallo;
        }
        ActualizaMarcador();

    }

    public void ActualizaMarcador()
    {

        TextoPuntos.text = puntuacion.ToString();
    }

}
