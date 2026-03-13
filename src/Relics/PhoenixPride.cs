using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

//TODO: IMPL
// - Gain 1 extra energy per turn but at the start of your turn take 2 unavoidable damage
[Pool(typeof(EventRelicPool))]
public class PhoenixPride() : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
}