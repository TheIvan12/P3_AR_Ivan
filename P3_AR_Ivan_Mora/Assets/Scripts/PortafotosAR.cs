using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortafotosAR : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] Fotos; //lista de sprites

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = Fotos[0];

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetrocederFoto()
    {
        Debug.Log("Foto retrocedida");
    }
}
