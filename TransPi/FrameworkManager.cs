using System;
using System.Collections.Generic;
using System.Linq;
using Canon.Eos.Framework;
using Canon.Eos.Framework.Eventing;

namespace TransPi
{
    public sealed class FrameworkManager
    {
        private EosFramework _framework;

        public event EventHandler CameraAdded;

        public IEnumerable<EosCamera> GetCameras()
        {
            if (_framework != null)
            {
                using (var cameras = _framework.GetCameraCollection())
                    return cameras.ToArray();
            }

            return null;
        }

        public void LoadFramework()
        {
            if (_framework == null)
            {
                _framework = new EosFramework();
                _framework.CameraAdded += HandleCameraAdded;
            }
        }

        private void HandleCameraAdded(object sender, EventArgs eventArgs)
        {
            if (CameraAdded != null)
            {
                CameraAdded(this, eventArgs);
            }
        }
    }
}
