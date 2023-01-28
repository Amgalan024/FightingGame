﻿using System;
using MVC.Configs.Enums;
using MVC.Gameplay.Constants;
using MVC.Gameplay.Models.Player;
using MVC.Models;
using MVC.StateMachine.States;
using MVC_Pattern.Scripts.Gameplay.Models.StateMachineModels.StateModels;
using MVC_Pattern.Scripts.Gameplay.Services.StateMachine;
using VContainer.Unity;

namespace MVC.Controllers
{
    public class PlayerInputController : IInitializable, ITickable, IDisposable
    {
        private readonly IStateMachine _stateMachine;
        private readonly RunStateModel _runStateModel;

        private readonly PlayerModel _playerModel;
        private readonly InputFilterModelsContainer _inputFilterModelsContainer;

        public PlayerInputController(PlayerContainer playerContainer, IStateMachine stateMachine,
            RunStateModel runStateModel)
        {
            _stateMachine = stateMachine;
            _runStateModel = runStateModel;
            _playerModel = playerContainer.Model;
            _inputFilterModelsContainer = playerContainer.InputFilterModelsContainer;
        }

        void IInitializable.Initialize()
        {
            _playerModel.OnTurned += _inputFilterModelsContainer.SwitchMovementControllers;

            HandleAttackInput();
            HandleJumpInput();
            HandleMovementInput();
        }

        void ITickable.Tick()
        {
            foreach (var inputFilterModel in _inputFilterModelsContainer.InputFilterModelsByType)
            {
                inputFilterModel.Value.HandleInputDown();
                inputFilterModel.Value.HandleInputUp();
            }

            HandleBlockInput();
        }

        void IDisposable.Dispose()
        {
            _playerModel.OnTurned -= _inputFilterModelsContainer.SwitchMovementControllers;
        }

        private void HandleAttackInput()
        {
            _inputFilterModelsContainer.InputFilterModelsByType[ControlType.Punch].OnInputDown +=
                _stateMachine.ChangeState<PunchState>;

            _inputFilterModelsContainer.InputFilterModelsByType[ControlType.Kick].OnInputDown +=
                _stateMachine.ChangeState<KickState>;
        }

        private void HandleJumpInput()
        {
            _inputFilterModelsContainer.InputFilterModelsByType[ControlType.Jump].OnInputDown +=
                _stateMachine.ChangeState<JumpState>;
        }

        private void HandleMovementInput()
        {
            _inputFilterModelsContainer.InputFilterModelsByType[ControlType.MoveForward].OnInputDown += () =>
            {
                _runStateModel.SetData(_inputFilterModelsContainer.InputFilterModelsByType[ControlType.MoveForward],
                    MovementType.Forward, PlayerAnimatorData.Forward);

                _stateMachine.ChangeState<RunState>();
            };

            _inputFilterModelsContainer.InputFilterModelsByType[ControlType.MoveBackward].OnInputDown += () =>
            {
                _runStateModel.SetData(
                    _inputFilterModelsContainer.InputFilterModelsByType[ControlType.MoveBackward],
                    MovementType.Backward, PlayerAnimatorData.Backward);

                _stateMachine.ChangeState<RunState>();
            };

            _inputFilterModelsContainer.InputFilterModelsByType[ControlType.Crouch].OnInputDown += () =>
            {
                _stateMachine.ChangeState<CrouchState>();
            };
        }

        private void HandleBlockInput()
        {
            _playerModel.IsBlocking.Value =
                _inputFilterModelsContainer.InputFilterModelsByType[ControlType.Block].IsKeyPressedWithFilter;
        }
    }
}