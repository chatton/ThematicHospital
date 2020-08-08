using Hospital.Locations;

namespace State.Shared
{
    public interface IReceptionVisitor
    {
        void VisitReception();
        void LeaveReception();
        ReceptionDesk TargetReceptionDesk();
    }
}