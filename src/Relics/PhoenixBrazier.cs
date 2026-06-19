using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace ArqPhoenixAncient.Relics;

[Pool(typeof(EventRelicPool))]
public class PhoenixBrazier : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("ExhaustAmount", 3),
        new CardsVar(1)
    ];

    public override bool ShowCounter => true;

    public override int DisplayAmount => !IsActivating ? CardsExhausted : DynamicVars["ExhaustAmount"].IntValue;

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
    public int CardsExhausted
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            Status = field == DynamicVars["ExhaustAmount"].BaseValue - 1
                ? RelicStatus.Active
                : RelicStatus.Normal;
            InvokeDisplayAmountChanged();
        }
    }

    private int EtherealCount
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card,
        bool causedByEthereal)
    {
        if (card.Owner == Owner)
        {
            if (causedByEthereal)
            {
                int num = EtherealCount;
                EtherealCount = num + 1;
            }
            else
            {
                int num = CardsExhausted;
                CardsExhausted = num + 1;
                await DrawIfThresholdMet();
            }
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner.Creature))
        {
            CardsExhausted += EtherealCount;
            EtherealCount = 0;
            await DrawIfThresholdMet();
        }
    }

    private async Task DrawIfThresholdMet()
    {
        if (!(CardsExhausted < DynamicVars["ExhaustAmount"].BaseValue))
        {
            await TaskHelper.RunSafely(DoActivateVisuals());
            IEnumerable<CardModel> enumerable = PileType.Exhaust.GetPile(Owner).Cards.ToList()
                .StableShuffle(Owner.RunState.Rng.Niche)
                .Take((int)(CardsExhausted / DynamicVars["ExhaustAmount"].BaseValue));
            await CardPileCmd.Add(enumerable, PileType.Draw, CardPilePosition.Random);
            CardsExhausted %= DynamicVars["ExhaustAmount"].IntValue;
        }
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        EtherealCount = 0;
        return Task.CompletedTask;
    }
}