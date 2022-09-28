#nullable enable
using System;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace LeaderboardCore.Configuration
{
    internal class PluginConfig : IDisposable
    {
        public string? LastLeaderboard { get; set; } = null;
        
        public virtual void Changed() { }
        
        public void Dispose()
        {
            Changed();
        }
    }
}