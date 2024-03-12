# HectiQ---Unity-programmer-test
Chaouch Mohamed Ayoub Test 

## Key Features

This project implements an advanced AI for a game character tasked with navigating through an environment, dodging obstacles, collecting items,   shields for protection,and Emotional Change. Here's an overview of the main scripts that orchestrate the AI's behavior:

### DodgeBehaviour

This script enables the AI to dynamically dodge obstacles encountered along its path. Utilizing calculations based on the positions and trajectories of obstacles, the AI adjusts its direction to avoid collisions.

### ShieldBehaviour

This script allows the AI to utilize environmental shields for protection against threats. The AI can identify the nearest shields and strategically use them to block obstacles.

### CollectBehaviour

The AI is also designed to collect valuable items (treasures) scattered throughout the environment. This script manages the logic for detection and collection, adjusting the AI's path to include treasures while maintaining safe navigation.


### EmotionalBehaviour

for the moment The AI integrates a system of emotions which displays its state in the game by an emoji system but afterwards we can implement the system which influences its decision-making in real time. Emotions such as fear, excitement, or caution change the AI's behavior, affecting its strategies for dodging, gathering, and using shields.
## Configuration System

The project includes a flexible configuration system that allows enabling or disabling certain features based on player needs or test requirements. Currently, this system enables:

- Enabling/Disabling treasure collection, allowing the AI to focus on dodging or using shields.
- Enabling/Disabling the shield system, offering variety in the protection strategies available to the AI.
- ShieldSearchRadius : Search radius for a blocker usable as a shield
- AvoidanceRadius : The avoid radius
- _treasureRadius: Treasure Radius


