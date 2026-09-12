namespace Syntac.ConfigManagement
{
    /// <summary>
    /// Marker interface for ScriptableObject configs that should be visible in the Config Manager window.
    /// </summary>
    /// <remarks>
    /// Convention: implement both members explicitly (<c>string IVisibleConfig.ConfigName =&gt; ...</c>)
    /// so they do not pollute the config asset's public API or show up in its Inspector.
    /// </remarks>
    public interface IVisibleConfig
    {
        /// <summary>
        /// Display name for this config in the Config Manager.
        /// </summary>
        string ConfigName { get; }

        /// <summary>
        /// Category name for grouping configs (e.g., "Network", "Audio", "Graphics").
        /// </summary>
        string Category { get; }
    }
}
