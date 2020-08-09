using System.Linq;
using Hospital;
using Hospital.Locations;
using Patients;
using State;
using State.Patient;
using State.Shared;
using State.Staff;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Staff
{
    public class Doctor : MonoBehaviour, IRoomSeeker, IMachineProvider
    {
        private StateMachine _stateMachine;
        private NavMeshAgent _agent;
        private Animator _animator;

        private bool _isCurrentlyManningStation;
        public Patient CurrentPatient { get; set; }

        private Machine _machine;
        private IPositionProvider _positionProvider;

        // private DiagnosisRoom _targetRoom;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _stateMachine = BuildStateMachine();
        }

        private void Update()
        {
            _stateMachine.Tick(Time.deltaTime);
        }

        private bool ThereIsAFreeDiagnosticsRoomThatNeedsADoctor()
        {
            foreach (DiagnosisRoom room in FindObjectsOfType<DiagnosisRoom>())
            {
                if (room.HasRoomForStaff() && room.DoctorIsNeeded)
                {
                    _positionProvider = room;
                    room.RegisterStaff(this);

                    return true;
                }
            }

            return false;
        }

        private bool ThereIsATreatmentRoomThatNeedsADoctor()
        {
            foreach (TreatmentRoom room in FindObjectsOfType<TreatmentRoom>())
            {
                if (room.HasRoomForStaff() && room.DoctorIsNeeded)
                {
                    _positionProvider = room;
                    room.RegisterStaff(this);
                    _machine = room.GetComponentInChildren<Machine>();
                    Assert.IsNotNull(_machine, "The machine of treatment room: " + room.name + " was null!");
                    return true;
                }
            }

            return false;
        }

        private StateMachine BuildStateMachine()
        {
            StateMachine sm = new StateMachine();
            IState idleState = new IdleState();
            IState seekingDiagnostics = new SeekingDiagnosisRoomState(_agent, CharacterType.Staff, _animator, this);
            IState performDiagnosis = new PerformDiagnosisState(this);
            IState preformTreatmentState = new PerformTreatmentState(_agent, this, _animator, this);


            sm.AddTransition(idleState, preformTreatmentState, ThereIsATreatmentRoomThatNeedsADoctor);

            // If there are any diagnostics rooms that need a doctor, we will head there.
            sm.AddTransition(idleState, seekingDiagnostics, ThereIsAFreeDiagnosticsRoomThatNeedsADoctor);

            // We can begin the diagnosis process as soon as we have a patient
            sm.AddTransition(seekingDiagnostics, performDiagnosis, () => CurrentPatient != null);

            // Once we have diagnosed the patient, we can go back to being idle
            sm.AddTransition(performDiagnosis, idleState, () => CurrentPatient == null);


            sm.SetState(idleState);
            return sm;
        }

        public IPositionProvider GetPositionProvider()
        {
            return _positionProvider;
        }

        public Machine GetMachine()
        {
            return _machine;
        }
    }
}