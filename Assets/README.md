# UnityAirplaneEnvironment

![](../.github/img/logo-no-background.png)


# About

UnityAirplaneEnvironment - is a repo containing Unity environment source code,

This project is part of the [TensorAirSpace](https://github.com/TensorAirSpace) project.

TensorAirSpace - is a set of control components, an OpenAI Gym simulation environment and Reinforcement Learning (RL) algorithms

The root folder is Assets.

# Prerequirements

- Installed Unity (2021 or higher)

# How to run

Open Unity Editor and open scene `Scenes/EnvScene.unity`.
Click 'Run' button.

# Troubleshooting

Sometimes you can see message like

```
An error occured: Package has incvalid dependecies:
com.unity.mlagents package
[com.unity.mlagents@v.x.x] cannot be found
```

That's typical error that can be found [there](https://docs.unity3d.com/Manual/upm-errors.html).

First of all you should open in Unity Editor

Tab `Window/Package Manager` and update ML Agents package (In projects/ML Agents) (will be marked as red).

That should fix the trouble.

# Folder architecture

```
Assets - your root workfolder
├── ML-Agents - there some specific data for ML Agents
├── Plugins - there holds add ons (check this folder README for details)
├── Scenes - there holds scenes for demo
└── Scripts - there holds our scripts 
```


