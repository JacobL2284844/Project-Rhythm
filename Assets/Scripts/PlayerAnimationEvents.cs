using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public CharacterAnimation characterAnimation;
    public ThirdPersonController thirdPersonController;

    public void JumpOffWallRun()
    {
        thirdPersonController.ExitWallRun();
    }

}
