#version 330 core
out vec4 FragColor;
in vec3 colorFrag;

void main()
{
    FragColor = vec4(colorFrag, 1.0);
}