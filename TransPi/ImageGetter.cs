using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Canon.Eos.Framework;
using Canon.Eos.Framework.Eventing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Threading;
using System.IO;
using TransPi;

public delegate void ImageReceived(Image newImage);

namespace TransPi
{
    class ImageGetter
    {
        private static ImageGetter instance;

        private readonly FrameworkManager _manager;
        private EosCamera camera;
        private bool singleShotInProgress;
        private bool cameraLoaded;

        public event ImageReceived imageReceived;

        //Birth
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ImageGetter()
        {
            _manager = new FrameworkManager();
            _manager.CameraAdded+=ManagerOnCameraAdded;
            loadFunction();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static ImageGetter sharedInstance()
        {
            if (instance == null)
            {
                instance = new ImageGetter();
            }

            return instance;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ManagerOnCameraAdded(object sender, EventArgs eventArgs)
        {
            loadCameras();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Public Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void getImage()
        {
            if (singleShotInProgress == true)
            {
                return;
            }

            safeCall(() =>
            {
                if (camera != null)
                {
                    singleShotInProgress = true;
                    camera.TakePicture();
                }
                else
                {
                    MessageBox.Show("Камера не подключена");
                }
            }, ex => catchShootException(ex));
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        //Inner Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadFunction()
        {
            StartUp();

            if (cameraLoaded == false)
            {
                loadCameras();
                cameraLoaded = true;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void StartUp()
        {
            safeCall(() => { _manager.LoadFramework(); }, ex => { MessageBox.Show(ex.ToString());});
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void loadCameras()
        {
            IEnumerable<EosCamera> cameras = _manager.GetCameras();

            if (cameras != null)
            {
                foreach (var _camera in cameras)
                {
                    _camera.PictureTaken += imageTaken;
                    camera = _camera;
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void safeCall(Action action, Action<Exception> exceptionHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exceptionHandler(ex);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageTaken(object sender, EosImageEventArgs e)
        {
            if (singleShotInProgress == true)
            {
                singleShotInProgress = false;
            }

            //изображение получено
            if (imageReceived != null)
            {
                imageReceived(e.GetImage());
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void catchShootException(Exception exception)
        {
            singleShotInProgress = false;
            MessageBox.Show(exception.ToString());
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
