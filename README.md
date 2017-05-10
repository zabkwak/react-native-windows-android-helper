# react-native-windows-android-helper
Helper for android developers on Windows using React Native to execute its commands.

## Usage
Simply clone the repository and run the start command.

```bash
"React Native Controller.exe" [path_to_the_react-native_project] start
```

The program will run `react-native start` command of react-native, which starts the packager. If the packager is ready `react-native run-android` is called. After the build is successful `react-native log-android` starts login the android device.

## Warning
The program is still in development but it can save some time with the react native commands. The react-native command is searched in `%HOMEPATH%\AppData\Roaming\npm\react-native.cmd`. 

# TODO
1) Clean command
2) Validation of the project directory
3) Optional react-native.cmd location
4) Exception handler