using RimWorld;
using UnityEngine;

namespace BrokenPlankFramework
{
    public interface IRenderable
    {
        public abstract void DrawAt(Vector3 drawPos, BodyTypeDef bodyType);

        // Must be added to BrokenPlankCache.renderCache to work
        // BrokenPlankCache.AddCache(this, BrokenPlankCache.renderCache, pawn.thingIDNumber)
    }
}