
# import sys
# sys.path.remove('/opt/ros/kinetic/lib/python2.7/dist-packages')

import numpy as np
import cv2
import json
import sys
import matplotlib.pyplot as plt

from extended_json import ExtendedJsonEncoder

"""
CONFIG
"""

L_CAMERA_DEVICE = 1
R_CAMERA_DEVICE = 0
IMAGE_WIDTH = 640
IMAGE_HEIGHT = 480
CALIBRATION_DATA = "stereo_calibration_data.json"
CALIBRATION_FILEPATH = "F:\\Repository\\STEREOCAM_commit\\CSharp\\StereoCameraDemoApp\\StereoCameraDemoApp\\data\\"

"""
"""

print("load calibaration data")

fr = open(CALIBRATION_FILEPATH + CALIBRATION_DATA, 'r')
calibration_data = json.load(fr)

cameramatrixl = np.array(calibration_data['cameramatrixl'])
cameramatrixr = np.array(calibration_data['cameramatrixr'])
distcoeffsl = np.array(calibration_data['distcoeffsl'])
distcoeffsr = np.array(calibration_data['distcoeffsr'])
R = np.array(calibration_data['R'])
T = np.array(calibration_data['T'])

print("End load calibaration data")

capl = cv2.VideoCapture(L_CAMERA_DEVICE)
capr = cv2.VideoCapture(R_CAMERA_DEVICE)

capl.set(cv2.CAP_PROP_FRAME_WIDTH, IMAGE_WIDTH)
capl.set(cv2.CAP_PROP_FRAME_HEIGHT, IMAGE_HEIGHT)
capr.set(cv2.CAP_PROP_FRAME_WIDTH, IMAGE_WIDTH)
capr.set(cv2.CAP_PROP_FRAME_HEIGHT, IMAGE_HEIGHT)

imgl = np.zeros((IMAGE_HEIGHT, IMAGE_WIDTH, 3), np.uint8)
imgr = np.zeros((IMAGE_HEIGHT, IMAGE_WIDTH, 3), np.uint8)

# SGBM Parameters -----------------
# wsize default 3; 5; 7 for SGBM reduced size image; 15 for SGBM full size. image (1300px and above); 5 Works nicely
window_size = 11
min_disp = 4
num_disp = 128  # max_disp has to be dividable by 16 f. E. HH 192, 256
left_matcher = cv2.StereoSGBM_create(
    minDisparity=min_disp,  # 視差の下限
    numDisparities=num_disp,  # 視差の上限
    blockSize=window_size,  # 窓サイズ 3..11
    P1=8 * 3 * window_size**2,  # 視差の滑らかさを制御するパラメータ1
    P2=32 * 3 * window_size**2,  # 視差の滑らかさを制御するパラメータ2
    disp12MaxDiff=1,
    uniquenessRatio=15,
    speckleWindowSize=50,  # 視差の滑らかさの最大サイズ. 50-100
    speckleRange=1,  # 視差の最大変化量. 1 or 2
    preFilterCap=63,
    mode=cv2.STEREO_SGBM_MODE_SGBM_3WAY
)

right_matcher = cv2.ximgproc.createRightMatcher(left_matcher)

# FILTER Parameters
lmbda = 80000
sigma = 1.2
visual_multiplier = 1.0

wls_filter = cv2.ximgproc.createDisparityWLSFilter(matcher_left=left_matcher)
wls_filter.setLambda(lmbda)
wls_filter.setSigmaColor(sigma)

while True:
    if not (capl.grab() and capr.grab()):
        print("No more frames")
        break

    capl.read(imgl)
    capr.read(imgr)

    cv2.imshow("Capture", cv2.hconcat([imgl, imgr]))

    imgl_gray = cv2.cvtColor(imgl, cv2.COLOR_BGR2GRAY)
    imgr_gray = cv2.cvtColor(imgr, cv2.COLOR_BGR2GRAY)

    # 平行化変換のためのRとPおよび3次元変換行列Qを求める
    flags = 0
    alpha = 1
    R1, R2, P1, P2, Q, validPixROI1, validPixROI2 = cv2.stereoRectify(
        cameramatrixl, distcoeffsl, cameramatrixr, distcoeffsr, (IMAGE_WIDTH, IMAGE_HEIGHT), R, T, flags, alpha, (IMAGE_WIDTH, IMAGE_HEIGHT))

    # 平行化変換マップを求める
    m1type = cv2.CV_32FC1
    map1_l, map2_l = cv2.initUndistortRectifyMap(
        cameramatrixl, distcoeffsl, R1, P1, (IMAGE_WIDTH, IMAGE_HEIGHT), m1type)  # m1type省略不可
    map1_r, map2_r = cv2.initUndistortRectifyMap(
        cameramatrixr, distcoeffsr, R2, P2, (IMAGE_WIDTH, IMAGE_HEIGHT), m1type)

    # ReMapにより平行化を行う
    interpolation = cv2.INTER_NEAREST  # INTER_RINEARはなぜか使えない
    imgl_gray = cv2.remap(imgl_gray, map1_l, map2_l,
                          interpolation)  # interpolation省略不可
    imgr_gray = cv2.remap(imgr_gray, map1_r, map2_r, interpolation)

    # imgl_gray = cv2.GaussianBlur(imgl_gray, (5, 5), 0)
    # imgr_gray = cv2.GaussianBlur(imgr_gray, (5, 5), 0)

    # cv2.imshow("Left", imgl_gray)
    # cv2.imshow("Right", imgr_gray)

    # cv2.imshow("Left", imgl)
    # cv2.imshow("Right", imgr)

    displ = left_matcher.compute(imgl_gray, imgr_gray)
    dispr = right_matcher.compute(imgr_gray, imgl_gray)
    # displ = left_matcher.compute(imgl, imgr)
    # dispr = right_matcher.compute(imgr, imgl)

    # cv2.imshow("Disparity", (displ.astype(
    #     np.float32) / 16.0 - min_disp) / num_disp)

    displ = np.int16(displ)
    dispr = np.int16(dispr)

    filtered_img = wls_filter.filter(displ, imgl, None, dispr)
    filtered_img = cv2.normalize(
        src=filtered_img, dst=filtered_img, beta=0, alpha=255, norm_type=cv2.NORM_MINMAX)
    filtered_img = np.uint8(filtered_img)
    # cv2.imshow("Disparity", filtered_img)

    #cv2.imshow("Stereo", cv2.hconcat([imgl_gray, imgr_gray, filtered_img]))
    cv2.imshow("Stereo", filtered_img)

    key = cv2.waitKey(1) & 0xFF

    # sが押された場合は(show with colorbar)
    if key == ord('s'):
        plt.figure(figsize=(10,9))
        plt.imshow(filtered_img)
        plt.colorbar()
        plt.show()

        # camera parameters
        fov = 60  # [deg] 視野角
        B = 16  # [cm] カメラ間距離
        width = filtered_img.shape[1]  # [px] 横ピクセル数
        # f = 1.0 # [cm]

        # ratio: px per cm
        fov_ = fov * np.pi / 180  # [rad]
        ratio = (width/2) / np.tan(fov_/2)

        # convert disparity to depth
        depth = B * ratio * 16.0 / filtered_img
        # this line above is equivalent to the line below
        # depth = B * f / (disparity / 16.0 / ratio) #C#

        # set unexpected values to zero 
        depth[np.where(depth < 0)] = 0
        depth[np.where(depth > 500)] = 0

        # show result
        plt.imshow(depth)
        plt.colorbar()
        plt.show()

    # q or esc が押された場合は終了する
    if key == ord('q') or key == 27:
        break

capl.release()
capr.release()

cv2.destroyAllWindows()
