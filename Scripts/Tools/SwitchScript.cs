using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchScript : MonoBehaviour
{
    #region Cadenas de Texto
    //----------------------------------------------------------------------
    // Cadenas de texto
    //----------------------------------------------------------------------
    const string sCircle = "circle";
    const string sTextIzquierda = "TextQuienIzquierda";
    const string sTextDerecha = "TextQuienDerecha";
    //----------------------------------------------------------------------
    #endregion

    #region Variables
    //----------------------------------------------------------------------
    // Variables
    //----------------------------------------------------------------------
    public int status = -1;

    [SerializeField] GameObject switchButton;
    [SerializeField] TextMeshProUGUI textQuienIzquierda;
    [SerializeField] TextMeshProUGUI textQuienDerecha;
    //----------------------------------------------------------------------
    #endregion

    #region MonoBehaviour
    /*--------------------------------------------------------------------*\
    |* Metodos / Funciones MonoBehaviour
    \*--------------------------------------------------------------------*/
    private void Awake()
    {
        Button switchComponet = GetComponent<Button>();
        if (switchComponet)
        {
            switchComponet.onClick.AddListener(ClickOnSwitch);
        }
        GameObject[] todos = Tool.getAllGameObjects();
        switchButton = Tool.getObjectByName(todos, sCircle);
        textQuienIzquierda = Tool.getObjectByName(todos, sTextIzquierda).GetComponent<TextMeshProUGUI>();
        textQuienDerecha = Tool.getObjectByName(todos, sTextDerecha).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        //------------------------------------------------------------------
        // Valor interno de la posicion de la imagen del boton del switch
        //------------------------------------------------------------------
        float x = 25f;
        //------------------------------------------------------------------

        x = x * status;

        Vector2 pos = switchButton.transform.localPosition;
        pos.x = pos.x * status;
        switchButton.transform.localPosition = pos;
        Tool.LogColor("OnEnable Switch status: " + status + "  X: [" + pos.x + "]", Color.yellow);
        showIAPlayer();
    }
    //----------------------------------------------------------------------
    #endregion

    #region Metodos Propios
    /*--------------------------------------------------------------------*\
    |* Metodos / Funciones Propias
    \*--------------------------------------------------------------------*/
    public void ClickOnSwitch()
    {
        status = -status;
        Vector3 localPos = switchButton.transform.localPosition;
        switchButton.transform.localPosition = new Vector3(-localPos.x, localPos.y, localPos.z);
        Tool.LogColor("Switch Switch status: " + status + "  X: [" + -localPos.x + "]", Color.yellow);
        showIAPlayer();
    }

    public void showIAPlayer()
    {
        if (status == -1)
        {
            textQuienIzquierda.text = "IA";
            textQuienDerecha.text = "Jugador";
        }
        else
        {
            textQuienIzquierda.text = "Jugador";
            textQuienDerecha.text = "IA";
        }
    }
    //----------------------------------------------------------------------
    #endregion
}
