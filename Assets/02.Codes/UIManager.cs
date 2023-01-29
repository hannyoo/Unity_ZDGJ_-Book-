using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    private void Start()
    {
        // UnityAction을 사용하여 이벤트 연결
        action = () => OnButtonClick(startButton.name);
        startButton.onClick.AddListener(action);
       
        // delegate 활용 이벤트 연결
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        // 람다식을 활용 이벤트 연결
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));

    }

    public void OnButtonClick(string msg)
    { 
        Debug.Log($"Click Button : {msg}");       
    }
}
