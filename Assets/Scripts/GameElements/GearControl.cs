using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GearControl : MonoBehaviour
{
    public float gearSpeed = 20.0f;

    private List<TextMeshPro> textComponents = new List<TextMeshPro>();

    [SerializeField]
    private GameObject test_player;

    private int divisions; //param
    private float originAngle; //param

    // Start is called before the first frame update
    void Start()
    {
        divisions = 18;
        originAngle = 5.0f;

        Vector3 parentScale = transform.localScale;
        float radius = 10.0f; //param
        float height = 5.0f; //param

        for (int i = 1; i <= divisions; i++)
        {
            float angleDegree = 20.0f * (i - 1) + originAngle;
            float angle = Mathf.Deg2Rad * angleDegree;

            GameObject textObj = new GameObject($"TextMeshPro_{i}");
            textObj.transform.SetParent(transform);

            TextMeshPro text = textObj.AddComponent<TextMeshPro>();
            text.text = i.ToString();
            text.fontSize = 20;
            text.alignment = TextAlignmentOptions.Center;

            float xPosition = radius * Mathf.Cos(angle);
            float yPosition = radius * Mathf.Sin(angle);
            textObj.transform.localPosition = new Vector3(xPosition / parentScale.x, yPosition / parentScale.y, height / parentScale.z);
            textObj.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f) * Quaternion.AngleAxis(angleDegree - 90.0f, Vector3.up) * Quaternion.Euler(10.0f, 0.0f, 0.0f);

            textComponents.Add(text);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, gearSpeed * Time.deltaTime);

        // Debug.Log(GetLocalDirectionToPlayer());
    }

    float GetLocalDirectionToPlayer()
    {
        // 現在のオブジェクトの位置を基準とする
        Vector3 currentPosition = transform.position;

        // test_playerの位置を取得
        Vector3 playerPosition = test_player.transform.position;

        // 方向ベクトルを計算
        Vector3 direction = playerPosition - currentPosition;

        // 方向ベクトルを正規化
        Vector3 normalizedDirection = direction.normalized;

        // ローカル座標系に変換
        Vector3 localDirection = transform.InverseTransformDirection(normalizedDirection);

        // 角度を計算(-180~180)
        float angle = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }

    int GetPocketLanded(float localDirection)
    {
        // -10から始まる範囲にシフト
        float shiftedDirection = localDirection + 10;

        // divisionsの範囲でポケット番号を計算
        int pocketNumber = Mathf.FloorToInt(shiftedDirection / 20) + 1;

        return pocketNumber;
    }


    public void OnPlayerEnterChildTrigger()
    {
        Debug.Log(GetLocalDirectionToPlayer());
        Debug.Log(GetPocketLanded(GetLocalDirectionToPlayer()));

    }
}
