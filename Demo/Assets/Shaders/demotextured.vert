#version 330 core
in vec3 position;
in vec2 textureCoords;

out vec2 fragTextureCoords;

void main()
{
    fragTextureCoords = textureCoords;
    gl_Position = vec4(position, 1.0);
}