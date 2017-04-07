using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldTrigger : MonoBehaviour
{
    public InputField customTextField;
    public Color normal, highlighted;
    public Renderer screem;
    public void OnMouseUp()
    {
        EventSystem.current.SetSelectedGameObject(customTextField.gameObject);
    }

    public void OnMouseEnter()
    {
        screem.material.color = highlighted;
    }

    public void OnMouseExit()
    {
        screem.material.color = normal;
    }

}
