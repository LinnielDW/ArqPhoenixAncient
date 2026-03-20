using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace ArqPhoenixAncient.Cards;

public class Devastation() : CustomCardModel(2,
    CardType.Attack,
    CardRarity.Ancient,
    TargetType.AllEnemies)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
    }

    protected override void OnUpgrade()
    {

    }
}