using ArqPhoenixAncient.Cards;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

[Pool(typeof(EventRelicPool))]
public class PhoenixTorch : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            return HoverTipFactory.FromCardWithCardHoverTips<Devastation>();
        }
    }

    public override async Task AfterObtained()
    {
        List<CardPileAddResult> results = [];
        for (var i = 0; i < 2; i++)
        {
            CardModel cardModel = Owner.RunState.CreateCard<Devastation>(Owner);
            var cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck);
            results.Add(cardPileAddResult);
        }
        CardCmd.PreviewCardPileAdd(results, 2f);
    }
}