using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using ScpProximityChat.Enums;
using UserSettings.ServerSpecific;

namespace ScpProximityChat
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "ScpProximityChat";
        public override string Author { get; } = "Bolton";
        public override Version Version { get; } = new(1, 0, 2);
        public override Version RequiredExiledVersion { get; } = new(9, 0, 0);

        private EventHandlers _eventHandlers;

        public override void OnEnabled()
        {
            Config.ScpRoles.RemoveWhere(role => !role.IsScp() || role == RoleTypeId.Scp079);

            _eventHandlers = new EventHandlers(Config);
            _eventHandlers.RegisterEvents();

            if (Config.ActivationType == ActivationType.ServerSpecificSettings)
            {
                List<ServerSpecificSettingBase> settings = ServerSpecificSettingsSync.DefinedSettings?.ToList() ?? new List<ServerSpecificSettingBase>();
                settings.AddRange(new ServerSpecificSettingBase[]
                {
                    new SSGroupHeader(Config.SettingHeaderLabel),
                    new SSKeybindSetting(Config.KeybindId, Config.KeybindLabel, hint: Config.KeybindHint)
                });
                ServerSpecificSettingsSync.DefinedSettings = settings.ToArray();
                ServerSpecificSettingsSync.SendToAll();
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _eventHandlers.UnregisterEvents();

            base.OnDisabled();
        }
    }
}
