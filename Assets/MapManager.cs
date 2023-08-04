using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class MapManager : MonoBehaviour
{
    public RawImage mapRawImage;

    [Header("�� ���� �Է�")]
    public string strBaseURL = ""; 
    public string latitude = ""; // ����
    public string longitude = ""; // �浵
    public int level = 14; // ������ Ȯ�� �� ���
    public int mapWidth; // ���α���
    public int mapHeight; // ���α���
    public string strAPIKey = ""; // ���̹� API ����Ű
    public string secretKey = ""; // ���̹� API ���Ű
    

    // Start is called before the first frame update
    void Start()
    {
        mapRawImage = GetComponent<RawImage>();
        StartCoroutine(MapLoader());
    }

    IEnumerator MapLoader()
    {
        string str = strBaseURL + "?w=" + mapWidth.ToString() + "&h=" + mapHeight.ToString() + "&center=" + longitude + "," + latitude + "&level=" + level.ToString();

        Debug.Log(str.ToString());

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(str);

        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", strAPIKey);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", secretKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            mapRawImage.texture = DownloadHandlerTexture.GetContent(request);
        }
    }
}