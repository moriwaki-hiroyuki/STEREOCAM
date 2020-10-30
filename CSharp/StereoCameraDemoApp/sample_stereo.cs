using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using System.Diagnostics;
using OpenCvSharp.Extensions;
using OpenCvSharp.CPlusPlus;

namespace StereoVision
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string filepath = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            Cv.SetCaptureProperty(capture1, CaptureProperty.FrameWidth, pictureBox3.Width);
            Cv.SetCaptureProperty(capture1, CaptureProperty.FrameHeight, pictureBox3.Height);
            Cv.SetCaptureProperty(capture2, CaptureProperty.FrameWidth, pictureBox3.Width);
            Cv.SetCaptureProperty(capture2, CaptureProperty.FrameHeight, pictureBox3.Height);
            string filename = @"C:\Users\PCUser\Pictures\calib\stereoCalibrate.xml";
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Title = "開くファイルを選択してください";
            //ofd.RestoreDirectory = true;
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    filename = ofd.FileName;
            //}
            //ofd.Dispose();
            CvFileStorage fs = Cv.OpenFileStorage(filename, null, FileStorageMode.Read);
            CvFileNode param = Cv.GetFileNodeByName(fs, null, "mapLX");
            mapLX = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "mapLY");
            mapLY = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "mapRX");
            mapRX = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "mapRY");
            mapRY = Cv.Read<CvMat>(fs, param);
            Cv.ReleaseFileStorage(fs);
            filename = @"C:\Users\PCUser\Pictures\calib\camera1.xml";
            //ofd.Title = "左カメラ補正ファイルを選択してください";
            //filename = "";
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    filename = ofd.FileName;
            //}
            fs = new CvFileStorage(filename, null, FileStorageMode.Read);
            param = Cv.GetFileNodeByName(fs, null, "intrinsic");
            intrinsicL = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "distortion");
            distortionL = Cv.Read<CvMat>(fs, param);
            Cv.ReleaseFileStorage(fs);

            filename = @"C:\Users\PCUser\Pictures\calib\camera1.xml";
            //ofd.Title = "右カメラ補正ファイルを選択してください";
            //filename = "";
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    filename = ofd.FileName;
            //}
            //ofd.Dispose();
            fs = new CvFileStorage(filename, null, FileStorageMode.Read);
            param = Cv.GetFileNodeByName(fs, null, "intrinsic");
            intrinsicR = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "distortion");
            distortionR = Cv.Read<CvMat>(fs, param);
            Cv.ReleaseFileStorage(fs);

            timer1.Enabled = true;
        }

        CvMat mapLX, mapLY, mapRX, mapRY, intrinsicL, distortionL, intrinsicR, distortionR, rotation;
        CvCapture capture1 = Cv.CreateCameraCapture(0);
        CvCapture capture2 = Cv.CreateCameraCapture(1);
        IplImage frame1 = new IplImage();
        IplImage frame2 = new IplImage();
        Bitmap bmp1, bmp2, bufR, bufL, bmp4, bmp5;

        private void timer1_Tick(object sender, EventArgs e)
        {
            CvSize size = new CvSize(pictureBox1.Size.Width, pictureBox1.Size.Height);
            using (IplImage imgR = Cv.QueryFrame(capture1))
            using (IplImage imgL = Cv.QueryFrame(capture2))
            using (IplImage miniR = new IplImage(size, imgR.Depth, imgR.ElemChannels))
            using (IplImage miniL = new IplImage(size, imgR.Depth, imgR.ElemChannels))
            using (CvArr greyR = Cv.CreateImage(new CvSize(imgR.Width, imgR.Height), BitDepth.U8, 1))
            using (CvArr greyL = Cv.CreateImage(new CvSize(imgR.Width, imgR.Height), BitDepth.U8, 1))
            using (IplImage dispBM = new IplImage(imgR.Width, imgR.Height, BitDepth.F32, 1))
            using (IplImage disp = new IplImage(imgR.Width, imgR.Height, BitDepth.U8, 1))
            using (CvStereoBMState stereo = new CvStereoBMState(StereoBMPreset.Narrow, trackBar1.Value))
            using (IplImage dstR = new IplImage(new CvSize(pictureBox3.Size.Width, pictureBox3.Size.Height), imgR.Depth, imgR.ElemChannels))
            using (IplImage dstL = new IplImage(new CvSize(pictureBox3.Size.Width, pictureBox3.Size.Height), imgR.Depth, imgR.ElemChannels))
            {
                //using (IplImage chessR = chesscheck(imgR))
                //{
                //    Cv.Resize(chessR, miniR, Interpolation.Cubic);
                //    bmp4 = miniR.ToBitmap();
                //    pictureBox4.Image = bmp4;
                //}
                //using (IplImage chessL = chesscheck(imgL))
                //{
                //    Cv.Resize(chessL, miniR, Interpolation.Cubic);
                //    bmp5 = miniR.ToBitmap();
                //    pictureBox5.Image = bmp5;
                //}
                Cv.Remap(imgR, dstR, mapLX, mapLY);
                Cv.Remap(imgL, dstL, mapRX, mapRY);
                bufR = dstR.ToBitmap();
                bufL = dstL.ToBitmap();
                //Cv.Undistort2(imgR, dstR, intrinsicR, distortionR);
                //Cv.Undistort2(imgL, dstL, intrinsicL, distortionL);
                Cv.Resize(dstR, miniR, Interpolation.Cubic);
                Cv.Resize(dstL, miniL, Interpolation.Cubic);
                //Cv.Resize(imgR, miniR, Interpolation.Cubic);
                //Cv.Resize(imgL, miniL, Interpolation.Cubic);
                bmp1 = miniR.ToBitmap();
                bmp2 = miniL.ToBitmap();
                //bmp1 = dstR.ToBitmap();
                //bmp2 = dstL.ToBitmap();
                pictureBox1.Image = bmp1;
                pictureBox2.Image = bmp2;
                //Cv.CvtColor(imgR, greyR, ColorConversion.RgbaToGray);
                //Cv.CvtColor(imgL, greyL, ColorConversion.RgbaToGray);
                Cv.CvtColor(dstR, greyR, ColorConversion.RgbaToGray);
                Cv.CvtColor(dstL, greyL, ColorConversion.RgbaToGray);
                int smoothsize = trackBar2.Value;
                Cv.Smooth(greyR, greyR, SmoothType.Gaussian, smoothsize, smoothsize, 0, 0);
                Cv.Smooth(greyL, greyL, SmoothType.Gaussian, smoothsize, smoothsize, 0, 0);
                //Cv.FindStereoCorrespondenceBM(greyL, greyR, dispBM, stereo);
                Cv.FindStereoCorrespondence(greyL, greyR, DisparityMode.Birchfield, disp, trackBar1.Value, 25, 5, 12, 15, 25);
                //IplImage disp2 = new IplImage(pictureBox3.Width, pictureBox3.Height, BitDepth.F32, 1);
                //Cv.FindStereoCorrespondenceGC(greyL, greyR, disp,disp2, stereo);
                bmp = disp.ToBitmap();
                pictureBox3.Image = bmp;
            }
        }

        //frame1 = Cv.QueryFrame(capture1);
        //bmp1 = frame1.ToBitmap();
        //pictureBox1.Image = bmp1;
        //frame2 = Cv.QueryFrame(capture2);
        //bmp2 = frame2.ToBitmap();
        //pictureBox2.Image = bmp2;
        int cnt;
        Bitmap bmp;

        private void button1_Click(object sender, EventArgs e)
        {
            bufR.Save(filepath + "calib1_" + cnt.ToString("00") + ".bmp");
            bufL.Save(filepath + "calib2_" + cnt.ToString("00") + ".bmp");
            cnt++;
            button1.Text = "next" + cnt.ToString("00");
        }//写真撮影

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.Title = "記録するフォルダの選択";
            if (sfd.ShowDialog() == DialogResult.OK)
                filepath = sfd.FileName;
            string[] path = filepath.Split(@"\".ToCharArray());
            filepath = "";
            for (int i = 0; i < path.Length - 1; i++)
                filepath += path[i] + "\\";
            sfd.Dispose();
        }//写真保存フォルダ設定

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.Title = "左カメラ補正ファイルを選択してください";
            string filename = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            CvFileStorage fs = new CvFileStorage(filename, null, FileStorageMode.Read);
            CvFileNode param = Cv.GetFileNodeByName(fs, null, "objectpoint");
            CvMat objectpoint = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "pointcount");
            CvMat pointcount = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "image_points");
            CvMat imgpointL = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "intrinsic");
            intrinsicL = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "distortion");
            distortionL = Cv.Read<CvMat>(fs, param);
            Cv.ReleaseFileStorage(fs);

            ofd.Title = "右カメラ補正ファイルを選択してください";
            filename = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            ofd.Dispose();
            fs = new CvFileStorage(filename, null, FileStorageMode.Read);
            param = Cv.GetFileNodeByName(fs, null, "image_points");
            CvMat imgpointR = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "intrinsic");
            intrinsicR = Cv.Read<CvMat>(fs, param);
            param = Cv.GetFileNodeByName(fs, null, "distortion");
            distortionR = Cv.Read<CvMat>(fs, param);
            Cv.ReleaseFileStorage(fs);

            CvMat R = Cv.CreateMat(3, 3, MatrixType.F64C1);
            CvMat T = Cv.CreateMat(3, 1, MatrixType.F64C1);
            Cv.StereoCalibrate(objectpoint, imgpointL, imgpointR, pointcount,
                intrinsicL, distortionL, intrinsicR, distortionR,
                new CvSize(pictureBox3.Width, pictureBox3.Height), R, T);
            CvMat R1 = Cv.CreateMat(3, 3, MatrixType.F64C1);
            CvMat R2 = Cv.CreateMat(3, 3, MatrixType.F64C1);
            CvMat P1 = Cv.CreateMat(3, 4, MatrixType.F64C1);
            CvMat P2 = Cv.CreateMat(3, 4, MatrixType.F64C1);
            Cv.StereoRectify(intrinsicL, intrinsicR, distortionL, distortionR,
                new CvSize(pictureBox3.Width, pictureBox3.Height), R, T, R1, R2, P1, P2);
            CvArr mapLX = Cv.CreateImage(new CvSize(pictureBox3.Width, pictureBox3.Height), BitDepth.F32, 1);
            CvArr mapLY = Cv.CreateImage(new CvSize(pictureBox3.Width, pictureBox3.Height), BitDepth.F32, 1);
            CvArr mapRX = Cv.CreateImage(new CvSize(pictureBox3.Width, pictureBox3.Height), BitDepth.F32, 1);
            CvArr mapRY = Cv.CreateImage(new CvSize(pictureBox3.Width, pictureBox3.Height), BitDepth.F32, 1);
            Cv.InitUndistortRectifyMap(intrinsicL, distortionL, R1, P1, mapLX, mapLY);
            Cv.InitUndistortRectifyMap(intrinsicR, distortionR, R2, P2, mapRX, mapRY);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.Title = "記録するファイルを選択してください";
            filename = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName;
            }
            sfd.Dispose();
            fs = Cv.OpenFileStorage(filename, null, FileStorageMode.Write);
            fs.Write("mapLX", mapLX);
            fs.Write("mapLY", mapLY);
            fs.Write("mapRX", mapRX);
            fs.Write("mapRY", mapRY);
            Cv.ReleaseFileStorage(fs);
            R.Dispose();
            R1.Dispose();
            R2.Dispose();
            P1.Dispose();
            P2.Dispose();
            mapLX.Dispose();
            mapLY.Dispose();
            mapRX.Dispose();
            mapRY.Dispose();
            timer1.Enabled = true;
        }//ステレオキャリブレーション

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            int IMAGE_NUM = 25;
            const int PAT_ROW = 7;
            const int PAT_COL = 10;
            const int PAT_SIZE = PAT_ROW * PAT_COL;
            int ALL_POINTS = IMAGE_NUM * PAT_SIZE;
            const double CHESS_SIZE = 23.2;

            int i, j, k;
            int corner_count;
            bool found;
            int[] p_count = new int[IMAGE_NUM];
            IplImage[] src_img = new IplImage[IMAGE_NUM];
            CvSize pattern_size = Cv.Size(PAT_COL, PAT_ROW);
            CvPoint3D32f[] objects = new CvPoint3D32f[ALL_POINTS];
            CvPoint2D32f[][] corners = new CvPoint2D32f[IMAGE_NUM][];
            CvMat object_points = null;
            CvMat image_points = null;
            CvMat point_counts = null;
            CvMat intrinsic = Cv.CreateMat(3, 3, MatrixType.F32C1);
            CvMat rotation = Cv.CreateMat(1, 3, MatrixType.F32C1);
            CvMat translation = Cv.CreateMat(1, 3, MatrixType.F32C1);
            CvMat distortion = Cv.CreateMat(1, 4, MatrixType.F32C1);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;
            string filename = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            ofd.Dispose();
            string[] Path = filename.Split(@"\".ToCharArray());
            filename = "";
            for (i = 0; i < Path.Length - 1; i++)
                filename += Path[i] + @"\";
            filename += Path[Path.Length - 2] + "_";
            for (i = 0; i < IMAGE_NUM; i++)
            {
                try
                {
                    string path = filename + i.ToString("00") + ".bmp";
                    src_img[i] = Cv.LoadImage(path, LoadMode.Color);
                }
                catch { return; }
            }
            for (i = 0; i < IMAGE_NUM; i++)
            {
                for (j = 0; j < PAT_ROW; j++)
                {
                    for (k = 0; k < PAT_COL; k++)
                    {
                        objects[i * PAT_SIZE + j * PAT_COL + k] = new CvPoint3D32f(
                            j * CHESS_SIZE, k * CHESS_SIZE, 0);
                    }
                }
            }
            object_points = new CvMat(false);
            Cv.InitMatHeader(object_points, ALL_POINTS, 3, MatrixType.F32C1, objects);
            int found_num = 0;
            Cv.NamedWindow("Calibration", WindowMode.AutoSize);
            for (i = 0; i < IMAGE_NUM; i++)
            {
                bool fail = false;
                found = Cv.FindChessboardCorners(src_img[i], pattern_size, out corners[i], out corner_count);
                richTextBox1.Text += i.ToString();
                if (found)
                {
                    richTextBox1.Text += "ok\n";
                    found_num++;
                }
                else
                {
                    richTextBox1.Text += "fail\n";
                    fail = true;
                }
                IplImage src_gray = Cv.CreateImage(Cv.GetSize(src_img[i]), BitDepth.U8, 1);
                Cv.CvtColor(src_img[i], src_gray, ColorConversion.BgrToGray);
                Cv.FindCornerSubPix(src_gray, corners[i], corner_count,
                    Cv.Size(3, 3), Cv.Size(-1, -1), Cv.TermCriteria(CriteriaType.Iteration | CriteriaType.Epsilon, 20, 0.03));
                Cv.DrawChessboardCorners(src_img[i], pattern_size, corners[i], found);
                p_count[i] = corner_count;
                Cv.ShowImage("Calibration", src_img[i]);
                Cv.WaitKey(fail ? 0 : 1);
            }
            Cv.DestroyWindow("Calibration");
            if (found_num != IMAGE_NUM) return;
            CvPoint2D32f[] corners2 = null;
            {
                int count = 0;
                for (i = 0; i < IMAGE_NUM; i++)
                    count += corners[i].Length;
                corners2 = new CvPoint2D32f[count];
                for (i = j = 0; i < IMAGE_NUM; i++)
                    for (k = 0; k < corners[i].Length; k++)
                        corners2[j++] = corners[i][k];
            }

            image_points = new CvMat(false);
            point_counts = new CvMat(false);
            Cv.InitMatHeader(image_points, ALL_POINTS, 1, MatrixType.F32C2, corners2);
            Cv.InitMatHeader(point_counts, IMAGE_NUM, 1, MatrixType.S32C1, p_count);
            Cv.CalibrateCamera2(object_points, image_points, point_counts, new CvSize(1280, 800), intrinsic, distortion);
            CvMat sub_image_points, sub_object_points;
            int Base = 0;
            Cv.GetRows(image_points, out sub_image_points, Base * PAT_SIZE, (Base + 1) * PAT_SIZE);
            Cv.GetRows(object_points, out sub_object_points, Base * PAT_SIZE, (Base + 1) * PAT_SIZE);
            Cv.FindExtrinsicCameraParams2(sub_object_points, sub_image_points, intrinsic, distortion, rotation, translation);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.Title = "記録するファイルを選択してください";
            filename = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName;
            }
            sfd.Dispose();
            CvFileStorage fs = Cv.OpenFileStorage(filename, null, FileStorageMode.Write);
            fs.Write("objectpoint", object_points);
            fs.Write("image_points", image_points);
            fs.Write("pointcount", point_counts);
            fs.Write("intrinsic", intrinsic);
            fs.Write("rotation", rotation);
            fs.Write("translation", translation);
            fs.Write("distortion", distortion);
            Cv.ReleaseFileStorage(fs);
            for (i = 0; i < IMAGE_NUM; i++)
                Cv.ReleaseImage(src_img[i]);
            timer1.Enabled = true;
        }//カメラキャリブレーション

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int nod = trackBar1.Value;
            if (nod % 16 > 0)
            {
                int div = nod / 16;
                nod = div * 16 + 16;
            }
            trackBar1.Value = nod;
            textBox1.Text = nod.ToString();
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            int smoothsize = trackBar2.Value;
            if (smoothsize % 2 == 0)
                smoothsize++;
            trackBar2.Value = smoothsize;
            textBox2.Text = trackBar2.Value.ToString();
        }
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            textBox3.Text = trackBar3.Value.ToString();
        }
        private IplImage chesscheck(IplImage img)
        {
            IplImage img2 = img.Clone();
            bool found;
            int corner_count
                , PAT_ROW = 7
                , PAT_COL = 10;
            CvSize pattern_size = Cv.Size(PAT_COL, PAT_ROW);
            CvPoint2D32f[] corners;
            found = Cv.FindChessboardCorners(img2, pattern_size, out corners, out corner_count);
            IplImage src_gray = Cv.CreateImage(Cv.GetSize(img2), BitDepth.U8, 1);
            Cv.CvtColor(img2, src_gray, ColorConversion.BgrToGray);
            Cv.FindCornerSubPix(src_gray, corners, corner_count,
                Cv.Size(3, 3), Cv.Size(-1, -1), Cv.TermCriteria(CriteriaType.Iteration | CriteriaType.Epsilon, 20, 0.03));
            Cv.DrawChessboardCorners(img2, pattern_size, corners, found);
            return img2;
        }
    }
}