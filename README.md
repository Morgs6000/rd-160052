# rd-160052
 
Este projeto tem como objetivo recriar o Minecraft usando a linguagem C# e a biblioteca OpenTK.

Este é uma continuação da rd-132328:
- https://github.com/Morgs6000/rd-132328

## Ferramentas e Tecnologias
<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/vscode/vscode-original.svg" /></code> VS Code

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/csharp/csharp-original.svg" /></code> C#

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/opengl/opengl-original.svg" /></code> OpenGL

<code><img height="30" src="https://avatars.githubusercontent.com/u/5914736?s=280&v=4" /></code> OpenTK

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/nuget/nuget-original.svg" /></code> StbImageSharp

## Adições
### Blocos
<code><img height="30" src="https://github.com/user-attachments/assets/cd3ccb9e-22a2-466d-bb84-9b8000bd0a71" /></code> Pedregulho
- Recicla a textura anterior para pedra, embora ligeiramente modificada.

<code><img height="30" src="https://github.com/user-attachments/assets/b99d92cc-8492-431a-ad5b-3e20ecbf5db5" /></code> Terra
- Novo bloco.
- Gerado sob blocos de grama.
- Transforma-se em grama quando está proximo a um bloco de grama.

<code><img height="30" src="https://github.com/user-attachments/assets/25a79cef-2b0a-481f-9f91-5107237b884a" /></code> Tábuas
- Novo bloco de madeira.

### Jogabilidade
Controles
- Mira (+).
  - Nesta versão, era muito fino.
- Seleção de blocos usando as teclas numéricas de <code><img height="30" src="https://github.com/user-attachments/assets/63b71c6b-1730-4432-8b91-52bacfb4051f" /></code> a <code><img height="30" src="https://github.com/user-attachments/assets/996cfdda-59bc-4ae1-a6f7-4c49df74ea0a" /></code>.
  - O building block selecionado no momento agora é exibido no canto superior direito da tela.
- Modo de tela cheia.
- Pressionar <code><img height="30" src="https://github.com/user-attachments/assets/73570047-5811-4197-ab5a-7415e2b8e738" /></code> gerará mobs.

### Geral
- Suporte para diferentes tipos de blocos e blocos dinâmicos.
- Uma fina camada de neblina agora pode ser vista à distância das superfícies de blocos iluminados.

## Mudanças
### Blocos
Bloco de Grama
- Textura alterada de <code><img height="30" src="https://github.com/user-attachments/assets/d83ce445-af95-40d0-8ab4-e7f13310b52d" /></code> para <code><img height="30" src="https://github.com/user-attachments/assets/a6e2ce56-1012-40e4-9460-2153c9f3fe48" /></code>.
- Os blocos de grama voltam a ser terra com o tempo quando não têm luz ou quando um bloco está em cima dele.
- Removida a capacidade de colocar blocos de grama.
Pedra
- Textura alterada de <code><img height="30" src="https://github.com/user-attachments/assets/0c452370-7abe-446d-83e3-6fbdfde17aff" /></code> para <code><img height="30" src="https://github.com/user-attachments/assets/103dfe9c-f91d-41ca-a2cf-5cb42ce43ba4" /></code>.

### Mobs
<code><img src="https://minecraft.wiki/images/thumb/Steve_JE1.png/40px-Steve_JE1.png?0814f" /></code> <code><img src="https://minecraft.wiki/images/thumb/Old_human_mob_walking_animation.gif/40px-Old_human_mob_walking_animation.gif?a4465" /></code> Mob
- Reduzimos o número de criaturas que aparecem de 100 para 10.
- Criaturas que caem abaixo do nível -y100 agora são removidas do mundo.
- Criaturas agora parecem mais escuras quando na sombra.

### Entidades não-mob
Particulas
- Adição de entidades de partículas de bloco.
  - Aparecem quando os blocos são quebrados.
  - Caia no chão antes de desaparecer.

### Geração de mundo
- Saliencias podem aparecer agora, formando colinas e às vezes penhascos.

### Geral
Luz
- As sombras foram iluminadas.

### Jogabilidade
- O jogo agora roda no modo de tela cheia.

## Técnico
Geração de niveis
- A menos que uma versão **level.dat** mais antiga seja lançada, o mundo não é mais plano.
- Carregar um mundo de versões pré-clássicas mais antigas faz com que todos os blocos se tornem pedra.

## Curiosidades
- O "rd" antes do número da versão significa RubyDung, um jogo em que Notch estava trabalhando antes do Minecraft, cuja base de código foi posteriormente reutilizada para o Minecraft.
- Uma duplicata de rd-161348 no inicializador é rotulada como "rd-20090515", no entanto, rd-20090515 não está nos servidores de download do Minecraft e tentar baixá-lo baixa rd-161348, uma versão posterior.

## Referencias
- https://minecraft.wiki/w/Java_Edition_pre-Classic_rd-20090515
- https://minecraft.wiki/w/Java_Edition_pre-Classic_rd-160052

## To-Do

## Progresso
