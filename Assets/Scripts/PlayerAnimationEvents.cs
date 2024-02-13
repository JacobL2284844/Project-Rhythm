using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public CharacterAnimation characterAnimation;
    public ThirdPersonController thirdPersonController;

    [SerializeField]GameObject weaponEquiped;
    [SerializeField]GameObject weaponStored;
    public void JumpOffWallRun()
    {
        thirdPersonController.WallJump();
        thirdPersonController.ExitWallRun();
    }
    public void ShowWeapon()
    {
        weaponEquiped.SetActive(true);
        weaponStored.SetActive(false);
    }
    public void HideWeapon()
    {
        weaponEquiped.SetActive(false);
        weaponStored.SetActive(true);
    }
}
