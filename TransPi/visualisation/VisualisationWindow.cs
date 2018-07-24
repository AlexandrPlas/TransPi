using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Gwen.Control;
using ClassLibrary;
using System.IO;

namespace TransPi.Visualisation
{
    class VisualisationWindow : GameWindow
    {
        int shaderProgram;

        Vector2 oldMousePos;
        SceneNode node;
        ICamera cam;
        PerspectiveProjeciton proj;
        Mesh mesh;
        int projLoc, viewLoc, modelLoc;


        Gwen.Input.OpenTK gwenInput;
        Canvas canvas;
        StatusBar statusBar;
        DockBase dock;
        RadioButtonGroup radioCamera;
        Gwen.Renderer.OpenTK renderer;
        Label labelPosX;
        Label labelPosY;
        Label labelPosZ;
        Label labelTips;
        Label labelSpeed;

        Button checkVao1;
        Button checkVao2;
        Button checkVao3;
        Button checkVao4;
        Button checkVao5;
        Button checkVao6;
        Button checkVao7;
        Button checkVao8;
        Button checkVao9;
        Button checkVao10;
        Button checkVao11;
        Button checkVao12;
        Button checkVao13;


        private const string boundCameraTip = @"Перемещения точки привязки:
W, A - вперед/назад
S, D - влево/вправо
Space, C - вверх/вниз
Левая кнопка мыши: вращение вокруг точки привязки
Правая кнопка мыши: приближение/отдаление
Колесо мыши: скорость перемещения";
        private const string freeCameraTip = @"Перемещение камеры:
W, A - вперед/назад
S, D - влево/вправо
Space, C - вверх/вниз
Левая кнопка мыши: вращение камеры
Колесо мыши: скорость перемещения";
        private const string nodeTransformTip = @"Вращение объекта:
U, O: вокруг продольной оси
I, K: вокруг поперечной оси
J, L: вокруг вертикальной оси";


        public VisualisationWindow(string obj_path, ICamera cam, int fsaa_samples = 0, bool vsync = false)
            : base(100, 100, new GraphicsMode(32, 24, 0, fsaa_samples))
        {
            this.Width = DisplayDevice.Default.Width;
            this.Height = DisplayDevice.Default.Height - 70;
            this.Location = new System.Drawing.Point(0, 30);

            this.cam = cam;
            this.mesh = Mesh.FromObject(obj_path)[0];
            if (!vsync)
                this.VSync = VSyncMode.Off;
        }

        public VisualisationWindow(ZArrayDescriptor desc, Mesh.ColoringMethod method, ICamera cam, int fsaa_samples = 0, bool vsync = false)
            : base(100, 100, new GraphicsMode(32, 24, 0, fsaa_samples))
        {
            this.Width = DisplayDevice.Default.Width;
            this.Height = DisplayDevice.Default.Height - 70;
            this.Location = new System.Drawing.Point(0, 30);

            this.cam = cam;
            this.mesh = Mesh.FromZArray(desc, method, null);
            if (!vsync)
                this.VSync = VSyncMode.Off;
        }

        public VisualisationWindow(ZArrayDescriptor desc, Mesh.ColoringMethod method, System.Drawing.Bitmap texture, ICamera cam, int fsaa_samples = 0, bool vsync = false)
            : base(100, 100, new GraphicsMode(32, 24, 0, fsaa_samples))
        {
            this.Width = DisplayDevice.Default.Width;
            this.Height = DisplayDevice.Default.Height - 70;
            this.Location = new System.Drawing.Point(0, 30);

            this.cam = cam;
            this.mesh = Mesh.FromZArray(desc, method, texture);
            if (!vsync)
                this.VSync = VSyncMode.Off;
        }

        public VisualisationWindow(ZArrayDescriptor desc, Mesh.ColoringMethod method, ICamera cam, int width, int height, int fsaa_samples, bool vsync)
            : base(width, height, new GraphicsMode(32, 24, 0, fsaa_samples))
        {
            this.cam = cam;
            this.mesh = Mesh.FromZArray(desc, method, null);
            if (!vsync)
                this.VSync = VSyncMode.Off;
        }

        private void CreateProgram()
        {
            ShaderWrapper vshader = ShaderWrapper.FromFile(ShaderType.VertexShader, "..\\..\\visualisation\\shaders\\shader.vert");
            ShaderWrapper fshader = ShaderWrapper.FromFile(ShaderType.FragmentShader, "..\\..\\visualisation\\shaders\\shader.frag");

            vshader.Compile();
            fshader.Compile();

            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vshader.handle);
            GL.AttachShader(shaderProgram, fshader.handle);

            int status;
            GL.LinkProgram(shaderProgram);
            GL.GetProgram(shaderProgram, ProgramParameter.LinkStatus, out status);
            if (status == 0)
                throw new Exception(GL.GetProgramInfoLog(shaderProgram));

            projLoc = GL.GetUniformLocation(shaderProgram, "projection");
            viewLoc = GL.GetUniformLocation(shaderProgram, "view");
            modelLoc = GL.GetUniformLocation(shaderProgram, "model");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CreateProgram();

            node = new SceneNode(mesh, modelLoc);
            proj = new PerspectiveProjeciton(3.14159f / 4, 2.0f, 5000.0f, (float)this.Width / this.Height);

            //GL.Enable(EnableCap.PrimitiveRestart);
            //GL.PrimitiveRestartIndex(Mesh.restartIndex);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.DepthRange(0.0f, 1.0f);
            //GL.Enable(EnableCap.DepthClamp);

            //GL.Enable(EnableCap.CullFace);
            //GL.FrontFace(FrontFaceDirection.Ccw);
            //GL.CullFace(CullFaceMode.Back);

            setupUi();
        }

        void radioCamera_SelectionChanged(Base control, EventArgs args)
        {
            RadioButtonGroup rg = (RadioButtonGroup)control;
            if (rg.SelectedName == "free")
            {
                cam = new FreeCamera(new Vector3(0, 0, 1), new Vector3(0, 0, -700));
                labelTips.SetText(freeCameraTip + "\n\n" + nodeTransformTip);
                labelTips.SizeToContents();
            }
            else if (rg.SelectedName == "bound")
            {
                cam = new BoundCamera(new Vector3(0, 0, 0), -1.57f, 1.57f, 600.0f);
                labelTips.SetText(boundCameraTip + "\n\n" + nodeTransformTip);
                labelTips.SizeToContents();
            }
        }

        void cameraReset_Clicked(Base control, EventArgs args)
        {
            radioCamera_SelectionChanged(radioCamera, null);
        }

        void objLoad_Clicked(Base control, EventArgs args)
        {
            Stream myStream = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "obj files (*.obj)|*.obj";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            FileInfo fileInfo = new FileInfo(openFileDialog1.FileName);
                            if (fileInfo.Extension.ToLower() == ".obj")
                            {
                                //this.mesh = Mesh.FromObject(openFileDialog1.FileName)[0];
                                //node = new SceneNode(mesh, modelLoc);

                                node.addMeshSceneNode(Mesh.FromObject(openFileDialog1.FileName));
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Загрузка не удалась. " + ex.Message);
                }
            }
        }

        void objSave_Clicked(Base control, EventArgs args)
        {
            Stream myStream;
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = "3D obj files (*.obj)|*.obj";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        FileInfo fileInfo = new FileInfo(saveFileDialog1.FileName);
                        if (fileInfo.Extension.ToLower() == ".obj")
                            {
                                myStream.Dispose();
                                node.SaveMeshSceneNode(saveFileDialog1.FileName);

                      }      }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Сохранение не удалось. " + ex.Message);
                }
            }
        }
        int vaosell=0;
        void vaoRotate_Clicked(Base control, EventArgs args)
        {
            vaosell = (int)((Gwen.Control.Button)control).UserData;
            TransPi.Forms.RotateForm subForm = new TransPi.Forms.RotateForm();
            subForm.Rotate1 += RotateFunc;
            subForm.Move1 += MoveFunc;
            subForm.ShowDialog();                
        }

        private void RotateFunc(double k1, double k2, double k3)
        {
            node.VaoRotate(k1, k2, k3, vaosell);
        }

        private void MoveFunc(double k1, double k2, double k3)
        {
            node.VaoMove(k1,k2,k3,vaosell);
        } 

        private void setupUi()
        {
            
            
            renderer = new Gwen.Renderer.OpenTK();
            Gwen.Skin.Base skin = new Gwen.Skin.TexturedBase(renderer, "DefaultSkin.png");
            canvas = new Canvas(skin);
            canvas.SetSize(Width, Height);

            gwenInput = new Gwen.Input.OpenTK(this);
            gwenInput.Initialize(canvas);

            Mouse.ButtonDown += Mouse_ButtonDown;
            Mouse.ButtonUp += Mouse_ButtonUp;
            Mouse.Move += Mouse_Move;
            Mouse.WheelChanged += Mouse_Wheel;

            canvas.ShouldDrawBackground = true;
            canvas.BackgroundColor = System.Drawing.Color.FromArgb(0, 150, 170, 170);

            // file
            GroupBox fileGroup = new GroupBox(canvas);
            fileGroup.SetText("Файл");
            fileGroup.SizeToChildren();
            fileGroup.SetSize(140, 70);
            Button loadButton = new Gwen.Control.Button(fileGroup);
            loadButton.SetText("Загрузить модель");
            loadButton.Clicked += objLoad_Clicked;
            loadButton.SizeToContents();

            Button saveButton = new Gwen.Control.Button(fileGroup);
            saveButton.SetText("Сохранить модель");
            saveButton.Clicked += objSave_Clicked;
            saveButton.SizeToContents();
            Gwen.Align.PlaceDownLeft(saveButton, loadButton, 5);

            // controls
            radioCamera = new RadioButtonGroup(canvas);
            radioCamera.AutoSizeToContents = true;
            radioCamera.SetText("Тип камеры");
            radioCamera.AddOption("Свободная", "free");
            radioCamera.AddOption("Привязанная", "bound");
            radioCamera.SetSelection(1);
            radioCamera.SelectionChanged += radioCamera_SelectionChanged;
            Gwen.Align.PlaceDownLeft(radioCamera, fileGroup, 5);

            // coord
            GroupBox posGroup = new GroupBox(canvas);
            posGroup.SetText("Позиция камеры");
            posGroup.SizeToChildren();
            posGroup.SetSize(150, 120);
            Gwen.Align.PlaceDownLeft(posGroup, radioCamera, 45);

            labelPosX = new Label(posGroup);
            labelPosY = new Label(posGroup);
            labelPosZ = new Label(posGroup);
            labelPosX.SetText("X: 0.0");
            labelPosY.SetText("Y: 1.0");
            labelPosZ.SetText("Z: 2.0");
            Gwen.Align.PlaceDownLeft(labelPosY, labelPosX, 5);
            Gwen.Align.PlaceDownLeft(labelPosZ, labelPosY, 5);

            // Mesh list

            GroupBox meshControlPanel = new GroupBox(canvas);
            meshControlPanel.SetText("Список моделей");
            meshControlPanel.SizeToChildren();
            meshControlPanel.SetSize(140, 400);
            Gwen.Align.PlaceDownLeft(meshControlPanel, posGroup, 5);

            checkVao1 = new Button(meshControlPanel);
            checkVao1.Clicked += vaoRotate_Clicked;

            checkVao2 = new Button(meshControlPanel);
            checkVao2.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao2, checkVao1, 5);

            checkVao3 = new Button(meshControlPanel);
            checkVao3.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao3, checkVao2, 5);

            checkVao4 = new Button(meshControlPanel);
            checkVao4.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao4, checkVao3, 5);

            checkVao5 = new Button(meshControlPanel);
            checkVao5.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao5, checkVao4, 5);

            checkVao6 = new Button(meshControlPanel);
            checkVao6.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao6, checkVao5, 5);

            checkVao7 = new Button(meshControlPanel);
            checkVao7.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao7, checkVao6, 5);

            checkVao8 = new Button(meshControlPanel);
            checkVao8.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao8, checkVao7, 5);

            checkVao9 = new Button(meshControlPanel);
            checkVao9.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao9, checkVao8, 5);

            checkVao10 = new Button(meshControlPanel);
            checkVao10.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao10, checkVao9, 5);

            checkVao11 = new Button(meshControlPanel);
            checkVao11.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao11, checkVao10, 5);

            checkVao12 = new Button(meshControlPanel);
            checkVao12.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao12, checkVao11, 5);

            checkVao13 = new Button(meshControlPanel);
            checkVao13.Clicked += vaoRotate_Clicked;
            Gwen.Align.PlaceDownLeft(checkVao13, checkVao12, 5);

            /*foreach (int ak in aks)
            {
                Button txtBox = new Button(meshControlPanel);
                txtBox.Text = string.Format("vao #{0}", ak);
                txtBox.SetPosition(txtBox.X, txtBox.Y + (ak-1)*30);
                txtBox.UserData = ak;
                //node.GetListVao().Any(p=>p==ak);
                if (!node.GetListVao().Any(p => p == ak))
                    txtBox.Hide();
                txtBox.Clicked += vaoRotate_Clicked;
            }*/
            

            /*labelSpeed = new Label(canvas);
            Gwen.Align.PlaceDownLeft(labelSpeed, posGroup, 5);

            labelTips = new Label(canvas);
            labelTips.SetText(boundCameraTip + "\n\n" + nodeTransformTip);
            labelTips.SizeToContents();
            Gwen.Align.PlaceDownLeft(labelTips, labelSpeed, 15);*/

            statusBar = new Gwen.Control.StatusBar(canvas);
            statusBar.Dock = Gwen.Pos.Bottom;
        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs args)
        {
            gwenInput.ProcessMouseMessage(args);
        }

        void Mouse_ButtonUp(object sender, MouseButtonEventArgs args)
        {
            gwenInput.ProcessMouseMessage(args);
        }

        void Mouse_Move(object sender, MouseMoveEventArgs args)
        {
            gwenInput.ProcessMouseMessage(args);
        }

        void Mouse_Wheel(object sender, MouseWheelEventArgs args)
        {
            gwenInput.ProcessMouseMessage(args);
        }

        float shift = 2.5f;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            cam.update();

            if (Mouse[MouseButton.Left])
            {
                cam.rotLeft((oldMousePos.X - Mouse.X) / this.Width * (float)Math.PI);
                cam.rotDown((oldMousePos.Y - Mouse.Y) / this.Height * (float)Math.PI);
            }

            if (Mouse[MouseButton.Right])
                cam.onMouseRightPressed(Math.Sign(Mouse.Y - oldMousePos.Y)
                    * Vector2.Subtract(oldMousePos, new Vector2(Mouse.X, Mouse.Y)).Length);

            oldMousePos.X = Mouse.X;
            oldMousePos.Y = Mouse.Y;
            shift += 0.1f * shift * (float)Mouse.WheelDelta;
            if (shift < 0.003)
                shift = 0.003f;

            if (Keyboard[Key.W])
                cam.moveForward(shift);
            if (Keyboard[Key.S])
                cam.moveForward(-shift);
            if (Keyboard[Key.A])
                cam.moveRight(-shift);
            if (Keyboard[Key.D])
                cam.moveRight(shift);
            if (Keyboard[Key.Space])
                cam.moveUp(shift);
            if (Keyboard[Key.C])
                cam.moveUp(-shift);

            if (Keyboard[Key.R])
            {
                proj.FieldOfView += 0.01f;
                if (proj.FieldOfView > 3.14f)
                    proj.FieldOfView = 3.14f;
                setProjectionUniform();
            }

            if (Keyboard[Key.F])
            {
                proj.FieldOfView -= 0.01f;
                if (proj.FieldOfView < 0.01f)
                    proj.FieldOfView = 0.01f;
                setProjectionUniform();
            }

            if (Keyboard[Key.I])
                node.rotate(new Vector3(1.0f, 0.0f, 0.0f), shift / 70);
            if (Keyboard[Key.K])
                node.rotate(new Vector3(1.0f, 0.0f, 0.0f), -shift / 70);
            if (Keyboard[Key.J])
                node.rotate(new Vector3(0.0f, 1.0f, 0.0f), shift / 70);
            if (Keyboard[Key.L])
                node.rotate(new Vector3(0.0f, 1.0f, 0.0f), -shift / 70);
            if (Keyboard[Key.U])
                node.rotate(new Vector3(0.0f, 0.0f, 1.0f), shift / 70);
            if (Keyboard[Key.O])
                node.rotate(new Vector3(0.0f, 0.0f, 1.0f), -shift / 70);

            if (Keyboard[Key.Escape])
                this.Exit();

            Vector3 cameraPos = cam.getPosition();
            labelPosX.SetText("X: " + cameraPos.X);
            labelPosY.SetText("Y: " + cameraPos.Y);
            labelPosZ.SetText("Z: " + cameraPos.Z);

            
                checkVao1.Hide();
                checkVao2.Hide();
                checkVao3.Hide();
                checkVao4.Hide();
                checkVao5.Hide();
                checkVao6.Hide();
                checkVao7.Hide();
                checkVao8.Hide();
                checkVao9.Hide();
                checkVao10.Hide();
                checkVao11.Hide();
                checkVao12.Hide();
                checkVao13.Hide();
            List<int> _vaoNum = new List<int>();
            
            foreach (int asd in node.GetListVao())
            {
                _vaoNum.Add(asd);
            }
            int num = _vaoNum.Count;
            if (num >= 1) { checkVao1.Show(); checkVao1.UserData = _vaoNum[0]; checkVao1.Text = String.Format("vao#{0}", _vaoNum[0]); }
            if (num >= 2) { checkVao2.Show(); checkVao2.UserData = _vaoNum[1]; checkVao2.Text = String.Format("vao#{0}", _vaoNum[1]); }
            if (num >= 3) { checkVao3.Show(); checkVao3.UserData = _vaoNum[2]; checkVao3.Text = String.Format("vao#{0}", _vaoNum[2]); }
            if (num >= 4) { checkVao4.Show(); checkVao4.UserData = _vaoNum[3]; checkVao4.Text = String.Format("vao#{0}", _vaoNum[3]); }
            if (num >= 5) { checkVao5.Show(); checkVao5.UserData = _vaoNum[4]; checkVao5.Text = String.Format("vao#{0}", _vaoNum[4]); }
            if (num >= 6) { checkVao6.Show(); checkVao6.UserData = _vaoNum[5]; checkVao6.Text = String.Format("vao#{0}", _vaoNum[5]); }
            if (num >= 7) { checkVao7.Show(); checkVao7.UserData = _vaoNum[6]; checkVao7.Text = String.Format("vao#{0}", _vaoNum[6]); }
            if (num >= 8) { checkVao8.Show(); checkVao8.UserData = _vaoNum[7]; checkVao8.Text = String.Format("vao#{0}", _vaoNum[7]); }
            if (num >= 9) { checkVao9.Show(); checkVao9.UserData = _vaoNum[8]; checkVao9.Text = String.Format("vao#{0}", _vaoNum[8]); }
            if (num >= 10) { checkVao10.Show(); checkVao10.UserData = _vaoNum[9]; checkVao10.Text = String.Format("vao#{0}", _vaoNum[9]); }
            if (num >= 11) { checkVao11.Show(); checkVao11.UserData = _vaoNum[10]; checkVao11.Text = String.Format("vao#{0}", _vaoNum[10]); }
            if (num >= 12) { checkVao12.Show(); checkVao12.UserData = _vaoNum[11]; checkVao12.Text = String.Format("vao#{0}", _vaoNum[11]); }
            if (num >= 13) { checkVao13.Show(); checkVao13.UserData = _vaoNum[12]; checkVao13.Text = String.Format("vao#{0}", _vaoNum[12]); }

            //labelSpeed.SetText("Скорость перемещения: " + shift);

            if (renderer.TextCacheSize > 50)
                renderer.FlushTextCache();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            this.Title = this.RenderFrequency.ToString() + " fps";
            statusBar.SetText(this.RenderFrequency.ToString() + " fps");

            GL.UseProgram(shaderProgram);
            Matrix4 hack = cam.getMatrix();
            GL.UniformMatrix4(viewLoc, false, ref hack);
            node.render();
            GL.UseProgram(0);

            canvas.RenderCanvas();

            SwapBuffers();
        }

        private void setProjectionUniform()
        {
            GL.UseProgram(shaderProgram);
            Matrix4 hack = proj.Matrix;
            GL.UniformMatrix4(projLoc, false, ref hack);
            GL.UseProgram(0);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, this.Width, this.Height);

            proj.AspectRatio = (float)this.Width / this.Height;
            setProjectionUniform();

            // canvas
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width, Height, 0, -1, 1);
            canvas.SetSize(Width, Height);
            //dock.SetSize(Width, Height);

            OnUpdateFrame(null);
            OnRenderFrame(null);
        }
    }
}