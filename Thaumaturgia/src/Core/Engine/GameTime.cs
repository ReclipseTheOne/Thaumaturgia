using System;

namespace Thaumaturgia.Core.Engine {
    public class GameTime {
        private class FPS {
            public int FrameCount { get; set; }
            public float TotalFrameTime { get; set; }

            public FPS() {
                FrameCount = 0;
                TotalFrameTime = 0f;
            }

            public void Update(float dt) {
                FrameCount++;
                TotalFrameTime += dt / 1000f; // Convert ms -> s
            }

            public int GetFPS() {
                if (TotalFrameTime == 0f) return 0;
                return (int) (FrameCount / TotalFrameTime);
            }
        }

        private DateTime _lastFrameTime;
        private readonly FPS _fps = new();
        public float DeltaTime { get; private set; } // Time between frames in milliseconds

        public GameTime() {
            _lastFrameTime = DateTime.Now;
        }

        public float ElapsedMS() {
            var currentFrameTime = DateTime.Now;
            return (float) (currentFrameTime - _lastFrameTime).TotalMilliseconds;
        }

        public float ElapsedS() {
            var currentFrameTime = DateTime.Now;
            return (float) (currentFrameTime - _lastFrameTime).TotalSeconds;
        }

        public void Update() {
            var currentFrameTime = DateTime.Now;
            DeltaTime = ((float) (currentFrameTime - _lastFrameTime).TotalSeconds) * 1000f; // Convert to milliseconds
            _lastFrameTime = currentFrameTime;
            _fps.Update(DeltaTime);
        }

        public int GetFPS() {
            return _fps.GetFPS();
        }
    }
}