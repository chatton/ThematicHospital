using Characters.Patients;
using Hospital.Locations;
using State;
using State.Patient;
using State.Shared;

namespace Characters.Staff
{
    public class Receptionist : BaseStaff, IReceptionVisitor, ILocationSeeker<Receptionist, Patient>
    {
        private ReceptionDesk _targetDesk;
        private bool _isCurrentlyManningStation;

        protected override StateMachine BuildStateMachine()
        {
            StateMachine sm = new StateMachine();
            IState idleState = new IdleState();

            IState seekingReceptionState =
                new SeekingReceptionState(_agent, this, CharacterType.Staff, _animator, this);
            sm.AddTransition(idleState, seekingReceptionState,
                () => !_isCurrentlyManningStation && ReceptionDeskAvailable());

            sm.SetState(idleState);

            return sm;
        }

        private bool ReceptionDeskAvailable()
        {
            ReceptionDesk receptionDesk = FindAvailableReceptionDesks();
            if (receptionDesk == null)
            {
                return false;
            }

            receptionDesk.RegisterStaff(this);
            _targetDesk = receptionDesk;
            return true;
        }


        #region IReceptionVisitor functions

        public void VisitReception()
        {
            _isCurrentlyManningStation = true;
        }

        public void LeaveReception()
        {
            _isCurrentlyManningStation = false;
        }

        #endregion

        public TwoSpotLocation<Receptionist, Patient> GetLocation()
        {
            return _targetDesk;
        }
    }
}