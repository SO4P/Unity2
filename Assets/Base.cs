using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{

    public class Director : System.Object
    {
        private static Director _instance;
        public SceneController currentSceneController { get; set; }

        public static Director getInstance()
        {
            if (_instance == null)
            {
                _instance = new Director();
            }
            return _instance;
        }
    }

    public interface SceneController
    {
        void loadResources();
    }

    public interface UserAction
    {
        void moveBoat();
        void moveCharacter(MyCharacterController character);
        void reStart();
        void reLoad(int pri,int dev);
    }
}
