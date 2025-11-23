using UnityEngine;

public class PlaySound : MonoBehaviour
{
    #region Variables
    //----------------------------------------------------------------------
    // Variables
    //----------------------------------------------------------------------
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

    public enum eFxClip
    {
        Transitions = 0,    // Transiciones
        WinGame = 1,        // Ganamos el juego
        Applause = 2,       // Aplauso de tanto
        LoseGame = 3,       // Perdemos el juego contra IA, me falta implementar
        Rebote = 4,         // Rebote en pared
        Golpe = 5,          // Golpe de raqueta
        ScorePoint = 6,     // Punto conseguido
        PowerUp = 7,        // PowerUp activado
        NumClips = 8        // Numero de clips
    }

    public enum eFondoClip
    {
        Murmullos = 0,      // Transiciones
        Generic = 1,        // Ganamos el juego
        NumFondos = 2       // Numero de clips
    }

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
        audioFxSource = GetComponent<AudioSource>();
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
    // Carga desde Resources que no estamos usando
    //----------------------------------------------------------------------
    public static void InitAudioClips()
    {
        listaFxAudioClip = new AudioClip[7];
        listaFxAudioClip[(int)eFxClip.Transitions] = Resources.Load<AudioClip>("Audio/Transitions");
        listaFxAudioClip[(int)eFxClip.WinGame] = Resources.Load<AudioClip>("Audio/WinGame");
        listaFxAudioClip[(int)eFxClip.LoseGame] = Resources.Load<AudioClip>("Audio/LoseGame");
        listaFxAudioClip[(int)eFxClip.Rebote] = Resources.Load<AudioClip>("Audio/Rebote");
        listaFxAudioClip[(int)eFxClip.Golpe] = Resources.Load<AudioClip>("Audio/Golpe");
        listaFxAudioClip[(int)eFxClip.ScorePoint] = Resources.Load<AudioClip>("Audio/ScorePoint");
        listaFxAudioClip[(int)eFxClip.PowerUp] = Resources.Load<AudioClip>("Audio/PowerUp");
    }

    //----------------------------------------------------------------------
    // no lo cargamos aqui sino en el gameobject que le asocia.
    // se carga la lista en el editor y lugo se pasa por esta funcion
    //----------------------------------------------------------------------
    public static void InitFxClips(AudioClip[] lista)
    {
        listaFxAudioClip = lista;
    }

    public static void InitFondoClips(AudioClip[] lista)
    {
        listaFondoAudioClip = lista;
    }

    //----------------------------------------------------------------------
    // Y con esta se ejecutan los efectos sonoros
    //----------------------------------------------------------------------
    public static void PlayFxClip(eFxClip clip)
    {
        if (isActiveSounds)
        {
            try
            {
                AudioClip clipToPlay = listaFxAudioClip[(int)clip];
                if (clipToPlay != null)
                    audioFxSource.PlayOneShot(clipToPlay);
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

    public static void PlayFondoClip(eFondoClip clip)
    {
        if (isActivateBakcgroundSound)
        {
            try
            {
                AudioClip clipToPlay = listaFondoAudioClip[(int)clip];
                if (clipToPlay != null)
                    audioBackgroundSource.PlayOneShot(clipToPlay);
            }
            catch (System.Exception ex)
            {
                Tool.LogColor(" PlaySound.PlayFxClip: [" + ex.Message + "]", Color.red);
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
    // asociamos el audiosource con la musica de fondo cargada desde el
    // editor.
    //----------------------------------------------------------------------
    public static void SetAudioBackgoundSource(AudioSource source)
    {
        audioBackgroundSource = source;
    }
    //----------------------------------------------------------------------
    #endregion
}
