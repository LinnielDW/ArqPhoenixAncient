using ArqPhoenixAncient.Relics;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Models;

namespace ArqPhoenixAncient.Ancients;

// [HarmonyPatch(typeof(Underdocks),"AllAncients", MethodType.Getter)]
// public class Underdocks_AllAncients_Patch
// {
//     public static void Postfix(ref IEnumerable<AncientEventModel> __result)
//     {
//         __result =  new List<AncientEventModel> {ModelDb.AncientEvent<PhoenixAncient>()};
//     }
// }

public class PhoenixAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools => new(
        MakePool(
            AncientOption<PhoenixHeart>(),
            AncientOption<PhoenixTalon>(),
            AncientOption<PhoenixFeather>()
        ),
        MakePool(
            AncientOption<PhoenixWrath>(),
            AncientOption<PhoenixPride>(),
            AncientOption<PhoenixGreed>()
        ),
        MakePool(
            AncientOption<PhoenixBrazier>(),
            AncientOption<PhoenixPyre>(),
            AncientOption<PhoenixTorch>()
            // AncientOption<PhoenixTears>()
        )
    );


    public override Color ButtonColor => new(0.25f, 0.3f, 0.4f, 0.8f);

    public override Color DialogueColor => new("422622");

    public override bool IsValidForAct(ActModel act)
    {
        return ActModelExtensions.ActNumber(act) >= 3;
    }

    protected override AncientDialogueSet DefineDialogues()
    {
        /*Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
            string text = CharKey<Defect>();
            dictionary[text] = new AncientDialogue[]
            {
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                }
            };
            string text2 = CharKey<Ironclad>();
            dictionary[text2] = new AncientDialogue[]
            {
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                }
            };
            string text3 = CharKey<Silent>();
            dictionary[text3] = new AncientDialogue[]
            {
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                }
            };
            string text4 = CharKey<Regent>();
            dictionary[text4] = new AncientDialogue[]
            {
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                }
            };
            string text5 = CharKey<Necrobinder>();
            dictionary[text5] = new AncientDialogue[]
            {
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                },
                new("", "", "")
                {
                    IsRepeating = true
                }
            };*/

        List<AncientDialogue> agnosticDialog = new List<AncientDialogue>(
            new AncientDialogue[]
            {
                new("")
                {
                    IsRepeating = true
                },
                new("", "")
                {
                    IsRepeating = true
                },
                new("")
                {
                    IsRepeating = true
                }
            });


        AncientDialogueSet ancientDialogueSet = new AncientDialogueSet
        {
            FirstVisitEverDialogue = new AncientDialogue(""),
            CharacterDialogues = new Dictionary<string, IReadOnlyList<AncientDialogue>>(),
            AgnosticDialogues = agnosticDialog
        };
        return ancientDialogueSet;
    }
}