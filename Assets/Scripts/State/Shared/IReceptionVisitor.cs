namespace State.Shared
{
    public interface IReceptionVisitor
    {
        void VisitReception();
        void LeaveReception();
    }
}