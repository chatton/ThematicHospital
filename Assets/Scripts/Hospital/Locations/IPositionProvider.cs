using UnityEngine;
using State.Patient;

namespace Hospital.Locations
{
    public interface IPositionProvider
    {
        Vector3 GetPosition(CharacterType type);
    }
}