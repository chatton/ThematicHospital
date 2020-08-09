using Characters;
using UnityEngine;

namespace Hospital.Locations
{
    public interface ILocationSeeker<TStaff, TPatient> where TPatient : MonoBehaviour where TStaff : BaseStaff
    {
        TwoSpotLocation<TStaff, TPatient> GetLocation();
    }
}