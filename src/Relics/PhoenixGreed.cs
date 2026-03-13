using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

//TODO: IMPL
// Gain 1 extra energy per turn but shuffle a burn into your draw pile
[Pool(typeof(EventRelicPool))]
public class PhoenixGreed() : CustomRelicModel
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
}