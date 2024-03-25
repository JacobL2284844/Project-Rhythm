//jacob lueke, script detects when an interactable walks in this npc, then interacts with the player or npc.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteractingArea : MonoBehaviour
{
    public NPCStateManager my_stateManager;// my state manager: set in inspector
    public RecieveNPCInteractor there_receiver;// specifies the receiving script for an interaction
    public RecieveNPCInteractor my_receiver;// set in inspector

    private void OnTriggerStay(Collider collider)
    {
        there_receiver = collider.transform.GetComponent<RecieveNPCInteractor>();// gets reciever script for found interactable

        if (there_receiver != null)//if reciever has been found
        {
            //my_receiver.myLookObj = there_receiver.myHead;

            if (collider.tag == "Player")
            {
                my_receiver.myLookObj = there_receiver.myHead;//if found player

                my_stateManager.SetState(my_stateManager.chaseState);//enter chase state
                my_stateManager.currentIdleDuration = 0.1f;
            }
            else if (collider.tag == "NPC_Mesh")
            {
                there_receiver.WeInteract_NPC(my_receiver);
                my_receiver.myLookObj = there_receiver.myHead;// tell this npc to look at collided npc's head

                //if finds a npc chasing player
                NPCStateManager there_stateManager = there_receiver.stateManager;
                if (there_stateManager.currantStateStr == "Chace")
                {
                    my_stateManager.currentIdleDuration = 0;
                    my_stateManager.SetState(my_stateManager.chaseState);//set state
                }
            }

        }
    }

    private void OnTriggerExit(Collider collider)//out of interaction range: set by collider
    {
        if (collider.tag == "NPC_Mesh")
        {
            //Debug.Log("bye other npc");
            my_receiver.myLookObj = null;// set look object to nothing
            there_receiver = null;//set saved script for other npc to null.
        }
        else if (collider.tag == "Player")
        {
        }
    }
}