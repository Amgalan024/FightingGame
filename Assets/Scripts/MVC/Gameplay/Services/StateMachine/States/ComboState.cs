﻿using MVC.Gameplay.Models;
using MVC.Models;
using MVC.Views;
using UnityEngine;
using VContainer.Unity;

namespace MVC.StateMachine.States
{
    public class ComboState : CommonStates.AttackState
    {
        public string Name { set; get; }
        public int Damage { set; get; }

        public ComboState(StateModel stateModel, StateMachineModel stateMachineModel, PlayerView playerView) : base(
            stateModel, stateMachineModel, playerView)
        {
        }

        public override void Enter()
        {
            base.Enter();

            PlayerView.Animator.Play(Name);
            PlayerView.Animator.SetBool(Name, true);
            StateModel.PlayerModel.IsAttacking.Value = true;
            StateModel.PlayerAttackModel.Damage = Damage;
            StateModel.PlayerModel.IsDoingCombo.Value = true;
        }

        public override void Exit()
        {
            base.Exit();

            PlayerView.Animator.SetBool(Name, false);
            StateModel.PlayerModel.IsDoingCombo.Value = false;
        }
    }
}