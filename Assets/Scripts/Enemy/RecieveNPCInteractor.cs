//07/04/2023
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveNPCInteractor : MonoBehaviour
{
    public Transform myHead;// this characters head: if npc set in inspector: set to armature spine, if player set to main camera
    public Transform myLookObj;// specifies the currant object for the interacting npc to look at

    public bool isNPC = true;//set in inspector, false if player

    //look at
    [SerializeField] private Animator animator;
    public float lookWeight = 1f;

    public Vector3 modelPos = new Vector3(0, 0, 0);

    public NPCStateManager stateManager;//used for chase
    private void Awake()
    {
        if (gameObject.tag == "NPC")// if this receiver is on a npc
        {
            isNPC = true;

            animator = GetComponent<Animator>();
        }
        else if (gameObject.tag == "Player")// if this receiver is on player
        {
            isNPC = false;
            myLookObj = myHead;
        }
    }

    //if two npc's interacting
    public void WeInteract_NPC(RecieveNPCInteractor other_reciever)// specifies the state manager of the other interacting npc
    {
        //myLookObj is other npc's head
        myLookObj = other_reciever.myHead;//sets the object for self to look at as other npc's head
        Debug.Log("two npc's interact");
    }
    void OnAnimatorIK()// npc too animate head to look at
    {
        //if the IK is active, set the position and rotation directly to the goal
        // Set the look target position, if one has been assigned
        if (myLookObj != null)
        {
            animator.SetLookAtWeight(lookWeight);
            animator.SetLookAtPosition(myLookObj.position);
        }
        else//if nothing to look at
        {
            animator.SetLookAtWeight(0);
        }
    }

    public void CancleIdleActionAnimations()
    {
        //stateManager.CancleIdleActions();
    }

    public void AttackAnimStart()//all called in punch animation
    {
        //stateManager.AttackAnimStart();
    }
    public void AttackAnimHit()
    {
        //stateManager.AttackAnimHit();
    }
    public void AttackAnimEnd()
    {
        //stateManager.AttackAnimEnd();
    }
}