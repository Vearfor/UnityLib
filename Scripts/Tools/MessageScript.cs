using System.Collections;
using TMPro;
using UnityEngine;

namespace TauriLand.Libreria
{
    public class MessageScript : MonoBehaviour
    {
        #region Variables
        //------------------------------------------------------------------
        // Variables
        //------------------------------------------------------------------
		bool mostrandoMensaje = false;
		Color oldColorMensaje = Color.black;
        //------------------------------------------------------------------
        #endregion

        #region MonoBehaviour
        /*----------------------------------------------------------------*\
        |* Metodos / Funciones MonoBehabiuor
        \*----------------------------------------------------------------*/
        //------------------------------------------------------------------
        #endregion

        #region Metodos Propios
        /*----------------------------------------------------------------*\
        |* Metodos / Funciones Propios
        \*----------------------------------------------------------------*/
		public void lanzaMensaje(TextMeshProUGUI textMensaje, float seconds)
		{
			if (textMensaje)
			{
				StartCoroutine(muestraMensaje(textMensaje, seconds));
			}
		}
		
        //------------------------------------------------------------------
        // La corrutina
        //------------------------------------------------------------------
        IEnumerator muestraMensaje(TextMeshProUGUI textMensaje, float seconds)
		{
			if(!mostrandoMensaje)
			{
				mostrandoMensaje = true;
				oldColorMensaje = textMensaje.color;
				Color newColorMensaje = textMensaje.color;
				float alphaDecrease = 0.1f;
				textMensaje.gameObject.SetActive(true);
				yield return new WaitForSeconds(seconds);
				while (textMensaje.color.a - alphaDecrease > 0)
				{
					newColorMensaje.a -= alphaDecrease;
					textMensaje.color = newColorMensaje;

					// No se debe de usar Sleep, detiene el hilo principal
					// Por narices, hay que utilizar corrutinas
					// Thread.Sleep(100);
					yield return new WaitForSeconds(0.1f);
				}
				textMensaje.gameObject.SetActive(false);
				textMensaje.color = oldColorMensaje;
				mostrandoMensaje = false;
			}
			yield return null;
		}
        //------------------------------------------------------------------
        #endregion
    }
}
