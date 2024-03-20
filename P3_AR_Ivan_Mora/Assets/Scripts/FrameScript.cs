using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameScript : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite[] fotos;

    public int IndexFoto = 0; //contador para llevar el recuento de en que posicion estamos

    public bool loopFotos = false;

    public float anchoOriginal;
    public float escalaOriginal;

   
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
       
        anchoOriginal = sr.sprite.rect.width; //pregunto el ancho del sprite actual al sprite renderer
        escalaOriginal = transform.localScale.x; // guardo la escala del objeto en el eje X

        ActualizaSprite();
    }

   
    void Update()
    {
       
    }

    public void ActualizaSprite()
    {
        sr.sprite = fotos[IndexFoto]; // Cambio a la foto nueva

        float anchoNuevo = sr.sprite.rect.width; // Pregunto por el ancho de la imagen nueva
        float altoNuevo = sr.sprite.rect.height; //pregunto por el alto de la imagen nueva 
       

        // Claculamos la nueva escala. Multiplicamos ancho y escala original, y dividimos por el mayor de ancho o alto
        float escalaNueva = (anchoOriginal * escalaOriginal) / Mathf.Max(anchoNuevo, altoNuevo); // Calculamos la nueva escala

        transform.localScale = new Vector3(escalaNueva, escalaNueva, escalaNueva); // Asignamos la escala nueva al objeto
    }

    public void SiguienteSprite()
    {
        IndexFoto++;

        if(loopFotos)
        {
            if (IndexFoto >= fotos.Length)
            {
                IndexFoto = 0;

            }
        }

        else
        {
            if (IndexFoto >= fotos.Length)
            {
                IndexFoto = fotos.Length - 1;

            }

        }

        ActualizaSprite();





    }

    public void AnteriorSprite()
    {
        IndexFoto--;


        if (loopFotos)
        {
            if (IndexFoto < 0)
            {

                IndexFoto = fotos.Length - 1;

            }
        }

        else
        {
            if (IndexFoto < 0)
            {

                IndexFoto = 0;

            }
        }

        ActualizaSprite();


    }
}
