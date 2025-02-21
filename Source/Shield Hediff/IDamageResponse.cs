using Verse;

namespace BrokenPlankFramework
{
    public interface IDamageResponse
    {
        public abstract void PreApplyDamage(ref DamageInfo dinfo, ref bool absorbed);

        // Must be added to BrokenPlankCache.responderCache to work
        // BrokenPlankCache.AddCache(this, BrokenPlankCache.responderCache, pawn.thingIDNumber)
    }
}