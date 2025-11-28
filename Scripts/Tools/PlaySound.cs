// using Codice.Client.BaseCommands.BranchExplorer;
using System;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    #region Variables
    //----------------------------------------------------------------------
    // Variables
    //----------------------------------------------------------------------
    //----------------------------------------------------------------------
    // Vamos a experimentar con un audio source propio
    // No dependera del gameObject al cual se asocie.
    // Espero con ello que se mantenga un AudiSource activo entre escenas
    //----------------------------------------------------------------------
    public static AudioSource staticAudioSource = null;

    //----------------------------------------------------------------------
    // Audio source para los efectos sonoros
    // - en este programa es el audio source del MainCanvas
    //----------------------------------------------------------------------
    public static AudioSource audioFxSource;

    //----------------------------------------------------------------------
    // Audio source para la musica de fondo
    // Asocio la musica directamente en el editor
    // - en este programa es el audio source del Game
    //----------------------------------------------------------------------
    public static AudioSource audioBackgroundSource;

    //----------------------------------------------------------------------
    // Flags que utilizamos para activar o desactivar el sonido
    //----------------------------------------------------------------------
    public static bool isActiveSounds = true;
    public static bool isActivateBakcgroundSound = false;
    public static bool isPaused = false;

    public static AudioClip[] listaAudioClip;
    public static AudioClip[] listaFxAudioClip;
    public static AudioClip[] listaFondoAudioClip;
    //----------------------------------------------------------------------
    #endregion

    #region MonoBehaviour
    /*--------------------------------------------------------------------*\
    |* Metodos / Funciones MonoBehaviour
    \*--------------------------------------------------------------------*/
    public void Awake()
    {
        Tool.LogColor("Awake PlaySound [" + name + "]", Color.green);

        if (staticAudioSource == null)
        {
            staticAudioSource = new AudioSource();
        }
    }

    public void OnEnable()
    {
        Tool.LogColor("OnEnable PlaySound [" + name + "]", Color.green);
    }
    //----------------------------------------------------------------------
    #endregion

    #region Metodos Propios
    /*--------------------------------------------------------------------*\
    |* Metodos / Funciones Propias
    \*--------------------------------------------------------------------*/
    //----------------------------------------------------------------------
    // no lo cargamos aqui sino en el gameobject que le asocia.
    // se carga la lista en el editor y lugo se pasa por esta funcion
    //----------------------------------------------------------------------
    public static void InitClips(AudioClip[] lista)
    {
        listaAudioClip = lista;
    }

    //----------------------------------------------------------------------
    // Y el play con nuestro AudioSource.
    // Creado como statc dentro de Playsound
    // y utilizando una lista de clips que nos habran informado en la
    // funcion anterior.
    //----------------------------------------------------------------------
    public static void PlayClip(int index)
    {
        if (isActiveSounds && staticAudioSource)
        {
            try
            {
                if (index > -1 && index < listaAudioClip.Length)
                {
                    AudioClip clipToPlay = listaAudioClip[index];
                    if (clipToPlay != null)
                        staticAudioSource.PlayOneShot(clipToPlay);
                }
            }
            catch (System.Exception ex)
            {
                Tool.LogColor(" PlaySound.PlayClip: [" + ex.Message + "]", Color.red);
            }
        }
    }


    //----------------------------------------------------------------------
    // La lista de los efectos sonoros
    //----------------------------------------------------------------------
    public static void InitFxClips(AudioClip[] lista)
    {
        listaFxAudioClip = lista;
    }

    //----------------------------------------------------------------------
    // La lista de la musica de fondo
    //----------------------------------------------------------------------
    public static void InitFondoClips(AudioClip[] lista)
    {
        listaFondoAudioClip = lista;
    }

    //----------------------------------------------------------------------
    // Y con esta se ejecutan los efectos sonoros
    //----------------------------------------------------------------------
    public static void PlayFxClip(int index)
    {
        if (isActiveSounds)
        {
            try
            {
                if (index > -1 && index < listaFxAudioClip.Length)
                {
                    AudioClip clipToPlay = listaFxAudioClip[index];
                    if (clipToPlay != null)
                        audioFxSource.PlayOneShot(clipToPlay);
                }
            }
            catch (System.Exception ex)
            {
                Tool.LogColor(" PlaySound.PlayFxClip: [" + ex.Message + "]", Color.red);
            }
        }
    }

    public static void SetAudioFxSource(AudioSource source)
    {
        audioFxSource = source;
    }


    //----------------------------------------------------------------------
    // asociamos el audiosource con la musica de fondo cargada desde el
    // editor.
    //----------------------------------------------------------------------
    public static void SetAudioBackgoundSource(AudioSource source)
    {
        audioBackgroundSource = source;
    }

    //----------------------------------------------------------------------
    // Si lo asociamos a una lista:
    //----------------------------------------------------------------------
    public static void PlayFondoClip(int index)
    {
        if (isActivateBakcgroundSound)
        {
            try
            {
                if (index > -1 && index < listaFxAudioClip.Length)
                {
                    AudioClip clipToPlay = listaFondoAudioClip[index];
                    if (clipToPlay != null)
                        audioBackgroundSource.PlayOneShot(clipToPlay);
                }
            }
            catch (System.Exception ex)
            {
                Tool.LogColor(" PlaySound.PlayFondoClip: [" + ex.Message + "]", Color.red);
            }
        }
    }

    //----------------------------------------------------------------------
    // el play para la musica de fondo
    //----------------------------------------------------------------------
    public static void Play()
    {
        if (isActivateBakcgroundSound)
        {
            try
            {
                Tool.LogColor(" Play audioBackgroundSource.resource.name: " + audioBackgroundSource.resource.name, Color.aliceBlue);
                audioBackgroundSource.Play();
            }
            catch (System.Exception ex)
            {
                Tool.LogColor(" PlaySound.Play: [" + ex.Message + "]", Color.red);
            }
        }
    }

    //----------------------------------------------------------------------
    // el stop para la musica de fondo
    //----------------------------------------------------------------------
    public static void Stop()
    {
        if (isActivateBakcgroundSound)
        {
            try
            {
                Tool.LogColor(" Stop audioBackgroundSource.resource.name: " + audioBackgroundSource.resource.name, Color.aliceBlue);
                audioBackgroundSource.Stop();
            }
            catch (System.Exception ex)
            {
                Tool.LogColor(" PlaySound.Stop: [" + ex.Message + "]", Color.red);
            }
        }
    }

    public static void Pause()
    {
        if (isActivateBakcgroundSound)
        {
            try
            {
                Tool.LogColor(" Pause audioBackgroundSource.resource.name: " + audioBackgroundSource.resource.name, Color.aliceBlue);
                isPaused = !(audioBackgroundSource.isPlaying);

                audioBackgroundSource.Pause();
            }
            catch (System.Exception ex)
            {
                Tool.LogColor(" PlaySound.Pause: [" + ex.Message + "]", Color.red);
            }
        }
    }
    //----------------------------------------------------------------------
    #endregion
}
