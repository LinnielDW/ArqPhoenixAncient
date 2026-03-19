using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;


//glass jar icon
// ticks down health for some kind of buff
[Pool(typeof(EventRelicPool))]
public class PhoenixTears() : CustomRelicModel
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    
}