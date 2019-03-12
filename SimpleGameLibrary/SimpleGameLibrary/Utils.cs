using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Microsoft.Xna.Framework;

namespace SimpleGameLibrary
{

    public static class Utils
    {
        /* A static instance of a random number generator to be used anywhere */
        public static Random RNG = new Random();

        /* Converting base64 to decimal */
        private const string base64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        public static int Base64ToDecimal(char input)
        {
            return base64.IndexOf(input);
        }
        
        public static Vector2? GetIntersection(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
        {
            float A1 = p2.Y - p1.Y;
            float B1 = p1.X - p2.X;
            float C1 = A1 * p1.X + B1 * p1.Y;

            float A2 = q2.Y - q1.Y;
            float B2 = q1.X - q2.X;
            float C2 = A2 * q1.X + B2 * q1.Y;

            float delta = A1 * B2 - A2 * B1;
            if (delta == 0)
                return null;

            Vector2 intersection = new Vector2(
                (B2 * C1 - B1 * C2) / delta,
                (A1 * C2 - A2 * C1) / delta
            );

            RectangleF rect1 = new RectangleF(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
            RectangleF rect2 = new RectangleF(q1.X, q1.Y, q2.X - q1.X, q2.Y - q1.Y);
            
            if (rect1.Width < 0)
            {
                rect1.Width = -rect1.Width;
                rect1.X -= rect1.Width;
            }

            if (rect1.Height < 0)
            {
                rect1.Height = -rect1.Height;
                rect1.Y -= rect1.Height;
            }

            if (rect2.Width < 0)
            {
                rect2.Width = -rect2.Width;
                rect2.X -= rect2.Width;
            }

            if (rect2.Height < 0)
            {
                rect2.Height = -rect2.Height;
                rect2.Y -= rect2.Height;
            }

            rect1.Inflate(1f, 1f);
            rect2.Inflate(1f, 1f);

            if (!(rect1.Contains(intersection.X, intersection.Y) && rect2.Contains(intersection.X, intersection.Y)))
                return null;

            return intersection;
        }

        /* Extends the line |p1, p2| to a certain percentage of its length */
        public static void Extend(ref Vector2 p1, ref Vector2 p2, float percentage)
        {
            Vector2 oldp1 = new Vector2(p1.X, p1.Y);

            p1 = p1 + (p2 - p1) * ((1 - percentage) / 2);
            p2 = p2 + (oldp1 - p2) * ((1 - percentage) / 2);
        }

        public static void Extend(Vector2 in1, Vector2 in2, out Vector2 res1, out Vector2 res2, float percentage)
        {
            res1 = new Vector2(in1.X, in1.Y);
            res2 = new Vector2(in2.X, in2.Y);

            Extend(ref res1, ref res2, percentage);
        }

        /* Returns the angle at which the ray (p1 -> p2) reflects when it hits the point of incidence of the side (q1 -> q2) */
        public static float GetAngle(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
        {
            float normalAng = (AngleBetweenPoints(q1, q2) - 90) % 360;
            float invRayAngle = (AngleBetweenPoints(p1, p2) - 180) % 360;

            if (normalAng < 0)
                normalAng += 360;

            if (invRayAngle < 0)
                invRayAngle += 360;

            float result = invRayAngle + (normalAng - invRayAngle) * 2;

            if (result < 0)
                result += 360;
            result %= 360;

            return result;
        }

        /* returns true if the angle is in the range (low, high)*/
        public static bool AngleInRange(float angle, float low, float high)
        {
            angle %= 360;
            low %= 360;
            high %= 360;

            if (angle < 0) angle += 360;
            if (low < 0) low += 360;
            if (high < 0) high += 360;
            
            if (low > high)
                return (angle > low || angle < high);
            else
                return (angle > low && angle < high);
        }

        /* Returns the angle between two points */
        public static float AngleBetweenPoints(Vector2 p, Vector2 q)
        {
            float ang = MathHelper.ToDegrees((float)Math.Atan2(q.Y - p.Y, q.X - p.X));
            if (ang < 0)
                ang = 360 + ang;

            return ang;
        }

        /* Returns true if value is in the range [low, high) */
        public static bool IsInRange(int value, int low, int high)
        {
            return (value >= low && value < high);
        }
    }
}
