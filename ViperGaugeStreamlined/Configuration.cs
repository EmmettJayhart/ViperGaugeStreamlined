using Dalamud.Configuration;
using System;

namespace ViperGaugeStreamlined;

[Serializable]
public class Configuration : IPluginConfiguration
{

    public int Version { get; set; } = 1;

    public bool HideVipersight { get; set; } = true;

    public bool HideAnguine { get; set; } = true;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
