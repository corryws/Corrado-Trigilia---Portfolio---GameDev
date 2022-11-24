using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressEnter : MonoBehaviour
{   
    public GameObject blackscreen,press_enter_image,MainButton;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))EnterPressed();
    }

    void EnterPressed()
    {
        blackscreen.SetActive(true);
        MainButton.SetActive(true);
        this.gameObject.GetComponent<MenuButtonSetting>().enabled = true;
        press_enter_image.SetActive(false);
        this.enabled = false;
    }
}
