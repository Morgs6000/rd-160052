using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script é um componente que adiciona um efeito de destaque a um objeto quando um raio é lançado da câmera e atinge um objeto na cena.
public class Highlight : MonoBehaviour {
    // O objeto de destaque a ser ativado e posicionado quando o raio é atingido.
    [SerializeField] private GameObject highlight;
        
    // Referências aos componentes MeshFilter e MeshRenderer do objeto de destaque.
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
    // A malha a ser usada pelo objeto de destaque.
    private Mesh highlighMesh;

    // As listas de vértices e triângulos da malha.
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    // O material a ser usado pelo objeto de destaque.
    [SerializeField] private Material material;

    // A transformação da câmera a ser usada para lançar o raio.
    [SerializeField] private Transform cam;
    // A distância máxima em que o raio pode atingir um objeto.
    private float rangeHit = 5.0f;
    // A máscara de camadas a ser usada para determinar quais objetos o raio pode atingir.
    [SerializeField] private LayerMask groundMask;

    // Um enumerador que representa os lados do objeto de destaque.
    private enum HighlighSide { RIGHT, LEFT, TOP, BOTTOM, FRONT, BACK }

    // O número total de vértices na malha.
    private int vertexIndex;
    
    // Inicializa o objeto de destaque, criando sua malha e adicionando os componentes MeshFilter e MeshRenderer.
    private void Start() {
        // Adicione os componentes MeshFilter e MeshRenderer ao objeto de destaque.
        meshFilter = (MeshFilter)highlight.AddComponent(typeof(MeshFilter));
        meshRenderer = (MeshRenderer)highlight.AddComponent(typeof(MeshRenderer));

        // Crie a malha
        highlighMesh = new Mesh();
        highlighMesh.name = "Highlight";

        // Gere a malha para o objeto de destaque.
        HighlighGen();
        // Adicione a malha ao MeshFilter e defina o material do MeshRenderer.
        MeshRenderer();
    }

    // Atualiza o objeto de destaque a cada quadro.
    private void Update() {
        // Atualize a posição do objeto de destaque de acordo com o resultado do raio lançado da câmera.
        HighlightUpdates();
        // Atualize a transparência do material do objeto de destaque para criar um efeito de piscar.
        ColorUpdate();
    }

    // Atualiza a transparência do material do objeto de destaque para criar um efeito de piscar.
    private void ColorUpdate() {
        // Defina a cor inicial com uma transparência de 0,5.
        Color colorA = material.color;
        colorA.a = 0.5f;

        // Defina a cor final com uma transparência de 0.
        Color colorB = material.color;
        colorB.a = 0.0f;

        // A velocidade do efeito de piscar.
        float speed = 2;

        // Defina o material do objeto de destaque e altere sua cor usando o método Lerp do Unity para criar o efeito de piscar.
        meshRenderer.material = material;
        meshRenderer.material.color = Color.Lerp(colorA, colorB, Mathf.PingPong(Time.time * speed, 1));
    }

    // Atualiza a posição do objeto de destaque de acordo com o resultado do raio lançado da câmera.
    private void HighlightUpdates() {
        RaycastHit hit;

        // Lança um raio a partir da câmera e armazena o resultado em hit.
        if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
            // Ative o objeto de destaque.
            highlight.SetActive(true);

            // Calcula a posição do objeto atingido pelo raio.
            Vector3 pointPos = hit.point - hit.normal / 2;
            
            // Posicione o objeto de destaque na posição do objeto atingido, arredondando os valores de posição para inteiros.
            highlight.transform.position = new Vector3(
                Mathf.FloorToInt(pointPos.x),
                Mathf.FloorToInt(pointPos.y),
                Mathf.FloorToInt(pointPos.z)
            );
        }
        else {
            // Desative o objeto de destaque se o raio não atingir nenhum objeto.
            highlight.SetActive(false);          
        }
    }

    // Gere a malha para o objeto de destaque.
    private void HighlighGen() {
        // Adicione os vértices para cada lado do objeto de destaque.
        VerticesAdd(HighlighSide.RIGHT);
        VerticesAdd(HighlighSide.LEFT);
        VerticesAdd(HighlighSide.TOP);
        VerticesAdd(HighlighSide.BOTTOM);
        VerticesAdd(HighlighSide.FRONT);
        VerticesAdd(HighlighSide.BACK);
    }

    // Adicione a malha ao MeshFilter e defina o material do MeshRenderer.
    private void MeshRenderer() {
        // Defina os vértices e triângulos da malha.
        highlighMesh.vertices = vertices.ToArray();
        highlighMesh.triangles = triangles.ToArray();

        // Recalcule as normais da malha e otimize-a.
        highlighMesh.RecalculateNormals();
        highlighMesh.Optimize();

        // Adicione a malha ao MeshFilter do seu GameObject
        meshFilter.mesh = highlighMesh;
    }

    // Adicione os vértices da malha para o lado especificado do objeto de destaque.
    private void VerticesAdd(HighlighSide side) {
        switch(side) {
            case HighlighSide.RIGHT: {
                // Adicione os vértices para o lado leste do objeto de destaque.
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 0, 1));

                break;
            }
            case HighlighSide.LEFT: {
                // Adicione os vértices para o lado oeste do objeto de destaque.
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case HighlighSide.TOP: {
                // Adicione os vértices para o topo do objeto de destaque.
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 1, 0));

                break;
            }
            case HighlighSide.BOTTOM: {
                // Adicione os vértices para o fundo do objeto de destaque.
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case HighlighSide.FRONT: {
                // Adicione os vértices para o lado norte do objeto de destaque.
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 0, 1));

                break;
            }
            case HighlighSide.BACK: {
                // Adicione os vértices para o lado sul do objeto de destaque.
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 0, 0));

                break;
            }
        }

        // Adicione os triângulos para o lado atual do objeto de destaque.
        TrianglesAdd();
    }

    // Adicone os Triangulos dos Vertices para renderizar a face
    private void TrianglesAdd() {
        // Primeiro Tiangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(1 + vertexIndex);
        triangles.Add(2 + vertexIndex);

        // Segundo Triangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(2 + vertexIndex);
        triangles.Add(3 + vertexIndex);

        vertexIndex += 4;
    }
}
