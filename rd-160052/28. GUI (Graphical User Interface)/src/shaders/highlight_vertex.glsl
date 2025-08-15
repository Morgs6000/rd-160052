#version 330 core // Define a versao do GLSL (OpenGL Shading Language) como 3.30

// Define um atributo de entrada para o vertex shader
// 'layout(location = 0)' especifica que este atributo esta associado a localizacao 0
// 'in vec3 aPos' declara um vetor de 3 componentes (x, y, z) que representa a posicao do vertice
layout(location = 0) in vec3 aPos;

// Define um atributo de entrada para as coordenadas de textura
// 'layout(location = 1)' especifica que este atributo esta associado a localizacao 1
// 'in vec2 aTexCoord' declara um vetor de 2 componentes (s, t) que representa as coordenadas de textura
layout(location = 1) in vec2 aTexCoord;

// Define um atributo de entrada para a cor
// 'layout(location = 2)' especifica que este atributo esta associado a localizacao 2
// 'in vec3 aColor' declara um vetor de 3 componentes (r, g, b) que representa a cor do vertice
layout(location = 2) in vec3 aColor;

// Define uma variavel de saida para as coordenadas de textura
// 'out vec2 texCoord' declara um vetor de 2 componentes que sera passado para o fragment shader
out vec2 texCoord;

// Define uma variavel de saida para a cor
// 'out vec3 color' declara um vetor de 3 componentes que sera passado para o fragment shader
out vec3 color;

// Define um uniform para a matriz de visualizacao (view)
// 'uniform mat4 view' declara uma matriz 4x4 que representa a transformacao de visualizacao
uniform mat4 view;

// Define um uniform para a matriz de projecao (projection)
// 'uniform mat4 projection' declara uma matriz 4x4 que representa a transformacao de projecao
uniform mat4 projection;

// Funcao principal do vertex shader
void main() {
    // Define a posicao do vertice no espaco de clip (coordenadas normalizadas)
    // 'aPos' e um vec3 (x, y, z), e ele e convertido para vec4 (x, y, z, w)
    // w e definido como 1.0f (coordenada homogenea)
    gl_Position = vec4(aPos, 1.0f);

    // Aplica a transformacao de visualizacao (view) a posicao do vertice
    gl_Position *= view;

    // Aplica a transformacao de projecao (projection) a posicao do vertice
    gl_Position *= projection;

    // Passa as coordenadas de textura para o fragment shader
    texCoord = aTexCoord;

    // Passa a cor para o fragment shader
    color = aColor;
}
