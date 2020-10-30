import numpy as np
import cv2
import PIL.Image as Image
from io import BytesIO
from unrealcv import client

def get_frame(client, camera_id=0, location=None, rotation=None, fov=None, img_ext="png"):
    if not location is None:
        location_str = " ".join(map(str, location))
        client.request(f'vset /camera/{camera_id}/location {location_str}') # x y z
    if not rotation is None:
        rotation_str = " ".join(map(str, rotation))
        client.request(f'vset /camera/{camera_id}/rotation {rotation_str}') # pitch yaw roll
    if not fov is None:
        client.request(f'vset /camera/{camera_id}/horizontal_fieldofview {fov}') # pitch yaw roll

    res = client.request(f'vget /camera/{camera_id}/lit {img_ext}')

    img = Image.open(BytesIO(res))
    npy = np.asarray(img)[:,:,:3]

    return cv2.cvtColor(npy, cv2.COLOR_RGB2BGR)

if __name__ == "__main__":
    try:
        res = client.connect() # connect to UE4 via UnrealCV
        print(res)

        img_l = get_frame(client, location=[125, 16, 130], rotation=[0, 180, 0], fov=60)
        img_r = get_frame(client, location=[125, 0, 130], rotation=[0, 180, 0], fov=60)

        cv2.imwrite('unrealcv_desk_l.png', img_l)
        cv2.imwrite('unrealcv_desk_r.png', img_r)

    finally:
        client.disconnect()
        cv2.destroyAllWindows()
