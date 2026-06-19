using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace ArqPhoenixAncient.Relics;

[Pool(typeof(EventRelicPool))]
public class PhoenixPyre : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new IntVar("HandFuel", 1),
            new IntVar("DrawFuel", 3)
        ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Fuel>()];


    [SavedProperty]
    public bool GainFuelInNextCombat
    {
        get;
        set
        {
            AssertMutable();
            if (field == value)
            {
                return;
            }

            field = value;
            Status = field ? RelicStatus.Active : RelicStatus.Normal;
        }
    }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not RestSiteRoom)
        {
            return;
        }

        GainFuelInNextCombat = true;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState.RoundNumber == 1 && GainFuelInNextCombat)
        {
            var fuelCard = Owner.Creature.CombatState?.CreateCard<Fuel>(Owner);
            await CardPileCmd.AddGeneratedCardToCombat(fuelCard, PileType.Hand, Owner);
            await CardPileCmd.AddToCombatAndPreview<Fuel>(Owner.Creature,
                PileType.Draw, DynamicVars["DrawFuel"].IntValue, Owner, CardPilePosition.Random);
        }
    }
}