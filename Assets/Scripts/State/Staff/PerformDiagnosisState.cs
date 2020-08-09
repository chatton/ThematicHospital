using System;
using System.Net.Sockets;
using Conditions;
using Core;
using State;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Staff
{
    public class PerformDiagnosisState : IState
    {
        private readonly Doctor _doctor;


        private float _elapsedTime;
        private readonly RotationHandler _rotationHandler;
        private Animator _animator;
        private static readonly int Talking = Animator.StringToHash("Talking");
        private static readonly int Walking = Animator.StringToHash("Walking");

        public PerformDiagnosisState(Doctor doctor)
        {
            _doctor = doctor;
            _rotationHandler = doctor.GetComponent<RotationHandler>();
            _animator = doctor.GetComponentInChildren<Animator>();
        }

        public void OnEnter()
        {
            Debug.Log("Performing diagnosis!");
            Assert.IsNotNull(_doctor.CurrentPatient, "Doctor entered PerformDiagnosisState when the patient was null!");
            _rotationHandler.SetTarget(_doctor.CurrentPatient.transform.position);
            _animator.SetBool(Talking, true);
        }

        public void OnExit()
        {
            Debug.Log("Exiting diagnosis state!");
            _animator.SetBool(Talking, false);
            _animator.SetBool(Walking, false);
            _rotationHandler.ClearTarget();
        }

        public void Tick(float deltaTime)
        {
            if (_doctor.CurrentPatient.HasBeenDiagnosed)
            {
                return;
            }

            if (_doctor.GetComponent<NavMeshAgent>().destination == _doctor.transform.position)
            {
                _animator.SetBool(Walking, false);
                _doctor.GetComponent<RotationHandler>().SetTarget(_doctor.CurrentPatient.transform.position);
            }

            _elapsedTime += deltaTime;
            // TODO: Doctor stats can effect diagnosis time
            if (_elapsedTime >= _doctor.CurrentPatient.condition.baseSecondToDiagnosis)
            {
                _elapsedTime = 0;
                _doctor.CurrentPatient.HasBeenDiagnosed = true;
                _doctor.CurrentPatient = null;
            }
        }
    }
}