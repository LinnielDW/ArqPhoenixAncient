using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace ArqPhoenixAncient.Relics;

//TODO: IMPL
// add a card to deck. 
[Pool(typeof(EventRelicPool))]
public class PhoenixTorch() : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    
}