using UnityEngine;

namespace MiniGame
{
    public class GameManager : MonoBehaviour 
    {
        public GameObject logObject;
        GameObject[] rollers;

        void Awake()
        {
            for(int i = 0; i < logObject.transform.childCount; i++)
            {
                rollers[i] = logObject.transform.GetChild(i).gameObject;
            }
        }

        public void StartGame()
        {
            for(int i = 0; i < rollers.Length; i++)
            {
                rollers[i].GetComponent<RollerController>().enabled = true;
            }
        }
    }
}
