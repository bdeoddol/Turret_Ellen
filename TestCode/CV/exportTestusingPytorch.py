import torch
import torchvision
import onnxruntime
from torchvision.models.detection import SSDLite320_MobileNet_V3_Large_Weights

# weights = SSDLite320_MobileNet_V3_Large_Weights.DEFAULT
# model = torchvision.models.detection.ssdlite320_mobilenet_v3_large(weights=weights)
# model.eval()
# #setting weights to default on COCOv1
# #https://docs.pytorch.org/vision/main/models.html
# #https://docs.pytorch.org/vision/main/models/generated/torchvision.models.detection.ssdlite320_mobilenet_v3_large.html#torchvision.models.detection.ssdlite320_mobilenet_v3_large



# trace_input = torch.randn(3, 320, 320)
# torch.onnx.export(model, [trace_input], "ssdLite.onnx", input_names=["input"], dynamo=True, report=False, verbose=False)

print(torch.cuda.is_available())






