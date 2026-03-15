using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace ArqPhoenixAncient.Relics;

[Pool(typeof(EventRelicPool))]
public class PhoenixHeart : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new PowerVar<RegenPower>(2),
        // new HpLossVar(20)
    };

    private bool _usedThisTurn;
    private bool UsedThisTurn
    {
        get => _usedThisTurn;
        set
        {
            AssertMutable();
            _usedThisTurn = value;
        }
    }

    //Lose [blue]{HpLoss}%[/blue] of your Max HP.
    /*public override async Task AfterObtained()
    {
        var num = Math.Max(1, Owner.Creature.MaxHp * (DynamicVars.HpLoss.BaseValue / 100));
        await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Owner.Creature, num, false);
    }*/

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
        {
            return Task.CompletedTask;
        }
        UsedThisTurn = false;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        UsedThisTurn = false;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && CombatManager.Instance.IsInProgress && UsedThisTurn)
        {
            if (cardPlay.Card.Type == CardType.Power)
            {
                Flash();
                
                var burnCard = Owner.Creature.CombatState?.CreateCard<Burn>(Owner);
                if (burnCard != null)
                {
                    await CardPileCmd.AddGeneratedCardToCombat(burnCard, PileType.Draw,
                        true, CardPilePosition.Random);

                    CardCmd.Preview(burnCard);
                    await Cmd.Wait(1f);
                }

                
                await PowerCmd.Apply<RegenPower>(Owner.Creature, DynamicVars["RegenPower"].BaseValue, Owner.Creature, null);
                
            }
        }
    }
}