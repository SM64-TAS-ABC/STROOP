#version 110

in vec4 Color;
in vec2 TexCoords;

uniform sampler2D tex;

void main()
{
    gl_FragColor = Color * texture2D(tex, TexCoords);
}
