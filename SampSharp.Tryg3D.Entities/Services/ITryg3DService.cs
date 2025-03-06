using SampSharp.Entities.SAMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampSharp.Tryg3D.Entities.Services
{
    public interface ITryg3DService
    {
        float GetPointInFrontOfPlayer(Player player, float radius, out Vector2 point);
        Vector2 GetPointInFront2D(Vector2 position, float angle, float radius);
        void SwapFloat(ref float variable1, ref float variable2);
        void GetPointInCircle(float x, float y, float radius, out float tx, out float ty);
        Vector2 GetPointInCircle(Vector2 center, float radius);
    }
}
