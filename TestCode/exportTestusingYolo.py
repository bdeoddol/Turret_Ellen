import torch
from ultralytics import YOLO

model = YOLO("yolo26m.pt") # https://docs.ultralytics.com/models/yolo26#performance-metrics
# model.export(format="onnx")

torch.save(core_model, "core_model.pt")



# pyTorch_model = model.model
# trace_input = torch.randn(1, 3, 1080, 1920)
# torch.onnx.export(core_model, trace_input, dynamic_shapes=True, dynamo=True)






