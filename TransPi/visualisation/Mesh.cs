using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using ClassLibrary;
using System.Drawing;

namespace TransPi.Visualisation
{
    class Mesh
    {
        public enum ColoringMethod
        {
            Grayscale,
            Fullcolor,
            Texture
        }

        public struct Vector4b
        {
            private byte v1, v2, v3, v4;

            public Vector4b(byte v1, byte v2, byte v3)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
                this.v4 = 0xff;
            }

            public byte V1
            {
                get { return v1; }
                set { v1 = value; }
            }

            public byte V2
            {
                get { return v2; }
                set { v2 = value; }
            }

            public byte V3
            {
                get { return v3; }
                set { v3 = value; }
            }

            public byte V4
            {
                get { return v4; }
                set { v4 = value; }
            }

            public override string ToString()
            {
                string s = "(" + v1 + ", " + v2 + ", " + v3 + ", " + v4 + ")";
                return s;
            }
        }

        public static uint restartIndex = 0xffffffff;
        private int primitiveCount = 0, elementOffset = 0;
        private int vao;
        private PrimitiveType primitiveType;
        public Tuple<Vector3[], Vector4b[], uint[]> _meshData;
        private Vector3 _beg, _naprav;

        public void SetCoord(Vector3 beg, Vector3 napr)
        {
            this._beg = beg;
            this._naprav = napr;
        }

        public Vector3 GetBegCoord()
        {
            return this._beg;
        }

        public Vector3 GetNaprCoord()
        {
            return this._naprav;
        }

        private static void invertIfNegative(ref Vector3 v)
        {
            if (v.X < 0)
                v.X = -v.X;
            if (v.Y < 0)
                v.Y = -v.Y;
            if (v.Z < 0)
                v.Z = -v.Z;
        }

        private static void calcFullcolor(ref Vector3 v_in, ref Vector4b v_out)
        {
            invertIfNegative(ref v_in);
            v_out.V1 = (byte)(127.0f * v_in.X);
            v_out.V2 = (byte)(127.0f * v_in.Y);
            v_out.V3 = (byte)(127.0f * v_in.Z);
            v_out.V4 = 0x7f;
        }

        private static void calcGrayscale(ref Vector3 v_in, ref Vector4b v_out)
        {
            invertIfNegative(ref v_in);
            v_out.V1 = (byte)(127.0f * v_in.Z);
            v_out.V2 = (byte)(127.0f * v_in.Z);
            v_out.V3 = (byte)(127.0f * v_in.Z);
            v_out.V4 = 0x7f;
        }

        private static void calcTexture(ref Color v_in, ref Vector4b v_out)
        {
            v_out.V1 = (byte)(v_in.R/2);
            v_out.V2 = (byte)(v_in.G/2);
            v_out.V3 = (byte)(v_in.B/2);
            v_out.V4 = 0x7f;
        }

        private static void calcTexture(ref Vector3 v_in, ref Vector4b v_out)
        {
            v_out.V1 = (byte)(v_in.X / 2);
            v_out.V2 = (byte)(v_in.Y / 2);
            v_out.V3 = (byte)(v_in.Z / 2);
            v_out.V4 = 0x7f;
        }

        private static int arrangeData(Vector3[] positions, Vector4b[] colors, uint[] elements)
        {
            
            // send vertex positions
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                new IntPtr(positions.Length * Vector3.SizeInBytes),
                positions, BufferUsageHint.StreamDraw);

            // send vertex colors
            int cbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, cbo);
            GL.BufferData(BufferTarget.ArrayBuffer,
                new IntPtr(colors.Length * 4),
                colors, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // send index data
            int ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                new IntPtr(elements.Length * 4),
                elements, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            // create and setup vertex array object
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, cbo);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Byte, true, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BindVertexArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            return vao;
        }

        public static void VaoUpdate(Vector3[] positions, int vao)
        {
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, (vao*3)-2);

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                new IntPtr(positions.Length * Vector3.SizeInBytes),
                positions, BufferUsageHint.StreamDraw);
                
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public Mesh(int offset, PrimitiveType type)
        {
            elementOffset = offset;
            primitiveType = type;
            SetCoord(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
        }

        public void SetMeshVector(Vector3[] positions)
        {
            Vector4b[] colors = this._meshData.Item2;
            uint[] elements = this._meshData.Item3;
            this._meshData = new Tuple<Vector3[], Vector4b[], uint[]>(positions, colors, elements);
        }

        public Mesh(Vector3[] positions, Vector4b[] colors, uint[] elements)
        {
            this._meshData = new Tuple<Vector3[], Vector4b[], uint[]>(positions, colors, elements);
            vao = arrangeData(positions, colors, elements);
            float xmin=0,ymin=0;
            foreach (Vector3 nn in positions)
            {
                if (xmin < nn.X) xmin = nn.X;
                if (ymin < nn.Y) ymin = nn.Y;
            }


            SetCoord(new Vector3(xmin, ymin, 0), new Vector3(1, 0, 0));

            positions = null;
            colors = null;
            elements = null;
            GC.Collect();
        }

        public Mesh(Vector3[] positions, Vector4b[] colors, uint[] elements, int offset, PrimitiveType type)
        {
            elementOffset = offset;
            primitiveType = type;

            this._meshData = new Tuple<Vector3[], Vector4b[], uint[]>(positions, colors, elements);
            vao = arrangeData(positions, colors, elements);

            SetCoord(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

            positions = null;
            colors = null;
            elements = null;
            GC.Collect();
        }


        public void render()
        {
            GL.BindVertexArray(vao);
            GL.DrawElements(primitiveType, primitiveCount, DrawElementsType.UnsignedInt, elementOffset * 4);
            GL.BindVertexArray(0);
        }

        private static Tuple<Vector3[], Vector4b[], uint[]> ParseZArray(ZArrayDescriptor desc, ColoringMethod coloring, Bitmap texture)
        {
            Vector3[,] vertexPositions = new Vector3[desc.width, desc.height];
            Vector3[,] normals = new Vector3[desc.width, desc.height];

            

            int z_max = 0, z_min = 0;
            int x_min = desc.width+1, y_min = desc.height+1;
            int x_lenght = 0, y_lenght = 0;
            for (int i = 0; i < desc.width; ++i)
                for (int j = 0; j < desc.height; ++j)
                {
                    if (desc.array[i, j] != 0)
                    {
                        if (i<x_min) x_min = i;
                        if (j<y_min) y_min = j;
                        if (i>x_lenght) x_lenght = i;
                        if (j>y_lenght) y_lenght = j;
                    }

                    if (desc.array[i,j] > z_max)
                        z_max = (int)desc.array[i,j];
                    if (desc.array[i,j] < z_min)
                        z_min = (int)desc.array[i,j];
                }
            x_lenght -= x_min; y_lenght -= y_min;
            float zCenterShift = (z_max + z_min) / 2;
            float xCenterShift = desc.width / 2;
            float yCenterShift = desc.height / 2;

            // this cycle to be optimized (?)
            for (int i = x_min; i < x_lenght + x_min - 1; ++i)
            {
                for (int j = y_min; j < y_lenght + y_min - 1; ++j)
                {
                    float x = i - xCenterShift;
                    float y = yCenterShift - j;

                    vertexPositions[i + 1, j + 1] = new Vector3(x + 1, y + 1, (float)desc.array[i + 1,j + 1] - zCenterShift);
                    vertexPositions[i + 1, j] = new Vector3(x + 1, y, (float)desc.array[i + 1, j] - zCenterShift);
                    vertexPositions[i, j] = new Vector3(x, y, (float)desc.array[i, j] - zCenterShift);

                    vertexPositions[i, j + 1] = new Vector3(x, y + 1, (float)desc.array[i, j + 1] - zCenterShift);
                    vertexPositions[i + 1, j + 1] = new Vector3(x + 1, y + 1, (float)desc.array[i + 1, j + 1] - zCenterShift);
                    vertexPositions[i, j] = new Vector3(x, y, (float)desc.array[i, j] - zCenterShift);

                    Vector3 norm1 = Vector3.Cross(
                        vertexPositions[i + 1, j + 1] - vertexPositions[i, j],
                        vertexPositions[i + 1, j] - vertexPositions[i, j]).Normalized();

                    Vector3 norm2 = Vector3.Cross(
                        vertexPositions[i, j + 1] - vertexPositions[i, j],
                        vertexPositions[i + 1, j + 1] - vertexPositions[i, j]).Normalized();

                    Vector3.Add(ref normals[i, j], ref norm1, out normals[i, j]);
                    Vector3.Add(ref normals[i + 1, j], ref norm1, out normals[i + 1, j]);
                    Vector3.Add(ref normals[i + 1, j + 1], ref norm1, out normals[i + 1, j + 1]);
                    Vector3.Add(ref normals[i, j], ref norm2, out normals[i, j]);
                    Vector3.Add(ref normals[i, j + 1], ref norm2, out normals[i, j + 1]);
                    Vector3.Add(ref normals[i + 1, j + 1], ref norm2, out normals[i + 1, j + 1]);
                }
            }

            // vertexes color calculated from average vertex normal
            Vector4b[] colors = new Vector4b[x_lenght * y_lenght];
            bool[] fill = new bool[x_lenght * y_lenght];
            uint ptr = 0;
            for (int i = x_min; i < x_lenght + x_min; ++i)
            {
                for (int j = y_min; j < y_lenght + y_min; ++j)
                {
                    if (desc.array[i, j] == 0) fill[ptr]=false;
                    else                       fill[ptr]=true;
                    Vector3.Multiply(ref normals[i, j], 0.166666666f, out normals[i, j]);
                    Color coll = new Color();
                    if (texture != null) coll = texture.GetPixel(i,j);
                    switch (coloring)
                    {
                        case ColoringMethod.Fullcolor:
                            calcFullcolor(ref normals[i, j], ref colors[ptr++]);
                            break;
                        case ColoringMethod.Grayscale:
                            calcGrayscale(ref normals[i, j], ref colors[ptr++]);
                            break;
                        case ColoringMethod.Texture:
                            calcTexture(ref coll, ref colors[ptr++]);
                            break;
                    }
                }
            }

            normals = null;
            GC.Collect();

            // data arrangement
            Vector3[] positions = new Vector3[x_lenght * y_lenght];
            //uint[] elements = new uint[2 * (x_lenght - 1) * (y_lenght + 1)];
            //uint[] elements = new uint[6 * (x_lenght - 1) * (y_lenght - 1)];
            List<uint> ele = new List<uint>();
            ptr = 0;
            for (int i = x_min; i < x_lenght + x_min; ++i)
                for (int j = y_min; j < y_lenght + y_min; ++j)
                    positions[ptr++] = vertexPositions[i, j];

            vertexPositions = null;
            GC.Collect();

            ptr = 0;

            for (uint i = 0; i < x_lenght - 1; ++i)
            {
                uint j;
                for (j = 0; j < y_lenght - 1; ++j)
                {
                    if (fill[i * y_lenght + j] && fill[(i + 1) * y_lenght + j] && fill[i * y_lenght + j + 1]) 
                    {
                        ele.Add((uint)(i * y_lenght + j));
                        ele.Add( (uint)((i + 1) * y_lenght + j));
                        ele.Add( (uint)(i * y_lenght + j + 1));
                    }
                    if (fill[(i + 1) * y_lenght + j] && fill[(i + 1) * y_lenght + j + 1] && fill[i * y_lenght + j + 1])
                    {
                        ele.Add((uint)((i + 1) * y_lenght + j));
                        ele.Add ((uint)((i + 1) * y_lenght + j + 1));
                        ele.Add( (uint)(i * y_lenght + j + 1));
                    }
                    
                }
            }
            uint[] elements = ele.ToArray();
            /*for (uint i = 0; i < x_lenght  - 1; ++i)
            {
                uint j;
                for (j = 0; j < y_lenght ; ++j)
                {
                    elements[ptr++] = (uint)(i * y_lenght + j);
                    elements[ptr++] = (uint)((i + 1) * y_lenght + j);
                }
                //elements[ptr++] = restartIndex;
                elements[ptr++] = (uint)((i + 1) * y_lenght + j - 1);
                elements[ptr++] = (uint)((i + 1) * y_lenght);
            }*/

            return new Tuple<Vector3[], Vector4b[], uint[]>(positions, colors, elements);
        }

        public static Mesh FromZArray(ZArrayDescriptor desc, ColoringMethod coloring, Bitmap texture)
        {
            Tuple<Vector3[], Vector4b[], uint[]> meshData = ParseZArray(desc, coloring, texture);
            Mesh rv = new Mesh(meshData.Item1, meshData.Item2, meshData.Item3);
            rv.primitiveCount = meshData.Item3.Length;
            rv.primitiveType = PrimitiveType.Triangles;
            return rv;
        }

        public static void ZArrayToObject(ZArrayDescriptor desc, ColoringMethod coloring, string path, Bitmap texture)
        {
            Tuple<Vector3[], Vector4b[], uint[]> meshData = ParseZArray(desc, coloring, texture);

            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("o surface");

            // write verteces
            for (int i = 0; i < meshData.Item1.Length; ++i)
                sw.WriteLine("v " + meshData.Item1[i].X
                    + " " + meshData.Item1[i].Y
                    + " " + meshData.Item1[i].Z);

            // write normals
            for (int i = 0; i < meshData.Item2.Length; ++i)
                sw.WriteLine("vt " + ((float)meshData.Item2[i].V1) / 127.0f
                    + " " + ((float)meshData.Item2[i].V2) / 127.0f
                    + " " + ((float)meshData.Item2[i].V3) / 127.0f);

            // write indices
            for (int i = 0; i < meshData.Item3.Length - 2; i++) 
            { 
                int v1 = (int)(meshData.Item3[i++] + 1);
                int v2 = (int)(meshData.Item3[i++] + 1);
                int v3 = (int)(meshData.Item3[i] + 1);

                sw.WriteLine("f " + v1 + "/" + v1
                    + " " + v2 + "/" + v2
                    + " " + v3 + "/" + v3);
            }
/*
            for (int i = 0; i < desc.width - 1; ++i)
                for (int j = 0; j < desc.height - 1; ++j)
                {
                    int v1 = 1 + (i * desc.height + j);
                    int v2 = v1 + 1;           // = 1 + (i * desc.height + j + 1);
                    int v3 = v2 + desc.height; // = 1 + ((i + 1) * desc.height + j + 1);
                    int v4 = v1 + desc.height; // = 1 + ((i + 1) * desc.height + j);

                    sw.WriteLine("f " + v1 + "//" + v1 + " "
                        + v2 + "//" + v2 + " "
                        + v3 + "//" + v3 + " "
                        + v4 + "//" + v4 + " ");
                }*/

            sw.WriteLine("");
            sw.Close();
        }

        public static void MeshToObject(List<Mesh> meshes, string path)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("o surface");
            int count_vec = 0;
            foreach (Mesh mesh in meshes) 
            { 

            Tuple<Vector3[], Vector4b[], uint[]> meshData = mesh._meshData;

            // write verteces
            for (int i = 0; i < meshData.Item1.Length; ++i)
                sw.WriteLine("v " + meshData.Item1[i].X
                    + " " + meshData.Item1[i].Y
                    + " " + meshData.Item1[i].Z);

            // write normals
            for (int i = 0; i < meshData.Item2.Length; ++i)
                sw.WriteLine("vt " + ((float)meshData.Item2[i].V1) / 127.0f
                    + " " + ((float)meshData.Item2[i].V2) / 127.0f
                    + " " + ((float)meshData.Item2[i].V3) / 127.0f);

            // write indices
            for (int i = 0; i < meshData.Item3.Length - 2; i++)
            {
                int v1 = (int)(meshData.Item3[i++] + 1 + count_vec);
                int v2 = (int)(meshData.Item3[i++] + 1 + count_vec);
                int v3 = (int)(meshData.Item3[i] + 1 + count_vec);

                sw.WriteLine("f " + v1 + "/" + v1
                    + " " + v2 + "/" + v2
                    + " " + v3 + "/" + v3);
            }
            sw.WriteLine("g ");
            count_vec += meshData.Item1.Length;
            sw.WriteLine("");
            }
            sw.Close();
        }

        private static int ParseIndex(int idx, int vcnt)
        {
            if (idx < 0)
                return vcnt + idx;
            else
                return idx - 1;
        }

        private static void AddNormal(List<Vector3> vertices, List<Vector3> normals, int i1, int i2, int i3)
        {
            Vector3 normal = Vector3.Cross(vertices[i1] - vertices[i2], vertices[i3] - vertices[i2]).Normalized();
            normals[i1] += normal;
            normals[i2] += normal;
            normals[i3] += normal;
        }

        public int GetVao()
        {
            return this.vao;
        }

        private static Tuple<Vector3[], Vector4b[], uint[]> ListToArrayMesh(List<Vector3> verticesMesh, List<Vector3> colorersMesh, List<uint> elementsMesh)
        {
            Vector4b[] colors = new Vector4b[verticesMesh.Count];
            for (int i = 0; i < verticesMesh.Count; ++i)
            {
                Vector3 n = verticesMesh[i];
                calcFullcolor(ref n, ref colors[i]);
            }

            Vector3[] positions = new Vector3[verticesMesh.Count];
            for (int i = 0; i < verticesMesh.Count; ++i)
                positions[i] = verticesMesh[i];

            uint[] elems = new uint[elementsMesh.Count];
            for (int i = 0; i < elementsMesh.Count; ++i)
                elems[i] = elementsMesh[i];

            return new Tuple<Vector3[], Vector4b[], uint[]>(positions, colors, elems); 
        }

        public static List<Mesh> FromObject(string path)
        {
            uint lineno = 1;
            StreamReader sr = new StreamReader(path);
            PrimitiveType ptype = PrimitiveType.Triangles;

            List<Mesh> meshes = new List<Mesh>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector3> colorers = new List<Vector3>();
            List<uint> elements = new List<uint>();
            int offset = 0, primitiveCnt = 0;
            Mesh mesh = new Mesh(0, ptype);
            List<Vector3> verticesMesh = new List<Vector3>();
            List<Vector3> colorersMesh = new List<Vector3>();
            List<uint> elementsMesh = new List<uint>();

            float xmin = 20000, ymin = 20000,
                  xmax = -20000, ymax = -20000;

            for (string current_line = sr.ReadLine();
                current_line != null;
                current_line = sr.ReadLine(), lineno++)
            {
                string[] split = current_line.Split(null);
                if (split.Length == 1 && split[0] == "")
                    continue;

                // get rid of empty strings (caused by repeated delimiters):
                // v\x20\x20\x200.1234 converts to: {"v", "", "", "0.1234"}
                int notempty = 0;
                for (int i = 0; i < split.Length; ++i)
                    if (split[i] != "")
                        ++notempty;

                string[] tok = split;
                if (notempty != split.Length)
                {
                    tok = new string[notempty];
                    for (int i = 0, j = 0; i < split.Length; ++i)
                        if (split[i] != "")
                            tok[j++] = split[i];
                }

                switch (tok[0])
                {
                    case "v":
                        if (tok.Length < 4)
                            throw new Exception("Parsing error at line " + lineno + ": expected at least 4 tokens");

                        vertices.Add(new Vector3(float.Parse(tok[1]), float.Parse(tok[2]), float.Parse(tok[3])));
                        verticesMesh.Add(new Vector3(float.Parse(tok[1]), float.Parse(tok[2]), float.Parse(tok[3])));
                        normals.Add(new Vector3());
                        break;

                    case "vt":
                        if (tok.Length < 4)
                            throw new Exception("Parsing error at line " + lineno + ": expected at least 4 tokens");

                        colorers.Add(new Vector3(float.Parse(tok[1]), float.Parse(tok[2]), float.Parse(tok[3])));
                        colorersMesh.Add(new Vector3(float.Parse(tok[1]), float.Parse(tok[2]), float.Parse(tok[3])));
                        break;

                    case "vn":
                        if (tok.Length < 4)
                            throw new Exception("Parsing error at line " + lineno + ": expected at least 4 tokens");

                        colorers.Add(new Vector3(float.Parse(tok[1]), float.Parse(tok[2]), float.Parse(tok[3])));
                        colorersMesh.Add(new Vector3(float.Parse(tok[1]), float.Parse(tok[2]), float.Parse(tok[3])));
                        break;

                    case "f":
                        if (tok.Length < 4)
                            throw new Exception("Parsing error at line " + lineno + ": expected at least 4 tokens");

                        elements.Add(uint.Parse(tok[1].Split('/')[0]) - 1);
                        elements.Add(uint.Parse(tok[2].Split('/')[0]) - 1);
                        elements.Add(uint.Parse(tok[3].Split('/')[0]) - 1);
                        
                        elementsMesh.Add(uint.Parse(tok[1].Split('/')[0]) - 1);
                        elementsMesh.Add(uint.Parse(tok[2].Split('/')[0]) - 1);
                        elementsMesh.Add(uint.Parse(tok[3].Split('/')[0]) - 1);

                        offset += 3;
                        primitiveCnt += 3;

                            /*
                        int i1 = ParseIndex(int.Parse(tok[1].Split('/')[0]), vertices.Count);
                        int i2 = ParseIndex(int.Parse(tok[2].Split('/')[0]), vertices.Count);
                        int i3 = ParseIndex(int.Parse(tok[3].Split('/')[0]), vertices.Count);
                        int i4 = tok.Length >= 5 ? int.Parse(tok[4].Split('/')[0]) : 0;

                        AddNormal(vertices, normals, i1, i2, i3);
                        elements.Add((uint)i1);
                        elements.Add((uint)i2);
                        elements.Add((uint)i3);
                        offset += 3;
                        primitiveCnt += 3;

                        if (i4 != 0)
                        {
                            i4 = ParseIndex(i4, vertices.Count);
                            elements.Add((uint)i1);
                            elements.Add((uint)i3);
                            elements.Add((uint)i4);
                            AddNormal(vertices, normals, i1, i3, i4);
                            offset += 3;
                            primitiveCnt += 3;
                        }*/
                        break;
                    case "o":
                    case "g":
                        if (mesh != null && primitiveCnt != 0) //wrong
                        {
                            mesh.primitiveCount = primitiveCnt;
                            primitiveCnt = 0;
                            //mesh._meshData = ListToArrayMesh(verticesMesh, colorersMesh, elementsMesh);
                            Vector4b[] colors1 = new Vector4b[colorersMesh.Count];
                            for (int i = 0; i < colorersMesh.Count; ++i)
                            {
                                //Vector3 n = normals[i].Normalized();
                                Vector3 n = colorersMesh[i];
                                calcFullcolor(ref n, ref colors1[i]);
                            }

                            // arrange
                            Vector3[] positions1 = new Vector3[verticesMesh.Count];
                            for (int i = 0; i < verticesMesh.Count; ++i)
                                positions1[i] = verticesMesh[i];

                            uint[] elems1 = new uint[elementsMesh.Count];
                            for (int i = 0; i < elementsMesh.Count; ++i)
                                elems1[i] = elementsMesh[i];

                            mesh._meshData = new Tuple<Vector3[], Vector4b[], uint[]>(positions1, colors1, elems1);
                            
                            meshes.Add(mesh);
                            verticesMesh = new List<Vector3>();
                            colorersMesh = new List<Vector3>();
                            elementsMesh = new List<uint>();
                        }
                        mesh = new Mesh(offset, ptype);
                        break;
                }
            }

            if (meshes.Count == 0 && primitiveCnt == 0)
                return null;

            if (primitiveCnt != 0)
            {
                mesh.primitiveCount = primitiveCnt;

                

                //mesh._meshData = ListToArrayMesh(verticesMesh, colorersMesh, elementsMesh);

                Vector4b[] colors1 = new Vector4b[colorersMesh.Count];
                for (int i = 0; i < colorersMesh.Count; ++i)
                {
                    //Vector3 n = normals[i].Normalized();
                    Vector3 n = colorersMesh[i];
                    calcFullcolor(ref n, ref colors1[i]);
                }

                // arrange
                Vector3[] positions1 = new Vector3[verticesMesh.Count];
                for (int i = 0; i < verticesMesh.Count; ++i)
                    positions1[i] = verticesMesh[i];

                uint[] elems1 = new uint[elementsMesh.Count];
                for (int i = 0; i < elementsMesh.Count; ++i)
                    elems1[i] = elementsMesh[i];
                mesh._meshData = new Tuple<Vector3[], Vector4b[], uint[]>(positions1, colors1, elems1);
                meshes.Add(mesh);
            }


            //Tuple<Vector3[], Vector4b[], uint[]> meshCoord = ListToArrayMesh(vertices, colorers, elements);
            //int vao = arrangeData(meshCoord.Item1, meshCoord.Item2, meshCoord.Item3);

            // normals
            Vector4b[] colors = new Vector4b[normals.Count];
            for (int i = 0; i < colorers.Count; ++i)
            {
                //Vector3 n = normals[i].Normalized();
                Vector3 n = colorers[i];
                calcFullcolor(ref n, ref colors[i]);
            }

            // arrange
            Vector3[] positions = new Vector3[vertices.Count];
            for (int i = 0; i < vertices.Count; ++i)
            {
                positions[i] = vertices[i];
                if (xmin > vertices[i].X) xmin = vertices[i].X;
                if (ymin > vertices[i].Y) ymin = vertices[i].Y;
                if (xmax < vertices[i].X) xmax = vertices[i].X;
                if (ymax < vertices[i].Y) ymax = vertices[i].Y;
            }
                

            uint[] elems = new uint[elements.Count];
            for (int i = 0; i < elements.Count; ++i)
                elems[i] = elements[i];


            double angleY = 40 * Math.PI / 180;
            Vector3 axisY = new Vector3(0.0f, 1.0f, 0.0f);
            axisY.Normalize();
            axisY = axisY * (float)Math.Sin(angleY / 2);
            float scalarY = (float)Math.Cos(angleY / 2);
            Quaternion qr = new Quaternion(axisY, scalarY);
            Vector3 per = new Vector3(1, 0, 0);
            per = SceneNode.quatTransformToVector(qr, per);
            per.Normalize();

            int vao = arrangeData(positions, colors, elems);
            foreach (Mesh m in meshes)
            {
                mesh.SetCoord(new Vector3(0, 0, (float)((xmin + xmax) / 3.5)), per);
                m.vao = vao;
            }
            return meshes;
        }

        
    }
}
