using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyScript : MonoBehaviour
{
    static string[] partyMembers = new string[4];
    static int partyIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(partyIndex >= 1){
            partyMembers[0] = "Player";
        }
        partyMembers[0] = "Solo";
    }

    /*
    public void addMember(Companion companion){
        partyMembers[0] = "Player";
        partyIndex++;
        partyMembers[partyIndex] = companion.getInfo;
    }*/

    public string getPartyInfo(){
        string fullPartyInfo = "Party:";
        for(int i = 0; i < partyIndex; i++){
            if(i < partyIndex - 1){
                fullPartyInfo = fullPartyInfo + partyMembers[i] + ", ";
            }else{
                fullPartyInfo = fullPartyInfo + partyMembers[i];
            }
        }

        return fullPartyInfo;
    }
}