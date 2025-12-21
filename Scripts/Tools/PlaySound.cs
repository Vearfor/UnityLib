using System;
using UnityEngine;

namespace TauriLand.Libreria
{
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
        // Necesitamos un GameObject con un audiosource como componente
        //----------------------------------------------------------------------
        static GameObject audioObject = null;
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
        public static bool isActiveBakcgroundSound = false;
        public static bool isPaused = false;

        public static AudioClip[] listaAudioClip;
        public static AudioClip[] listaFxAudioClip;
        public static AudioClip[] listaFondoAudioClip;
        //----------------------------------------------------------------------
        // Los otros atributos dados hasta ahora:
        //----------------------------------------------------------------------
        //[HideInInspector] // Para ocultar publics que no queremos que se vean
        //                  // en el inspector
        //[Header ("Texto")] // Para separar por categorias 
        //[Space] // Agrega un espacio vertical
        //[Space(20)] // supongo que 20
        //[Range(min,max)] // deslizador
        //[TextArea (3,6)] // El campo string que esta marcando aparece en el
        //                 // inspector con 3 lineas minimas, y permite 6 lineas
        //                 // maximas antes de salir el scroll
        //----------------------------------------------------------------------
        // Atributo de clase mencionado:
        //----------------------------------------------------------------------
        // [CreateAssetMenu]    que todavia no se para que.
        // - atributo de clase
        // - encima de clases que heredan de ScriptableObject
        // - para crear instancias del ScriptableObject desde el menu: (?)
        //
        //   +   (Right Click) + Create --> [Mi Categoria] --> [Mi Nombre]
        //
        // [CreateAssetMenu (
        //      fileName = "NuevaArma",
        //      menuName = "Juego/Arma",
        //      order = 1
        // )]
        //
        //  (???????)
        //
        // [RequireComponent (typeof(Rigidbody))]  para indicar que el
        // GameObject debe tener un componente especifico
        // - en el ejemplo, un RigidBody.
        //----------------------------------------------------------------------
        #endregion


        #region MonoBehaviour
        /*--------------------------------------------------------------------*\
        |* Metodos / Funciones MonoBehaviour
        \*--------------------------------------------------------------------*/
        public void Awake()
        {
            Tool.LogColor("Awake PlaySound [" + name + "]", Color.green);

            if (staticAudioSource == null && audioObject)
            {
                // No funciona el new, sigue devolviendo null, tiene que
                // pertenecer a un GameObject como componente
                // staticAudioSource = new AudioSource();

                // Hay que hacer esto
                audioObject = new GameObject("StaticAudioSource");  // Crea el GameObject con este nombre
                                                                    // Le agregamos el AudioSource, y al agregarlo ya tenemos el 'staticAudioSource'
                staticAudioSource = audioObject.AddComponent<AudioSource>();
                // Lo hemos hecho static, ya distinto de null: no debería de crearse de nuevo
                // Y hacemos, entonces que no se destruya entre escenas:
                DontDestroyOnLoad(audioObject);
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
            if (isActiveSounds && staticAudioSource && listaAudioClip != null && listaAudioClip.Length > 0)
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
            if (isActiveSounds && listaFxAudioClip != null && listaFxAudioClip.Length > 0)
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
            if (isActiveBakcgroundSound && audioBackgroundSource)
            {
                try
                {
                    if (index > -1 && index < listaFondoAudioClip.Length)
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
        public static void Play(int index = 0)
        {
            if (isActiveBakcgroundSound && audioBackgroundSource)
            {
                try
                {
                    // Tool.LogColor(" Play audioBackgroundSource.resource.name: " + audioBackgroundSource.resource.name, Color.aliceBlue);
                    if (audioBackgroundSource.clip==null && listaFondoAudioClip.Length>0 && index>-1 && index<listaFondoAudioClip.Length)
                    {
                        audioBackgroundSource.clip = listaFondoAudioClip[index];
                    }
                    if (audioBackgroundSource.clip)
                    {
                        audioBackgroundSource.loop = true;
                        audioBackgroundSource.Play();
                    }
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
            if (isActiveBakcgroundSound && audioBackgroundSource)
            {
                try
                {
                    // Tool.LogColor(" Stop audioBackgroundSource.resource.name: " + audioBackgroundSource.resource.name, Color.aliceBlue);
                    if (audioBackgroundSource.clip)
                    {
                        audioBackgroundSource.Stop();
                    }
                }
                catch (System.Exception ex)
                {
                    Tool.LogColor(" PlaySound.Stop: [" + ex.Message + "]", Color.red);
                }
            }
        }

        public static void Pause()
        {
            if (isActiveBakcgroundSound && audioBackgroundSource)
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
}
