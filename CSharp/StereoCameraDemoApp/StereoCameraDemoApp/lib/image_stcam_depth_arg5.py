# To add a new cell, type '# %%'
# To add a new markdown cell, type '# %% [markdown]'
# %%
# from IPython import get_ipython

# %%
import numpy as np
import cv2
import sys
from matplotlib import pyplot as plt
#get_ipython().run_line_magic('matplotlib', 'inline')

# read images
if len(sys.argv) == 5:
    firstArgument = (sys.argv[1])
    secondArgument = (sys.argv[2])
    thirdArgument = (sys.argv[3])
    fourthArgument = (sys.argv[4])

#imgL = cv2.imread("E:\\Repository\\STEREOCAM_commit\\CSharp\\StereoCameraDemoApp\\StereoCameraDemoApp\\Images\\unrealcv_desk_l.png", 0)
#imgR = cv2.imread("E:\\Repository\\STEREOCAM_commit\\CSharp\\StereoCameraDemoApp\\StereoCameraDemoApp\\Images\\unrealcv_desk_r.png", 0)

imgL = cv2.imread(firstArgument, 0)
imgR = cv2.imread(secondArgument, 0)

imgL2 = cv2.imread(thirdArgument, 0)
imgR2 = cv2.imread(fourthArgument, 0)

# # left camera image
# plt.figure(figsize=(13,3))
# plt.imshow(imgL)
# plt.show()
#cv2.imshow("imgL",cv2.imread("E:\\Repository\\STEREOCAM_commit\\CSharp\\StereoCameraDemoApp\\StereoCameraDemoApp\\Images\\unrealcv_desk_l.png", 1))
cv2.imshow("imgL",cv2.imread(firstArgument, 1))

# # right camera image
# plt.figure(figsize=(13,3))
# plt.imshow(imgR)
# plt.show()
#cv2.imshow("imgR",cv2.imread("E:\\Repository\\STEREOCAM_commit\\CSharp\\StereoCameraDemoApp\\StereoCameraDemoApp\\Images\\unrealcv_desk_r.png", 1))
cv2.imshow("imgR",cv2.imread(secondArgument, 1))

cv2.imshow("imgL2",cv2.imread(thirdArgument, 1))
cv2.imshow("imgR2",cv2.imread(fourthArgument, 1))

# estimate depth
stereo = cv2.StereoBM_create(numDisparities=64, blockSize=15)
disparity_px_16 = stereo.compute(imgL, imgR)

stereo2 = cv2.StereoBM_create(numDisparities=64, blockSize=15)
disparity_px_16_2 = stereo2.compute(imgL2, imgR2)

# # raw output
# plt.figure(figsize=(13,3))
# plt.imshow(disparity_px_16)
# plt.colorbar()
# plt.show()

# # output value in pixel
# plt.figure(figsize=(13,3))
# plt.imshow(disparity_px_16/16)
# plt.colorbar()
# plt.show()

# camera parameters
fov = 60  # [deg] 視野角
B = 16  # [cm] カメラ間距離
width = disparity_px_16.shape[1]  # [px] 横ピクセル数

fov2 = 90  # [deg] 視野角
B2 = 20  # [cm] カメラ間距離
width2 = disparity_px_16_2.shape[1]  # [px] 横ピクセル数
# f = 1.0 # [cm]

# ratio: px per cm
fov_ = fov * np.pi / 180  # [rad]
ratio = (width/2) / np.tan(fov_/2)

fov2_ = fov2 * np.pi / 180  # [rad]
ratio2 = (width/2) / np.tan(fov2_/2)

# convert disparity to depth
depth = B * ratio * 16.0 / disparity_px_16
depth2 = B2 * ratio2 * 16.0 / disparity_px_16_2

# this line above is equivalent to the line below
# depth = B * f / (disparity_px_16 / 16.0 / ratio)

# set unexpected values to zero 
depth[np.where(depth < 0)] = 0
depth[np.where(depth > 500)] = 0

depth2[np.where(depth2 < 0)] = 0
depth2[np.where(depth2 > 500)] = 0

# show result
plt.imshow(depth)
plt.colorbar()
plt.show()

plt.imshow(depth2)
plt.colorbar()
plt.show()

# %%



