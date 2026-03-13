using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

// Gain 1 extra energy per turn. At the start of your turn, shuffle a burn into your draw pile.
[Pool(typeof(EventRelicPool))]
public class PhoenixGreed : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            return new List<DynamicVar>([
                new EnergyVar(1)
            ]);
        }
    }
    
    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != Owner)
        {
            return amount;
        }
        return amount + DynamicVars.Energy.IntValue;
    }
    
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (player == Owner && combatState.RoundNumber % 2 == 1)
        {
            Flash();
            var burnCard = combatState.CreateCard<Burn>(Owner);
            
            var readOnlyList = await CardPileCmd.AddGeneratedCardToCombat(burnCard, PileType.Draw, true, CardPilePosition.Random);
            CardCmd.PreviewCardPileAdd(readOnlyList);
            await Cmd.Wait(3f);
        }
    }
}