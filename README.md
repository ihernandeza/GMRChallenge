# GMRChallenge
# Programming Challenge

Unity App thats load an external Json File.

## Notes
- Download "Assets" Folder and drop it on a new Unity Standalone Project.
Created on Unity 2020.1.5f1
- Executable file is located at bin.zip, please download and extract this file.
- Executable file is located at binMiniJson.zip, please download and extract this file.

## bin vs binMiniJson
- Bin executable, and JSON.unity scene, are using the built-in Unity Json functions. There are limited, because dynamic data cant be loaded, Unity MUST know what keys (rows, in this case) are you triying to read.
- Bin MiniJson executable, and MiniJSON.unity, are a little bit messy, I still prefer the first option (JsonUtility) but with this script, we can load dinamic fields from the Json, as required on the Challenge.
