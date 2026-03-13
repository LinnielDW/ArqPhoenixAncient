using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

//TODO: IMPL
[Pool(typeof(EventRelicPool))]
public class PhoenixGreed() : CustomRelicModel
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
}