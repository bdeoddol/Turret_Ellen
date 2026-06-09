import cv2 as cv



img = cv.imread("testImgs/magazine.jpg") 
print(img)
print(img.shape)

print ("---------------")
print("blobFromImage output:")
img = cv.dnn.blobFromImage(img, swapRB=True, ddepth=cv.CV_32F )
print(img)
print(img.shape)

print ("---------------")
