using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Essentials;
using Vintagestory.ServerMods;

[assembly: ModInfo("temperateedit", "temperateedit",
                    Authors = new string[] { "Gregno24" },
                    Version = "1.2.0")]

namespace TemperateEdit
{
    public class TemperateEditModSystem : ModSystem
    {
        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);
            var harmony = new Harmony("gregno24.TemperateEdit");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(GenMaps), "initWorldGen")]
    public class TemperateEditPatch
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            bool foundRealistic = false;

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldstr && (string)codes[i].operand == "realistic")
                {
                    foundRealistic = true;
                    continue;
                }

                if (foundRealistic)
                {
                    if (codes[i].opcode == OpCodes.Ldc_I4_6)
                    {
                        codes[i] = new CodeInstruction(OpCodes.Ldc_I4, 12);
                    }
                    else if (codes[i].opcode == OpCodes.Ldc_I4_S && (sbyte)codes[i].operand == 14)
                    {
                        codes[i] = new CodeInstruction(OpCodes.Ldc_I4, 16);
                        break;
                    }
                }
            }

            return codes;
        }
    }
}