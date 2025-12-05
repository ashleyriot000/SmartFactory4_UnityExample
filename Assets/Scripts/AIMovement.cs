using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    //AI의 행동 상태
    public enum MoveState
    {
        None = 0,
        Watching,
        Traceing,
        Returning,
        Max
    }
    [Header("타겟과 AI")]
    //AI 에이전트
    public NavMeshAgent agent;
    //이동 목적지 위치정보
    public Transform target;
    public MoveState currentState = MoveState.Watching;

    [Header("감시 모드 관련")]
    //AI의 눈의 위치 정보
    public Transform watchPoint;
    public Transform eyePoint;
    [Range(10f, 200f)]
    //AI의 최대 시야거리
    public float watchableDistance = 10f;
    [Range(10f, 90f)]
    //AI의 최대 시야각
    public float watchableAngle = 30f;
    //감시모드시 최대 회전각
    public float watchRotateAngle = 15f;
    //감시모드시 회전 속도
    public float watchRotateSpeed = 20f;

    [Header("추적 모드 관련")]
    //AI 판단 딜레이 
    public float interval = 1f;
    //자폭 거리
    public float explosionDistance = 3f;
    //최대 추적 기간
    public float maxTracingDuration = 30f;
    //폭발 공격력
    public int explosionDamage = 1;


    //왼쪽으로 회전하는 중인가
    private bool _isLeft = false;
    //판단을 내려야 하는 시간
    private float _waitTime = 0f;
    //추적을 멈춰야 하는 시간
    private float _tracingExitTime = 0f;

    //씬 뷰에서 선택 상관없이 호출되어 필요한 정보를 화면에 그려줄 수 있다.
    private void OnDrawGizmos()
    {
        //시야 반경을 원형 그리기
        Handles.color = Color.red;
        Handles.DrawWireDisc(eyePoint.position, Vector3.up, watchableDistance);

        //부채꼴의 왼쪽/오른쪽 경계선을 계산
        Vector3 viewAngleLeft = CalculateAngle(-watchableAngle, false);
        Vector3 viewAngleRight = CalculateAngle(watchableAngle, false);
        //해서 그리기
        Handles.DrawLine(eyePoint.position, eyePoint.position + viewAngleLeft * watchableDistance);
        Handles.DrawLine(eyePoint.position, eyePoint.position + viewAngleRight * watchableDistance);

        //시야가 닿는 범위 그리기
        Handles.color = new Color(1f, 0f, 0f, 0.1f);
        //DrawSolidArc(중심점, 회전축, 시작 방향, 각도, 반지름);
        Handles.DrawSolidArc(eyePoint.position, Vector3.up, viewAngleLeft, watchableAngle * 2f, watchableDistance);
    }

    private Vector3 CalculateAngle(float angle, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angle += eyePoint.eulerAngles.y;
        }

        //삼각함수를 이용해서 x축에 Sin(angle), z축에 Cos(angle)로 원을 그리는 각도를 구할 수 있다.
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    void Start()
    {
        //시작시 AI 에이전트 미리 캐싱해놓는다.
        agent = GetComponent<NavMeshAgent>();

        switch (currentState)
        {
            case MoveState.Watching:
                SetWatchMode();
                break;
            case MoveState.Traceing:
                SetTraceMode();
                break;
            case MoveState.Returning:
                SetReturnMode();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case MoveState.Watching:
                Watch();
                break;
            case MoveState.Traceing:
                Trace();
                break;
            case MoveState.Returning:
                Return();
                break;
            default:
                break;
        }
    }

    private void SetWatchMode()
    {
        _isLeft = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        currentState = MoveState.Watching;
    }

    private void Watch()
    {
        //삼항 연산자를 이용해서 왼쪽으로 회전할지 오른쪽으로 회전할 지 정한다음 목표 회전값을 구한다.
        //사원수로 된 결과값 = Quaternion.Euler(360개념의 벡터값)
        //사원수 * 사원수 = 회전값에 회전을 더한 값을 얻을 수 있다.
        Quaternion destination = _isLeft ? watchPoint.rotation * Quaternion.Euler(0f, -watchRotateAngle, 0f) :
            watchPoint.rotation * Quaternion.Euler(0f, watchRotateAngle, 0f);

        //목표 회전값까지 서서히 회전하도록 Quaternion.RotateToward함수를 사용.
        //Quaternion.RotateToward(현재 회전값, 목표 회전값, 한번에 회전할 수 있는 최대각);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destination, watchRotateSpeed * Time.deltaTime);

        //Quaternion.Angle함수는 두 회전값의 각도 차이를 구하는 함수
        //두 회전값의 차이 각도 = Quaternion.Angle(1번째 회전값, 2번째 회전값);
        if (Quaternion.Angle(transform.rotation, destination) < 0.001f)
        {
            _isLeft = !_isLeft;
        }

        //타겟을 추적할 수 있는 상태인지 확인하고 가능하면 추적상태로 전환
        if (CheckTarget(false, out float distance))
        {
            SetTraceMode();
        }
    }

    //추적상태로 전환하는 함수
    private void SetTraceMode()
    {
        //현재 상태를 추적상태로 바꾸고
        currentState = MoveState.Traceing;
        //AI가 위치와 회전을 갱신할 수 있게 변경하고
        agent.updatePosition = true;
        agent.updateRotation = true;
        //AI에게 목적지를 설정한다.
        agent.SetDestination(target.position);

        //다음 명령 하달 시간을 갱신하고
        _waitTime = Time.time + interval;
        //추적 만료 시간을 설정한다.
        _tracingExitTime = Time.time + maxTracingDuration;
    }

    //추적하는 함수
    private void Trace()
    {
        //추적 만료시간이 지났는지 혹은 추적가능 거리를 벗어났는지 확인해 리턴모드로 전환한다.
        if(_tracingExitTime < Time.time || !CheckTarget(true, out float distance))
        {
            SetReturnMode();
            return;
        }

        //폭발 가능거리까지 접근했는지 확인해 자폭한다.
        if(distance < explosionDistance)
        {
            Explosion();
        }

        //다음 명령 하달 시간이 되었는지 확인하고 안되었으면 return
        if (_waitTime > Time.time)
            return;

        //명령 하달하고 다음 명령 하달 시간을 갱신
        _waitTime = Time.time + interval;
        agent.SetDestination(target.position);
    }

    private void Explosion()
    {
        LifeCounter counter = target.GetComponent<LifeCounter>();
        Debug.Log(counter);
        if(counter != null)
        {
            counter.TakeDamage(explosionDamage);
        }

        Destroy(gameObject);
    }

    //리턴모드로 전환하는 함수
    private void SetReturnMode()
    {
        //현재 상태를 리턴모드로 바꾼다.
        currentState = MoveState.Returning;
        //AI에게 위치, 회전 변경권을 내준다.
        agent.updateRotation = true;
        agent.updatePosition = true;

        //감시 지역으로 목적지를 설정한다.
        agent.SetDestination(watchPoint.position);
    }    

    //리턴모드
    private void Return()
    {
        //귀환 도중에 타겟을 감지하면 다시 추적모드로 전환
        if(CheckTarget(false, out float distance))
        {
            SetTraceMode();
            return;
        }        

        //현재 목적지 경로 계산중이 아니고 목적지까지 남은 거리가 도달 거리보다 적게 남은 경우 감시모드로 전환
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            SetWatchMode();
    }

    //타겟이 시야 거리, 시야각 안에 감지했는지 확인해 감지여부를 반환함.
    private bool CheckTarget(bool onlyDistanceCheck, out float targetDistance)
    {        
        //목적지(벡터3) - 현재 위치 => 현재 위치 기준 목적지의 방향과 거리를 구할 수 있다.
        Vector3 toward = target.position - eyePoint.position;
        //Vector3.manitude 변수를 이용해서 벡터값에서 거리를 구할 수 있다.
        targetDistance = toward.magnitude;
        if (targetDistance > watchableDistance)
        {
            return false;
        }

        //만약 거리만 확인하는 상태라면 찾았다고 리턴
        if (onlyDistanceCheck)
            return true;

        //벡터의 내적을 이용해서 정면 방향에 목적지가 있는지 값을 구할 수 있다.
        //목적방향과 완전히 일치할 경우 1, 완전 반대방향은 -1, 90도 방향이면 0
        if (Vector3.Dot(eyePoint.forward, toward.normalized) < Mathf.Cos(watchableAngle * Mathf.Deg2Rad))
        {
            return false;
        }

        return true;
    }
}

