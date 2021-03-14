#version 330 core
in vec3 position;
in vec2 textureCoords;

out vec2 fragTextureCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    fragTextureCoords = textureCoords;
    gl_Position = vec4(position, 1.0) * model * view * projection;
}