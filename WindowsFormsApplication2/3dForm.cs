using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2;
using devDept.Eyeshot.Standard;
using devDept.Eyeshot;

namespace kMeans
{
    public partial class _3dForm : Form
    {
        BackgroundWorker worker = new BackgroundWorker();

        public _3dForm()
        {
            InitializeComponent();
            InitializeThreads();
            viewportProfessional1.Camera.ProjectionMode = cameraProjectionType.Orthographic;
        }
        public void InitializeThreads()
        {
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<MulticolorOnVerticesMesh> meshes = e.Result as List<MulticolorOnVerticesMesh>;
            if (viewportProfessional1.Entities.Count > 0)
            {
                viewportProfessional1.Entities.Clear();
                meshes.ForEach(mesh => viewportProfessional1.Entities.Add(mesh));
                viewportProfessional1.Refresh();
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<MulticolorOnVerticesMesh> meshes = new List<MulticolorOnVerticesMesh>();
            List<Point3D> pnts = new List<Point3D>();
            List<List<WindowsFormsApplication2.ColoredPoint>> data = e.Argument as List<List<WindowsFormsApplication2.ColoredPoint>>;
            Dictionary<string, int> edgeinfo = new Dictionary<string,int>();

            for (int i = 0; i < data.Count; i++)
            {
                edgeinfo = GetColumnAndRowCounts(data[i]);
                meshes.Add(CreateMesh(data[i], edgeinfo["rows"]));
            }

            e.Result = meshes;
        }
        devDept.Eyeshot.Standard.MulticolorPointCloud cloud = new devDept.Eyeshot.Standard.MulticolorPointCloud(10, 5.0f);
        devDept.Eyeshot.Standard.MulticolorPointCloud InverseCloud = new devDept.Eyeshot.Standard.MulticolorPointCloud(10, 5.0f);

        devDept.Eyeshot.Standard.MulticolorOnVerticesMesh m_mesh = new MulticolorOnVerticesMesh(10);

        public void UpdateSurfacePointMesh(List<WindowsFormsApplication2.ColoredPoint> xyPnts, int cols)
        {
            List<Point3D> pnts = new List<Point3D>();
            for (int i = 0; i < xyPnts.Count; i++)
                pnts.Add(new Point3D(xyPnts[i].X, xyPnts[i].Y, xyPnts[i].val));

            cloud = new devDept.Eyeshot.Standard.MulticolorPointCloud(pnts.Count, 5.0f);
            //InverseCloud = new MulticolorPointCloud(pnts.Count, 5.0f);
            for (int i = 0; i < pnts.Count; i++)
            {
                cloud.SetPoint(i, pnts[i].X, pnts[i].Y, pnts[i].Z, xyPnts[i].color.R, xyPnts[i].color.G, xyPnts[i].color.B);
                //InverseCloud.SetPoint(i, pnts[i].X, pnts[i].Y, -pnts[i].Z, xyPnts[i].color.R, xyPnts[i].color.G, xyPnts[i].color.B);
            }

            if (viewportProfessional1.Entities.Count > 0)
            {
                viewportProfessional1.Entities[0] = cloud;
                viewportProfessional1.Refresh();
            }
        }

        public void UpdateSurfacePointMesh(List<List<WindowsFormsApplication2.ColoredPoint>> xyPnts, int cols)
        {
            List<MulticolorPointCloud> clouds = new List<MulticolorPointCloud>();
            List<Point3D> pnts = new List<Point3D>();
            for (int i = 0; i < xyPnts.Count; i++)
            {
                for (int j = 0; j < xyPnts[i].Count; j++)
                    pnts.Add(new Point3D(xyPnts[i][j].X, -xyPnts[i][j].Y, xyPnts[i][j].val));
                clouds.Add(new MulticolorPointCloud(pnts.Count, 5.0f));
            }
            viewportProfessional1.Entities.Clear();
            for (int i = 0; i < xyPnts.Count; i++)
            {
                int count = 0;
                foreach (ColoredPoint pnt in xyPnts[i])
                {
                    clouds[i].SetPoint(count, pnt.X, pnt.Y, pnt.val, pnt.color.R, pnt.color.G, pnt.color.B);
                    count++;
                }
                viewportProfessional1.Entities.Add(clouds[i]);
            }

            viewportProfessional1.Refresh();
        }

        public void UpdateSurfaceMesh(List<WindowsFormsApplication2.ColoredPoint> xyPnts, int cols)
        {
            Dictionary<string, int> edgeinfo = GetColumnAndRowCounts(xyPnts);
            CreateFullMesh(xyPnts, edgeinfo["rows"] - 4);

            if (viewportProfessional1.Entities.Count > 0)
            {
                viewportProfessional1.Entities[1] = m_mesh;
                viewportProfessional1.Refresh();
            }
        }

        public void UpdateSurfaceMeshParts(List<List<WindowsFormsApplication2.ColoredPoint>> xyPnts, int cols)
        {
            if (worker.IsBusy)
            {
                //worker.CancelAsync();
                //Application.DoEvents();
                return;
            }

            worker.RunWorkerAsync(xyPnts);
        }

        Dictionary<string, int> GetColumnAndRowCounts(List<ColoredPoint> data)
        {
            ColoredPoint p = new ColoredPoint(new System.Drawing.Point(-1, -1), Color.Black);

            int rowCount = 0;
            int ColCount = 0;

            data.ForEach(pnt =>
            {
                if (pnt.X != p.X && p.X < pnt.X)
                {
                    ColCount++;
                    p.X = pnt.X;
                }
                if (pnt.Y != p.Y && p.Y < pnt.Y)
                {
                    rowCount++;
                    p.Y = pnt.Y;
                }
            });

            return new Dictionary<string, int>() { { "rows", rowCount }, { "cols", ColCount } };


        }

        public devDept.Eyeshot.Standard.MulticolorOnVerticesMesh CreateMesh(List<WindowsFormsApplication2.ColoredPoint> coloredPs, int cols)
        {
            List<Point3D> vertices = new List<Point3D>(coloredPs.Count);

            for (int i = 0; i < coloredPs.Count; i++)
                vertices.Add(new Point3D(coloredPs[i].X, coloredPs[i].Y, coloredPs[i].val));

            int rows = vertices.Count / cols;

            devDept.Eyeshot.Standard.MulticolorOnVerticesMesh mesh = new MulticolorOnVerticesMesh(vertices.Count);

            // create the mesh
            System.Drawing.Color c = System.Drawing.Color.SlateBlue;
            if (mesh.Vertices.Length != vertices.Count)
                mesh.ResizeVertices(vertices.Count);

            mesh.NormalAveragingMode = devDept.Eyeshot.Standard.Mesh.normalAveragingType.Averaged;
            //mesh.EntityData = this; //point back to this object

            // set the vertices
            mesh.Vertices = vertices.ToArray();
            mesh.UpdateBoundingBox();

            //color the vertices
            //int i = ReColorMesh(colorvals);

            for (int i = 0; i < coloredPs.Count; i++)
                mesh.SetVertex(i, coloredPs[i].color.R, coloredPs[i].color.G, coloredPs[i].color.B);

            //if (i != vertices.Length)
            //return -3;

            // set vertex indices
            mesh.Triangles.Clear();
            mesh.Triangles.Capacity = (rows - 1) * (cols - 1) * 2;
            for (int j = 0; j < (rows - 1); j++)
            {
                for (int i = 0; i < (cols - 1); i++)
                {

                    mesh.Triangles.Add(new Mesh.Triangle(i + j * cols,
                                                                              i + j * cols + 1,
                                                                              i + (j + 1) * cols + 1));
                    mesh.Triangles.Add(new Mesh.Triangle(i + j * cols,
                                                                              i + (j + 1) * cols + 1,
                                                                              i + (j + 1) * cols));
                }
            }



            mesh.ComputeEdges();
            mesh.ComputeNormals();
            mesh.NormalAveragingMode = Mesh.normalAveragingType.None;
            //m_mesh.Rotate(Math.PI, new Vector3D(0, 0, 1));
            return mesh;
        }

        public int CreateFullMesh(List<WindowsFormsApplication2.ColoredPoint> coloredPs, int cols)
        {
            List<Point3D> vertices = new List<Point3D>(coloredPs.Count);
            for (int i = 0; i < coloredPs.Count; i++)
                vertices.Add(new Point3D(coloredPs[i].X, coloredPs[i].Y, coloredPs[i].val));

            int rows = vertices.Count / cols;

            // create the mesh
            System.Drawing.Color c = System.Drawing.Color.SlateBlue;
            if (m_mesh.Vertices.Length != vertices.Count)
                m_mesh.ResizeVertices(vertices.Count);

            m_mesh.NormalAveragingMode = devDept.Eyeshot.Standard.Mesh.normalAveragingType.Averaged;
            m_mesh.EntityData = this; //point back to this object

            // set the vertices
            m_mesh.Vertices = vertices.ToArray();
            m_mesh.UpdateBoundingBox();

            //color the vertices
            //int i = ReColorMesh(colorvals);

            for (int i = 0; i < coloredPs.Count; i++)
                m_mesh.SetVertex(i++, coloredPs[i].color.R, coloredPs[i].color.G, coloredPs[i].color.B);

            //if (i != vertices.Length)
            //return -3;

            // set vertex indices
            m_mesh.Triangles.Clear();
            m_mesh.Triangles.Capacity = (rows - 1) * (cols - 1) * 2;
            for (int j = 0; j < (rows - 1); j++)
            {
                for (int i = 0; i < (cols - 1); i++)
                {

                    m_mesh.Triangles.Add(new Mesh.Triangle(i + j * cols,
                                                                              i + j * cols + 1,
                                                                              i + (j + 1) * cols + 1));
                    m_mesh.Triangles.Add(new Mesh.Triangle(i + j * cols,
                                                                              i + (j + 1) * cols + 1,
                                                                              i + (j + 1) * cols));
                }
            }



            m_mesh.ComputeEdges();
            m_mesh.ComputeNormals();
            m_mesh.NormalAveragingMode = Mesh.normalAveragingType.None;
            //m_mesh.Rotate(Math.PI, new Vector3D(0, 0, 1));
            return m_mesh.Triangles.Count;
        }

        private void _3dForm_Load(object sender, EventArgs e)
        {
            viewportProfessional1.Entities.Add(cloud);
            viewportProfessional1.Entities.Add(m_mesh);
            viewportProfessional1.DisplayMode = viewportDisplayType.Shaded;
        }


    }
}
