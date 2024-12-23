using System;
using System.Collections.Generic;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using PlayerRoles;
using ScpProximityChat.Enums;

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
                HeaderSetting header = new HeaderSetting(Config.SettingHeaderLabel);
                IEnumerable<SettingBase> settingBases = new SettingBase[]
                {
                    header,
                    new KeybindSetting(Config.KeybindId, Config.KeybindLabel, default, hintDescription: Config.KeybindHint),
                };

                SettingBase.Register(settingBases);
                SettingBase.SendToAll();
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
