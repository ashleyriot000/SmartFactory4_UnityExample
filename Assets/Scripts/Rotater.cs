using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 90f;
    public Vector3 rotateAxis = Vector3.up;

    private float _distance = 0f;

    void Start()
    {
        if (target == null)
            return;

        //목적지에서 현재 위치를 빼면 목적지를 향하는 방향과 거리를 알 수 있다.
        Vector3 forward = target.position - transform.position;

        //벡터에서 magnitude변수를 이용해 벡터값이 가진 거리를 알아낼 수 있다.
        _distance = forward.magnitude;
        //LookRotation함수로 목적방향으로 회전시킬 수 있다.
        transform.rotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        //공전하기 위해서는 먼저 회전시키고 위치를 변경해야 함.
        transform.Rotate(rotateSpeed * Time.deltaTime * rotateAxis, Space.World);

        //타겟이 없으면 위치 변경하지 않아도 됨.
        if (target != null)
        {
            //목적위치 + (바라보는 방향의 반대방향 X 거리) = 공전시 있어야 하는 위치.
            transform.position = target.position + (-transform.forward * _distance);
        }
    }
}
