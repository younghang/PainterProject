# PainterProject
This repo is built using C# by VS 2019. It is mainly about 2D graph painting. 
Features are listed as following:
- 2D shapes painting (such as circle, ellipse. rectangle, line), together with text, and images.
- 2D shapes edit (scaling, translating, rotating, copy, deleting).
- Immediate contorls based on GDI painting (button, textbox, label, shape button, panel)
- A painting canvas with zooming, moving (all the features are based on this paiting canvas).
- G code display based on the painting canvas. (support G01 G02 G03 G00 G91 G90)
- A 2D physical system with object collision detecting, forces, kinematics and momenta calcations) 
- A simple 2D Game framework with scene management. (such as Russian Block, greedy snake)

## Catagories
### Shape painting
- selection and edit with anchor points.
- paint style like: color, linewidth, isfill
- moving, scaling, rotating and copy, delete.
- a command system (redo/ undo / line, etc)
- shape supported: line, arc, rectangle, circle, ellipse, polygon, bspline. image and text.

### Immediate controls
