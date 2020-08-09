using UnityEngine;

namespace Hospital.Locations
{
    public interface ILocationSeeker<TStaff, TPatient> where TPatient : MonoBehaviour where TStaff : MonoBehaviour
    {
        TwoSpotLocation<TStaff, TPatient> GetLocation();
    }
}