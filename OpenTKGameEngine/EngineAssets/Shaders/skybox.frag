#version 330 core
in vec2 fragTextureCoords;

out vec4 fragColor;

uniform samplerCube skyboxTexture;

void main()
{
    fragColor = texture(skyboxTexture, fragTextureCoords);
}