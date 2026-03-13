using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArqPhoenixAncient.Relics;

// - The first time you lose health on your turn gain 1 energy. 
[Pool(typeof(EventRelicPool))]
public class PhoenixWrath : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            return new List<DynamicVar>([new EnergyVar(1)]);
        }
    }
    

    private bool _triggeredThisTurn;
    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
        {
            return Task.CompletedTask;
        }
        _triggeredThisTurn = false;
        return Task.CompletedTask;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (Owner.Creature.CombatState != null)
        {
            //If player turn
            if (Owner.Creature.CombatState.CurrentSide == Owner.Creature.Side)
            {
                //If player && actually took damage && not already triggered
                if (target == Owner.Creature && result.UnblockedDamage > 0 && !_triggeredThisTurn)
                {
                    _triggeredThisTurn = true;
                    Flash();
                    await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
                }
            }
        }
    }

}