using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using BehaviorTreeNodeGraphEditor;
using Unity.VisualScripting;

public class RotateText : MonoBehaviour
{
    [SerializeField] private List<char> NonRotatableCharacters;
    [SerializeField] static int ShiftChar = 0;
    [SerializeField] private char[] ShiftCharacters = new char[ShiftChar];
    [SerializeField] private float[] ShiftXPixels = new float[ShiftChar];
    [SerializeField] private float[] ShiftYPixels = new float[ShiftChar];

    private TMP_Text textMeshPro;
    private void Awake()
    {
        TryGetComponent(out textMeshPro);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // メッシュ更新
        textMeshPro.ForceMeshUpdate();

        var textInfo = textMeshPro.textInfo;
        if (textInfo.characterCount == 0)
        {
            return;
        }

        //文字毎にloop
        for (int index = 0; index < textInfo.characterCount; index++)
        {
            var charaInfo = textInfo.characterInfo[index];

            //ジオメトリない文字はスキップ
            if (!charaInfo.isVisible)
            {
                continue;
            }

            //回転させたくない文字のリストに登録されていたらスキップ
            if (IsNonrotatableCharactor(charaInfo.character))
            {
                continue;
            }

            //Material参照しているindex取得
            int materialIndex = charaInfo.materialReferenceIndex;

            //頂点参照しているindex取得
            int vertexIndex = charaInfo.vertexIndex;

            //頂点(dest->destinationの略)
            Vector3[] destVertices = textInfo.meshInfo[materialIndex].vertices;

            float angle = 90;

            //文字の位置の制御
            float[] shiftPixel = GetPixelShiftCharactor(charaInfo.character);
            var center = (destVertices[vertexIndex + 1] + destVertices[vertexIndex + 3]) / 2;

            if (shiftPixel[0] != 0 || shiftPixel[1] != 0)
            {
                center = new Vector2(center.x + shiftPixel[0], center.y + shiftPixel[1]);
            }

            destVertices[vertexIndex + 0] += -center;
            destVertices[vertexIndex + 1] += -center;
            destVertices[vertexIndex + 2] += -center;
            destVertices[vertexIndex + 3] += -center;


            //回転
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle), Vector3.one);

            destVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destVertices[vertexIndex + 0]);
            destVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destVertices[vertexIndex + 1]);
            destVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destVertices[vertexIndex + 2]);
            destVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destVertices[vertexIndex + 3]);

            destVertices[vertexIndex + 0] += center;
            destVertices[vertexIndex + 1] += center;
            destVertices[vertexIndex + 2] += center;
            destVertices[vertexIndex + 3] += center;
        }

        //ジオメトリ更新
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            //メッシュ情報
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    bool IsNonrotatableCharactor(char character)
    {
        return NonRotatableCharacters.Any(x => x == character);
    }

    float[] GetPixelShiftCharactor(char character)
    {
        int index = System.Array.IndexOf(ShiftCharacters, character);
        float[] pixel = new float[2];
        if (0 <= index && index < ShiftXPixels.Length && index < ShiftYPixels.Length)
        {
            pixel[0] = ShiftXPixels[index];
            pixel[1] = ShiftYPixels[index];
        }
        return pixel;
    }
}
