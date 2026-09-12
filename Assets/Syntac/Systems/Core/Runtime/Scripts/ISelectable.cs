namespace Syntac.Core
{
    public interface ISelectable
    {
        event OnSelectDelegate Selected;
        event OnDeselectDelegate Deselected;

        bool IsSelected { get; }

        void Select();
        void Deselect();

        delegate void OnSelectDelegate();
        delegate void OnDeselectDelegate();
    }
}
