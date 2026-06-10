import cv2 as cv



img = cv.imread("testImgs/sampleWebcam.jpg") 
print(img)
print(img.shape)

print ("---------------")
print("blobFromImage output:")
img = cv.dnn.blobFromImage(img, swapRB=True, ddepth=cv.CV_32F )
print(img)
print(img.shape)

print ("---------------")
print("flattened ver")
flat_copy = img.reshape(-1)
# or flat_copy = img.flatten()
print(flat_copy)
