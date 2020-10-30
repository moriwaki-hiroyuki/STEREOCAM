import cv2
import numpy as np
import matplotlib.pyplot as plt

"""
CONFIG
"""
CAMERA_DEVICE_L = 1
CAMERA_DEVICE_R = 0

IMAGE_WIDTH = 640
IMAGE_HEIGHT = 480

"""
"""

vL = cv2.VideoCapture(CAMERA_DEVICE_L)
vR = cv2.VideoCapture(CAMERA_DEVICE_R)
while( ( vL.isOpened() ) and ( vR.isOpened() ) ):
    rL, bgrL = vL.read()
    if ( rL == False ):
        break
    rR, bgrR = vR.read()
    if ( rR == False ):
        break
    mono0 = cv2.cvtColor(bgrL, cv2.COLOR_BGR2GRAY)
    mono1 = cv2.cvtColor(bgrR, cv2.COLOR_BGR2GRAY)
    f0 = np.array( mono0, dtype='float32')
    f1 = np.array( mono1, dtype='float32')
    dxdy, response = cv2.phaseCorrelate(f0, f1) 
    #print(dxdy[0], dxdy[1]) 
    bgrLafter = cv2.warpAffine(bgrL, np.float32([[1, 0, dxdy[0]], [0, 1, dxdy[1]]]), (f0.shape[1], f0.shape[0]))

    window_size = 3
    min_disp = 16
    num_disp = 80-min_disp
    stereo = cv2.StereoSGBM_create(minDisparity = min_disp,
        numDisparities = num_disp,
        blockSize = 15,
        P1 = 8*3*window_size**2,
        P2 = 32*3*window_size**2,
        disp12MaxDiff = 1,
        uniquenessRatio = 10,
        speckleWindowSize = 200,
        speckleRange = 1
    )
    disparity = stereo.compute(bgrLafter, bgrR).astype(np.float32) / 16.0

    cv2.imshow("bgrL", bgrL)
    cv2.imshow("bgrR", bgrR)
    cv2.imshow("bgrLafter", bgrLafter)
    cv2.imshow('disparity', (disparity-min_disp)/num_disp)

    key = cv2.waitKey(1) & 0xFF

    # sが押された場合は(show with colorbar)
    if key == ord('s'):
        plt.figure(figsize=(10,9))
        plt.imshow(disparity)
        plt.colorbar()
        plt.show()

        # camera parameters
        fov = 60  # [deg] 視野角
        B = 16  # [cm] カメラ間距離
        width = disparity.shape[1]  # [px] 横ピクセル数
        # f = 1.0 # [cm]

        # ratio: px per cm
        fov_ = fov * np.pi / 180  # [rad]
        ratio = (width/2) / np.tan(fov_/2)

        # convert disparity to depth
        depth = B * ratio * 16.0 / disparity
        # this line above is equivalent to the line below
        # depth = B * f / (disparity / 16.0 / ratio)C#

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
vL.release()
vR.release()
cv2.destroyAllWindows()