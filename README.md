# Portal 2 Turret Software
Hi this is an unfinished project built to accompany a Portal 2 Turret that I will be building very (very) soon. I will return to it when I can.

The application is built in Visual Studio peforms object detection on humans to track and (very soon) follow subjects by utilizing neural networks and all that jazz. 

Because training and building neural networks is not the main focus of the project, I used Ultralytics' YOLOv26 model to perform object detection and (very soon) tracking.

The YOLOv26 model is exported to an ONNX graph and deployed in a C# environment using ONNX Runtime. 
This project also supports __LEGACY__ CUDA and cuDNN (because i have a Geforce GTX 1060) as well as regular old CPU compute.

## Features
To sum, here is a list of features/methods on how I built this project instead of describing it in a paragraph
* Ultralytics' YOLOv26 pytorch model exported to an ONNX graph
* Deployed in a C# runtime environment using ONNX Runtime
* To track, the app utilizes the BYTETRACK algorithm to label detected subjects with unique and persistent IDs across multiple frames
* Utilizes either/or CUDA or CPU execution provider decided interally upon startup
* Multithreaded design for concurrent webcam live streaming and image pre/post-processing in real-time using OpenCV API
* Internal state machine automating the behaviors of the turret by polling ( i plan to change it and make it event-driven instead)
	* includes a Inactive, Idle, Tracking, Detection Lost/Searching, and Remote Control state
* Supports serial port connection and communication used to operate the servos of the Portal 2 Turret based on the statemachines state
* The remote control state allows the user to command the turret manually via cursor movement. 
* The tracking state is programmed to track detected subjects, if there are multiple, it will cycle through at a 4 second interval

## Etc
Because I'm intrested in Computer Vision, I learned quite a bit of things while building this application. To go along with learning I also wrote a little guide at the same time. It details almost (if not) all of the things I came across/learned while building the Object Detection/Tracking portion of this project. 

I found it to be quite helpful for me, and I tried keeping it as detailed yet beginner friendly. To be fair I was also as a beginner. It's written in a manner as if I was learning alongside you lol. You can find it below
[https://docs.google.com/document/d/1JqtpAFYeeRlK-9SzCf56p4S9fn-2rOcuw7v_r5ZfsFU/edit?usp=sharing](https://docs.google.com/document/d/1tiTIgEQXP8AxskOXVF4ECbfgAVH8WvgKKPgrW4UIoE0/edit?usp=sharing)

I also wrote a master-list that documents (almost) all the forums, articles, language references and documents I used while making this project. I was learning .NET, OpenCV, and ONNX/ONNX Runtime at the same time so I wanted to keep record. You can find it at the link below
[https://docs.google.com/document/d/1JqtpAFYeeRlK-9SzCf56p4S9fn-2rOcuw7v_r5ZfsFU/edit?usp=sharing](https://docs.google.com/document/d/1JqtpAFYeeRlK-9SzCf56p4S9fn-2rOcuw7v_r5ZfsFU/edit?usp=sharing)
