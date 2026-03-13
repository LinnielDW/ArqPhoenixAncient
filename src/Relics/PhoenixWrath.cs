using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

//TODO: IMPL
// - The first time you lose health on your turn gain 1 energy. 
[Pool(typeof(EventRelicPool))]
public class PhoenixWrath : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
}