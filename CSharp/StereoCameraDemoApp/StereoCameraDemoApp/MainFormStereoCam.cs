using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading;

namespace StereoCameraDemoApp
{
    public partial class MainFormStCam : Form
    {
        private int WIDTH = 640;
        private int HEIGHT = 480;
        private Mat frameL, frameR;
        private VideoCapture captureL, captureR;
        private Bitmap bmpL, bmpR;
        private Graphics graphicL, graphicR;

        private int SaveCnt = 0;
        private static string SAVEDIRPATH = @"F:\Repository\STEREOCAM_commit\CSharp\StereoCameraDemoApp\StereoCameraDemoApp\Images\";

        public MainFormStCam()
        {
            this.MaximizeBox = false;

            InitializeComponent();

            //ちらつき防止
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            captureL = new VideoCapture(1);
            captureR = new VideoCapture(0);
            if (!captureL.IsOpened())
            {
                MessageBox.Show("Left camera was not found!");
                this.Close();
            }
            if (!captureR.IsOpened())
            {
                MessageBox.Show("Right camera was not found!");
                this.Close();
            }

            captureL.FrameWidth = captureR.FrameWidth = WIDTH;
            captureL.FrameHeight = captureR.FrameHeight = HEIGHT;

            //取得先のMat作成
            frameL = new Mat(HEIGHT, WIDTH, MatType.CV_8UC3);
            frameR = new Mat(HEIGHT, WIDTH, MatType.CV_8UC3);

            //表示用のBitmap作成
            bmpL = new Bitmap(frameL.Cols, frameL.Rows, (int)frameL.Step(), System.Drawing.Imaging.PixelFormat.Format24bppRgb, frameL.Data);
            bmpR = new Bitmap(frameR.Cols, frameR.Rows, (int)frameR.Step(), System.Drawing.Imaging.PixelFormat.Format24bppRgb, frameR.Data);

            //PictureBoxを出力サイズに合わせる
            pictureBox_LCam.Width = frameL.Cols;
            pictureBox_LCam.Height = frameL.Rows;
            pictureBox_RCam.Width = frameR.Cols;
            pictureBox_RCam.Height = frameR.Rows;

            //描画用のGraphics作成
            graphicL = pictureBox_LCam.CreateGraphics();
            graphicR = pictureBox_RCam.CreateGraphics();

            //画像取得スレッド開始
            backgroundWorkerL.RunWorkerAsync();
            Thread.Sleep(100);
            backgroundWorkerR.RunWorkerAsync();
        }

        private void backgroundWorkerL_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //描画
            graphicL.DrawImage(bmpL, 0, 0, frameL.Cols, frameL.Rows);
        }

        private void backgroundWorkerR_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //描画
            graphicR.DrawImage(bmpR, 0, 0, frameR.Cols, frameR.Rows);
        }

        private void backgroundWorkerL_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;

            while (!backgroundWorkerL.CancellationPending)
            {
                //画像取得
                captureL.Grab();
                NativeMethods.videoio_VideoCapture_operatorRightShift_Mat(captureL.CvPtr, frameL.CvPtr);

                bw.ReportProgress(0);
            }
        }
        private void backgroundWorkerR_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;

            while (!backgroundWorkerR.CancellationPending)
            {
                //画像取得
                captureR.Grab();
                NativeMethods.videoio_VideoCapture_operatorRightShift_Mat(captureR.CvPtr, frameR.CvPtr);

                bw.ReportProgress(0);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //スレッドの終了を待機
            backgroundWorkerL.CancelAsync();
            backgroundWorkerR.CancelAsync();
            while ((backgroundWorkerL.IsBusy) || (backgroundWorkerR.IsBusy))
                Application.DoEvents();
        }

        private List<string> SaveImageFunc(bool flg)
        {
            //string filename_base = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            //string filenameL = filename_base + SaveCnt.ToString("_00") + "-Left.jpg";
            //string filenameR = filename_base + SaveCnt.ToString("_00") + "-Right.jpg";

            string filenameL = "calib1_" + SaveCnt.ToString("00") + "-Left.jpg";
            string filenameR = "calib2_" + SaveCnt.ToString("00") + "-Right.jpg";

            List<string> retFilePath = new List<string>();

            if(!frameL.Empty())
            {
                frameL.SaveImage(@SAVEDIRPATH + filenameL);
            }
            if (!frameR.Empty())
            {
                frameR.SaveImage(@SAVEDIRPATH + filenameR);
            }

            if(flg == true)
            {
                using (Mat cap = new Mat(filenameL))
                {
                    //保存されたキャプチャ画像の出力
                    Cv2.ImShow("LeftCam", frameL);
                }

                using (Mat cap = new Mat(filenameR))
                {
                    //保存されたキャプチャ画像の出力
                    Cv2.ImShow("RightCam", frameR);
                }
            }

            SaveCnt++;
            Console.WriteLine($"{DateTime.Now} next { SaveCnt.ToString("00")}");

            retFilePath.Add(@SAVEDIRPATH + filenameL);
            retFilePath.Add(@SAVEDIRPATH + filenameR);
            return retFilePath;

        }

        private void button_SaveImage_Click(object sender, EventArgs e)
        {
            SaveImageFunc(true);
        }

        private void button_CalibrateCreate_Click(object sender, EventArgs e)
        {
            var app = new ProcessStartInfo();

            app.FileName = "py";
            app.Arguments = @"F:\Repository\STEREOCAM_commit\CSharp\StereoCameraDemoApp\StereoCameraDemoApp\lib\stereo_camera_calibrate.py";
            app.UseShellExecute = true;

            Process.Start(app);

#if false
            //timer1.Enabled = false;
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
            Mat[] src_img = new Mat[IMAGE_NUM];

            Size2d pattern_size = new Size2d(PAT_COL, PAT_ROW);
            Point3f[] objects = new Point3f[ALL_POINTS];
            Point2f[][] corners = new Point2f[IMAGE_NUM][];

            Mat object_points = null;
            Mat image_points = null;
            Mat point_counts = null;
            Mat intrinsic =new Mat(3, 3, MatType.CV_32FC1);
            Mat rotation = new Mat(1, 3, MatType.CV_32FC1);
            Mat translation = new Mat(1, 3, MatType.CV_32FC1);
            Mat distortion = new Mat(1, 4, MatType.CV_32FC1);

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
                    //src_img[i] = Cv.LoadImage(path, LoadMode.Color);
                    src_img[i] = Cv2.ImRead(path);
                }
                catch { return; }
            }
            for (i = 0; i < IMAGE_NUM; i++)
            {
                for (j = 0; j < PAT_ROW; j++)
                {
                    for (k = 0; k < PAT_COL; k++)
                    {
                        objects[i * PAT_SIZE + j * PAT_COL + k] = new Point3f(
                            (float)(j * CHESS_SIZE), (float)(k * CHESS_SIZE), 0);
                    }
                }
            }
            object_points = new Mat<Point3f>();
            Mat.InitMatHeader(object_points, ALL_POINTS, 3, MatType.CV_32FC1, objects);

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
#endif
        }

        private void button_StereoMatch_Click(object sender, EventArgs e)
        {
            var PYTHON_HOME = Environment.ExpandEnvironmentVariables(@"C:\Users\stj\AppData\Local\Programs\Python\Python38\python.exe");
            PythonEngine.PythonHome = PYTHON_HOME;

            using (Py.GIL())
            {
                string CALIBRATION_DATA = "stereo_calibration_data.json";
                string CALIBRATION_FILEPATH = @"F:\Repository\STEREOCAM_commit\CSharp\StereoCameraDemoApp\StereoCameraDemoApp\data\";
                string filepath = CALIBRATION_FILEPATH + CALIBRATION_DATA;

                dynamic np = Py.Import("numpy");
                dynamic cv2 = Py.Import("cv2");
                Console.WriteLine("load calibaration data");

                var calibration_data = StereoCalibDataFunc.FromFile(filepath);

                //var cameramatrixl = np.array(calibration_data['cameramatrixl']);
                //var cameramatrixr = np.array(calibration_data['cameramatrixr']);
                //var distcoeffsl = np.array(calibration_data['distcoeffsl']);
                //var adistcoeffsr = np.array(calibration_data['distcoeffsr']);
                //var R = np.array(calibration_data['R']);
                //var T = np.array(calibration_data['T']);

                calibration_data.TryGetValue("cameramatrixl", out List<List<double>> Value_cmtxL);
                var cameramatrixl = np.array(Value_cmtxL);
                //Mat<double> cameramatrixl = new Mat<double>(3, 3);

                //Mat cameramatrixl = Cv2.InRange(hsv, input1, input2, dst);

                //var indexer = cameramatrixl.GetGenericIndexer<double>();
                //for (int j = 0; j < cameramatrixl.Rows; j++)
                //    for (int i = 0; i < cameramatrixl.Cols; i++)
                //    {
                //        indexer = Value_cmtxL[j][i];//xは書き込む値  
                //    }


                calibration_data.TryGetValue("cameramatrixr", out List<List<double>> Value_cmtxR);
                var cameramatrixr = np.array(Value_cmtxR);

                calibration_data.TryGetValue("distcoeffsl", out List<List<double>> Value_distL);
                var distcoeffsl = np.array(Value_distL);

                calibration_data.TryGetValue("distcoeffsr", out List<List<double>> Value_distR);
                var distcoeffsr = np.array(Value_distR);

                calibration_data.TryGetValue("R", out List<List<double>> Value_R);
                var R = np.array(Value_R);

                calibration_data.TryGetValue("T", out List<List<double>> Value_T);
                var T = np.array(Value_T);

                Console.WriteLine("End load calibaration data");

                //var capl = cv2.VideoCapture(2);
                //var capr = cv2.VideoCapture(0);

                //capl.set(cv2.CAP_PROP_FRAME_WIDTH, WIDTH);
                //capl.set(cv2.CAP_PROP_FRAME_HEIGHT, HEIGHT);
                //capr.set(cv2.CAP_PROP_FRAME_WIDTH, WIDTH);
                //capr.set(cv2.CAP_PROP_FRAME_HEIGHT, HEIGHT);

                //var imgl = np.zeros((HEIGHT, WIDTH, 3), np.uint8);
                //var imgr = np.zeros((HEIGHT, WIDTH, 3), np.uint8);


                // SGBM Parameters -----------------
                // wsize default 3; 5; 7 for SGBM reduced size image; 15 for SGBM full size. image (1300px and above); 5 Works nicely
                var window_size = 11;
                var min_disp = 4;
                var num_disp = 128;  // max_disp has to be dividable by 16 f. E. HH 192, 256
                var left_matcher = cv2.StereoSGBM_create(
                                                            min_disp,                       // 視差の下限 minDisparity
                                                            num_disp,                       // 視差の上限 numDisparities
                                                            window_size,                    // 窓サイズ 3..11 blockSize
                                                            8 * 3 * window_size * 2,        // 視差の滑らかさを制御するパラメータ1
                                                            32 * 3 * window_size * 2,       // 視差の滑らかさを制御するパラメータ2
                                                            1,                              // disp12MaxDiff
                                                            15,                             // uniquenessRatio
                                                            50,                             // 視差の滑らかさの最大サイズ. 50-100 speckleWindowSize
                                                            1,                              // 視差の最大変化量. 1 or 2 speckleRange
                                                            63,                             // preFilterCap
                                                            2                               // mode（STEREO_SGBM_MODE_SGBM_3WAY）
                );

                var right_matcher = cv2.ximgproc.createRightMatcher(left_matcher);

                // FILTER Parameters
                var lmbda = 80000;
                var sigma = 1.2;
                var visual_multiplier = 1.0;

                var wls_filter = cv2.ximgproc.createDisparityWLSFilter(left_matcher);
                wls_filter.setLambda(lmbda);
                wls_filter.setSigmaColor(sigma);


                while(true){

                    if (!captureL.IsOpened() || !captureR.IsOpened())
                    {
                        Console.WriteLine("No more frames");
                        break;
                    }

                    var filepaths = SaveImageFunc(false);
                    var filenameL = filepaths.Where(elem => elem.Contains("Left")==true).ToList();
                    var filenameR = filepaths.Where(elem => elem.Contains("Right") == true).ToList();

                    var index = 0;
                    if (SaveCnt > 0)
                    {
                        index = SaveCnt - 1;
                    }

                    Mat matL_gray = new Mat(filenameL[index]);
                    Cv2.CvtColor(matL_gray, matL_gray, ColorConversionCodes.BGR2GRAY);
                    Cv2.ImShow("LeftCam", matL_gray);

                    Mat matR_gray = new Mat(filenameR[index]);
                    Cv2.CvtColor(matR_gray, matR_gray, ColorConversionCodes.BGR2GRAY);
                    Cv2.ImShow("RightCam", matR_gray);


                    //平行化変換のためのRとPおよび3次元変換行列Qを求める
                    var alpha = 1.0;
                    Mat R1 = new Mat(3, 3, MatType.CV_64FC1);
                    Mat R2 = new Mat(3, 3, MatType.CV_64FC1);
                    Mat P1 = new Mat(3, 4, MatType.CV_64FC1);
                    Mat P2 = new Mat(3, 4 ,MatType.CV_64FC1);
                    Mat Q  = new Mat(4, 4, MatType.CV_64FC1);
                    OpenCvSharp.Size ImageSize = new OpenCvSharp.Size(WIDTH, HEIGHT);

                    Cv2.StereoRectify(cameramatrixl, distcoeffsl, cameramatrixr, distcoeffsr, ImageSize, R, T, R1, R2, P1 ,P2, Q, 0, alpha, ImageSize);


                    //平行化変換マップを求める
                    Mat map1_l = new Mat();
                    Mat map2_l = new Mat();
                    Mat map1_r = new Mat();
                    Mat map2_r = new Mat();

                    Cv2.InitUndistortRectifyMap(cameramatrixl, distcoeffsl, R1, P1, ImageSize, MatType.CV_32FC1, map1_l, map2_l);
                    Cv2.InitUndistortRectifyMap(cameramatrixr, distcoeffsr, R2, P2, ImageSize, MatType.CV_32FC1, map1_r, map2_r);

                    //ReMapにより平行化を行う
                    Cv2.Remap(matL_gray, matL_gray, map1_l, map2_l, InterpolationFlags.Linear);
                    Cv2.Remap(matR_gray, matR_gray, map1_r, map2_r, InterpolationFlags.Linear);


                    Cv2.ImShow("Left", matL_gray);
                    Cv2.ImShow("Right", matR_gray);


                    //                    displ = left_matcher.compute(imgl_gray, imgr_gray)
                    //    dispr = right_matcher.compute(imgr_gray, imgl_gray)
                    //    # displ = left_matcher.compute(imgl, imgr)
                    //    # dispr = right_matcher.compute(imgr, imgl)

                    //    # cv2.imshow("Disparity", (displ.astype(
                    //    #     np.float32) / 16.0 - min_disp) / num_disp)

                    //    displ = np.int16(displ)
                    //    dispr = np.int16(dispr)

                    //    filtered_img = wls_filter.filter(displ, imgl, None, dispr)
                    //    filtered_img = cv2.normalize(
                    //        src = filtered_img, dst = filtered_img, beta = 0, alpha = 255, norm_type = cv2.NORM_MINMAX)
                    //    filtered_img = np.uint8(filtered_img)
                    //    # cv2.imshow("Disparity", filtered_img)

                    //    cv2.imshow("Stereo", cv2.hconcat([imgl_gray, imgr_gray, filtered_img]))
                    break;
                };

            }
        }

        private void button_PyStereo_Click(object sender, EventArgs e)
        {
            var pythonScriptPath = @"F:\Repository\STEREOCAM_commit\CSharp\StereoCameraDemoApp\StereoCameraDemoApp\lib\image_stcam_depth_arg3.py";

            var index = 0;
            if (SaveCnt > 0)
            {
                index = SaveCnt - 1;
            }

            var arguments = new List<string>
            {
                pythonScriptPath,
                @SAVEDIRPATH + "calib1_" + index.ToString("00") + "-Left.jpg",
                @SAVEDIRPATH + "calib2_" + index.ToString("00") + "-Right.jpg"
            };

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(@"C:\Users\stj\AppData\Local\Programs\Python\Python38\python.exe")
                {
                    UseShellExecute = true,
                    Arguments = string.Join(" ", arguments),
                },
            };

            process.Start();
        }

        private void button_PyStereoMOV_Click(object sender, EventArgs e)
        {
            captureL.Release();
            captureR.Release();
            Cv2.DestroyAllWindows();

            var app = new ProcessStartInfo();

            app.FileName = "py";
            app.Arguments = @"F:\Repository\STEREOCAM_commit\CSharp\StereoCameraDemoApp\StereoCameraDemoApp\lib\stereo.py";
            app.UseShellExecute = true;

            Process.Start(app);
        }

        private void button_stUnrealCV_Click(object sender, EventArgs e)
        {
            var pythonScriptPath = @"F:\Repository\STEREOCAM_commit\CSharp\StereoCameraDemoApp\StereoCameraDemoApp\lib\image_stcam_depth_arg5.py";

            var arguments = new List<string>
            {
                pythonScriptPath,
                @SAVEDIRPATH + "unrealcv_desk_l_FoV60_3m.png",
                @SAVEDIRPATH + "unrealcv_desk_r_FoV60_3m.png",
                @SAVEDIRPATH + "unrealcv_human_l_FoV90_4m_180cm.png",
                @SAVEDIRPATH + "unrealcv_human_r_FoV90_4m_180cm.png"
            };

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(@"C:\Users\stj\AppData\Local\Programs\Python\Python38\python.exe")
                {
                    UseShellExecute = true,
                    Arguments = string.Join(" ", arguments),
                },
            };

            process.Start();
        }
    }
}
