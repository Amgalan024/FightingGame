﻿using Cysharp.Threading.Tasks;
using MVC.Gameplay.Constants;
using MVC.Gameplay.Models;
using MVC.Gameplay.Services;
using MVC.Views;
using UnityEngine;

namespace MVC.StateMachine.States
{
    public class DashForwardState : State, IFixedTickState
    {
        public DashForwardState(StateModel stateModel, PlayerView playerView, FightSceneStorage storage) : base(
            stateModel, playerView, storage)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateModel.InputActionModelsContainer.SetAllInputActionModels(false);

            StateModel.InputActionModelsContainer.SetAttackInputActionsFilter(true);
            StateModel.InputActionModelsContainer.SetJumpInputActionsFilter(true);

            PlayerView.IdleToMoveAnimationAsync(PlayerAnimatorData.Forward, Token).Forget();
        }

        public override void Exit()
        {
            base.Enter();

            PlayerView.MoveToIdleAnimationAsync(PlayerAnimatorData.Forward, Token).Forget();
        }

        void IFixedTickState.OnFixedTick()
        {
            if (!Input.GetKey(StateModel.InputModelsContainer.MoveForward.Key))
            {
                PlayerView.Rigidbody.velocity = Vector3.zero;

                StateModel.StateMachineProxy.ChangeState<IdleState>();
            }
            else
            {
                var velocity = PlayerView.Rigidbody.velocity;

                velocity.x = StateModel.PlayerModel.MaxMovementSpeed * 2 * PlayerView.GetPlayerDirection();

                PlayerView.Rigidbody.velocity = velocity;
            }
        }
    }
}