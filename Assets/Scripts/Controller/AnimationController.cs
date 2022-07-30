using UnityEngine;

// 캐릭터 애니메이션 컨트롤러
public class AnimationController : MonoBehaviour
{
    // 애니메이터
    Animator anim;

    // 애니메이션 가능 여부
    bool canAnim = true;

    // 애니메이션 활성화 여부
    // toggle = ture 애니메이션 활성화
    // toggle = false 애니메이션 비활성화 및 즉시 종료
    public void OnAnimator(bool toggle)
    {
        canAnim = toggle;

        if (canAnim == false)
        {
            anim.ResetTrigger("onJump");
            anim.Play("Wait");
        }
    }

    // 점프 애니메이션 실행
    public void PlayJump()
    {
        if (canAnim)
            anim.SetTrigger("onJump");
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null) Debug.Log("[경고] 애니메이터가 존재하지 않습니다");
    }
}