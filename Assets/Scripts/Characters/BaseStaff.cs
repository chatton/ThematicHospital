using Hospital.Locations;
using State;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public abstract class BaseStaff : Character
    {
        protected Room FindAvailableRoomWithDoctorNeededOfType(RoomType type)
        {
            foreach (Room room in FindObjectsOfType<Room>())
            {
                if (room.HasRoomForStaff() && room.DoctorIsNeeded && room.Type == type)
                {
                    return room;
                }
            }

            return null;
        }


        protected TwoSpotLocation<TBaseStaff, TPatient> FindAvailableTwoSpotLocation<TBaseStaff, TPatient>()
            where TBaseStaff : BaseStaff where TPatient : MonoBehaviour
        {
            foreach (TwoSpotLocation<TBaseStaff, TPatient> location in
                FindObjectsOfType<TwoSpotLocation<TBaseStaff, TPatient>>())
            {
                if (location.HasRoomForStaff())
                {
                    return location;
                }
            }

            return null;
        }

        protected ReceptionDesk FindAvailableReceptionDesks()
        {
            // find the first desk that that has a free slot
            foreach (ReceptionDesk receptionDesk in FindObjectsOfType<ReceptionDesk>())
            {
                if (receptionDesk.HasRoomForStaff())
                {
                    return receptionDesk;
                }
            }

            return null;
        }
    }
}