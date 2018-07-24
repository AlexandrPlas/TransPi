using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TransPi.Visualisation
{
    class SceneNode
    {
        private List<Mesh> meshs;
        private int modelLoc;

        private Quaternion orientation = new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);
        private Vector3 vOffset;
        
        public SceneNode(Mesh mesh, int modelMatrixLoc)
        {
            this.meshs = new List<Mesh> {mesh};
            modelLoc = modelMatrixLoc;
        }

        public void addMeshSceneNode(Mesh mesh)
        {
            this.meshs.Add(mesh);
        }

        public void addMeshSceneNode(List<Mesh> meshes)
        {
            foreach (Mesh mesh in meshes)
                this.meshs.Add(mesh);
        }

        public List<int> GetListVao()
        {
            List<int> vaotr = new List<int>();
            foreach (Mesh mesh in this.meshs)
                vaotr.Add(mesh.GetVao());
            return vaotr;
        }

        public void SaveMeshSceneNode(string path)
        {
            Mesh.MeshToObject(this.meshs, path);
        }

        public void VaoRotate(double rx, double ry, double rz, int vao)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<uint> elements = new List<uint>();
            List<TransPi.Visualisation.Mesh.Vector4b> colorers = new List<TransPi.Visualisation.Mesh.Vector4b>();
            foreach (Mesh mesh in this.meshs)
            {
                if (mesh.GetVao() == vao)
                {
                    double angleY = ry * Math.PI / 180;
                    Vector3 axisY = new Vector3(0.0f, 1.0f, 0.0f);
                    axisY.Normalize();
                    axisY = axisY * (float)Math.Sin(angleY / 2);
                    float scalarY = (float)Math.Cos(angleY / 2);
                    Quaternion qr = new Quaternion(axisY, scalarY);

                    Vector3 per = quatTransformToVector(qr, mesh.GetNaprCoord());

                    double angleX = rx * Math.PI / 180;
                    Vector3 axisX = per;//new Vector3(1.0f, 0.0f, 0.0f);
                    axisX.Normalize();
                    axisX = axisX * (float)Math.Sin(angleX / 2);
                    float scalarX = (float)Math.Cos(angleX / 2);
                    Quaternion qr2 = new Quaternion(axisX, scalarX);
                    per = quatTransformToVector(qr2, mesh.GetNaprCoord());
                    qr *= qr2;
                    per.Normalize();
                    mesh.SetCoord(mesh.GetBegCoord(), per);
                    
                    double angleZ = rz * Math.PI / 180;
                    Vector3 axisZ = new Vector3(0.0f, 0.0f, 1.0f);
                    axisZ.Normalize();
                    axisZ = axisZ * (float)Math.Sin(angleZ / 2);
                    float scalarZ = (float)Math.Cos(angleZ / 2);
                    Quaternion qr3 = new Quaternion(axisZ, scalarZ);

                    qr *= qr3;

                    Vector3 begin = mesh.GetBegCoord();
                    List<Vector3> verticesMesh = new List<Vector3>();
                    foreach (Vector3 ratt in mesh._meshData.Item1)
                    {
                        Vector3 rerr = quatTransformToVector(qr, ratt - begin);
                        verticesMesh.Add(rerr);
                        vertices.Add(rerr);
                    }
                    foreach (TransPi.Visualisation.Mesh.Vector4b ratt in mesh._meshData.Item2)
                    {
                        colorers.Add(ratt);
                    }
                    foreach (uint ratt in mesh._meshData.Item3)
                    {
                        elements.Add(ratt);
                    }
                    Vector3[] positionsMesh = verticesMesh.ToArray();
                    mesh.SetMeshVector(positionsMesh);
                }
            }

            Vector3[] positions = vertices.ToArray();
            Mesh.VaoUpdate(positions, vao);
        }

        public void VaoMove(double mx, double my, double mz,int vao)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<uint> elements = new List<uint>();
            List<TransPi.Visualisation.Mesh.Vector4b> colorers = new List<TransPi.Visualisation.Mesh.Vector4b>();
            foreach (Mesh mesh in this.meshs)
            {
                if (mesh.GetVao() == vao)
                {
                    Vector3 rotate=mesh.GetNaprCoord();
                    List<Vector3> verticesMesh = new List<Vector3>();
                    foreach (Vector3 ratt in mesh._meshData.Item1)
                    {
                        Vector3 rerr = ratt - (rotate * (int)mx);

                        double angleY = 90 * Math.PI / 180;
                        Vector3 axisY = new Vector3(0.0f, 1.0f, 0.0f);
                        axisY.Normalize();
                        axisY = axisY * (float)Math.Sin(angleY / 2);
                        float scalarY = (float)Math.Cos(angleY / 2);
                        Quaternion qr = new Quaternion(axisY, scalarY);
                        Vector3 per = quatTransformToVector(qr, rotate);

                        rerr += (per *(int)mz);
                        Vector3 perr = new Vector3(rerr.X, (float)(rerr.Y - my), rerr.Z);
                        verticesMesh.Add(perr);
                        vertices.Add(perr);
                    }
                    foreach (TransPi.Visualisation.Mesh.Vector4b ratt in mesh._meshData.Item2)
                    {
                        colorers.Add(ratt);
                    }
                    foreach (uint ratt in mesh._meshData.Item3)
                    {
                        elements.Add(ratt);
                    }
                    Vector3[] positionsMesh = verticesMesh.ToArray();
                    mesh.SetMeshVector(positionsMesh);
                }
            }

            Vector3[] positions = vertices.ToArray();
            Mesh.VaoUpdate(positions, vao);
        }

        private static Quaternion quatMulVector(Quaternion a,Vector3 b) 
        {
            Quaternion res = new Quaternion(); 
            res.W = -a.X * b.X - a.Y * b.Y - a.Z * b.Z;
            res.X = a.W * b.X + a.Y * b.Z - a.Z * b.Y;
            res.Y = a.W * b.Y - a.X * b.Z + a.Z * b.X;
            res.Z = a.W * b.Z + a.X * b.Y - a.Y * b.X;
            return  res;
        }

        public static Vector3 quatTransformToVector(Quaternion q,Vector3 v) 
        {
            Quaternion t;
            
            t = quatMulVector(q, v);
            t *= Quaternion.Invert(q);
            Vector3 res;
            res.X = t.X;
            res.Y = t.Y;
            res.Z = t.Z;

            return res;
        }

        public void render()
        {
            Matrix4 hack = getMatrix();
            GL.UniformMatrix4(modelLoc, false, ref hack);
            foreach (Mesh mesh in meshs)
            {
                mesh.render();
            }
        }

        private Matrix4 getMatrix()
        {
            return Matrix4.Identity * Matrix4.CreateTranslation(vOffset) * Matrix4.CreateFromQuaternion(orientation);
        }

        public void rotate(Vector3 axis, float angle)
        {
            axis.Normalize();
            axis = axis * (float)Math.Sin(angle / 2.0f);
            float scalar = (float)Math.Cos(angle / 2.0f);
            rotate(new Quaternion(axis, scalar));
        }

        public void rotate(Quaternion orient)
        {
            orientation *= orient;
            orientation.Normalize();
        }

        public void setOrientation(Quaternion orient)
        {
            orientation = orient;
        }

        public void offset(Vector3 offset)
        {
            vOffset += offset;
        }

        public void setTransition(Vector3 offset)
        {
            vOffset = offset;
        }
    }
}
