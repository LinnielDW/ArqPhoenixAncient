using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

// At the start of battle gain 3 strength and 3 dexterity, at the start of each turn lose 1 max health. 
[Pool(typeof(EventRelicPool))]
public class PhoenixPyre : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new HpLossVar(1),
            new PowerVar<StrengthPower>(4),
            new PowerVar<DexterityPower>(4)
        ]);

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (player == Owner)
        {
            Flash();
            await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars.HpLoss.BaseValue, false);
            
            if (combatState.RoundNumber == 1)
            {
                await PowerCmd.Apply<StrengthPower>(Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, null);
                await PowerCmd.Apply<DexterityPower>(Owner.Creature, DynamicVars.Dexterity.BaseValue, Owner.Creature, null);
            }
        }
    }
}