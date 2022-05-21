# PainterProject
This repo is built using C# by VS 2019. It is mainly about 2D graph painting. 
Features are listed as following:
- 2D shapes painting (such as circle, ellipse. rectangle, line), together with text, and images.
- 2D shapes edit (scaling, translating, rotating, copy, deleting).
- Immediate contorls based on GDI painting (button, textbox, label, shape button, panel)
- A painting canvas with zooming, moving (all the features are based on this paiting canvas).
- G code display based on the painting canvas. (support G01 G02 G03 G00 G91 G90)
- A 2D physical system with object collision detecting, forces, kinematics and momenta calculations) 
- A simple 2D Game framework with scene management. (such as Russian Block, greedy snake)

## Catagories
### Shape painting
- selection and edit with anchor points.
- paint style like: color, linewidth, isfill
- moving, scaling, rotating and copy, delete.
- a command system (redo/ undo / line, etc)
- shape supported: line, arc, rectangle, circle, ellipse, polygon, bspline. image and text.

![image](https://github.com/younghang/PainterProject/blob/master/pics/shape_paint.png)


### Immediate controls
- Support button, textbox, imageButton, panel, etc. 
<img src="https://github.com/younghang/PainterProject/blob/master/pics/immediate_controls.png" width="50%">

### Gcode
- Support G91/G90 G00 G01 G02 G03, etc.
- Gcode edit
- Gcode simulation
- position logging
- process control
<img src="https://github.com/younghang/PainterProject/blob/master/pics/gcode1.png" width="80%">
<img src="https://github.com/younghang/PainterProject/blob/master/pics/gcode2.png" width="80%">

### Game
- object collision detecting, forces, kinematics and momenta calculations
- cmd support (moving, pause, enable/disable forces)
- emit spark, floating panel, hit check.
- map editing
<img src="https://github.com/younghang/PainterProject/blob/master/pics/game.png" width="80%">
<img src="https://github.com/younghang/PainterProject/blob/master/pics/game_cmd.png" width="80%">
<img src="https://github.com/younghang/PainterProject/blob/master/pics/map_edit.png" width="80%">
 
#### other game
<img src="https://github.com/younghang/PainterProject/blob/master/pics/greedy_snake.png" width="50%">
<img src="https://github.com/younghang/PainterProject/blob/master/pics/russian_block.png" width="50%">
