using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampSharp.Tryg3D.Entities.Services
{
    public class Tryg3DService : ITryg3DService
    {
        private IEntityManager _entityManager;

        public Tryg3DService(IEntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        /// <summary>
        /// Computes a point in front of the player given a radius.
        /// Returns the angle (in degrees) used for the calculation,
        /// and outputs the 2D point (from the player's X and Y coordinates) via an out parameter.
        /// </summary>
        /// <param name="player">The player whose position and angle are used.</param>
        /// <param name="radius">The distance from the player to the point.</param>
        /// <param name="point">The computed point in front of the player (X, Y).</param>
        /// <returns>The angle in degrees used in the computation.</returns>
        public float GetPointInFrontOfPlayer(Player player, float radius, out Vector2 point)
        {
            // Get the player's current 3D position.
            Vector3 pos = player.Position;
            float angle;

            // If the player is in a vehicle, use the vehicle's angle.
            if (player.Vehicle != null)
            {
                // Get Vehicle the player is in.
                Vehicle playerVehicle = _entityManager.GetComponent<Vehicle>(player.Vehicle);

                // Get Z rotation of vehicle.
                angle = playerVehicle.Rotation.Z;
            }
            else
            {
                // Otherwise, use the player's facing rotation.
                angle = player.Angle;
            }

            // Compute the new X,Y point based on the current position, angle, and radius.
            point = GetPointInFront2D(new Vector2(pos.X, pos.Y), angle, radius);
            return angle;
        }

        /// <summary>
        /// Calculates a new 2D point in front of the given position by applying an offset
        /// based on the angle and radius. The math uses sine and cosine (after converting the angle
        /// from degrees to radians) to adjust the coordinates.
        /// </summary>
        /// <param name="position">The starting position (X, Y).</param>
        /// <param name="angle">The angle (in degrees) to use for the calculation.</param>
        /// <param name="radius">The distance to move in front of the position.</param>
        /// <returns>A new Vector2 representing the computed position.</returns>
        public Vector2 GetPointInFront2D(Vector2 position, float angle, float radius)
        {
            // Convert the angle from degrees to radians.
            float rad = angle * (float)Math.PI / 180f;

            // Compute new coordinates.
            // Note: The Pawn code subtracts the sine term from x and adds the cosine term to y.
            float newX = position.X - (radius * (float)Math.Sin(rad));
            float newY = position.Y + (radius * (float)Math.Cos(rad));
            return new Vector2(newX, newY);
        }

        /// <summary>
        /// Swaps the values of two float variables.
        /// </summary>
        /// <param name="variable1">First float (passed by reference).</param>
        /// <param name="variable2">Second float (passed by reference).</param>
        public void SwapFloat(ref float variable1, ref float variable2)
        {
            float tmp = variable2;
            variable2 = variable1;
            variable1 = tmp;
        }

        /// <summary>
        /// Computes a random point inside a circle centered at (x, y) with a given radius.
        /// This version uses separate x and y parameters.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The circle's radius.</param>
        /// <param name="tx">Output x-coordinate of the computed point.</param>
        /// <param name="ty">Output y-coordinate of the computed point.</param>
        public void GetPointInCircle(float x, float y, float radius, out float tx, out float ty)
        {
            // Generate two random floats between 1/1000000 and 1.
            float alfa = (Random.Shared.Next(1, 1000001)) / 1000000f;
            float beta = (Random.Shared.Next(1, 1000001)) / 1000000f;

            // Ensure alfa is not greater than beta.
            if (beta < alfa)
            {
                SwapFloat(ref alfa, ref beta);
            }

            // Calculate the new point using trigonometry.
            // Math.Cos and Math.Sin expect angles in radians.
            tx = x + (beta * radius * (float)Math.Cos(2.0 * Math.PI * alfa / beta));
            ty = y + (beta * radius * (float)Math.Sin(2.0 * Math.PI * alfa / beta));
        }

        /// <summary>
        /// Computes and returns a random point inside a circle centered at the specified location with a given radius.
        /// </summary>
        /// <param name="center">The center of the circle as a Vector2.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>A Vector2 representing the computed point.</returns>
        public Vector2 GetPointInCircle(Vector2 center, float radius)
        {
            GetPointInCircle(center.X, center.Y, radius, out float tx, out float ty);
            return new Vector2(tx, ty);
        }
    }
}
