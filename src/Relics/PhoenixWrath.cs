using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArqPhoenixAncient.Relics;

[Pool(typeof(EventRelicPool))]
public class PhoenixWrath : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override async Task AfterObtained()
    {
        foreach (var cardModel in PileType.Deck.GetPile(Owner).Cards.ToList()
                     .Where(cardModel => ModelDb.Enchantment<FieryWrath>().CanEnchant(cardModel)))
        {
            CardCmd.Enchant<FieryWrath>(cardModel, 1);
            NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(NCardEnchantVfx.Create(cardModel));
        }
    }
}

public class FieryWrath : CustomEnchantmentModel
{
    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && card.Type == CardType.Attack;
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(1, ValueProp.Unblockable | ValueProp.Unpowered),
        new IntVar("DamageScaling", 2)
    ];

    protected override void OnEnchant()
    {
        Card.AddKeyword(CardKeyword.Exhaust);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != Card)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, cardPlay.Card.Owner.Creature, DynamicVars.Damage.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered, cardPlay.Card.Owner.Creature);

        Amount += DynamicVars["DamageScaling"].IntValue;
        if (Card.DeckVersion == null) return;
        var enchantment = Card.DeckVersion.Enchantment;
        Amount = enchantment.Amount;
        enchantment.Amount = Amount + DynamicVars["DamageScaling"].IntValue;
    }

    public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
    {
        if (!props.IsPoweredAttack())
        {
            return 0m;
        }

        return Amount - 1;
        ;
    }
}