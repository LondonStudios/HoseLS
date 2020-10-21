using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace HoseScript
{
    public class Main : BaseScript
    {
        public bool HoseActivated = false;
        public bool ButtonPressed = false;

        public bool SelectedPedWeapon = false;

        public bool usedScript = false;

        public Dictionary<int, float> SyncedParticles = new Dictionary<int, float> { };
        public Main()
        {
            RequestParticles();

            TriggerEvent("chat:addSuggestion", "/hose", "Activate or deactivate the fire hose");

            EventHandlers["Client:SyncAtOffset"] += new Action<Vector3, Vector3, float, float>(async (coords, offset, heading, pitch) =>
            {
                UseParticleFxAssetNextCall("core");
                SetParticleFxShootoutBoat(1);

                var handle = StartParticleFxLoopedAtCoord("water_cannon_jet", coords.X, coords.Y, coords.Z, pitch, 0f, heading, 0.5f, false, false, false, false);
                Offset(offset, heading, pitch);
                await Delay(200);
                StopParticleFxLooped(handle, false);
            });

            // Types: 1 = Add, 2 = Remove, 3 = Update Pitch
            EventHandlers["Client:SyncEntityLoop"] += new Action<int, float, int>((netId, pitch, type) =>
            {
                if (type == 1)
                {
                    SyncedParticles.Add(netId, pitch);
                    ContinueParticles(netId, NetToPed(netId));
                }
                else if (type == 2)
                {
                    if (SyncedParticles.ContainsKey(netId))
                    {
                        SyncedParticles.Remove(netId);
                    }
                }
                else if (type == 3)
                {
                    SyncedParticles[netId] = pitch;
                }
            });
        }


        private async void ContinueParticles(int netId, int entity)
        {
            UseParticleFxAssetNextCall("core");
            var handle = StartParticleFxLoopedOnEntity("water_cannon_jet", entity, 0.2f, 0.15f, 0.0f, 0.1f, 0.0f, 0.0f, 0.7f, false, false, false);
            var coords = GetEntityCoords(entity, true);
            UseParticleFxAssetNextCall("core");
            var handleOffset = StartParticleFxLoopedOnEntity("water_cannon_spray", entity, 0.2f, 9.0f + SyncedParticles[netId] * 0.4f, 0f, 0.1f, 0.0f, 0.0f, 0.9f, false, false, false);      
            UseParticleFxAssetNextCall("core");
            var handleOffset2 = StartParticleFxLoopedOnEntity("water_cannon_spray", entity, 0.2f, 9.0f + SyncedParticles[netId] * 0.4f, 0f, 0.1f, 0.0f, 0.0f, 1.2f, false, false, false);
            while (SyncedParticles.ContainsKey(netId))
            {
                SetParticleFxLoopedOffsets(handle, 0.26f, 0.2f, 0.13f, SyncedParticles[netId], 0.0f, 0.0f);
                SetParticleFxLoopedOffsets(handleOffset, 0.2f, 9.5f + SyncedParticles[netId] * 0.4f, -0.6f, SyncedParticles[netId], 0.0f, 0.8f);
                SetParticleFxLoopedOffsets(handleOffset2, 0.2f, 5.0f + SyncedParticles[netId] * 0.4f, SyncedParticles[netId] - 23.0f, SyncedParticles[netId], 0.0f, 0.0f);
                await Delay(100);
            }
            StopParticleFxLooped(handle, false);
            StopParticleFxLooped(handleOffset, false);
            StopParticleFxLooped(handleOffset2, false);
            await Delay(0);
        }

        private async void Offset(Vector3 offset, float heading, float pitch)
        {
            UseParticleFxAssetNextCall("core");
            SetParticleFxShootoutBoat(1);
            var handle = StartParticleFxLoopedAtCoord("water_cannon_spray", offset.X, offset.Y, offset.Z, pitch, 0f, heading, 1f, false, false, false, false);
            await Delay(200);
            StopParticleFxLooped(handle, false);
            await Delay(0);
        }

        private void ShowNotification(string text)
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString(text);
            EndTextCommandThefeedPostTicker(false, false);
        }

        [Command("hose")]
        private void HoseCommand()
        {
            if (HoseActivated)
            {
                ShowNotification("Fire hose is now ~r~disabled~w~.");
                HoseActivated = false;
                ButtonPressed = false;
                RemoveWeaponFromPed(PlayerPedId(), (uint)GetHashKey("WEAPON_FIREEXTINGUISHER"));
            }
            else
            {
                ShowNotification("Fire hose is now ~b~enabled~w~.");
                if (!usedScript)
                {
                    ShowNotification("~p~HoseLS ~w~made by ~b~London Studios ~w~and ~b~Adam Fenton~w~.");
                    usedScript = true;
                }
                HoseActivated = true;
                var weapon = GetHashKey("WEAPON_FIREEXTINGUISHER");
                var ped = PlayerPedId();
                GiveWeaponToPed(ped, (uint)weapon, 1000, false, false);
                SetCurrentPedWeapon(ped, (uint)weapon, true);
                DisableControls();
                ContinueHose(ped);
            }
        }

        private async void ControlParticles()
        {
            var net = PedToNet(PlayerPedId());
            if (IsPlayerFreeAiming(PlayerId()))
            {
                TriggerServerEvent("Server:SyncEntityLoop", net, GetGameplayCamRelativePitch(), 2);
                TriggerServerEvent("Server:SyncEntityLoop", net, GetGameplayCamRelativePitch(), 1);

                while (ButtonPressed)
                {
                    var relativePitch = GetGameplayCamRelativePitch();
                    var offset = GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0.2f, 9.0f + relativePitch * 0.4f, 0.0f);
                    float groundZ = 0f;
                    GetGroundZFor_3dCoord(offset.X, offset.Y, offset.Z, ref groundZ, false);
                    TriggerServerEvent("Server:SyncEntityLoop", net, GetGameplayCamRelativePitch(), 3);

                    await Delay(100);
                }
                TriggerServerEvent("Server:SyncEntityLoop", net, GetGameplayCamRelativePitch(), 2);
            }
            await Delay(0);
        }

        private async void ContinueHose(int ped)
        {
            var playerId = PlayerId();
            while (HoseActivated) 
            {
                if (SelectedPedWeapon && IsDisabledControlPressed(0, 24))
                {
                    ButtonPressed = true;
                    ControlParticles();
                    while (IsDisabledControlPressed(0, 24))
                    {
                        if (!IsPlayerFreeAiming(playerId))
                        {
                            Debug.WriteLine("false");
                            break;
                        }
                        if (IsPedInAnyVehicle(ped, true) || IsPauseMenuActive() || !SelectedPedWeapon)
                        {
                            ButtonPressed = false;
                            break;
                        }
                        await Delay(0);
                    }
                    ButtonPressed = false;
                }
                await Delay(0);
            }
            await Delay(0);
        }

        private async void DisableControls()
        {
            var weapon = GetHashKey("WEAPON_FIREEXTINGUISHER");
            var ped = PlayerPedId();
            while (HoseActivated)
            {
                SelectedPedWeapon = (GetSelectedPedWeapon(ped) == weapon);
                if (SelectedPedWeapon)
                {
                    DisableControlAction(0, 24, true);
                    DisablePlayerFiring(PlayerId(), true);
                }
                await Delay(0);
            }
            await Delay(0);
        }

        public async void RequestParticles()
        {
            RequestNamedPtfxAsset("core");
            while (!HasNamedPtfxAssetLoaded("core"))
            {
                await Delay(100);
            }
            UseParticleFxAssetNextCall("core");
        }
    }
}
