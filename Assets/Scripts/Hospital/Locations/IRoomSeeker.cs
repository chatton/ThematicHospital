namespace Hospital.Locations
{
    public interface IRoomSeeker<TRoom>
    {
        TRoom GetTargetRoom();
    }
}