﻿using MVC.Gameplay.Models.Player;
using MVC_Pattern.Scripts.Gameplay.Services.StateMachine;
using UnityEngine;

namespace MVC.StateMachine.States
{
    public class CrouchState : IPlayerState, IFixedTickState
    {
        public PlayerContainer PlayerContainer { get; }
        public IStateMachine StateMachine { get; set; }

        public CrouchState(PlayerContainer playerContainer)
        {
            PlayerContainer = playerContainer;
        }

        public void Enter()
        {
            PlayerContainer.Model.IsCrouching.Value = true;

            ConfigureInputActionFilters();
        }

        public void Exit()
        {
            PlayerContainer.Model.IsCrouching.Value = false;
        }

        void IFixedTickState.OnFixedTick()
        {
            HandleCrouchInput();
        }

        private void ConfigureInputActionFilters()
        {
            PlayerContainer.InputActionModelsContainer.SetAllInputActionModelFilters(false);

            PlayerContainer.InputActionModelsContainer.SetAttackInputActionFilters(true);
            PlayerContainer.InputActionModelsContainer.SetBlockInputActionFilters(true);
        }

        private void HandleCrouchInput()
        {
            if (!Input.GetKey(PlayerContainer.InputModelsContainer.Crouch.Key))
            {
                StateMachine.ChangeState<IdleState>();
            }
        }
    }
}