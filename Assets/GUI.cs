using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class ClickGUI : MonoBehaviour
{
    UserAction action;
    MyCharacterController characterController;

    public void setController(MyCharacterController characterCtrl)
    {
        characterController = characterCtrl;
    }

    void Start()
    {
        action = Director.getInstance().currentSceneController as UserAction;
    }

    void OnMouseDown()
    {
        if (gameObject.name == "boat")
        {
            action.moveBoat();
        }
        else
        {
            action.moveCharacter(characterController);
        }
    }
}

public class UserGUI : MonoBehaviour
{
    UserAction action;
    public int status = 0;
    GUIStyle style;
    GUIStyle buttonStyle;
    int priest = 3;
    int devil = 3;
    public string Pri = "priest";
    public string Dev = "devil";

    void Start()
    {
        action = Director.getInstance().currentSceneController as UserAction;

        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
    }
    void OnGUI()
    {
        if (status == -1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Gameover!", style);
        }
        else if (status == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", style);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 70, 2 * Screen.height / 3, 140, 80), "Restart", buttonStyle))
        {
            status = 0;
            action.reStart();
        }
        Pri = GUI.TextField(new Rect(80, 0, 60, 30), Pri);
        Dev = GUI.TextField(new Rect(80, 40, 60, 30), Dev);
        if (GUI.Button(new Rect(0,0,60,60),"Change")){
            int temp1 = 0;
            if(Pri != "priest" && int.TryParse(Pri, out temp1))
            {
                priest = temp1;
            }
            int temp2 = 0;
            if(Dev != "devil" && int.TryParse(Dev, out temp2))
            {
                devil = temp2;
            }
            status = 0;
            if (priest < devil)
                Debug.Log("Devils are more than Priests.");
            else if (priest + devil <= 12)
                action.reLoad(priest, devil);
            else
                Debug.Log("Too many characters.The sum of character must smaller than 13.");
        }
    }
}

