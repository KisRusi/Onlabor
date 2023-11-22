using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionFrame : MonoBehaviour
{
    public static ActionFrame instance = null;
    [SerializeField] private Button actionButton = null;
    [SerializeField] private Transform layoutGroup = null;

    private List<Button> buttons = new List<Button>();

    private void Awake()
    {
        instance = this;
    }

    public void SetActioncbuttons()
    {
        
    }
}
