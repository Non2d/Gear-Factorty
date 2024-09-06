using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;  // UnityWebRequest���g�p���邽�߂ɕK�v
using System.Text;  // Encoding���g�p���邽�߂ɕK�v

public class Gpt : MonoBehaviour
{
    private string apiUrl = "http://localhost:8000";  // FastAPI��URL

    // �R���[�`����API���N�G�X�g�𑗐M
    IEnumerator GetRequest()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            // ���N�G�X�g�̑��M�ƃ��X�|���X�̑ҋ@
            yield return request.SendWebRequest();

            // �G���[�n���h�����O
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // ���������ꍇ�̃��X�|���X
                Debug.Log("Response: " + request.downloadHandler.text);
            }
        }
    }

    private string apiUrl2 = "http://localhost:8000/gpt";  // FastAPI��URL

    // "Fire burning"�Ƃ�����������|�X�g
    IEnumerator PostRequest()
    {
        // ���N�G�X�g�f�[�^��JSON�`���ɃV���A���C�Y
        string jsonData = JsonUtility.ToJson(new TextRequest { text = "Fire burning" });

        // UnityWebRequest���g�p����POST���N�G�X�g���쐬
        using (UnityWebRequest request = new UnityWebRequest(apiUrl2, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // ���N�G�X�g�̑��M�ƃ��X�|���X�̑ҋ@
            yield return request.SendWebRequest();

            // �G���[�n���h�����O
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // ���������ꍇ�̃��X�|���X
                Debug.Log("Response: " + request.downloadHandler.text);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(GetRequest());
        StartCoroutine(PostRequest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���N�G�X�g�f�[�^�̃N���X��`
    [System.Serializable]
    public class TextRequest
    {
        public string text;
    }
}
