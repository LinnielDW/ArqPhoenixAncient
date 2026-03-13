using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace ArqPhoenixAncient.Relics;

//The first time you'd die during combat, resurrect on 1hp and gain 5 regen.
[Pool(typeof(EventRelicPool))]
public class PhoenixBrazier() : CustomRelicModel
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private bool _usedThisCombat;

    public bool UsedThisCombat
    {
        get
        {
            return _usedThisCombat;
        }
        set
        {
            AssertMutable();
            _usedThisCombat = value;
            Status = _usedThisCombat ? RelicStatus.Disabled : RelicStatus.Normal;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> {new HealVar(5)};


    public override bool ShouldDieLate(Creature creature)
    {
        return creature != Owner.Creature || UsedThisCombat;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();

        UsedThisCombat = true;
        await CreatureCmd.Heal(creature, DynamicVars.Heal.BaseValue, true);
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        UsedThisCombat = false;
        return Task.CompletedTask;
    }
}