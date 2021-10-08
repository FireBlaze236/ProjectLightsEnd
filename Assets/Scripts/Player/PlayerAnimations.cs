using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] PlayerMovement _playerMovementController;


    private void OnEnable()
    {
        _playerMovementController.OnPlayerMove += SetAnimatorBool;
        _playerMovementController.OnPlayerSpeedChange += SetAnimatorFloat;
        _playerMovementController.OnPlayerFall += SetAnimatorBool;
        _playerMovementController.OnJumpStart += SetAnimatorTrigger;
        _playerMovementController.OnJumpBlend += SetAnimatorLayerWeight;
    }


    private void SetAnimatorBool(string name, bool val)
    {
        _playerAnimator.SetBool(name, val);
    }

    private void SetAnimatorFloat(string name, float val)
    {
        _playerAnimator.SetFloat(name, val);
    }

    private void SetAnimatorTrigger(string name)
    {
        _playerAnimator.SetTrigger(name);
    }

    private void SetAnimatorLayerWeight(string name, float weight)
    {
        _playerAnimator.SetLayerWeight(_playerAnimator.GetLayerIndex(name), weight);
    }
}
