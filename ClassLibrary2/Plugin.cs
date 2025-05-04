using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace Plugin
{
    public class RadioactiveOutburst : Plugin<Config>
    {
        public override string Name => "RadioactiveOutburst";
        public override string Author => "Takizava";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 5, 2);

        private bool _isOutburstActive;
        private readonly List<CoroutineHandle> _coroutines = new List<CoroutineHandle>();

        public override void OnEnabled()
        {
            base.OnEnabled();
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Player.UsingItemCompleted += OnUsingItemCompleted;
            Log.Info("RadioactiveOutburst plugin enabled!");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Player.UsingItemCompleted -= OnUsingItemCompleted;
            StopAllCoroutines();
            base.OnDisabled();
            Log.Info("RadioactiveOutburst plugin disabled!");
        }

        private void OnRoundStarted()
        {
            StopAllCoroutines();
            _coroutines.Add(Timing.RunCoroutine(OutburstCycle()));
        }

        private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
        {
            StopAllCoroutines();
        }

        private void OnUsingItemCompleted(UsingItemCompletedEventArgs ev)
        {
            if (ev.Player == null || ev.Item == null) return;
            if (ev.Item.Type == ItemType.SCP500 && _isOutburstActive)
            {
                ev.Player.ShowHint("SCP-500 protected you from radiation!", 5f);
                ev.Player.EnableEffect(EffectType.AntiScp207, 30f);
            }
        }

        private IEnumerator<float> OutburstCycle()
        {
            Log.Debug("Starting OutburstCycle coroutine");
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(Config.OutburstInterval);

                if (!_isOutburstActive && Round.IsStarted)
                {
                    StartOutburst();
                    yield return Timing.WaitForSeconds(Config.OutburstDuration);
                    EndOutburst();
                }
            }
            Log.Debug("OutburstCycle coroutine stopped");
        }

        private void StartOutburst()
        {
            if (!Round.IsStarted) return;
            _isOutburstActive = true;
            Map.Broadcast(10, Config.OutburstStartMessage);
            _coroutines.Add(Timing.RunCoroutine(DamagePlayersInDangerZones()));
            Log.Debug("Radioactive outburst started");
        }

        private void EndOutburst()
        {
            _isOutburstActive = false;
            Map.Broadcast(5, Config.OutburstEndMessage);
            Log.Debug("Radioactive outburst ended");
        }

        private IEnumerator<float> DamagePlayersInDangerZones()
        {
            Log.Debug("Starting DamagePlayersInDangerZones coroutine");
            while (_isOutburstActive && Round.IsStarted)
            {
                foreach (Player player in Player.List.Where(p => p != null && !p.IsDead && !IsInSafeZone(p)))
                {
                    if (!player.ActiveEffects.Any(e => e.GetEffectType() == EffectType.AntiScp207))
                    {
                        player.Hurt(Config.DamagePerTick, "Radioactive outburst");
                        player.ShowHint("You're in a radiation zone! Find shelter or use SCP-500!", 2f);
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
            Log.Debug("DamagePlayersInDangerZones coroutine stopped");
        }

        private bool IsInSafeZone(Player player)
        {
            return player != null && Config.SafeZones.Contains(player.Zone);
        }

        private void StopAllCoroutines()
        {
            foreach (var coroutine in _coroutines)
            {
                if (!coroutine.IsRunning) continue;
                Timing.KillCoroutines(coroutine);
            }
            _coroutines.Clear();
            _isOutburstActive = false;
            Log.Debug("All coroutines stopped");
        }
    }
}