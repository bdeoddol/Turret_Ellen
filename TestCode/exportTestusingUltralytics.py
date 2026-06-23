import torch
from ultralytics import YOLO

model = YOLO("yolo26n.pt") # https://docs.ultralytics.com/models/yolo26#performance-metrics
model.export(format="onnx",batch=1,dynamic=True,nms=True)

#boy it's that easy :joy:





