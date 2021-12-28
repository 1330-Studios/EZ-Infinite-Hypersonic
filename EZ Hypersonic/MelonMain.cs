using System;
using System.Linq;

using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers.Behaviors.Attack;

using HarmonyLib;

using MelonLoader;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(EZ_InfiniteHypersonic.MelonMain), "EZ Infinite Hypersonic", "1.0", "1330 Studios LLC")]
[assembly: MelonColor(ConsoleColor.Red)]

namespace EZ_InfiniteHypersonic;
public sealed class MelonMain : MelonMod {

    public override void OnApplicationStart() {
        LoggerInstance.Msg("EZ Infinite Hypersonic Loaded!");

        HarmonyInstance.Patch(AccessTools.Method(typeof(GameModelLoader), nameof(GameModelLoader.Load)), postfix: new(AccessTools.Method(typeof(MelonMain), nameof(MelonMain.OnGameLoad))));
    }

    public static void OnGameLoad(ref GameModel __result) {
        __result.towers.AsParallel().ForAll(tower => {
            tower.range = 999;
            tower.behaviors.Do(model => {
                AttackModel potentialAttack = model.TryCast<AttackModel>(); 
                if (potentialAttack != null) {
                    potentialAttack.range = 999;

                    potentialAttack.weapons.Do(weapon => weapon.rate = 0);

                    int length = potentialAttack.weapons.Length;
                    for (int j = 0; j < length; j++)
                        potentialAttack.weapons = potentialAttack.weapons.AddItem(potentialAttack.weapons[j]).ToArray();
                }
            });
        });
    }
}