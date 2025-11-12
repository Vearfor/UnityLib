using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tool : MonoBehaviour
{
    //----------------------------------------------------------------------
    // Statics para el Log
    //----------------------------------------------------------------------
    static bool activoLog = false;
    static string logPath = string.Empty;
    static string logFileName = string.Empty;
    static string logFinalFileName = string.Empty;
    static StreamWriter logWriter = null;
    static bool withTime = false;
    static string programName = string.Empty;
    //----------------------------------------------------------------------


    //------------------------------------------------------------------
    // Constantes
    //------------------------------------------------------------------
    // Pondremos nombres en espaniol:
    // No me deja poner como const, pero por ahora, para lo que quiero
    // me vale.
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

    //------------------------------------------------------------------
    // Habria que colocarlo en otra parte:
    //  - comun a muchas otras clases y scripts
    //
    // ¿ pero en Unity donde colocas esto ?
    // Si que necesito cosas por aprender del C# en Unity.
    //------------------------------------------------------------------
    struct sMonth
    {
        public int monthNumDays;
        public string monthName;
    }
    //------------------------------------------------------------------
    // Y ahora un array para los meses
    //------------------------------------------------------------------
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


    /*====================================================================*\
    |* Funciones MonoBehaviour
    \*====================================================================*/
    private void Awake()
    {
        Debug.Log("Awake de General: [" + gameObject.name + "]");
        DebugBreak();

        string sDir = Directory.GetCurrentDirectory();
        programName = Path.GetFileName(sDir);
        Init("Files", programName, true);
        Log(" Inicio de [" + programName + "]\n");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start de General: [" + gameObject.name + "]");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        // if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Debug.ClearDeveloperConsole();
            Tool.Salir();
        }
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy");
        Log(" Fin de [" + programName + "]\n");
        if (logWriter != null)
        {
            Debug.Log("Close logWriter");
            CerrarLog();
        }
    }

    /*====================================================================*\
    |* Mis Funciones
    |* Y Statics para ser llamadas desde cualquier parte.
    \*====================================================================*/
    public static void Salir()
    {
        Debug.Log("Saliendo del juego...");
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

    public static void DebugBreak()
    {
        //------------------------------------------------------------------
        // Solo mientras estes funcionando en el editor de Unity
        //------------------------------------------------------------------
#if UNITY_EDITOR
        Debug.DebugBreak();
#endif
        //------------------------------------------------------------------
    }


    //----------------------------------------------------------------------
    // Log a fichero
    //----------------------------------------------------------------------
    public static int Init(string plogPath, string plogFileName, bool create = false)
    {
        if (!activoLog)
            return 0;

        // Debemos preguntar si el log ya esta levantado
        // Si no, seguimos
        // Asociamos los nombres
        logPath = plogPath;
        logFileName = plogFileName;
        // Si el directorio no existia lo creara
        // string name = Path.GetFileName(logPath);
        logFinalFileName = logPath + "/" + calcLogFileName(logFileName);
        Debug.Log(" LogFileName: [" + logFinalFileName + "]");
        if (create)
        {
            CreateLog();
        }

        return 0;
    }

    public static int Log (string log)
    {
        if (!activoLog)
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
        if (!activoLog)
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
        if (!activoLog)
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
        if (!activoLog)
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
            Debug.Log("Exception: CreateDirectory: [" + ex.Message + "]");
            return -1;
        }
        return 0;
    }

    static int CreateLogFile(string logPathFile)
    {
        if (!activoLog)
            return 0;

        try
        {
            logWriter = new StreamWriter(logPathFile, true);
        }
        catch (System.Exception ex)
        {
            // throw new Exception(ex.Message, ex);
            Debug.Log("Exception: CreateLogFile: [" + ex.Message + "]");
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

    //----------------------------------------------------------------------
    // Inteno evitar cuando los limites maximos no se devuelven
    //----------------------------------------------------------------------
    public static int rangoAleatorio(int minimo, int maximo)
    {
        int valor;

        valor = UnityEngine.Random.Range(minimo, maximo+1);
        // Pero si te devuelve maximo + 1
        // Lo descartamos
        if (valor == maximo + 1)
            valor = maximo;
        return valor;
    }
}
