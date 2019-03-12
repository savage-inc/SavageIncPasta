using UnityEngine;

public class Comfort : MonoBehaviour {

    // Variables
    int ComfortLvl = 100;
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // If ComfortLvl is < 100 then return to tarven
       // ComfortLvl is decreased by value constitution * 3 after battles
       // If ComfortLvl is <= 1 then regain if at tarven
       // If all party ComfortLvl is 0 game over
       // If potion used then ComfortLvl is increased
    }
    
}
