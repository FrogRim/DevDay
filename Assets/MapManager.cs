using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class MapManager : MonoBehaviour
{
    public RawImage mapRawImage;

    [Header("맵 정보 입력")]
    public string strBaseURL = ""; 
    public string latitude = ""; // 위도
    public string longitude = ""; // 경도
    public int level = 14; // 지도의 확대 및 축소
    public int mapWidth; // 가로길이
    public int mapHeight; // 세로길이
    public string strAPIKey = ""; // 네이버 API 공개키
    public string secretKey = ""; // 네이버 API 비밀키
    

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