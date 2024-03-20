using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private AudioSource musicSource;

    private void Awake()
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSFX(AudioClip clip)
    {
         sfxSource.PlayOneShot(clip);
    }

    public void playMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }
}
