using UnityEngine;

public class HardEdgeModel : MonoBehaviour
{
    private void Awake()
    {
        EmbedSoftEdgeToVertexColor(gameObject);
    }

    /// <summary>
    /// �\�t�g�G�b�W���𒸓_�J���[�ɖ��ߍ���
    /// </summary>
    /// <param name="obj"></param>
    private static void EmbedSoftEdgeToVertexColor(GameObject obj)
    {
        var meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in meshFilters)
        {
            var mesh = meshFilter.sharedMesh;
            var normals = mesh.normals;
            var vertices = mesh.vertices;
            var vertexCount = mesh.vertexCount;

            // �\�t�g�G�b�W�@�����̐���
            var softEdges = new Color[normals.Length];
            for (var i = 0; i < vertexCount; i++)
            {
                // �����ʒu�̒��_�̖@�����W�̕��ς�ݒ肷��
                var softEdge = Vector3.zero;
                for (var j = 0; j < vertexCount; j++)
                {
                    var v = vertices[i] - vertices[j];
                    if (v.sqrMagnitude < 1e-8f)
                    {
                        softEdge += normals[j];
                    }
                }
                softEdge.Normalize();
                softEdges[i] = new Color(softEdge.x, softEdge.y, softEdge.z, 0);
            }

            // ���_�J���[�ɖ��ߍ���
            mesh.colors = softEdges;
        }
    }
}