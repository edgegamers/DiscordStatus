﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace DiscordStatus
{
    public partial class DiscordStatus
    {
        [ConsoleCommand("css_request", "Request players from discord")]
        [RequiresPermissions("@css/generic")]
        public async void RequestPlayers(CCSPlayerController? player, CommandInfo command)
        {
            if (_chores.IsPlayerValid(player))
            {
                if (_chores.IsURLValid(_g.WConfig.StatusWebhookURL))
                {
                    await _webhook.RequestPlayers(player.PlayerName);
                    if (!_chores.IsPlayerValid(player)) return;
                    DSLog.LogToChat(player, "{GREEN}Request Sent");
                }
                else
                {
                    DSLog.Log(2, "Invalid webhook URL!");
                }
            }
        }

        [ConsoleCommand("css_update_names", "Update Name formats and save it to config")]
        [CommandHelper(minArgs: 1, usage: "[css_update_names {FLAG} {NAME}: KD | {KD}]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        [RequiresPermissions("@css/root")]
        public async void UpdateNames(CCSPlayerController? player, CommandInfo command)
        {
            _update.Stop();
            var names = command.ArgString;
            _g.NameFormat = names;
            await ConfigManager.SaveAsync("EmbedConfig", "NameFormat", names);
            await UpdateAsync();
            _update.Start();
            if (!_chores.IsPlayerValid(player)) return;
            DSLog.LogToChat(player, $"{{GREEN}}Name format updated to '{names}'!");
        }

        [ConsoleCommand("css_update_settings", "update embed config settings")]
        [RequiresPermissions("@css/root")]
        public async void UpdateEmbedConfig(CCSPlayerController? player, CommandInfo command)
        {
            _update.Stop();
            await ConfigManager.UpdateAsync(_g);
            await UpdateAsync();
            _update.Start();
            if (!_chores.IsPlayerValid(player)) return;
            DSLog.LogToChat(player, "{GREEN}Updated Embed with the updated config settings!");
        }
    }
}