using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class MyCharacterController
{
    public GameObject character;
    Move moveScript;
    ClickGUI clickGUI;
    bool characterType; //false is priest,true is devil

    bool onBoat;
    CoastController coastController;

    public MyCharacterController(bool Type)
    {
        characterType = Type;
        if (characterType)
            character = Object.Instantiate(Resources.Load("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        else
            character = Object.Instantiate(Resources.Load("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        moveScript = character.AddComponent(typeof(Move)) as Move;

        clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
        clickGUI.setController(this);
    }

    public void setName(string name)
    {
        character.name = name;
    }

    public void setPosition(Vector3 pos)
    {
        character.transform.position = pos;
    }

    public void moveToPosition(Vector3 destination)
    {
        moveScript.setDestination(destination);
    }

    public void getOnBoat(BoatController boat)
    {
        coastController = null;
        character.transform.parent = boat.getGameobj().transform;
        onBoat = true;
    }

    public void getOnCoast(CoastController coast)
    {
        coastController = coast;
        character.transform.parent = null;
        onBoat = false;
    }

    public bool isOnBoat()
    {
        return onBoat;
    }

    public CoastController getCoastController()
    {
        return coastController;
    }

    public string getName()
    {
        return character.name;
    }

    public bool getType()
    {
        return characterType;
    }

    public void reset()
    {
        moveScript.reset();
        coastController = (Director.getInstance().currentSceneController as Controller).startCoast;
        getOnCoast(coastController);
        setPosition(coastController.getEmptyPosition());
        coastController.getOnCoast(this);
    }
}

public class CoastController
{
    public GameObject coast;
    Vector3 from_pos = new Vector3(12, 1, 0);
    Vector3 to_pos = new Vector3(-12, 1, 0);
    Vector3[] positions;
    int side;    // to->-1, from->1
    
    MyCharacterController[] passengerPlaner;
    int sum = 6;

    public CoastController(string _to_or_from,int sum)
    {
        positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0),
                new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0), 
                new Vector3(12.5F,2.25F,0), new Vector3(13.5F,2.25F,0), new Vector3(14.5F,2.25F,0),
                new Vector3(15.5F,2.25F,0), new Vector3(16.5F,2.25F,0), new Vector3(17.5F,2.25F,0)};

        this.sum = sum;

        passengerPlaner = new MyCharacterController[this.sum];

        if (_to_or_from == "start")
        {
            coast = Object.Instantiate(Resources.Load("Perfabs/Stone", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
            coast.name = "start";
            side = 1;
        }
        else
        {
            coast = Object.Instantiate(Resources.Load("Perfabs/Stone", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
            coast.name = "end";
            side = -1;
        }
    }

    public int getEmptyIndex()
    {
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public Vector3 getEmptyPosition()
    {
        Vector3 pos = positions[getEmptyIndex()];
        pos.x *= side;
        return pos;
    }

    public void getOnCoast(MyCharacterController characterCtrl)
    {
        passengerPlaner[getEmptyIndex()] = characterCtrl;
    }

    public MyCharacterController getOffCoast(string passenger_name)
    {   // 0->priest, 1->devil
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] != null && passengerPlaner[i].getName() == passenger_name)
            {
                MyCharacterController charactorCtrl = passengerPlaner[i];
                passengerPlaner[i] = null;
                return charactorCtrl;
            }
        }
        Debug.Log("cant find passenger on coast: " + passenger_name);
        return null;
    }

    public int getSide()
    {
        return side;
    }

    public int[] getCharacterNum()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] == null)
                continue;
            if (!(passengerPlaner[i].getType()))
            {   // 0->priest, 1->devil
                count[0]++;
            }
            else
            {
                count[1]++;
            }
        }
        return count;
    }

    public void reset()
    {
        passengerPlaner = new MyCharacterController[this.sum];
    }
}


public class BoatController
{
    readonly GameObject boat;
    readonly Move moveableScript;
    readonly Vector3 fromPosition = new Vector3(5, 1, 0);
    readonly Vector3 toPosition = new Vector3(-5, 1, 0);
    readonly Vector3[] from_positions;
    readonly Vector3[] to_positions;

    // change frequently
    int side; // to->-1; from->1
    MyCharacterController[] passenger = new MyCharacterController[2];

    public BoatController()
    {
        side = 1;

        from_positions = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
        to_positions = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };

        boat = Object.Instantiate(Resources.Load("Perfabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
        boat.name = "boat";

        moveableScript = boat.AddComponent(typeof(Move)) as Move;
        boat.AddComponent(typeof(ClickGUI));
    }


    public void Move()
    {
        if (side == -1)
        {
            moveableScript.setDestination(fromPosition);
            side = 1;
        }
        else
        {
            moveableScript.setDestination(toPosition);
            side = -1;
        }
    }

    public int getEmptyIndex()
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public bool isEmpty()
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    public Vector3 getEmptyPosition()
    {
        Vector3 pos;
        int emptyIndex = getEmptyIndex();
        if (side == -1)
        {
            pos = to_positions[emptyIndex];
        }
        else
        {
            pos = from_positions[emptyIndex];
        }
        return pos;
    }

    public void GetOnBoat(MyCharacterController characterCtrl)
    {
        int index = getEmptyIndex();
        passenger[index] = characterCtrl;
    }

    public MyCharacterController GetOffBoat(string passenger_name)
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] != null && passenger[i].getName() == passenger_name)
            {
                MyCharacterController charactorCtrl = passenger[i];
                passenger[i] = null;
                return charactorCtrl;
            }
        }
        Debug.Log("Cant find passenger in boat: " + passenger_name);
        return null;
    }

    public GameObject getGameobj()
    {
        return boat;
    }

    public int getWhere()
    { // to->-1; from->1
        return side;
    }

    public int[] getCharacterNum()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] == null)
                continue;
            if (!passenger[i].getType())
            {   // 0->priest, 1->devil
                count[0]++;
            }
            else
            {
                count[1]++;
            }
        }
        return count;
    }

    public void reset()
    {
        moveableScript.reset();
        if (side == -1)
        {
            Move();
        }
        passenger = new MyCharacterController[2];
    }
}
