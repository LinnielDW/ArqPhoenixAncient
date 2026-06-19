using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArqPhoenixAncient.Relics;

[Pool(typeof(EventRelicPool))]
public class PhoenixGreed : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new DamageVar(2, ValueProp.Unblockable)
    ];

    public override bool ShowCounter => true;

    public override int DisplayAmount => !IsActivating ? CardsPlayed : DynamicVars["Cards"].IntValue;

    private bool IsActivating
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            InvokeDisplayAmountChanged();
        }
    }

    [SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
    public int CardsPlayed
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            Status = field == DynamicVars["Cards"].BaseValue - 1
                ? RelicStatus.Active
                : RelicStatus.Normal;
            InvokeDisplayAmountChanged();
        }
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && CardsPlayed == DynamicVars["Cards"].BaseValue - 1)
            cardPlay.Card.BaseReplayCount++;

        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner)
            return;

        CardsPlayed++;

        if (CardsPlayed >= DynamicVars["Cards"].BaseValue)
        {
            CardsPlayed = 0;
            cardPlay.Card.BaseReplayCount--;
            await TaskHelper.RunSafely(DoActivateVisuals());
            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars["Damage"].BaseValue,
                ValueProp.Unblockable, Owner.Creature);
        }
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }
}