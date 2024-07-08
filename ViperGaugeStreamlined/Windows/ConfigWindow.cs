using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ViperGaugeStreamlined.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

    public ConfigWindow(Plugin plugin) : base("Viper Gauge Streamlined")
    {
        Size = new Vector2(200, 80);
        SizeCondition = ImGuiCond.FirstUseEver;

        configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var hideVipersight = configuration.HideVipersight;
        if (ImGui.Checkbox("Hide Vipersight", ref hideVipersight))
        {
            configuration.HideVipersight = hideVipersight;
            configuration.Save();
        }

        var hideAnguine = configuration.HideAnguine;
        if (ImGui.Checkbox("Hide Anguine Tribute", ref hideAnguine))
        {
            configuration.HideAnguine = hideAnguine;
            configuration.Save();
        }
    }
}
