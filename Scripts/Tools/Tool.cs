using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tool : MonoBehaviour
{
    #region Log
    //----------------------------------------------------------------------
    // Statics para el Log
    // - Hay que separar y generar una clase propia para el Log
    //----------------------------------------------------------------------
    // Es para pruebas, ahora esta desactivado
    //----------------------------------------------------------------------
    public static bool isActiveLog { get; set; }
    //----------------------------------------------------------------------
    static string logPath = string.Empty;
    static string logFileName = string.Empty;
    static string logFinalFileName = string.Empty;
    static StreamWriter logWriter = null;
    static bool withTime = false;
    static string programName = string.Empty;
    //----------------------------------------------------------------------
    #endregion

    #region Tecla Repetida
    //----------------------------------------------------------------------
    // Statics Tecla Repetida
    //----------------------------------------------------------------------
    // Limites de tiempo
    //----------------------------------------------------------------------
    static float limTeclaRepetida = .6f;     // 1 segundo o 0.6f de tecla repetida
    //----------------------------------------------------------------------
    static float teclaRepetida = 0f;
    //----------------------------------------------------------------------
    public static bool isRepeatedKey
    {
        get
        {
            return (teclaRepetida > 0.1);
        }
    }
    //----------------------------------------------------------------------
    #endregion

    #region Constantes
    //----------------------------------------------------------------------
    // Constantes
    //----------------------------------------------------------------------
    // Pondremos nombres en espaniol:
    // No me deja poner como const, pero por ahora, para lo que quiero
    // me vale.
    // - Hay que encontrar un sitio comun para estos valores
    //----------------------------------------------------------------------
    static string[] weekName =
    {
        "Lunes",
        "Martes",
        "Miercoles",
        "Jueves",
        "Viernes",
        "Sabado",
        "Domingo"
    };

    //----------------------------------------------------------------------
    // Habria que colocarlo en otra parte:
    //  - comun a muchas otras clases y scripts
    //
    // ¿ pero en Unity donde colocas esto ?
    // Si que necesito cosas por aprender del C# en Unity.
    // - Hay que encontrar un sitio comun para estos valores
    //----------------------------------------------------------------------
    struct sMonth
    {
        public int monthNumDays;
        public string monthName;
    }
    //----------------------------------------------------------------------
    // Y ahora un array para los meses
    // - Hay que encontrar un sitio comun para estos valores
    //----------------------------------------------------------------------
    static sMonth[] months =
    {
        new sMonth { monthNumDays = 31, monthName = "enero" },
        new sMonth { monthNumDays = 28, monthName = "febrero" },    // No vamos a tener en cuenta los bisiestos
        new sMonth { monthNumDays = 31, monthName = "marzo" },
        new sMonth { monthNumDays = 30, monthName = "abril" },
        new sMonth { monthNumDays = 31, monthName = "mayo" },
        new sMonth { monthNumDays = 30, monthName = "junio" },
        new sMonth { monthNumDays = 31, monthName = "julio" },
        new sMonth { monthNumDays = 31, monthName = "agosto" },
        new sMonth { monthNumDays = 30, monthName = "septiembre" },
        new sMonth { monthNumDays = 31, monthName = "octubre" },
        new sMonth { monthNumDays = 30, monthName = "noviembre" },
        new sMonth { monthNumDays = 31, monthName = "diciembre" },
    };
    //----------------------------------------------------------------------
    #endregion

    #region MonoBehaviour
    /*====================================================================*\
    |* Funciones MonoBehaviour
    |* - Vamos a introducir Tool como componente.
    \*====================================================================*/
    private void Awake()
    {
        LogColor("Awake Tool: [" + gameObject.name + "]", Color.cyan);
        DebugBreak();

        string sDir = Directory.GetCurrentDirectory();
        programName = Path.GetFileName(sDir);
        Init("Files", programName, true);
        Log(" Inicio de [" + programName + "]\n");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LogColor("Start Tool: [" + gameObject.name + "]", Color.cyan);
    }

    // Update is called once per frame
    void Update()
    {
        controlEscape();
    }

    void OnDestroy()
    {
        LogColor("OnDestroy Tool: [" + gameObject.name + "]", Color.cyan);
        Log(" Fin de [" + programName + "]\n");
        if (logWriter != null)
        {
            LogColor("Close logWriter", Color.cyan);
            CerrarLog();
        }
    }
    //----------------------------------------------------------------------
    #endregion

    #region Properties
    public static bool isActiveToolKeyboard {  get; set; }
    #endregion

    #region Funciones Propias
    /*====================================================================*\
    |* Mis Funciones
    |* Y Statics para ser llamadas desde cualquier parte.
    \*====================================================================*/

    #region Tecla Repetida
    //----------------------------------------------------------------------
    // Control Comun de tecla repetida
    //----------------------------------------------------------------------
    public static void controlTeclaRepetida(float deltaTime)
    {
        if (teclaRepetida > 0)
            teclaRepetida -= deltaTime;
    }

    //----------------------------------------------------------------------
    // Establecemos el tiempo de espera por tecla repetida
    //----------------------------------------------------------------------
    public static void setTeclaRepetida()
    {
        teclaRepetida = limTeclaRepetida;
    }
    //----------------------------------------------------------------------
    #endregion

    public static void controlEscape()
    {
        if (isActiveToolKeyboard)
        {
            if (Keyboard.current.escapeKey.isPressed)
            // if (Input.GetKeyDown(KeyCode.Escape))
            {
                Tool.Salir();
            }
        }
    }

    #region Salir
    //----------------------------------------------------------------------
    // Funcion para salir comun a todos los programas
    //----------------------------------------------------------------------
    public static void Salir()
    {
        LogColor("Saliendo del juego...", Color.yellow);
#if UNITY_EDITOR
        //------------------------------------------------------------------
        // En el editor de Unity no se puede salir del juego, pero si parar
        // la ejecucion.
        //------------------------------------------------------------------
        EditorApplication.isPlaying = false;
        //------------------------------------------------------------------
#else
        //------------------------------------------------------------------
        // Cuando no estamos en el editor de Unity, si que se puede salir
        // del programa con:
        //------------------------------------------------------------------
        Application.Quit();
        //------------------------------------------------------------------
#endif
    }
    //----------------------------------------------------------------------
    #endregion


    public static void DebugBreak()
    {
        //------------------------------------------------------------------
        // Solo mientras estes funcionando en el editor de Unity
        // no me parece que funcione
        //------------------------------------------------------------------
#if UNITY_EDITOR
        Debug.DebugBreak();
#endif
        //------------------------------------------------------------------
    }


    #region LogColor
    //----------------------------------------------------------------------
    // Nuestro log con colores
    // - en vez de Debug.Log --> Tool.LogColor
    //----------------------------------------------------------------------
    public static void LogColor(string message, Color color)
    {
        string colorCode = UnityEngine.ColorUtility.ToHtmlStringRGB(color);
        Debug.Log($"<color=#{colorCode}>{message}</color>");
    }
    //----------------------------------------------------------------------
    #endregion


    #region Log a Fichero
    /*====================================================================*\
    |* Hay que hacer una clase propia para un log de fichero
    \*====================================================================*/
    //----------------------------------------------------------------------
    // Log a fichero
    //----------------------------------------------------------------------
    public static int Init(string plogPath, string plogFileName, bool create = false)
    {
        if (!isActiveLog)
            return 0;

        logPath = plogPath;
        logFileName = plogFileName;
        // Si el directorio no existia lo creara
        // string name = Path.GetFileName(logPath);
        logFinalFileName = logPath + "/" + calcLogFileName(logFileName);
        LogColor(" LogFileName: [" + logFinalFileName + "]", Color.white);
        if (create)
        {
            CreateLog();
        }

        return 0;
    }

    //----------------------------------------------------------------------
    // La llamada generica:
    // - en vez de Debug.Log --> Tool.Log
    //----------------------------------------------------------------------
    public static int Log (string log)
    {
        if (!isActiveLog)
            return 0;

        string mensaje = (withTime) ? DateTime.Now + "  " : "";
        mensaje += log;

        bool mustClose = false;
        if (logWriter == null)
        {
            mustClose = true;
            CreateLog();
        }

        if (logWriter != null)
            logWriter.Write(mensaje);

        if (mustClose)
        {
            CerrarLog();
        }

        return 0;
    }

    static int CreateLog()
    {
        if (!isActiveLog)
            return 0;

        bool res =
            (
                CreateDirectory(logPath) != 0 ||
                CreateLogFile(logFinalFileName) != 0
            );
        if (res)
            return -1;
        return 0;
    }

    static int CerrarLog()
    {
        if (!isActiveLog)
            return 0;

        if (logWriter != null)
        {
            logWriter.Close();
            logWriter = null;
        }
        return 0;
    }

    static int CreateDirectory(string path)
    {
        if (!isActiveLog)
            return 0;

        try
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        catch (System.Exception ex)
        {
            // throw new Exception(ex.Message, ex);
            LogColor("Exception: CreateDirectory: [" + ex.Message + "]", Color.red);
            return -1;
        }
        return 0;
    }

    static int CreateLogFile(string logPathFile)
    {
        if (!isActiveLog)
            return 0;

        try
        {
            logWriter = new StreamWriter(logPathFile, true);
        }
        catch (System.Exception ex)
        {
            LogColor("Exception: CreateLogFile: [" + ex.Message + "]", Color.red);
            return -1;
        }
        return 0;
    }

    static string calcLogFileName(string fileName)
    {
        string name;
        string ahora = DateTime.Now.ToString("HHmmss");

        name = logFileName + "_";
        name += DateTime.Now.Year + "_";
        name += DateTime.Now.Month + "_";
        name += DateTime.Now.Day + "_";
        name += ahora;
        name += ".log";

        return name;
    }
    //======================================================================
    #endregion

    //----------------------------------------------------------------------
    // Intento evitar cuando los limites maximos no se devuelven
    //----------------------------------------------------------------------
    public static int rangoAleatorio(int minimo, int maximo)
    {
        int valor;

        valor = UnityEngine.Random.Range(minimo, maximo + 1);
        // Pero si te devuelve maximo + 1
        // Lo descartamos
        if (valor == maximo + 1)
            valor = maximo;
        return valor;
    }

    //----------------------------------------------------------------------
    // Busca un GameObject por su nombre este o no este activo
    // - Esta es una funcion muy costosa.
    //   Intentar no usarla en Update o funciones que se llamen muchas
    //   veces.
    // - Awake y Start son buenos sitios para usarla.
    //----------------------------------------------------------------------
    public static GameObject lookForGameObject(string name)
    {
        GameObject[] objs = getAllGameObjects();
        GameObject obj = getObjectByName(objs, name);
        return obj;
    }

    //----------------------------------------------------------------------
    // Devuelve todos los GameObject
    //----------------------------------------------------------------------
    public static GameObject[] getAllGameObjects()
    {
        GameObject[] objs = GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        return objs;
    }

    //----------------------------------------------------------------------
    // Busca un GameObject por su nombre este o no este activo
    //----------------------------------------------------------------------
    public static GameObject getObjectByName(GameObject[] lista, string name)
    {
        if (lista != null && lista.Length>0)
        {
            foreach (GameObject go in lista)
            {
                if (go.name == name)
                {
                    return go;
                }
            }
        }
        return null;
    }
    //----------------------------------------------------------------------
    #endregion
}
