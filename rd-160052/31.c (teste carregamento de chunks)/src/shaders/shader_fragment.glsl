#version 330 core // Define a versao do GLSL (OpenGL Shading Language) como 3.30

// Define uma variavel de saida para o fragment shader
// 'out vec4 FragColor' declara um vetor de 4 componentes (RGBA) que representa a cor do fragmento
out vec4 FragColor;

// Funcao principal do fragment shader
void main() {
    // Define a cor do fragmento como um vetor RGBA
    // vec4(1.0f, 0.5f, 0.2f, 1.0f) representa:
    // - R = 1.0 (vermelho no maximo)
    // - G = 0.5 (verde na metade)
    // - B = 0.2 (azul em 20%)
    // - A = 1.0 (totalmente opaco)
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
}
