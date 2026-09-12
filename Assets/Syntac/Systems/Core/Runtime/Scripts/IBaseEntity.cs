using UnityEngine;

namespace Syntac.Core
{
    public interface IBaseEntity
    {
        string Name { get; }
        bool IsValid { get; }
        Transform Transform { get; }
        GameObject GameObject { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }

        void SetActive(bool isActive);
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        void Terminate();
    }
}
