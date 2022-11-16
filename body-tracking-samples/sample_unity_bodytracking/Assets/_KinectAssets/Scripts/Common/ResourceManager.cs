using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 리소스 매니저
public class ResourceManager
{
    // 추후 데이터 로드 방식 변경이 필요하다면 쉽게 변경 가능하도록 아래의 함수를 사용한다.  
    // 프로젝트 내에서 Resources.Load()를 직접 사용하지 않는다. 
    public static Object Load(string path)
    {
        return Resources.Load(path);
    }
    public static T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
}
