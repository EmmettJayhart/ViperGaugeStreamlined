using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ViperGaugeStreamlined.Windows;

namespace ViperGaugeStreamlined;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IFramework Framework { get; private set; } = null!;
    [PluginService] internal static IGameGui GameGui { get; private set; } = null!;

    public Configuration Configuration { get; init; }
    private ConfigWindow ConfigWindow { get; init; }
    public readonly WindowSystem WindowSystem = new("Viper Gauge Streamlined");
    private const string CommandName = "/vprgauge";

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        ConfigWindow = new ConfigWindow(this);
        WindowSystem.AddWindow(ConfigWindow);
        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open the config"
        });

        Framework.Update += OnUpdate;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        ConfigWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);

        Framework.Update -= OnUpdate;
        ResetVisibility();
    }

    private void DrawUi() => WindowSystem.Draw();
    private void ToggleConfigUi() => ConfigWindow.Toggle();
    private void OnCommand(string command, string args) => ToggleConfigUi();

    private void OnUpdate(IFramework framework)
    {
        var vipersight = GameGui.GetAddonByName("JobHudRDB0");
        if (vipersight != 0)
        {
            var vipersightVisibility = !Configuration.HideVipersight;
            SetVisibility(vipersight, 16, vipersightVisibility);
            SetVisibility(vipersight, 18, vipersightVisibility);
        }

        var anguineTribute = GameGui.GetAddonByName("JobHudRDB1");
        if (anguineTribute != 0)
        {
            var anguineTributeVisibility = !Configuration.HideAnguine;
            SetVisibility(anguineTribute, 3, anguineTributeVisibility);
        }
    }

    private void ResetVisibility()
    {
        var addon0 = GameGui.GetAddonByName("JobHudRDB0");
        if (addon0 != 0)
        {
            SetVisibility(addon0, 16, true);
            SetVisibility(addon0, 18, true);
        }

        var addon1 = GameGui.GetAddonByName("JobHudRDB1");
        if (addon1 != 0)
        {
            SetVisibility(addon1, 3, true);
        }
    }

    private unsafe void SetVisibility(nint addon, uint id, bool visibility)
    {
        var atkAddon = (AtkUnitBase*)addon;
        var element = atkAddon->GetNodeById(id);
        if (element != null && element->IsVisible())
            element->SetAlpha((byte)(visibility ? 0xFF : 0x00));
    }
}
