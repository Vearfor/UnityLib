using System.Collections;
using TMPro;
using UnityEngine;

public class MessageScript : MonoBehaviour
{
    #region Variables
    //----------------------------------------------------------------------
    // Variables
    //----------------------------------------------------------------------
    //----------------------------------------------------------------------
    // El time of life -1 es para que el mensaje no se desactive
    // El resto de valores positivos determina el tiempo que queremos que
    // el texto permanezca visible o activo.
    //----------------------------------------------------------------------
    // No usaremos la corrutina no se puede modificar una vez lanzada
    // Hay que esperar a que se cumpla el tiempo de espera.
    // Por esta razon utilizaremos el otro metodo. en este caso.
    //----------------------------------------------------------------------
    float timeOfLife = 0;
    string sTexto = "";
    TextMeshPro textMesh;
    #endregion

    #region MonoBehaviour
    /*--------------------------------------------------------------------*\
    |* Metodos / Funciones MonoBehabiuor
    \*--------------------------------------------------------------------*/
    private void Awake()
    {
        textMesh = gameObject.GetComponent<TextMeshPro>();
    }

    void OnEnable()
    {
        textMesh.text = sTexto;

        // Podemos hacerlo con una coroutina
        //if (timeOfLife !=-1)
        //{
        //    StartCoroutine(waitToHide(timeOfLife));
        //}
    }

    void Update()
    {
        procUpdate(Time.deltaTime);
    }
    #endregion

    #region Metodos Propios
    /*--------------------------------------------------------------------*\
    |* Metodos / Funciones Propios
    \*--------------------------------------------------------------------*/
    //----------------------------------------------------------------------
    // Esto es sin corutina
    // - Con la corrutina, en este caso, tengo que esperar para cambiar los
    //   valores.
    // - aqui por fuera puedo actualizar el timeOfLife fuera del Update
    //   con: setTime
    //----------------------------------------------------------------------
    void procUpdate(float deltaTime)
    {
        if (timeOfLife != -1)
        {
            if (timeOfLife > 0)
            {
                timeOfLife -= deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    //----------------------------------------------------------------------
    // La corrutina
    //----------------------------------------------------------------------
    //IEnumerator waitToHide(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    gameObject.SetActive(false);
    //}
    //----------------------------------------------------------------------

    public void setTexto(string pText)
    {
        sTexto = pText;
        textMesh.text = sTexto;
    }

    public void setTime(float fTime)
    {
        timeOfLife = fTime;
    }
    #endregion
}
