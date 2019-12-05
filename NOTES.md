# NOTES for development

The StarrySkyCamera is separate from the character because we don't want it to be in the same layer. If rotate the ship to check, rotate the camera as well

The mock solar system relies on the orbiter script, the orbiter script relies on its input x y z, which might be 0 0 0 so change it to the default numbers (1+e08):
Mercury: 579.1
Venus: 1082
Earth: 1496
Mars: 2279
Jupiter: 7785
Saturn: 14340
Uranus: 28710
Neptune: 44950
Pluto: 59060
Asteroid: NA
