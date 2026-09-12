using UnityEngine;

namespace Syntac.Core
{
    /// <summary>
    /// Base MonoBehaviour entity. Caches <c>gameObject</c>/<c>transform</c> at Awake to avoid the
    /// per-access native marshalling cost.
    /// </summary>
    /// <remarks>
    /// Never override <c>Awake</c> in a subclass — override <see cref="OnAwake"/> instead. Overriding
    /// <c>Awake</c> skips <see cref="Authorize"/> and leaves every cached reference null. If a subclass
    /// needs the cache before <c>Awake</c> (e.g. it was pooled and configured pre-activation), call
    /// <see cref="Authorize"/> explicitly first; the guard makes it idempotent.
    /// </remarks>
    public abstract class MonoEntity : MonoBehaviour, IBaseEntity
    {
        private GameObject m_GameObject;
        private Transform m_Transform;
        private bool m_IsAuthorized;

        public string Name => m_GameObject.name;
        public bool IsValid => m_IsAuthorized;
        public Transform Transform => m_Transform;
        public GameObject GameObject => m_GameObject;
        public Vector3 Position => m_Transform.position;
        public Quaternion Rotation => m_Transform.rotation;

        private void Awake()
        {
            if (!m_IsAuthorized)
                Authorize();

            OnAwake();
        }

        protected void Authorize()
        {
            m_IsAuthorized = true;
            m_GameObject = gameObject;
            m_Transform = transform;
        }

        // Protected methods for derived classes
        protected virtual void OnAwake() { }
        protected void SetActive(bool isActive) => m_GameObject.SetActive(isActive);
        protected void SetPosition(Vector3 position) => m_Transform.position = position;
        protected void SetRotation(Quaternion rotation) => m_Transform.rotation = rotation;
        protected void Terminate() => Destroy(m_GameObject);

        // Explicit interface implementations
        void IBaseEntity.SetActive(bool isActive) => SetActive(isActive);
        void IBaseEntity.SetPosition(Vector3 position) => SetPosition(position);
        void IBaseEntity.SetRotation(Quaternion rotation) => SetRotation(rotation);
        void IBaseEntity.Terminate() => Terminate();
    }
}
