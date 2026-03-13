using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

//TODO: IMPL
// - At the start of your turn all enemies take 1 damage. This damage increases by 1 every turn. 

[Pool(typeof(EventRelicPool))]
public class PhoenixPyre() : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    
}