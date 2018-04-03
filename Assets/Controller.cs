using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class Controller : MonoBehaviour , SceneController , UserAction
{
    Vector3 waterPosition = new Vector3(0, 0.5f, 0);
    Director director;
    public UserGUI userGUI;

    public CoastController startCoast;
    public CoastController endCoast;
    public BoatController boat;
    public MyCharacterController[] characters;
    public GameObject water;
    int priests = 3;
    int devils = 3;

    void Awake()
    {
        director = Director.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        characters = new MyCharacterController[6];
        loadResources();
    }

    public void loadResources()
    {
        GameObject temp = Instantiate(Resources.Load("Perfabs/Water", typeof(GameObject)), waterPosition, Quaternion.identity, null) as GameObject;
        water = temp;
        water.name = "water";
        startCoast = new CoastController("start",priests + devils);
        endCoast = new CoastController("end",priests + devils);
        boat = new BoatController();

        for(int i = 0;i < priests; i++)
        {
            MyCharacterController Pri = new MyCharacterController(false);
            Pri.setName("priest" + i);
            Pri.setPosition(startCoast.getEmptyPosition());
            Pri.getOnCoast(startCoast);
            startCoast.getOnCoast(Pri);
            characters[i] = Pri;
        }

        for (int i = 0; i < devils; i++)
        {
            MyCharacterController Dev = new MyCharacterController(true);
            Dev.setName("devil" + i);
            Dev.setPosition(startCoast.getEmptyPosition());
            Dev.getOnCoast(startCoast);
            startCoast.getOnCoast(Dev);
            characters[i + priests] = Dev;
        }
    }

    public void reStart()
    {
        boat.reset();
        startCoast.reset();
        endCoast.reset();
        for(int i = 0;i < priests + devils; i++)
        {
            characters[i].reset();
        }
    }

    public void reLoad(int pri,int dev)
    {
        reStart();
        if(pri != priests || dev != devils){
            Destroy(startCoast.coast);
            Destroy(endCoast.coast);
            startCoast = new CoastController("start",pri + dev);
            endCoast = new CoastController("end",pri + dev);

            for (int i = 0; i < priests + devils; i++)
            {
                Destroy(characters[i].character);
            }
            characters = new MyCharacterController[pri + dev];

            for (int i = 0;i < pri; i++)
            {
                MyCharacterController Pri = new MyCharacterController(false);
                Pri.setName("priest" + i);
                Pri.setPosition(startCoast.getEmptyPosition());
                Pri.getOnCoast(startCoast);
                startCoast.getOnCoast(Pri);
                characters[i] = Pri;
            }

            for (int i = 0; i < dev; i++)
            {
                MyCharacterController Dev = new MyCharacterController(true);
                Dev.setName("devil" + i);
                Dev.setPosition(startCoast.getEmptyPosition());
                Dev.getOnCoast(startCoast);
                startCoast.getOnCoast(Dev);
                characters[i + pri] = Dev;
            }
            priests = pri;
            devils = dev;
        }
    }

    public int gameStatus()  // 0 is not finished,1 is win and -1 is lose
    {
        int[] startCoastCounter = new int[2];
        int[] endCoastCounter = new int[2];
        int[] boatCounter = new int[2];
        startCoastCounter = startCoast.getCharacterNum();
        endCoastCounter = endCoast.getCharacterNum();
        boatCounter = boat.getCharacterNum();

        if (endCoastCounter[0] + endCoastCounter[1] == priests + devils)
            return 1;

        // count the numbers of priests and devils on boat and coast
        if (boat.getWhere() == 1)  //boat at startCoast
        {
            startCoastCounter[0] += boatCounter[0];
            startCoastCounter[1] += boatCounter[1];
        }
        else  //boat at endCoast
        {
            endCoastCounter[0] += boatCounter[0];
            endCoastCounter[1] += boatCounter[1];
        }
        if (startCoastCounter[0] < startCoastCounter[1] && startCoastCounter[0] > 0)
            return -1;
        if (endCoastCounter[0] < endCoastCounter[0] && endCoastCounter[0] > 0)
            return -1;
        return 0;
    }

    public void moveBoat()
    {
        if (gameStatus() == 0)
        {
            if (boat.isEmpty())
            {
                Debug.Log("Boat is empty.");
                return;
            }
            boat.Move();
            userGUI.status = gameStatus();
        }
    }

    public void moveCharacter(MyCharacterController character)
    {
        if (gameStatus() == 0)
        {
            if (character.isOnBoat()) //character on boat
            {
                boat.GetOffBoat(character.getName());
                if (boat.getWhere() == -1)
                {
                    character.moveToPosition(endCoast.getEmptyPosition());
                    character.getOnCoast(endCoast);
                    endCoast.getOnCoast(character);
                }
                else
                {
                    character.moveToPosition(startCoast.getEmptyPosition());
                    character.getOnCoast(startCoast);
                    startCoast.getOnCoast(character);
                }
            }
            else  //character at coast
            {
                if (boat.getEmptyIndex() == -1) // boat is full
                {
                    Debug.Log("Boat is full");
                    return;
                }
                if (character.getCoastController().getSide() == boat.getWhere())
                {
                    character.getCoastController().getOffCoast(character.getName());
                    character.moveToPosition(boat.getEmptyPosition());
                    character.getOnBoat(boat);
                    boat.GetOnBoat(character);
                }
            }
            userGUI.status = gameStatus();
        }
    }
}
