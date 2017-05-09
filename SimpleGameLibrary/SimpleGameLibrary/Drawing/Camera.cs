using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SimpleGameLibrary.Components;

namespace SimpleGameLibrary.Drawing
{
    public enum CameraMode { Instant, Lerp }

    public class Camera
    {
        #region Field Region

        private Vector2 position;
        private float width;
        private float height;
        private float zoom;
        private CameraMode cameraMode;
        private Component target;

        /* The upper-left (min) and lower-right (max) limits of the camera's viewport s*/
        private Vector2 min;
        private Vector2 max;

        #endregion

        #region Property Region

        public Matrix Transformation
        {
            get { return Matrix.CreateScale(zoom) * Matrix.CreateTranslation(new Vector3(-position, 0)); }
        }

        public Vector2 TargetPos
        {
            get { return target.Center * zoom - (new Vector2(width, height) / 2); }
        }

        #endregion

        #region Constructor Region

        public Camera(Component target, int width, int height, CameraMode cameraMode = CameraMode.Instant, Vector2? min = null, Vector2? max = null)
        {
            this.width = width;
            this.height = height;
            this.target = target;

            this.cameraMode = cameraMode;

            this.min = min != null ? (Vector2)min : new Vector2(float.MinValue);
            this.max = max != null ? (Vector2)max : new Vector2(float.MaxValue);

            zoom = 1f;
            position = TargetPos;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            /* Move to target */
            // Instant movement
            if (cameraMode == CameraMode.Instant)
            {
                position = TargetPos;
            }
            // Linear interpolation (Lerp)
            else if (cameraMode == CameraMode.Lerp)
            {
                position = Vector2.Lerp(position, TargetPos, 0.05f);
            }

            /* Clamp camera to be within boundaries */
            // right
            if (position.X + width > max.X)
                position.X = max.X - width;
            // left
            if (position.X < min.X)
                position.X = min.X;
            // top
            if (position.Y < min.Y)
                position.Y = min.Y;
            // bottom
            if (position.Y + height > max.Y)
                position.Y = max.Y - height;

            /* Clamp camera to screen pixels */
            position.X = (int)position.X;
            position.Y = (int)position.Y;
        }

        #endregion
    }
}
