using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    private Mesh voxelMesh;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    private enum VoxelSide { RIGHT, LEFT, TOP, BOTTOM, FRONT, BACK }

    private int vertexIndex;

    [SerializeField] private VoxelType voxelType;

    private void Start() {
        voxelMesh = new Mesh();
        voxelMesh.name = "Block";

        BlockGen();

        MeshRenderer();
    }

    private void Update() {
        
    }

    private void BlockGen() {
        VerticesAdd(VoxelSide.RIGHT);
        VerticesAdd(VoxelSide.LEFT);
        VerticesAdd(VoxelSide.TOP);
        VerticesAdd(VoxelSide.BOTTOM);
        VerticesAdd(VoxelSide.FRONT);
        VerticesAdd(VoxelSide.BACK);
    }

    private void MeshRenderer() {
        voxelMesh.vertices = vertices.ToArray();
        voxelMesh.triangles = triangles.ToArray();
        voxelMesh.uv = uv.ToArray();

        voxelMesh.RecalculateNormals();
        voxelMesh.Optimize();

        // Adiciona a malha um colisor
        GetComponent<MeshCollider>().sharedMesh = voxelMesh;

        // Adicione a malha ao MeshFilter do seu GameObject
        GetComponent<MeshFilter>().mesh = voxelMesh;
    }

    // Adicione os Vertices da Malha
    private void VerticesAdd(VoxelSide side) {
        switch(side) {
            case VoxelSide.RIGHT: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 0, 1));

                break;
            }
            case VoxelSide.LEFT: {
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case VoxelSide.TOP: {
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 1, 0));

                break;
            }
            case VoxelSide.BOTTOM: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case VoxelSide.FRONT: {
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 0, 1));

                break;
            }
            case VoxelSide.BACK: {
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 0, 0));

                break;
            }
        }

        TrianglesAdd();

        UVsPos(side);
    }

    // Adicone os Triangulos dos Vertices para renderizar a side
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

    // Adicione as UVs dos Vertices para renderizar a textura
    private void UVsAdd(Vector2 textureCoordinate) {
        Vector2 offset = new Vector2(
            0, 
            0
        );

        Vector2 textureSize = new Vector2(
            16 + offset.x,
            16 + offset.y
        );
        
        float x = textureCoordinate.x + offset.x;
        float y = textureCoordinate.y + offset.y;

        float _x = 1.0f / textureSize.x;
        float _y = 1.0f / textureSize.y;

        y = (textureSize.y - 1) - y;

        x *= _x;
        y *= _y;

        uv.Add(new Vector2(x, y));
        uv.Add(new Vector2(x, y + _y));
        uv.Add(new Vector2(x + _x, y + _y));
        uv.Add(new Vector2(x + _x, y));
    }

    // Pegue a posição da UV no Texture Atlas
    private void UVsPos(VoxelSide side) {
        // Pre-Classic | rd-132211
        
        // STONE
        if(voxelType == VoxelType.stone) {
            UVsAdd(new Vector2(1, 0));
        }

        // GRASS BLOCK
        if(voxelType == VoxelType.grass_block) {
            if(side == VoxelSide.TOP) {
                UVsAdd(new Vector2(0, 0));
                return;
            }
            if(side == VoxelSide.TOP) {
                UVsAdd(new Vector2(2, 0));
                return;
            }            
            UVsAdd(new Vector2(3, 0));
        }
        
        // DIRT
        if(voxelType == VoxelType.dirt) {
            UVsAdd(new Vector2(2, 0));
        }
        
        // COBBLESTONE
        if(voxelType == VoxelType.cobblestone) {
            UVsAdd(new Vector2(0, 1));
        }
        
        // OAK PLANKS
        if(voxelType == VoxelType.oak_planks) {
            UVsAdd(new Vector2(4, 0));
        }
    }
}
