using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArqPhoenixAncient.Relics;

[Pool(typeof(EventRelicPool))]
public class PhoenixTalon() : CustomRelicModel
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new HpLossVar(1),
            new PowerVar<StrengthPower>(1)
        ]);

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && cardPlay.Card.Type == CardType.Attack)
        {
            Flash();
            await CreatureCmd.Damage(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable, Owner.Creature);
            await PowerCmd.Apply<StrengthPower>(Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, null);
        }
    }
    
}

