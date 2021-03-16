#version 330 core
in vec2 fragTextureCoords;

out vec4 fragColor;

uniform sampler2D texture0;

void main()
{
    fragColor = texture(texture0, fragTextureCoords);
}